using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Implode : MonoBehaviour
{
    public float damage, radius, force;
    List<Collider2D> colliders = new List<Collider2D>();

    public void ImplodeForce()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, radius).ToList();

        for (int i = 0; i < colliders.Count; i++)
        {
            Collider2D collider = colliders[i];

            if (collider.attachedRigidbody != null)
            {
                Vector3 forceVector = (transform.position - collider.transform.position).normalized;
                forceVector.z = 0;

                collider.TryGetComponent(out IDamagable component);
                if (component != null && collider.tag != "Player")
                {
                    component.TakeDamage(damage, 0);
                    collider.GetComponent<NavMeshAgent>().Warp(transform.position);
                }
                else
                    collider.attachedRigidbody.MovePosition(transform.position + forceVector * force);
            }
        }
    }

    public void SelfDestroy() => Destroy(gameObject);

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
