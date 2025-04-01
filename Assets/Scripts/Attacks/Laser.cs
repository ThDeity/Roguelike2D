using UnityEngine;

public class Laser : MonoBehaviour
{
    public float damage, timeBtwDamage;
    private float _currentTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable component))
        {
            if (!collision.CompareTag(tag) && _currentTime <= 0)
            {
                component.TakeDamage(damage, 0);
                _currentTime = timeBtwDamage;
            }

            _currentTime -= Time.deltaTime;
        }
    }
}
