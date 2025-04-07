using UnityEngine;

public class Tentacles : Enemy
{
    [SerializeField] private Transform _tentacle1, _tentacle2, _point;
    [SerializeField] private float _secondOffset;

    bool _isAttack;
    protected override void Start()
    {
        base.Start();

        _secondOffset = Random.Range(_offset - 15, _offset + 15);
    }

    protected override void Update()
    {
        _time -= Time.deltaTime;

        if (_rotateToObj != null && !_isAttack)
        {
            Vector3 difference = _rotateToObj.position - _transform.position;
            float roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            _tentacle1.rotation = Quaternion.Euler(0f, 0f, roatZ + _offset);
            _tentacle2.rotation = Quaternion.Euler(0f, 0f, roatZ + _secondOffset);
        }

        if (isCharmed)
            _debuffs.FindEnemy();
    }

    protected override void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(_point.position, _point.right, _attackDistance, LayerMask.GetMask("Default"));
        RaycastHit2D hit2 = Physics2D.Raycast(_point.position, _point.right, _attackDistance, LayerMask.GetMask("Walls"));

        if (hit.collider == null && hit2.collider == null && (target.position - _transform.position).magnitude <= _attackDistance)
        {
            if (_time <= 0)
            {
                _animator.Play("Attack");
                _time = _reloadTime;
            }
        }
        else
        {
            if (hit.collider != null)
                Debug.Log(hit.collider);

            if (hit2.collider != null)
                Debug.Log(hit2.collider);
        }
    }

    public void Attack() => _isAttack = !_isAttack;
}
