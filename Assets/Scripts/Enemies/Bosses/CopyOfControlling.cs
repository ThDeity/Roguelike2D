using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CopyOfControlling : Enemy
{
    [SerializeField] private float _maxScale, _minScale, _minAngle, _maxAngle, _mxSpeedBuff;
    [SerializeField] private List<Transform> _lasers;

    public static List<CopyOfControlling> Copies = new List<CopyOfControlling>();
    public GameObject boss;

    private float _angle, _speed;
    private Rigidbody2D _rb;
    protected override void Start()
    {
        base.Start();
        Copies.Add(this);

        _speed = _agent.speed;
        _angle = Random.Range(_minAngle, _maxAngle);
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        _time -= Time.deltaTime;

        if (isCharmed)
            _debuffs.FindEnemy();
    }

    protected override void FindPoint()
    {
        _angle = Random.Range(_minAngle, _maxAngle);
        _currentPos = _points[Random.Range(0, _points.Count - 1)].position;

        float scale = Random.Range(_minScale, _maxScale);
        _lasers.ForEach(x => x.DOScaleX(0.3f * scale, Vector2.Distance(_currentPos, _transform.position) / _agent.speed));

        if (_speed > 0)
            _agent.speed = Random.Range(_speed, _speed * _mxSpeedBuff);
    }

    protected override void FixedUpdate()
    {
        _rb.MoveRotation(_rb.rotation + _angle * Time.fixedDeltaTime);

        if (Vector2.Distance(_currentPos, _transform.position) > _attackDistance)
            _agent.SetDestination(_currentPos);
        else
            FindPoint();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Copies.Remove(this);

        if (Copies.Count == 0 && boss != null)
            boss.SetActive(true);
    }
}
