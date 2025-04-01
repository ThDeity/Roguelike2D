using System.Collections;
using UnityEngine;

public class ShieldForEnemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected ValueSystem _bar = new ValueSystem();
    [SerializeField] private float _hpBonus;
    private float _currentHp, _maxHp;

    protected bool _isTakingDmg;
    protected float _damageTaking, _timeTaking;
    public void TakeDamage(float damage, float time)
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
            _isTakingDmg = true;
            _damageTaking = damage;
            _timeTaking = time;

            StartCoroutine(TakingDamage(time));
        }
    }

    protected IEnumerator TakingDamage(float time)
    {
        yield return new WaitForSeconds(time);

        _isTakingDmg = false;
        _damageTaking = 0;
    }

    private void OnDestroy()
    {
        if (transform.GetComponentInParent<Collider2D>() != null)
            GetComponentInParent<Collider2D>().enabled = true;
    }

    private void Start()
    {
        _currentHp = _maxHp = transform.GetComponentInParent<Enemy>().maxHp * _hpBonus;
        transform.tag = transform.parent.tag;
        _bar.Setup(_maxHp);
    }

    private void Update()
    {
        if (_isTakingDmg)
            TakeDamage(_damageTaking / _timeTaking * Time.deltaTime, 0);
    }
}
