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
            if (!collision.CompareTag(tag) && _currentTime <= 0)
            {
                component.TakeDamage(damage, timeOfTakingDmg);
                _currentTime = timeBtwDamage;
            }

            _currentTime -= Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable component))
        {
            if (!collision.CompareTag(tag))
                component.TakeDamage(damage * 0.5f, timeOfTakingDmg);
        }
    }
}
