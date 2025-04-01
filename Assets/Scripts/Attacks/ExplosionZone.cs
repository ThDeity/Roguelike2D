using UnityEngine;

public class ExplosionZone : MonoBehaviour
{
    [Tooltip("1 будет сильнее второго")]
    [SerializeField] private float _firstDmg, _secondDmg;
    [SerializeField] private int _timeOfTakingDmg;
    [SerializeField] private bool _isExplosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != tag && _isExplosion && collision.TryGetComponent(out IDamagable component))
        {
            component.TakeDamage(_firstDmg, 0);
            component.TakeDamage(_secondDmg, _timeOfTakingDmg);
        }
    }

    public void SelfDestroy()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
}
