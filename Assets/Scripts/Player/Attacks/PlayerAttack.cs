using UnityEngine;

public class PlayerAttack : RangeAttack
{
    protected Transform _player;
    [SerializeField] protected float _offset;

    protected static float Offset, ReloadTime;
    protected static GameObject Bullet;
    protected static Transform Point;
    public void Reset()
    {
        _offset = Offset;
        reloadTime = ReloadTime;

        bullet = Bullet;

        _point = Point;
    }

    protected virtual void Awake()
    {
        if (ReloadTime == 0)
        {
            Offset = _offset;
            ReloadTime = reloadTime;

            Bullet = bullet;

            Point = _point;
        }
    }

    protected override void Start()
    {
        _player = StaticValues.PlayerTransform;
        _point = transform.GetChild(0);
        base.Start();
    }

    public override void Shot()
    {
        if (_time <= 0)
        {
            Instantiate(bullet, _point.position, _player.rotation);

            _time = reloadTime;
        }
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseButtonDown();
    }

    public virtual void OnMouseButtonDown() => Shot();
}
