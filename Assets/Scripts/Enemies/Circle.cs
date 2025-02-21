using UnityEngine;

public class Circle : Enemy
{
    [SerializeField] protected GameObject _explosion;
    [SerializeField] protected Bomb _bomb;

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
        Destroy(gameObject);
    }

    public override void TakeDamage(float damage, int time)
    {
        Explosion();
        Destroy(gameObject);
    }
}
