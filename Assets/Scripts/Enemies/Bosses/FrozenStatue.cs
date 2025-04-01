using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RangeAttack))]
public class FrozenStatue : Enemy
{
    [SerializeField] private Transform _pointToShot;
    [SerializeField] private float _timeOfSleeping;
    [SerializeField] private Color _color;


    private RangeAttack _rangeAttack;
    private SpriteRenderer _sprite;
    private Collider2D _collider;

    public void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _transform = transform;

        if (!isBoss)
        {
            _hpBar = transform.GetComponentInChildren<Canvas>().gameObject;
            _hpBar.SetActive(false);
        }
    }

    protected override void Start()
    {
        base.Start();

        _isPlayerNear = true;
        _agent.enabled = false;

        _rangeAttack = GetComponent<RangeAttack>();
    }

    protected override void Update()
    {
        _time -= Time.deltaTime;

        if (target != null)
        {
            Vector3 difference = target.position - _transform.position;
            float roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            _pointToShot.rotation = Quaternion.Euler(0f, 0f, roatZ + _offset);
        }

        if (isCharmed)
            _debuffs.FindEnemy();
    }

    protected override void FixedUpdate()
    {
        if (!_isSleeping && _time <= 0)
        {
            _rangeAttack.ShotFromPoint();
            _time = _reloadTime;
        }
    }

    public void Disable()
    {
        _isSleeping = true;
        _sprite.enabled = false;
        _collider.enabled = false;

        _hpBar.SetActive(false);
        for (int i = 0; i < _transform.childCount; i++)
            _transform.GetChild(i).gameObject.SetActive(false);
    }

    public void Enable()
    {
        _isSleeping = false;
        _sprite.enabled = true;
        _collider.enabled = true;

        _sprite.color = Color.white;
        _bar.AddValue(maxHp);
        _currentHp = maxHp;

        for (int i = 0; i < _transform.childCount; i++)
        {
            if (_transform.GetChild(i).gameObject != _hpBar.gameObject)
                _transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    bool _isSleeping;
    public override void TakeDamage(float damage, float time)
    {
        if (_isSleeping) return;

        if (time == 0)
        {
            _currentHp -= damage;
            _bar.RemoveValue(damage);

            if (_currentHp <= 0)
            {
                _isSleeping = true;
                StartCoroutine(Sleep());
            }
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

        if (_hpBar != null && !_hpBar.activeInHierarchy)
            _hpBar.SetActive(true);
    }

    protected virtual IEnumerator Sleep()
    {
        _sprite.color = _color;
        _collider.enabled = false;
        _hpBar.SetActive(false);

        yield return new WaitForSeconds(_timeOfSleeping);

        _isSleeping = false;
        _collider.enabled = true;
        _sprite.color = Color.white;
        _bar.AddValue(maxHp);
        _currentHp = maxHp;
    }
}
