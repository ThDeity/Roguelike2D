using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(NavMeshAgent))]
public class HomingBullet : MonoBehaviour
{
    public float damage, critChance, lifeTime;
    protected NavMeshAgent _agent;
    private Transform _target;
    public int timeTakingDmg;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
        _agent = GetComponent<NavMeshAgent>();
        _target = StaticValues.PlayerTransform;
    }

    protected virtual void Update()
    {
        if (_target != null)
        {
            if (_agent.isOnNavMesh)
                _agent.SetDestination(_target.position);

            Quaternion rotation = Quaternion.LookRotation(_target.position - transform.position, transform.TransformDirection(Vector3.up));
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        collision.TryGetComponent(out IDamagable currentEnemy);
        if (currentEnemy != null && collision.tag != transform.tag)
        {
            damage *= Random.Range(0, 100) <= critChance ? 2 : 1;
            currentEnemy.TakeDamage(damage, timeTakingDmg);
        }

        if (currentEnemy == null || collision.tag != tag)
            Destroy(gameObject);
    }
}
