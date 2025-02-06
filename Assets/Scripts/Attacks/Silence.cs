using UnityEngine;

public class Silence : MonoBehaviour
{
    public float radius, timeOfSilence;

    public void Start()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        Bullet bullet = StaticValues.PlayerAttackList[0].bullet.GetComponent<Bullet>();

        foreach (Collider2D collider in colliders)
        {
            collider.TryGetComponent(out DebuffsEffects component);
            if (component != null && collider.tag != "Player")
                component.Silence(timeOfSilence);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void SelfDestroy() => Destroy(gameObject);
}
