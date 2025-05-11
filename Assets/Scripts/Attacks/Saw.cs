using UnityEngine;

public class Saw : MonoBehaviour
{
    public float damage, intervalBtwDmg, lifeTime;
    private float _currentTime = 0;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);

        if (StaticValues.PlayerTransform.localScale.x > 1)
            transform.localScale *= StaticValues.PlayerTransform.localScale.x;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable enemy) && collision.tag != tag && !collision.isTrigger)
        {
            if (_currentTime <= 0)
            {
                enemy.TakeDamage(damage, 0);
                _currentTime = intervalBtwDmg;
            }

            _currentTime -= Time.deltaTime;
        }
    }
}
