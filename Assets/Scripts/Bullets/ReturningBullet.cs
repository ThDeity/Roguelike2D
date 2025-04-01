using DG.Tweening;
using UnityEngine;

public class ReturningBullet : MeleeAttack
{
    public float _timeOfReturning, _maxSpeed, _minSpeed, _timeOfStopping;
    [SerializeField] protected Vector2 _bulletVector;
    protected float _speed, _currentTime;

    protected override void Start()
    {
        base.Start();
        
        _speed = _minSpeed;
        Destroy(gameObject, _timeOfReturning * 2 + _timeOfStopping);
        DOTween.To(() => _speed, x => _speed = x, _maxSpeed, _timeOfReturning);
    }

    protected virtual void FixedUpdate() => transform.Translate(_bulletVector * _speed * Time.deltaTime);

    bool _wasStopped;
    protected virtual void Update()
    {
        _currentTime += Time.deltaTime;

        if (!_wasStopped && _currentTime >= _timeOfReturning)
        {
            _wasStopped = true;
            _speed = 0;
        }
        else if (_wasStopped && _currentTime > _timeOfStopping + _timeOfReturning)
            _speed = -1 * _maxSpeed;
    }
}
