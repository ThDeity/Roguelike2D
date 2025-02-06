using UnityEngine;

public class RadarShot : MonoBehaviour
{
    public float radius, damageBoost;
    public int maxEnemiesCount;

    public void Start()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        Bullet bullet = StaticValues.PlayerAttackList[0].bullet.GetComponent<Bullet>();

        foreach (Collider2D collider in colliders)
        {
            collider.TryGetComponent(out IDamagable component);
            if (component != null && collider.tag != "Player" && maxEnemiesCount > 0)
            {
                component.TakeDamage(damageBoost * bullet.damage, bullet.timeTakingDmg);
                maxEnemiesCount--;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void SelfDestroy() => Destroy(gameObject);
}
