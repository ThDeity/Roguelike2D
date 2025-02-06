using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ballista : MonoBehaviour, IDamagable
{
    public float maxHp, radius, reloadTime, offset, lifeTime;
    public Bullet bullet;

    [SerializeField] private Transform _point;
    private float _currentHp, _currentCd;

    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _currentHp = maxHp;
        Destroy(gameObject, lifeTime);
    }

    private float _damageTaking, _timeTaking;
    public void TakeDamage(float damage, int time)
    {
        if (time == 0)
        {
            _currentHp -= damage;

            if (_currentHp <= 0)
                Destroy(gameObject);
            else if (_currentHp > maxHp)
                _currentHp = maxHp;
        }
        else
        {
            _damageTaking = damage;
            _timeTaking = time;
            StartCoroutine(TakingDamage());

            _damageTaking = 0;
            _timeTaking = 0;
        }
    }

    private IEnumerator TakingDamage()
    {
        for (int i = 0; i < (int)_timeTaking; i++)
        {
            float damage = _damageTaking / _timeTaking;
            TakeDamage(damage, 0);
            yield return new WaitForSeconds(1);
        }
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