using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossOfSpeed : Enemy
{
    [SerializeField] private float _timeOfCircle, _hpToSleep, _timeOfSleep, _radius, _dashTime, _dashSpeed, _timeBtwDashes, _timeBtwWawes, _timeBtwMelleeAttack, _trailTime, _trailDmg;
    [SerializeField] private int _indexOfAttack, _numOfRolls, _numOfBullets, _numOfWawes;
    [SerializeField] private GameObject _zoneOfCircle, _bullet, _fireTrail;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [Tooltip("Называть в той последовательности, в которой будет атака")]
    [SerializeField] private List<string> _animNames;
    [SerializeField] private Color _sleepColor;

    private List<FrozenStatue> _statues;
    private Rigidbody2D _rigidbody2D;
    private float _currentTime;

    bool _wasSleeping, _isSleeping, _isRolling;
    public override void TakeDamage(float damage, float time)
    {
        base.TakeDamage(damage, time);

        if (_currentHp <= maxHp * _hpToSleep && !_wasSleeping)
        {
            _wasSleeping = _isSleeping = true;

            if (gameObject != null)
                StartCoroutine(Sleep());
        }
    }

    private IEnumerator Sleep()
    {
        _spriteRenderer.color = _sleepColor;
        _animator.enabled = false;
        _agent.enabled = false;

        yield return new WaitForSeconds(_timeOfSleep);
        _isSleeping = false;
        _animator.enabled = true;
        _agent.enabled = true;

        _spriteRenderer.color = Color.white;
    }

    protected virtual Vector2 GeneratePos()
    {
        Transform point = _points[Random.Range(0, _points.Count - 1)];
        _randomDistance = Random.Range(-_radius, _radius);

        return new Vector2(_randomDistance + point.position.x, _randomDistance + point.position.y);
    }

    protected virtual IEnumerator SetCell(GameObject _zone)
    {
        if (_agent.isActiveAndEnabled)
            _agent.isStopped = true;
        GameObject zone = Instantiate(_zone, _transform);

        yield return new WaitForSeconds(_timeOfCircle);
        if (_agent.isActiveAndEnabled)
            _agent.isStopped = false;

        if (zone.TryGetComponent(out Collider2D collider2D))
            collider2D.enabled = true;

        if (zone.TryGetComponent(out Animator anim))
            anim.enabled = true;

        yield return new WaitForSeconds(1);

        if (zone != null)
            Destroy(zone.gameObject);
    }

    protected virtual void Cell()
    {
        if (target != null)
            StartCoroutine(SetCell(_zoneOfCircle));
    }

    private float time;
    private Vector2 _start, _end;
    private IEnumerator DashCoroutine()
    {
        if(target == null) yield break;

        _isRolling = true;

        for (int i = 0; i < _numOfRolls; i++)
        {
            _agent.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("EnemyDash");

            yield return new WaitForSeconds(time);

            _isRolling = false;
            _agent.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            _end = _transform.position;

            Transform t = Instantiate(_fireTrail.gameObject, _start, Quaternion.identity).transform;
            t.tag = tag;
            t.GetComponent<TrailOfFire>().lifeTime = _trailTime;
            t.GetComponent<TrailOfFire>().damage = _trailDmg;
            t.gameObject.layer = LayerMask.NameToLayer("Enemy");
            float distance = Vector2.Distance(_start, _end);

            t.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(transform.position.y - t.position.y, transform.position.x - t.position.x) * Mathf.Rad2Deg - 90);
            t.localScale = new Vector2(t.localScale.x * 3, t.localScale.y * distance);

            yield return new WaitForSeconds(_timeBtwDashes);

            if (target == null) yield break;

            _isRolling = true;
            _dashVector = (target.position - _transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(_transform.position, _dashVector, _dashSpeed * _dashTime, 0);
            Vector2 _point;
            if (hit.collider != null)
                _point = hit.point;
            else
                _point = (Vector2)_transform.position + (_dashVector * _dashSpeed * _dashTime);
            time = Vector2.Distance(_point, _transform.position) / _dashSpeed;
        }

        _agent.enabled = true;
    }

    private Vector2 _dashVector;
    public void Dash()
    {
        _start = _transform.position; 

        RaycastHit2D hit = Physics2D.Raycast(_transform.position, _dashVector, _dashSpeed * _dashTime, 0);
        Vector2 _point;
        if (hit.collider != null)
            _point = hit.point;
        else
            _point = (Vector2)_transform.position + (_dashVector * _dashSpeed * _dashTime);
        time = Vector2.Distance(_point, _transform.position) / _dashSpeed;

        StartCoroutine(DashCoroutine());
        _dashVector = (target.position - _transform.position).normalized;

        _time = _numOfRolls * (_timeBtwDashes + _dashTime);
    }

    public virtual IEnumerator Shot()
    {
        if (_agent.isActiveAndEnabled)
            _agent.isStopped = true;

        _time += _timeBtwWawes * _numOfWawes;
        _bullet.tag = tag;

        for (int j = 0; j < _numOfWawes; j++)
        {
            for (int i = 0; i < _numOfBullets; i++)
            {
                Transform bullet = Instantiate(_bullet, _transform.position, Quaternion.identity).transform;
                bullet.localEulerAngles = new Vector3(0,0,Random.Range(0, 360));
            }

            yield return new WaitForSeconds(_timeBtwWawes);
        }

        if (_agent.isActiveAndEnabled)
            _agent.isStopped = false;
    }

    public virtual IEnumerator AliveStatues()
    {
        _statues.ForEach(statue => statue.Enable());

        yield return new WaitForSeconds((_timeBtwDashes + _dashTime) * _numOfRolls + _timeBtwWawes * _numOfWawes + _timeOfCircle + 1);
        yield return new WaitForSeconds(1);

        _statues.ForEach(statue => statue.Disable());
    }

    private void Awake() => StaticValues.WasPrizeGotten = false;

    protected override void Start()
    {
        base.Start();
        _transform = transform;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _currentTime = _timeBtwMelleeAttack;

        _statues = FindObjectsOfType<FrozenStatue>().ToList();
        _statues.ForEach(statue => statue.Disable());
    }

    protected override void Update()
    {
        _time -= Time.deltaTime;
        _currentTime -= Time.deltaTime;

        if (!_isSleeping && target != null && !_isRolling)
        {
            Vector3 difference = target.position - _transform.position;
            float roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            _transform.rotation = Quaternion.Euler(0f, 0f, roatZ + _offset);

            if (_agent.isActiveAndEnabled)
                _agent.SetDestination(target.position);

            if (isCharmed)
                _debuffs.FindEnemy();
        }
    }

    protected override void FixedUpdate()
    {
        if (!_isSleeping && target != null)
        {
            var distance = _currentPos - (Vector2) _transform.position;
            var distanceBtwPlayer = target.position - _transform.position;

            if (!_isRolling && _currentTime <= 0 && distanceBtwPlayer.sqrMagnitude <= _attackDistance * _attackDistance)
            {
                _animator.Play("MeleeAttack");
                _currentTime = _timeBtwMelleeAttack;
            }
            else if (!_isRolling && distance.sqrMagnitude > _randomDistance * _randomDistance && _agent.isActiveAndEnabled)
                _agent.SetDestination(_currentPos);
            else if (!_isRolling)
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

            if (_isRolling)
                _rigidbody2D.velocity = Time.deltaTime * _dashSpeed * _transform.up;
        }
        else if (!_isSleeping && target == null && Vector2.Distance(_currentPos, _transform.position) > _randomDistance && !_isRolling)
            _agent.SetDestination(_currentPos);
        else if (!_isSleeping && !_isRolling)
            FindPoint();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _statues.ForEach(x => Destroy(x.gameObject));
        GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach(x => Destroy(x.gameObject));
        GameObject.FindGameObjectsWithTag(tag).ToList().ForEach(x => Destroy(x.gameObject));

        FindObjectOfType<SpawnPrize>().GivePrize();
        StaticValues.WasPrizeGotten = true;
    }
}