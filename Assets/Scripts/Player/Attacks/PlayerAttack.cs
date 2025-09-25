using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : RangeAttack
{
    public AudioClip shotSound;

    protected Transform _player;
    [SerializeField] protected float _offset;
    [SerializeField] protected Color _stickColor;
    public Joystick stick;

    protected static float Offset, ReloadTime;
    protected static AudioClip ShotSound;
    protected static GameObject Bullet;
    protected static Transform Point;
    public void Reset()
    {
        _offset = Offset;
        reloadTime = ReloadTime;

        bullet = Bullet;

        _point = Point;
        shotSound = ShotSound;
    }

    protected virtual void Awake()
    {
        stick = GameObject.FindGameObjectWithTag("Joystick").transform.GetChild(1).GetComponent<FixedJoystick>();
        if (ReloadTime == 0)
        {
            Offset = _offset;
            ReloadTime = reloadTime;

            Bullet = bullet;

            Point = _point;
            ShotSound = shotSound;
        }
    }

    protected Image _stickBackground;
    protected Color _usualColor;

    protected override void Start()
    {
        _player = StaticValues.PlayerTransform;
        _point = transform.GetChild(0);
        base.Start();

        if (stick != null)
        {
            _stickBackground = stick.GetComponent<Image>();
            _usualColor = _stickBackground.color;
        }
    }

    public override void Shot()
    {
        if (_time <= 0)
        {
            StaticValues.PlayerObj.effectsSource.PlayOneShot(shotSound);

            Instantiate(bullet, _point.position, _player.rotation);

            _time = reloadTime;
        }
    }

    public virtual void AutoShot()
    {
        if (_time > 0) return;

        if (SpawnEnemies.Enemies.Count > 0)
        {
            float distance = 100000000000000;
            Transform target = null;

            foreach (var enemy in SpawnEnemies.Enemies)
            {
                Vector2 dist = (enemy.transform.position - _player.position);

                if (dist.sqrMagnitude < distance)
                {
                    distance = dist.sqrMagnitude;
                    target = enemy.transform;
                }
            }

            if (target != null)
            {
                Vector2 difference = target.position - _player.position;

                float roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                _player.rotation = Quaternion.Euler(0f, 0f, roatZ + _offset);
            }

            OnMouseButtonDown();
        }
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseButtonDown();

        if (stick.Direction.sqrMagnitude > 0f)
        {
            _stickBackground.color = _usualColor;

            if (stick.Direction.sqrMagnitude > 0.4f)
                OnMouseButtonDown();
            else
                AutoShot();
        }
        else
            _stickBackground.color = _stickColor;
    }

    public virtual void OnMouseButtonDown() => Shot();
}
