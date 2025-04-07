using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ballista : MonoBehaviour, IDamagable
{
    public float maxHp, radius, reloadTime, offset, lifeTime, persentOfDmg;
    public Bullet bullet;

    [SerializeField] private Transform _point;
    private float _currentHp, _currentCd;

    private Animator _animator;

    protected static float MaxHp, Radius, ReloadTime, Offset, LifeTime, PersentOfDmg;
    protected static Bullet Projectile;
    protected static Transform Point;
    private void Start()
    {
        if (MaxHp == 0)
        {
            MaxHp = maxHp;
            Radius = radius;
            ReloadTime = reloadTime;
            Offset = offset;
            LifeTime = lifeTime;
            PersentOfDmg = persentOfDmg;

            Projectile = bullet;

            Point = _point;
        }

        _animator = GetComponent<Animator>();
        _currentHp = maxHp;
        Destroy(gameObject, lifeTime);
        bullet.damage = StaticValues.PlayerAttackList[0].bullet.GetComponent<Bullet>().damage * persentOfDmg;
    }

    private bool _isTakingDmg;
    protected float _damageTaking, _timeTaking;
    public void TakeDamage(float damage, float time)
    {
        if (time == 0)
        {
            _currentHp -= damage;

            if (_currentHp <= 0)
                Destroy(gameObject);
        }
        else
        {
            _isTakingDmg = true;
            _damageTaking = damage;
            _timeTaking = time;

            StartCoroutine(TakingDamage(time));
        }
    }

    private IEnumerator TakingDamage(float time)
    {
        yield return new WaitForSeconds(time);

        _isTakingDmg = false;
        _damageTaking = 0;
    }

    private Enemy _currentEnemy;
    private List<Enemy> _enemyList = new List<Enemy>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
            _enemyList.Add(enemy);

        if (_currentEnemy == null)
            FindEnemy();
    }

    public void Reset()
    {
        maxHp = MaxHp;
        radius = Radius;
        reloadTime = ReloadTime;
        offset = Offset;
        lifeTime = LifeTime;
        persentOfDmg = PersentOfDmg;

        bullet = Projectile;

        _point = Point;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_currentEnemy == collision)
            _currentEnemy = null;

        FindEnemy();
    }

    private void FindEnemy()
    {
        float _minDistance = float.MaxValue;

        foreach (Enemy enemy in _enemyList)
        {
            if (enemy != null)
            {
                if (Vector2.Distance(transform.position, enemy.transform.position) < _minDistance)
                {
                    _currentEnemy = enemy;
                    _minDistance = Vector2.Distance(transform.position,enemy.transform.position);
                }
            }
        }
    }

    private void Update()
    {
        _currentCd -= Time.deltaTime;

        if (_isTakingDmg)
            TakeDamage(_damageTaking / _timeTaking * Time.deltaTime, 0);

        if (_currentEnemy != null)
        {   
            Vector3 difference = _currentEnemy.transform.position - transform.position;
            float roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, roatZ + offset);

            if (_currentCd <= 0)
            {
                _currentCd = reloadTime;
                _animator.Play("BallistaAttack");
            }
        }
    }

    public void Shot() => Instantiate(bullet.gameObject, _point.position, transform.rotation);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}