using UnityEngine;

public class Sliced : Enemy
{
    [SerializeField] private float _timeOfShield;

    private void FindClosestEnemy()
    {
        float min = float.MaxValue;

        foreach (var enemy in SpawnEnemies.Enemies)
        {
            float distance = Vector2.Distance(enemy.transform.position, transform.position);

            if (distance < min && enemy != gameObject)
            {
                min = distance;
                target = enemy.transform;
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        FindClosestEnemy();
    }

    protected override void FixedUpdate()
    {
        if (_time <= 0)
        {
            _agent.isStopped = true;
            _animator.Play("Attack");
            _time = _reloadTime;
        }
        else if (Vector2.Distance(_currentPos, _transform.position) > _randomDistance)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_currentPos);
        }
        else
            FindPoint();
    }

    public void SetShields()
    {
        foreach (var enemy in SpawnEnemies.Enemies)
        {
            if (enemy != null && enemy != gameObject)
                enemy.GetComponent<DebuffsEffects>().Shield(_timeOfShield);
        }
    }
}
