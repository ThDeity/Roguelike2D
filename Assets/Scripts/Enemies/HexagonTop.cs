using UnityEngine;

public class HexagonTop : Enemy
{
    public bool _isAttack;

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
}
