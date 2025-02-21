using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Explosion : MonoBehaviour
{
    public float damage, radius, time;
    public GameObject explosionObj;

    public void Shockwave()
    {
        if (explosionObj != null)
            Instantiate(explosionObj, transform);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.attachedRigidbody != null && collider.tag != tag)
            {
                Vector3 forceVector = -(transform.position - collider.transform.position).normalized;
                forceVector.z = 0;

                float distance = Vector2.Distance(collider.transform.position, transform.position);
                float a = (radius - distance)/ distance;

                collider.TryGetComponent(out IDamagable component);
                if (component != null)
                    component.TakeDamage(damage, 0);

                collider.TryGetComponent(out NavMeshAgent agent);
                if (agent != null)
                    agent.Warp(forceVector * a);
                else
                    collider.attachedRigidbody.AddForce(forceVector * a * time);
                    //collider.attachedRigidbody.MovePosition(forceVector * a);
            }
        }
    }

    public void Boom(int forceSign = -1)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.attachedRigidbody != null && collider.tag != tag)
            {
                Vector3 forceVector = forceSign * (transform.position - collider.transform.position);
                forceVector.z = 0;
                forceVector = forceVector == Vector3.zero ? new Vector2(1, 1) : forceVector.normalized;
                
                collider.TryGetComponent(out IDamagable component);
                if (component != null)
                    component.TakeDamage(damage, 0);

                float distance = Vector2.Distance(collider.transform.position, transform.position);
                distance = distance == 0 ? 0.01f : distance;

                float force = Mathf.Pow(radius + forceSign * distance, 2) * collider.attachedRigidbody.mass / (time * time * 2);
                collider.attachedRigidbody.AddForce(forceVector.normalized * force);
            }
        }
    }

    public virtual void SelfDestroy() => Destroy(gameObject);

    protected virtual void OnTriggerEnter2D(Collider2D collision) => Instantiate(explosionObj,transform.position, Quaternion.identity);

    protected virtual void OnCollisionEnter2D(Collision2D collision) => Instantiate(explosionObj, transform.position, Quaternion.identity);

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}