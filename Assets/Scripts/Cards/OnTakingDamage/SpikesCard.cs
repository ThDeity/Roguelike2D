using UnityEngine;

public class SpikesCard : Card
{
    [SerializeField] private Bullet _spike;
    [SerializeField] private int _spikesCount, _spikesCountPlus;
    [SerializeField] private float _buff, _interval, _radius;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Spikes \n {_interval}s Interval \n +{_spikesCount} Spikes";
    }

    public void GivePrize()
    {
        if (!StaticValues.PlayerObj.TryGetComponent(out Spikes component))
        {
            component = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<Spikes>();

            component.spikesCount = _spikesCount;
            component.interval = _interval;
            component.spike = _spike;
            component.radius = _radius;
        }
        else
        {
            component.spikesCount += _spikesCountPlus;
            component.interval *= 2 - _buff;

            component.spike.damage *= _buff;
            component.spike.critChance *= _buff;
            component.spike.speed *= _buff;
            component.spike.maxDistance *= _buff;
            component.spike.lifeSteal += _buff;
        }

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}