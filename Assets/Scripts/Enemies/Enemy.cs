using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected float _attackDistance, _offset, _reloadTime;
    [SerializeField] protected ValueSystem _bar = new ValueSystem();
    public float maxHp;

    protected List<Transform> _points = new List<Transform>();
    protected float _time, _currentHp;
    protected DebuffsEffects _debuffs;
    protected Transform _transform;
    protected Vector2 _currentPos;
    protected NavMeshAgent _agent;
    protected Animator _animator;
    protected bool _isPlayerNear;

    public Transform target;
    public bool isCharmed, isBoss;

    protected float _damageTaking, _timeTaking;
    protected GameObject _hpBar;

    protected bool _isTakingDmg;
    public virtual void TakeDamage(float damage, float time)
    {
        if (time == 0)
        {
            _currentHp -= damage;
            _bar.RemoveValue(damage);

            if (_currentHp <= 0)
                Destroy(gameObject);
            else if (_currentHp > maxHp)
                _currentHp = maxHp;
        }
        else
        {
            _isTakingDmg = true;
            _damageTaking = damage;
            _timeTaking = time;

            StartCoroutine(TakingDamage(time));
        }

        if (!_isPlayerNear)
        {
            _isPlayerNear = true;
            _rotateToObj = target;
        }

        if (_hpBar != null && !_hpBar.activeInHierarchy)
            _hpBar.SetActive(true);
    }

    protected IEnumerator TakingDamage(float time)
    {
        yield return new WaitForSeconds(time);

        _isTakingDmg = false;
        _damageTaking = 0;
    }

    protected virtual void OnDestroy() => SpawnEnemies.Enemies.Remove(gameObject);

    public virtual float ChangeReloadCd(float change)
    {
        _reloadTime *= change;
        return _reloadTime;
    }

    protected float _randomDistance;
    protected Transform _rotateToObj;
    protected virtual void FindPoint()
    {
        _rotateToObj = _points[Random.Range(0, _points.Count - 1)];
        _currentPos = _rotateToObj.position;
        _randomDistance = Random.Range(0.5f, Vector2.Distance(_currentPos, _transform.position));
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == target)
        {
            _isPlayerNear = true;
            _rotateToObj = other.transform;
        }
    }

    protected virtual void Start()
    {
        _debuffs = GetComponent<DebuffsEffects>();
        _time = _reloadTime;
        _transform = transform;
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _bar.Setup(maxHp);

        target = StaticValues.PlayerTransform;
        _points = StaticValues.EnemiesPoint;
        FindPoint();

        if (!isBoss && _hpBar == null)
        {
            _hpBar = transform.GetComponentInChildren<Canvas>().gameObject;
            _hpBar.SetActive(false);
        }

        maxHp *= StaticValues.EnemyMaxHp;
        _currentHp = maxHp;
        _agent.speed *= StaticValues.EnemySpeed;
    }

    protected virtual void Update()
    {
        _time -= Time.deltaTime;

        if (_isTakingDmg)
            TakeDamage(_damageTaking / _timeTaking * Time.deltaTime, 0);

        if (_rotateToObj != null)
        {
            Vector3 difference = _rotateToObj.position - _transform.position;
            float roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            _transform.rotation = Quaternion.Euler(0f, 0f, roatZ + _offset);
        }

        if (isCharmed)
            _debuffs.FindEnemy();
    }

    protected virtual void FixedUpdate()
    {
        if (!_isPlayerNear && Vector2.Distance(_currentPos, _transform.position) > _randomDistance)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_currentPos);
        }
        else if (!_isPlayerNear || target == null)
            FindPoint();
        else if (target != null && Vector2.Distance(target.position, _transform.position) > _attackDistance)
        {
            _agent.isStopped = false;
            _agent.destination = target.position;
        }
        else if (_time <= 0)
        {
            _agent.isStopped = true;
            _animator.Play("Attack");
            _time = _reloadTime;
        }
        else
        {
            _agent.isStopped = false;
            _agent.destination = target.position;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }
}
