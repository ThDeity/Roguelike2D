using UnityEngine;

public class Triangle : Enemy
{
    [SerializeField] protected float _specialReload;
    [SerializeField] protected Transform _pointOfRay;
    protected float _specialTime;

    protected override void Start()
    {
        Physics2D.queriesHitTriggers = false;
        base.Start();
        _specialTime = _specialReload;
    }

    protected override void Update()
    {
        base.Update();
        _specialTime -= Time.deltaTime;
    }

    protected override void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(_pointOfRay.position, -_transform.right, _attackDistance);

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
        else if (hit.collider != null && hit.collider.transform == target)
        {
            _agent.isStopped = true;

            if (_specialTime <= 0)
            {
                _animator.Play("SpecialAttack");
                _specialTime = _specialReload;
            }
            else if (_time <= 0)
            {
                _animator.Play("Attack");
                _time = _reloadTime;
            }
        }
    }
}
