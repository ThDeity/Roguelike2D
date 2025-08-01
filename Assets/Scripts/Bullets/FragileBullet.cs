using UnityEngine;

public class FragileBullet : Bullet
{
    public Capsule gunner;
    [SerializeField] protected float _timeOfDebuff, _coefficient;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out IDamagable currentEnemy);
        if (currentEnemy != null)
            collision.GetComponent<DebuffsEffects>().ChangeGettingDmg(_timeOfDebuff, _coefficient);

        base.OnTriggerEnter2D(collision);
    }
}
