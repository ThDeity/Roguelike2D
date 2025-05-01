using DG.Tweening;
using UnityEngine;

public class Circle : Enemy
{
    [SerializeField] protected GameObject _explosion;
    [SerializeField] protected float _damage, _lifeTime;
    [SerializeField] protected Color _color;
    [SerializeField] protected Bomb _bomb;

    protected SpriteRenderer _spriteRenderer;

    protected override void Start()
    {
        base.Start();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.DOColor(_color, _lifeTime);

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
        exp.GetComponent<Explosion>().damage = _damage;
        exp.tag = tag;

        Destroy(gameObject);
    }

    public override void TakeDamage(float damage, float time)
    {
        Explosion();
        Destroy(gameObject);
    }
}
