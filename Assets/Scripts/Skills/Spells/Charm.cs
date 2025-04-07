using UnityEngine;

public class Charm : Skill
{
    public CharmSector zone;
    public float reloadTime, timeOfCharming, increaseParam;
    public int maxEnemies;

    private float _currentTime;
    private CharmSector _zone;

    protected static Vector2 ZoneScale;
    protected static float ReloadTime, TimeOfCharming, IncreaseParam;
    protected int MaxEnemies;
    public override void ResetAll()
    {
        zone.transform.localScale = ZoneScale;

        reloadTime = ReloadTime;
        timeOfCharming = TimeOfCharming;
        increaseParam = IncreaseParam;
        maxEnemies = MaxEnemies;
    }

    protected override void Start()
    {
        base.Start();

        if (MaxEnemies == 0)
        {
            ZoneScale = zone.transform.localScale;
            ReloadTime = reloadTime;
            TimeOfCharming = timeOfCharming;
            IncreaseParam = increaseParam;
            MaxEnemies = maxEnemies;
        }
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0 && Input.GetMouseButtonDown(1))
        {
            _zone = Instantiate(zone.gameObject, transform).GetComponent<CharmSector>();
            _isSkillCharged = true;
        }

        if (_isSkillCharged && Input.GetMouseButtonUp(1))
        {
            int enemies = 0;
            foreach(var effect in _zone.effects)
            {
                if (enemies < maxEnemies)
                    effect.Charming(timeOfCharming, increaseParam);

                enemies++;
            }

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
