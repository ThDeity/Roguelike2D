using UnityEngine;

public class FrozenBurstCard : Card
{
    [SerializeField] private float _interval, _freezingTime, _damage, _radius, _force, _increaseParam;
    [SerializeField] private GameObject _burst;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Frozen Burst \n +{_interval}s Interval \n +{_radius}m Radius \n +{_damage}% DMG";
    }

    public void GivePrize()
    {
        if (!StaticValues.PlayerObj.TryGetComponent(out FrozenBurst burst))
        {
            burst = StaticValues.PlayerObj.gameObject.AddComponent<FrozenBurst>();
            
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
