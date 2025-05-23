using UnityEngine;

public class HexagonTopRange : Enemy
{
    [SerializeField] protected Transform _pointOfRay;

    protected override void Start()
    {
        Physics2D.queriesHitTriggers = false;
        base.Start();
    }
    protected override void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(_pointOfRay.position, _transform.right, _attackDistance, 0);
        RaycastHit2D hit2 = Physics2D.Raycast(_pointOfRay.position, _transform.right, _attackDistance, 9);

        if (!_isPlayerNear && Vector2.Distance(_currentPos, _transform.position) > _randomDistance)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_currentPos);
        }
        else if (!_isPlayerNear || target == null)
            FindPoint();
        else if (Vector2.Distance(target.position, _transform.position) > _attackDistance)
        {
            _agent.isStopped = false;
            _agent.SetDestination(target.position);
        }
        else if (hit.collider == null && hit2.collider == null)
        {
            _agent.isStopped = true;

            if (_time <= 0)
            {
                _animator.Play("Attack");
                _time = _reloadTime;
            }
        }
    }
}
