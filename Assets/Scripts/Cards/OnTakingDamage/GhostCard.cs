using UnityEngine;

public class GhostCard : Card
{
    [SerializeField] private Color _color;
    [SerializeField] private float _interval, _timeOf, _buff;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Ghost \n {_timeOf}s Time \n {_interval}s Interval";
    }

    public void GivePrize()
    {
        if (!StaticValues.PlayerObj.TryGetComponent(out Ghost component))
        {
            component = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<Ghost>();

            component.timeOf = _timeOf;
            component.interval = _interval;
            component.color = _color;
        }
        else
        {
            component.timeOf *= _buff;
            component.interval *= 2 - _buff;
        }

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
