using UnityEngine;

public class ExplosionZone : MonoBehaviour
{
    [Tooltip("1 будет сильнее второго")]
    [SerializeField] private float _firstDmg, _secondDmg;
    [SerializeField] private int _timeOfTakingDmg;
    [SerializeField] private SaveZone _saveZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != tag && collision.TryGetComponent(out IDamagable component) && !_saveZone.collisions.Contains(collision))
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
