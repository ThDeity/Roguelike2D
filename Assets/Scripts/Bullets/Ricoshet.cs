using UnityEngine;

public class Ricoshet : Bullet
{
    public int riccochetsCount;
    public float damageIncreasePerBounce;
    private Collider2D _collider2D;

    float _startDmg;
    protected override void Start()
    {
        base.Start();
        _startDmg = damage;
        _bulletVector = -transform.right;
        _collider2D = GetComponent<Collider2D>();
    }

    protected override void FixedUpdate()
    {
        if (isActiveAndEnabled)
            _rigidbody2D.velocity = _bulletVector * speed;
    }

    public override void Reset()
    {
        base.Reset();
        _collider2D.isTrigger = true;
    }

    private void OnDisable() => Reset();

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.isTrigger) return;

        collision.transform.TryGetComponent(out IDamagable currentEnemy);
        collision.transform.TryGetComponent(out Bullet bullet);
        if (currentEnemy != null && collision.transform.tag != transform.tag && bullet == null)
        {
            damage *= Random.Range(0, 100) < critChance ? 2 : 1;
            currentEnemy.TakeDamage(damage, timeTakingDmg);
            StaticValues.PlayerObj.TakeDamage(-damage * lifeSteal, timeTakingDmg);
        }

        if (currentEnemy != null && isDrillAmmo)
        {
            _collider2D.isTrigger = true;
            return;
        }

        if (bullet == null)
        {
            if (riccochetsCount > 0)
                _bulletVector = Vector2.Reflect(_bulletVector.normalized, collision.contacts[0].normal).normalized;
            else if (currentEnemy == null || (!isDrillAmmo && currentEnemy != null))
                Destroy(gameObject);

            riccochetsCount--;

            if (damageIncreasePerBounce > 0)
                damage += _startDmg * damageIncreasePerBounce;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (isActiveAndEnabled)
            _collider2D.isTrigger = false;
    }
}
