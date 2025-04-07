using UnityEngine;

public class HexagonTop : Enemy
{
    public bool _isAttack;
    [SerializeField] private Transform _point;

    protected override void Start()
    {
        Physics2D.queriesHitTriggers = false;
        base.Start();
    }

    protected override void Update()
    {
        _time -= Time.deltaTime;

        if (_rotateToObj != null && !_isAttack)
        {
            Vector3 difference = _rotateToObj.position - _transform.position;
            float roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            _transform.rotation = Quaternion.Euler(0f, 0f, roatZ + _offset);
        }

        if (isCharmed)
            _debuffs.FindEnemy();
    }

    protected override void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(_point.position, _transform.right, _attackDistance, LayerMask.GetMask("Default"));
        RaycastHit2D hit2 = Physics2D.Raycast(_point.position, _transform.right, _attackDistance, LayerMask.GetMask("Walls"));

        if (!_isPlayerNear && Vector2.Distance(_currentPos, _transform.position) > _randomDistance)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_currentPos);
        }
        else if (!_isPlayerNear || target == null)
            FindPoint();
        else if (Vector2.Distance(target.position, _transform.position) > _attackDistance || hit.collider != null || hit2.collider != null)
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
