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
    public bool isCharmed;

    protected float _damageTaking;
    protected int _timeTaking;
    protected GameObject _hpBar;
    public virtual void TakeDamage(float damage, int time)
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
            _damageTaking = damage;
            _timeTaking = time;
            StartCoroutine(nameof(TakingDamage));
        }

        if (!_isPlayerNear)
        {
            _isPlayerNear = true;
            _rotateToObj = target;
        }

        if (!_hpBar.activeInHierarchy)
            _hpBar.SetActive(true);
    }

    private IEnumerator TakingDamage()
    {
        for (int i = 0; i < _timeTaking; i++)
        {
            float damage = _damageTaking / _timeTaking;
            TakeDamage(damage, 0);
            yield return new WaitForSeconds(1);
        }
    }

    protected void OnDestroy() => SpawnEnemies.Enemies.Remove(gameObject);

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
        _randomDistance = Random.Range(0.5f, Vector2.Distance(_currentPos, _transform.position) * 0.7f);
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

        target = Player.PlayerObj.transform;
        _points = StaticValues.EnemiesPoint;
        FindPoint();

        _hpBar = transform.GetComponentInChildren<Canvas>().gameObject;
        _hpBar.SetActive(false);

        maxHp *= StaticValues.EnemyMaxHp;
        _currentHp = maxHp;
        _agent.speed *= StaticValues.EnemySpeed;
    }

    protected virtual void Update()
    {
        _time -= Time.deltaTime;
        
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
            _agent.SetDestination(target.position);
        }
        else if (_time <= 0)
        {
            _agent.isStopped = true;
            _animator.Play("Attack");
            _time = _reloadTime;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }
}
