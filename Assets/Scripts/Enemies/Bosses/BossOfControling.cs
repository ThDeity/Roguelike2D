using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BossOfControling : Enemy
{
    [SerializeField] private float _timeBtwZoneActivation, _timeOfCell, _laserScale, _laserSpeedDebuff, _timeOfRotation, _offsetPlus, _hpToCopy, _radius;
    [SerializeField] private int _countOfCells, indexOfAttack, _numOfLaserRotations;
    [SerializeField] private GameObject _zoneOfCell, _laser, _copy;
    [Tooltip("Называть в той последовательности, в которой будет атака")]
    [SerializeField] private List<string> _animNames;
    [SerializeField] private List<Transform> _pointToCopy;

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

    protected override void Update()
    {
        _time -= Time.deltaTime;

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
        if (Vector2.Distance(_currentPos, _transform.position) > _randomDistance && !_isLaser)
            _agent.SetDestination(_currentPos);
        else if (!_isLaser)
            FindPoint();
        
        if (_time <= 0)
        {
            _animator.Play(_animNames[indexOfAttack]);

            if (_time <= 0)
                _time = _reloadTime * Random.Range(0.6f, 1);
            else
                _time += _reloadTime * Random.Range(0.6f, 1);

            indexOfAttack = indexOfAttack > _animNames.Count - 2 ? 0 : indexOfAttack + 1;
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        FindObjectOfType<SpawnPrize>().GivePrize();
        StaticValues.WasPrizeGotten = true;
    }
}
