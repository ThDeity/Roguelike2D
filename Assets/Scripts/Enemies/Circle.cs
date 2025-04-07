using UnityEngine;

public class Circle : Enemy
{
    [SerializeField] protected GameObject _explosion;
    [SerializeField] protected float _damage;
    [SerializeField] protected Bomb _bomb;

    protected override void Start()
    {
        base.Start();

        _damage *= StaticValues.EnemyDamage;
    }

    protected override void Update()
    {
        base.Update();

        if (_bomb.isExplosion)
            Explosion();
    }

    protected void Explosion()
    {
        GameObject exp = Instantiate(_explosion, _transform.position, _transform.rotation);
        exp.tag = tag;
        exp.GetComponent<Explosion>().damage = _damage;
        Destroy(gameObject);
    }

    public override void TakeDamage(float damage, float time)
    {
        Explosion();
        Destroy(gameObject);
    }
}
