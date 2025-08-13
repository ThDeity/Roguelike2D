using UnityEngine;

public class ExplosionZone : MonoBehaviour
{
    [Tooltip("1 ����� ������� �������")]
    [SerializeField] private float _firstDmg, _secondDmg;
    [SerializeField] private int _timeOfTakingDmg;
    [SerializeField] private SaveZone _saveZone;

    private void Start()
    {
        if (tag == "Enemy")
        {
            _firstDmg *= StaticValues.EnemyDamage;
            _secondDmg *= StaticValues.EnemyDamage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_saveZone != null && _saveZone.collisions.Contains(collision))
            return;

        if (collision.tag != tag && collision.TryGetComponent(out IDamagable component))
        {
            component.TakeDamage(_firstDmg, 0, false, 0);
            component.TakeDamage(_secondDmg, _timeOfTakingDmg, false, 0);
        }
    }

    public void SelfDestroy()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
}
