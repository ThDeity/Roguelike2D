using UnityEngine;

[RequireComponent (typeof(Collider2D), typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float damage, critChance, lifeTime, speed, lifeSteal, maxDistance, lifeTimeRange, timeTakingDmg;
    [SerializeField] protected Vector2 _bulletVector;
    protected Rigidbody2D _rigidbody2D;
    public bool isDrillAmmo;

    protected static float Damage, Crit, LifeTime, Speed, LifeSteal, MaxDistance, TimeTakingDmg;
    protected static Vector2 BulletVector, BulletScale;
    public void Reset()
    {
        damage = Damage;
        critChance = Crit;
        lifeTime = LifeTime;
        speed = Speed;
        lifeSteal = LifeSteal;
        maxDistance = MaxDistance;
        timeTakingDmg = TimeTakingDmg;

        _bulletVector = BulletVector;
        transform.localScale = BulletScale;

        isDrillAmmo = false;
    }

    protected virtual void Awake()
    {
        if (tag == "Player" && Damage == 0)
        {
            Damage = damage;
            Crit = critChance;
            LifeTime = lifeTime;
            Speed = speed;
            LifeSteal = lifeSteal;
            MaxDistance = maxDistance;
            TimeTakingDmg = timeTakingDmg;

            BulletVector = _bulletVector;
            BulletScale = transform.localScale;
        }
    }

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

            if (tag == "Player")
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
}
