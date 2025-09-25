using System.Collections;
using UnityEngine;

public class ShieldForEnemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected ValueSystem _bar = new ValueSystem();
    [SerializeField] private float _hpBonus;
    private float _currentHp, _maxHp;

    protected bool _isTakingDmg, _isLifesteal;
    protected float _damageTaking, _timeTaking, _lifestealToPlayer;
    public void TakeDamage(float damage, float time, bool isLifesteal, float lifesteal)
    {
        if (time == 0)
        {
            _currentHp -= damage;
            _bar.RemoveValue(damage);

            if (isLifesteal)
                StaticValues.PlayerObj.TakeDamage(-damage * lifesteal, 0, false, 0);

            if (_currentHp <= 0)
                Destroy(gameObject);
        }
        else
        {
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
    }

    protected IEnumerator TakingDamage(float time)
    {
        yield return new WaitForSeconds(time);

        _isTakingDmg = _isLifesteal = false;
        _damageTaking = _lifestealToPlayer = 0;
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
            TakeDamage(_damageTaking / _timeTaking * Time.deltaTime, 0, _isLifesteal, _lifestealToPlayer);
    }
}
