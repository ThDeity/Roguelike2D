using DG.Tweening;
using UnityEngine;

public class Circle : Enemy
{
    [SerializeField] protected GameObject _explosion;
    [SerializeField] protected float _damage, _lifeTime;
    [SerializeField] protected Color _color;

    protected SpriteRenderer _spriteRenderer;
    protected float _currentTime;

    protected override void Start()
    {
        base.Start();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.DOColor(_color, _lifeTime);

        _currentTime = _lifeTime;
        _damage *= StaticValues.EnemyDamage;
    }

    protected override void Update()
    {
        base.Update();

        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0)
            Explosion();
    }

    protected void Explosion()
    {
        GameObject exp = Instantiate(_explosion, _transform.position, _transform.rotation);
        exp.GetComponent<Explosion>().damage = _damage;

        exp.tag = tag;
        exp.layer = gameObject.layer;

        Destroy(gameObject);
    }

    public override void TakeDamage(float damage, float time)
    {
        Explosion();
        Destroy(gameObject);
    }
}
