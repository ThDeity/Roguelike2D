using UnityEngine;

public class FrozenBurstCard : Card
{
    [SerializeField] private float _interval, _freezingTime, _damage, _radius, _force, _increaseParam;
    [SerializeField] private GameObject _burst;
    [SerializeField] private AudioClip _clip;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Frozen Burst\n +{_interval}s Interval \n +{_radius}m Radius \n +{_damage}% DMG";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Ледяной Всплеск\n +{_interval}с Интервал\n +{_radius}м Радиус\n +{_damage}% ДМГ";
    }

    public override void GivePrize()
    {
        base.GivePrize();

        if (!StaticValues.PlayerObj.TryGetComponent(out FrozenBurst burst))
        {
            burst = StaticValues.PlayerObj.gameObject.AddComponent<FrozenBurst>();

            burst.burstSound = _clip;
            burst.interval = _interval;
            burst.timeOfFreezing = _freezingTime;
            burst.radius = _radius;
            burst.force = _force;
            burst.damage = _damage;
            burst.burst = _burst;
        }
        else
        {
            burst.interval *= _increaseParam;
            burst.timeOfFreezing *= _increaseParam;
            burst.radius *= _increaseParam;
            burst.force *= _increaseParam;
            burst.damage *= _increaseParam;
            burst.burst.transform.localScale *= _increaseParam;
        }

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
