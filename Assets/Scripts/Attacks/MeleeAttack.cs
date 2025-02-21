using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] protected float _damage;
    [SerializeField] protected int _critChance;

    protected void Start()
    {
        if (transform.tag == "Enemy")
        {
            _damage = _damage * StaticValues.EnemyDamage;
            _critChance = (int)Mathf.Round((float)_critChance * StaticValues.EnemyCrit);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable enemy) && collision.tag != tag)
        {
            float damage = Random.Range(0, 100) <= _critChance ? _damage * 2 : _damage;
            enemy.TakeDamage(damage, 0);
        }
    }
}
