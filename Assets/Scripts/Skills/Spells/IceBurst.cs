using UnityEngine;

public class IceBurst : Skill
{
    public GameObject zone, effects;
    public float reloadTime, freezingTime, radius, force, damage;

    private float _currentTime;
    private Transform _zone;

    protected static Vector2 ZoneScale;
    protected static float ReloadTime, FreezingTime, Radius, Force, Damage;
    public override void ResetAll()
    {
        zone.transform.localScale = ZoneScale;

        reloadTime = ReloadTime;
        freezingTime = FreezingTime;
        radius = Radius;
        force = Force;
        damage = Damage;
    }

    public void Improve(float value)
    {
        freezingTime *= value;
        radius *= value;
        force *= value;
        damage *= value;

        zone.transform.localScale *= value;
    }

    protected override void Start()
    {
        base.Start();
        zone.transform.localScale = Vector2.one * 2 * radius;

        if (ReloadTime == 0)
        {
            ZoneScale = zone.transform.localScale;
            ReloadTime = reloadTime;
            FreezingTime = freezingTime;
            Radius = radius;
            Force = force;
            Damage = damage;
        }    
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if (Input.GetMouseButtonDown(1) && _currentTime <= 0 && _zone == null)
        {
            _zone = Instantiate(zone, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity).transform;
            _isSkillCharged = true;
        }

        if (_zone != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _zone.position = new Vector3(pos.x, pos.y, 0);
        }

        if (_isSkillCharged && Input.GetMouseButtonUp(1) && _currentTime <= 0)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_zone.position, radius);
            Instantiate(effects, _zone.position, effects.transform.rotation);

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out DebuffsEffects effect))
                    effect.Freezing(freezingTime, force, damage, true);
            }

            StartCoroutine(StartTimer((int) reloadTime));
            _currentTime = reloadTime;

            _isSkillCharged = false;
            Destroy(_zone.gameObject);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}