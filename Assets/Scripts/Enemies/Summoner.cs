using System.Collections.Generic;
using UnityEngine;

public class Summoner : Enemy
{
    [SerializeField] private Enemy _bomb;
    [SerializeField] private List<Transform> _pointsToSummoning;
    [SerializeField] private float _cdToTeleport;

    private float _currentCd;

    public void Summon()
    {
        for (int i = 0; i < _pointsToSummoning.Count; i++)
        {
            Enemy bomb = Instantiate(_bomb, _pointsToSummoning[i].position, Quaternion.identity);

            if (isCharmed)
            {
                bomb.target = target;
                bomb.tag = tag;
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        _currentCd = _cdToTeleport;
    }

    protected override void Update()
    {
        _time -= Time.deltaTime;
        _currentCd -= Time.deltaTime;

        if (target != null)
        {
            Vector3 difference = target.position - _transform.position;
            float roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            _transform.rotation = Quaternion.Euler(0f, 0f, roatZ + _offset);
        }
    }

    protected override void FixedUpdate()
    {
        if (_currentCd <= 0)
        {
            FindPoint();
            _transform.position = new Vector2(_currentPos.x - _randomDistance, _currentPos.y - _randomDistance);

            _currentCd = _cdToTeleport;
        }
        else if (_time <= 0)
        {
            _animator.Play("Attack");
            _time = _reloadTime;
        }
    }
}
