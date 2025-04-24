using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class BossOfControling : Enemy
{
    [SerializeField] private float _timeBtwZoneActivation, _timeOfCell, _laserScale, _laserSpeedDebuff, _timeOfRotation, _offsetPlus, _hpToCopy, _radius,
                                                                                                                        _areaRadius, _timeBtwMelleeAttack;

    [SerializeField] private int _countOfCells, _indexOfAttack, _numOfLaserRotations, _numOfLasers;
    [SerializeField] private GameObject _zoneOfCell, _laser, _copy, _laserTrace;
    [SerializeField] private List<Transform> _pointToCopy;

    [Tooltip("Называть в той последовательности, в которой будет атака")]
    [SerializeField] private List<string> _animNames;


    private float _currentTime;
    bool _wasCopied;
    public override void TakeDamage(float damage, float time)
    {
        base.TakeDamage(damage, time);

        if (_currentHp <= maxHp * _hpToCopy && !_wasCopied && _currentHp > 0)
        {
            foreach (var p in _pointToCopy)
            {
                CopyOfControlling copy = Instantiate(_copy, p.position, Quaternion.identity).GetComponent<CopyOfControlling>();
                copy.boss = gameObject;
            }

            _wasCopied = true;
            gameObject.SetActive(false);
        }
    }

    public virtual void LasersAttack()
    {
        for (int i = 0; i < _numOfLasers; i++)
        {
            float x = Random.Range(-_areaRadius, _areaRadius);
            float y = Mathf.Sqrt(_areaRadius * _areaRadius - x * x);
            y = Random.Range(0, 2) == 0 ? -y : y;

            Instantiate(_laserTrace, new Vector2(x, y), Quaternion.identity);
        }
    }

    protected virtual Vector2 GeneratePos()
    {
        Transform point = _points[Random.Range(0, _points.Count - 1)];
        _randomDistance = Random.Range(-_radius, _radius);

        return new Vector2(_randomDistance + point.position.x, _randomDistance + point.position.y);
    }

    protected virtual IEnumerator SetCell(GameObject _zone, Vector2 pos)
    {
        GameObject zone = Instantiate(_zone, pos, Quaternion.identity);

        yield return new WaitForSeconds(_timeBtwZoneActivation);
        zone.GetComponent<SpriteRenderer>().color = Color.white;

        if (zone.TryGetComponent(out Collider2D collider2D))
            collider2D.enabled = true;

        if (zone.TryGetComponent(out Animator anim))
            anim.enabled = true;

        yield return new WaitForSeconds(_timeOfCell);

        if (zone != null)
            Destroy(zone.gameObject);
    }

    protected virtual void Cell()
    {
        if (target != null)
            StartCoroutine(SetCell(_zoneOfCell, target.position));
    }

    protected virtual void CellsAndExplosions(GameObject _zone)
    {
        if (target != null)
            StartCoroutine(SetCell(_zone, target.position));

        for (int i = 0; i < _countOfCells; i++)
            StartCoroutine(SetCell(_zone, GeneratePos()));
    }

    bool _isLaser;
    float _timeOfLaser;
    protected virtual void LaserAttack()
    {
        _time = _timeOfLaser = _numOfLaserRotations * _timeOfRotation;
        _isLaser = true;

        _laser.transform.localScale = new Vector2(0.3f, 0.3f);
        _laser.SetActive(true);
        _laser.transform.DOScaleX(_laser.transform.localScale.x * _laserScale, _timeOfLaser);

        StartCoroutine(Rotation());

        _agent.speed *= _laserSpeedDebuff;
    }

    protected IEnumerator Rotation()
    {
        float plus = _offsetPlus;
        for (int i = 0; i < _numOfLaserRotations; i++)
        {
            Vector3 difference = target.position - _transform.position;
            float roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            _transform.rotation = Quaternion.Euler(0f, 0f, roatZ + _offset);

            _laser.transform.localRotation = Quaternion.Euler(0f, 0f, _offsetPlus * i * Mathf.Pow(-1, i));
            Quaternion rot = Quaternion.Euler(0f, 0f, plus * Mathf.Pow(-1, i + 1));
            _laser.transform.DOLocalRotateQuaternion(rot, _timeOfRotation);

            yield return new WaitForSeconds(_timeOfRotation);
            plus = _offsetPlus * (i + 1);
        }
    }

    private void Awake() => StaticValues.WasPrizeGotten = false;

    protected override void Update()
    {
        _time -= Time.deltaTime;
        _currentTime -= Time.deltaTime;

        if (_isLaser)
        {
            _agent.SetDestination(target.position);

            _timeOfLaser -= Time.deltaTime;
            if (_timeOfLaser <= 0)
            {
                _agent.speed /= _laserSpeedDebuff;
                _laser.SetActive(false);
                _isLaser = false;
            }
        }

        if (isCharmed)
            _debuffs.FindEnemy();
    }

    protected override void FixedUpdate()
    {
        var distanceBtwPlayer = target.position - _transform.position;

        if (_currentTime <= 0 && distanceBtwPlayer.sqrMagnitude <= _attackDistance * _attackDistance && !_isLaser)
        {
            _animator.Play("MeleeAttack");
            _currentTime = _timeBtwMelleeAttack;
        }

        if ((_currentPos - (Vector2)_transform.position).sqrMagnitude > _randomDistance * _randomDistance && !_isLaser)
            _agent.SetDestination(_currentPos);
        else if (!_isLaser)
            FindPoint();
        
        if (_time <= 0)
        {
            _animator.Play(_animNames[_indexOfAttack]);

            if (_time <= 0)
                _time = _reloadTime * Random.Range(0.6f, 1);
            else
                _time += _reloadTime * Random.Range(0.6f, 1);

            _indexOfAttack = _indexOfAttack > _animNames.Count - 2 ? 0 : _indexOfAttack + 1;
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach(x => Destroy(x.gameObject));
        GameObject.FindGameObjectsWithTag(tag).ToList().ForEach(x => Destroy(x.gameObject));

        if (_currentHp <= 0)
        {
            FindObjectOfType<SpawnPrize>().GivePrize();
            StaticValues.WasPrizeGotten = true;
        }
    }
}
