using UnityEngine;

public class BallistaSkill : Skill
{
    public GameObject zone;
    public Ballista ballista;
    public float reloadTime, hpOfPlayer;

    private float _currentTime;
    private Transform _zone;

    protected static float ReloadTime, HpOfPlayer;
    public override void ResetAll()
    {
        zone.transform.localScale = Vector2.one;
        ballista.Reset();
        reloadTime = ReloadTime;
        hpOfPlayer = HpOfPlayer;
    }

    protected override void Start()
    {
        base.Start();

        ballista.maxHp = hpOfPlayer * StaticValues.PlayerObj.ChangeMxHp(1);
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
            Instantiate(ballista.gameObject, _zone.position, Quaternion.identity);

            StartCoroutine(StartTimer((int)reloadTime));
            _currentTime = reloadTime;

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
