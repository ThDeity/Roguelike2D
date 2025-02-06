using UnityEngine;

public class PlasmaShot : Skill
{
    public float reloadTime, dmgIncrease;
    public GameObject zone;
    public Bullet bullet;

    private Transform _point;
    private Transform _zone;

    private float _currentTime;
    protected override void Start()
    {
        base.Start();

        _point = transform.GetChild(0).transform;
        bullet.damage = StaticValues.PlayerAttackList[0].bullet.GetComponent<Bullet>().damage * dmgIncrease;
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if (Input.GetMouseButtonDown(1) && _currentTime <= 0)
            _zone = Instantiate(zone, transform).transform;

        if (Input.GetMouseButtonUp(1) && _zone != null)
        {
            _currentTime = reloadTime;
            StartCoroutine(StartTimer((int)reloadTime));

            Instantiate(bullet.gameObject, _point.position, transform.rotation);
            Destroy(_zone.gameObject);
        }
    }
}
