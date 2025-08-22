using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private float _timeOfDazzle, _dmg;
    [SerializeField] private AudioClip _trapSound;

    private void Start()
    {
        if (tag == "Enemy")
            _dmg *= StaticValues.EnemyDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != tag && collision.TryGetComponent(out DebuffsEffects component))
        {
            if (Enemy.EffectsSource != null)
                Enemy.EffectsSource.PlayOneShot(_trapSound);

            component.Dazzle(_timeOfDazzle);
            collision.GetComponent<IDamagable>().TakeDamage(_dmg, 0, false, 0);
        }
    }
}
