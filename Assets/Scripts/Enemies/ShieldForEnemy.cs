using System.Collections;
using UnityEngine;

public class ShieldForEnemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected ValueSystem _bar = new ValueSystem();
    [SerializeField] private float _hpBonus;
    private float _currentHp, _maxHp;

    protected float _damageTaking, _timeTaking;
    public void TakeDamage(float damage, int time)
    {
        if (time == 0)
        {
            _currentHp -= damage;
            _bar.RemoveValue(damage);

            if (_currentHp <= 0)
                Destroy(gameObject);
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

    private void OnDestroy() => GetComponentInParent<Collider2D>().enabled = true;

    private void Start()
    {
        _currentHp = _maxHp = transform.GetComponentInParent<Enemy>().maxHp * _hpBonus;
        transform.tag = transform.parent.tag;
        _bar.Setup(_maxHp);
    }
}
