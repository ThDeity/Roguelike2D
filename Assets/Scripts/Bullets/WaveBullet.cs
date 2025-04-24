using UnityEngine;

public class WaveBullet : MonoBehaviour
{
    public float amplitude;
    [SerializeField] private float _speed, _lifeTime, _damage, _timeOfFading, _lightDebuff;

    private void Start()
    {
        _damage *= StaticValues.EnemyDamage;
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        transform.position += transform.up * Mathf.Sin(transform.position.x) * amplitude;
        transform.position += transform.right * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger || collision.TryGetComponent(out Bullet bullet)) return;

        if (collision.tag != tag && collision.TryGetComponent(out IDamagable component))
            component.TakeDamage(_damage, 0);

        if (collision.TryGetComponent(out DebuffsEffects effects))
            effects.Fading(_timeOfFading, _lightDebuff);

        Destroy(gameObject);
    }
}