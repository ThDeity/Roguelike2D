using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private float _timeOfDazzle, _dmg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != tag && collision.TryGetComponent(out DebuffsEffects component))
        {
            component.Dazzle(_timeOfDazzle);
            collision.GetComponent<IDamagable>().TakeDamage(_dmg, 0);
        }
    }
}
