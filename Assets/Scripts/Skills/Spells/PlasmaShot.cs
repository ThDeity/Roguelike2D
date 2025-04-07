using UnityEngine;

public class PlasmaShot : Skill
{
    public float reloadTime, dmgIncrease;
    public GameObject zone;
    public Bullet bullet;

    private Transform _point;
    private Transform _zone;

    private float _currentTime;

    protected static Vector2 BulletScale;
    protected static float ReloadTime, DmgIncrease;
    public override void ResetAll()
    {
        bullet.transform.localScale = BulletScale;

        reloadTime = ReloadTime;
        dmgIncrease = DmgIncrease;
    }

    protected override void Start()
    {
        base.Start();

        if (reloadTime == 0)
        {
            BulletScale = bullet.transform.localScale;

            ReloadTime = reloadTime;
            DmgIncrease = dmgIncrease;
        }

        _point = transform.GetChild(0).transform;
        bullet.damage = StaticValues.PlayerAttackList[0].bullet.GetComponent<Bullet>().damage * dmgIncrease;
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if (Input.GetMouseButtonDown(1) && _currentTime <= 0)
        {
            _zone = Instantiate(zone, transform).transform;
            _isSkillCharged = true;
        }

        if (_isSkillCharged && Input.GetMouseButtonUp(1) && _zone != null)
        {
            _currentTime = reloadTime;
            StartCoroutine(StartTimer((int)reloadTime));

            Instantiate(bullet.gameObject, _point.position, transform.rotation);
            Destroy(_zone.gameObject);
            _isSkillCharged = false;
        }
    }

    public override void OnRollStarted()
    {
        if (_zone != null)
        {
            Destroy(_zone.gameObject);
            _isSkillCharged = false;
        }
    }
}
