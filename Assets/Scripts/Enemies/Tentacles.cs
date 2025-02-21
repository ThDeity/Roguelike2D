using UnityEngine;

public class Tentacles : Enemy
{
    [SerializeField] private Transform _tentacle1, _tentacle2;
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

    public void Attack() => _isAttack = !_isAttack;
}
