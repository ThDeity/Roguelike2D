using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Square : Enemy
{
    [SerializeField] protected float _dashForce, _dashCd, _speed, _dashRange;
    protected Rigidbody2D _rigidbody2D;
    protected float _dashCdValue;

    protected override void Start()
    {
        base.Start();
        _dashCdValue = _dashCd;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected override void FixedUpdate()
    {
        _dashCd -= Time.deltaTime;

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
            _rigidbody2D.AddForce(_transform.right * _dashForce * Time.deltaTime, ForceMode2D.Impulse);

        if (Vector2.Distance(target.transform.position, _transform.position) >= _dashRange && _dashCd <= 0)
        {
            _animator.Play("Dash");
            _dashCd = _dashCdValue;
            _agent.speed = 0;
            _agent.enabled = false;
        }
        else if (Vector2.Distance(target.transform.position, _transform.position) > _attackDistance && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            _agent.enabled = true;
            _agent.speed = _speed;
            _agent.SetDestination(target.transform.position);
        }
        else if (_time <= 0 && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            _agent.speed = 0;
            _agent.enabled = false;
            _animator.Play("Attack");
            _time = _reloadTime;
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _dashRange);
    }
}
