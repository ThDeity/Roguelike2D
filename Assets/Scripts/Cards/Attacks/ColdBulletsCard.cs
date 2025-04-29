using UnityEngine;

public class ColdBulletsCard : Card
{
    [SerializeField] private float _cdDebuff, _increaseTimeAndForce, _speedReduce, _time, _cd;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Cold Bullets \n -{(_cdDebuff - 1) * 100}% Reload \n +{(1 - _speedReduce) * 100}% Speed Reduce";
    }

    public void GiveOrize()
    {
        StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out ColdBullets component);
        if (component.time == 0)
        {
            component.speedReduce = _speedReduce;
            component.time = _time;
            component.cd = _cd;
        }
        else
        {
            component.speedReduce *= _increaseTimeAndForce;
            component.time *= _increaseTimeAndForce;
        }

        SetAttackParam(1, 0, 1, _cdDebuff);
        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
