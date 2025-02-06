using UnityEngine;

public class Plasma : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        collision.TryGetComponent(out IDamagable currentEnemy);
        if (currentEnemy != null && collision.tag != transform.tag)
        {
            damage *= Random.Range(0, 100) <= critChance ? 2 : 1;
            currentEnemy.TakeDamage(damage, timeTakingDmg);
            StaticValues.PlayerObj.TakeDamage(-damage * lifeSteal, timeTakingDmg);
        }
    }
}
