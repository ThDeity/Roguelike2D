using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FrozenBurst : MonoBehaviour
{
    public float interval, damage, radius, timeOfFreezing, force;
    public GameObject burst;

    private float _currentTime;
    private void Start() => _currentTime = interval;

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0)
        {
            GameObject burstObj = Instantiate(burst, transform);
            List<Collider2D> enemies = Physics2D.OverlapCircleAll(transform.position, radius, gameObject.layer).ToList();

            foreach (Collider2D enemy in enemies)
            {
                if (enemy.TryGetComponent(out DebuffsEffects effect) && enemy.tag != transform.tag)
                    effect.Freezing(timeOfFreezing, force, damage);
            }

            _currentTime = interval;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
