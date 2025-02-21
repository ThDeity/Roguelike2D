using UnityEngine;

[RequireComponent (typeof(Collider2D), typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float damage, critChance, lifeTime, speed, lifeSteal, maxDistance, lifeTimeRange;
    [SerializeField] protected Vector2 _bulletVector;
    protected Rigidbody2D _rigidbody2D;
    public int timeTakingDmg;
    public bool isDrillAmmo;

    protected virtual void Start()
    {
        lifeTime = maxDistance / speed;
        lifeTime += Random.Range(-lifeTimeRange, lifeTimeRange);
        _rigidbody2D = GetComponent<Rigidbody2D>();

        Destroy(gameObject, lifeTime);
    }

    protected virtual void FixedUpdate() => transform.Translate(_bulletVector * speed * Time.deltaTime);

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        collision.TryGetComponent(out IDamagable currentEnemy);
        collision.TryGetComponent(out Bullet bullet);
        if (currentEnemy != null && collision.tag != transform.tag)
        {
            damage *= Random.Range(0, 100) <= critChance ? 2 : 1;
            currentEnemy.TakeDamage(damage, timeTakingDmg);
            StaticValues.PlayerObj.TakeDamage(-damage * lifeSteal, timeTakingDmg);
        }

        if (bullet == null)
        {
            if (currentEnemy == null)
                Destroy(gameObject);
            else if (!isDrillAmmo && currentEnemy != null)
                Destroy(gameObject);
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, _bulletVector * maxDistance);
    }
}
