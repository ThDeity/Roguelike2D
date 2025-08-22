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

    [Tooltip("�������� � ��� ������������������, � ������� ����� �����")]
    [SerializeField] private List<string> _animNames;

    [SerializeField] AudioClip _laserSound, _lasersUnready, _cellSound;

    private float _currentTime;
    bool _wasCopied;
    public override void TakeDamage(float damage, float time, bool isLifesteal, float lifesteal)
    {
        base.TakeDamage(damage, time, isLifesteal, lifesteal);

        if (_currentHp <= maxHp * _hpToCopy && !_wasCopied && _currentHp > 0)
        {
            EffectsSource.clip = _laserSound;
            EffectsSource.loop = true;
            EffectsSource.Play();

            StopAllCoroutines();
            if (_isLaser)
            {
                DOTween.KillAll();
                _agent.speed /= _laserSpeedDebuff;
            }
            _laser.SetActive(false);
            _isLaser = false;
            _laser.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach(x => { if (x != gameObject && !x.transform.IsChildOf(transform) && !x.TryGetComponent(out MeleeAttack component)) Destroy(x.gameObject); });
            GameObject.FindGameObjectsWithTag(tag).ToList().ForEach(x => { if (x != gameObject && !x.transform.IsChildOf(transform) && !x.TryGetComponent(out MeleeAttack component)) Destroy(x.gameObject); });

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
        EffectsSource.PlayOneShot(_lasersUnready);

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
        if (_points == null || _points.Count == 0 || _points[0] == null)
        {
            GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
            StaticValues.EnemiesPoint.Clear();
            foreach (GameObject p in points)
                StaticValues.EnemiesPoint.Add(p.transform);
        }

        Transform point = _points[Random.Range(0, _points.Count)];
        _randomDistance = Random.Range(-_radius, _radius);

        return new Vector2(_randomDistance + point.position.x, _randomDistance + point.position.y);
    }

    protected virtual IEnumerator SetCell(GameObject _zone, Vector2 pos)
    {
        GameObject zone = Instantiate(_zone, pos, Quaternion.identity);
        EffectsSource.PlayOneShot(_cellSound);

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

        EffectsSource.clip = _laserSound;
        EffectsSource.loop = true;
        EffectsSource.Play();

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

    private void Awake()
    {
        if (EffectsSource == null)
            EffectsSource = GameObject.FindGameObjectWithTag("Effects").GetComponent<AudioSource>();

        StaticValues.WasPrizeGotten = false;
        StaticValues.CurrentRoomType = "Boss";
        _currentTime = _timeBtwMelleeAttack;

        _currentHp = maxHp;
    }

    protected void OnEnable()
    {
        if (StaticValues.EnemyMaxHp <= 0) return;

        if (EffectsSource.clip == _laserSound)
        {
            EffectsSource.loop = false;
            EffectsSource.clip = null;
        }

        StaticValues.WasPrizeGotten = false;
        StaticValues.CurrentRoomType = "Boss";
        base.Start();

        if (_wasCopied)
            TakeDamage(0.5f * _currentHp, 0, false, 0);
    }

    protected override void Update()
    {
        _time -= Time.deltaTime;
        _currentTime -= Time.deltaTime;

        if (_isTakingDmg && isActiveAndEnabled)
            TakeDamage(_damageTaking / _timeTaking * Time.deltaTime, 0, _isLifesteal, _lifestealToPlayer);

        if (_isLaser && _agent.isActiveAndEnabled && _agent.isOnNavMesh)
        {
            _agent.SetDestination(target.position);

            _timeOfLaser -= Time.deltaTime;
            if (_timeOfLaser <= 0)
            {
                _agent.speed /= _laserSpeedDebuff;
                _laser.SetActive(false);
                _isLaser = false;

                EffectsSource.loop = false;
                EffectsSource.clip = null;
            }
        }

        if (!isCharmed)
        {
            _time -= Time.deltaTime;
            _currentTime -= Time.deltaTime;
        }
        else
        {
            Debug.Log("ehehehe");
            _agent.isStopped = true;
        }
    }

    protected override void FixedUpdate()
    {
        if (!isCharmed)
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
        GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach(x => Destroy(x.gameObject));
        GameObject.FindGameObjectsWithTag(tag).ToList().ForEach(x => Destroy(x.gameObject));

        if (_currentHp <= 0)
        {
            FindObjectOfType<SpawnPrize>().GivePrize();
            StaticValues.WasPrizeGotten = true;
        }
    }
}
