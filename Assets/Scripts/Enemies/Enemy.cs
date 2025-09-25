using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected float _attackDistance, _offset, _reloadTime, _damagedEffectDuration;
    [SerializeField] protected ValueSystem _bar = new ValueSystem();
    [SerializeField] protected AudioClip _attackSound, _damagedSound;
    [SerializeField] protected Color _damagedColor;
    [SerializeField] protected SpriteRenderer _spriteEnemy;

    [SerializeField] protected int _chanceFallingOut;
    [SerializeField] protected GameObject _medkit;
    public float maxHp;

    protected List<Transform> _points = new List<Transform>();
    protected SpriteRenderer _spriteRenderer;
    protected float _time, _currentHp;
    protected DebuffsEffects _debuffs;
    protected Transform _transform;
    protected Vector2 _currentPos;
    protected NavMeshAgent _agent;
    protected Animator _animator;
    protected bool _isPlayerNear;
    protected Color _realColor;

    public Transform target;
    public bool isCharmed, isBoss;

    public static AudioSource EffectsSource;

    protected float _damageTaking, _timeTaking, _lifestealToPlayer;
    protected GameObject _hpBar;

    protected bool _isTakingDmg, _isLifesteal;
    public virtual float GetCurrentHp() { return _currentHp; }

    public virtual void TakeDamage(float damage, float time, bool isLifesteal, float lifesteal)
    {
        if (time == 0)
        {
            if (!_isTakingDmg)
            {
                AudioSource.PlayClipAtPoint(_damagedSound, _transform.position);
                //EffectsSource.PlayOneShot(_damagedSound);

                DOTween.Kill(_spriteRenderer);
                _spriteRenderer.color = _damagedColor;
                _spriteRenderer.DOColor(_realColor, _damagedEffectDuration);
            }

            _currentHp -= damage;
            _bar.RemoveValue(damage);

            if (isLifesteal)
                StaticValues.PlayerObj.TakeDamage(-damage * lifesteal, 0, false, 0);

            if (_currentHp <= 0)
            {
                StopAllCoroutines();
                DOTween.KillAll();

                int chance = Random.Range(0, 101);
                if (chance < _chanceFallingOut)
                    Instantiate(_medkit, _transform.position, Quaternion.identity);

                SpawnEnemies spawn = FindObjectOfType<SpawnEnemies>();
                if (spawn != null && gameObject != null)
                    spawn.RemoveEnemy(gameObject);

                Destroy(gameObject);
            }
            else if (_currentHp > maxHp)
                _currentHp = maxHp;
        }
        else
        {
            DOTween.Kill(_spriteRenderer);
            AudioSource.PlayClipAtPoint(_damagedSound, _transform.position);
            //EffectsSource.PlayOneShot(_damagedSound);
            _spriteRenderer.color = _damagedColor;
            _spriteRenderer.DOColor(_realColor, _damagedEffectDuration);

            _isTakingDmg = true;
            _damageTaking += damage;
            _timeTaking += time;

            if (isLifesteal)
            {
                _isLifesteal = true;
                _lifestealToPlayer = lifesteal;
            }

            if (_timeTaking > 0)
                StopCoroutine(TakingDamage(time));
            else
                StartCoroutine(TakingDamage(_timeTaking));
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

        _isTakingDmg = _isLifesteal = false;
        _damageTaking = _lifestealToPlayer = 0;
    }

    public virtual float ChangeReloadCd(float change)
    {
        _reloadTime *= change;
        return _reloadTime;
    }

    protected float _randomDistance;
    protected Transform _rotateToObj;
    protected virtual void FindPoint()
    {
        if (_points.Count == 0) return;

        _rotateToObj = _points[Random.Range(0, _points.Count)];
        if (_rotateToObj != null)
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
        if (_spriteEnemy != null)
            _spriteRenderer = _spriteEnemy;

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
            _realColor = _spriteRenderer.color;

        if (EffectsSource == null)
            EffectsSource = GameObject.FindGameObjectWithTag("Effects").GetComponent<AudioSource>();

        _debuffs = GetComponent<DebuffsEffects>();
        _time = _reloadTime;
        _transform = transform;
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        if (StaticValues.PlayerMovementObj != null)
            target = StaticValues.PlayerMovementObj.transform;
        _points = StaticValues.EnemiesPoint;
        FindPoint();

        if (!isBoss && _hpBar == null)
        {
            _hpBar = transform.GetComponentInChildren<Canvas>().gameObject;
            _hpBar.SetActive(false);
        }

        maxHp *= StaticValues.EnemyMaxHp;

        _bar.Setup(maxHp);
        _currentHp = maxHp;
        _agent.speed *= StaticValues.EnemySpeed;
    }

    protected virtual void Update()
    {
        _time -= Time.deltaTime;

        if (_isTakingDmg)
            TakeDamage(_damageTaking / _timeTaking * Time.deltaTime, 0, _isLifesteal, _lifestealToPlayer);

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
        if (_agent.isOnNavMesh)
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

                if (_attackSound != null)
                    EffectsSource.PlayOneShot(_attackSound);

                _time = _reloadTime;
            }
            else
            {
                _agent.isStopped = false;
                _agent.destination = target.position;
            }
        }
    }

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();
        DOTween.KillAll();
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance);
    }
}
