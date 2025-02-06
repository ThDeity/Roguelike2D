using UnityEngine;

public class TrailOfFire : MonoBehaviour
{
    public float damage, timeBtwDamage, lifeTime;
    public int timeOfTakingDmg;
    private float _currentTime;

    private void Start() => Destroy(gameObject, lifeTime);

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable component))
        {
            if (!collision.CompareTag("Player") && _currentTime <= 0)
            {
                component.TakeDamage(damage, timeOfTakingDmg);
                _currentTime = timeBtwDamage;
            }

            _currentTime -= Time.deltaTime;
        }
    }
}
