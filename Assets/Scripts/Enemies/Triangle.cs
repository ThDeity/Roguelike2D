using UnityEngine;

public class Triangle : Enemy
{
    [SerializeField] protected float _specialReload;
    [SerializeField] protected Transform _pointOfRay;
    protected float _specialTime;

    protected override void Start()
    {
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

        if (Vector2.Distance(target.transform.position, _transform.position) > _attackDistance || hit.collider == null || hit.collider.tag != "Player")
        {
            _agent.isStopped = false;
            _agent.SetDestination(target.transform.position);
        }
        else if (hit.collider != null && hit.collider.tag == "Player")
        {
            if (_specialTime <= 0)
            {
                _agent.isStopped = true;
                _animator.Play("SpecialAttack");
                _specialTime = _specialReload;
            }
            else if (_time <= 0)
            {
                _agent.isStopped = true;
                _animator.Play("Attack");
                _time = _reloadTime;
            }
        }
    }
}
