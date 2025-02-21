using UnityEngine;

public class Hook : MeleeAttack
{
    [SerializeField] private float _timeOfDazzle;
    private Transform _enemy;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != tag && collision.TryGetComponent(out IDamagable enemy))
        {
            enemy.TakeDamage(_damage, 0);
            collision.TryGetComponent(out DebuffsEffects component);
            if (component != null)
                component.Dazzle(_timeOfDazzle);

            _enemy = collision.transform;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform == _enemy)
            _enemy = null;
    }

    protected void Update()
    {
        if (_enemy != null) 
            _enemy.position = transform.position;
    }
}
