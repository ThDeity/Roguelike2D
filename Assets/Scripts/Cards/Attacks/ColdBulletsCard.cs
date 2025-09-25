using UnityEngine;

public class ColdBulletsCard : Card
{
    [SerializeField] private float _cdDebuff, _increaseTimeAndForce, _speedReduce, _time, _cd;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Cold Bullets \n -{(_cdDebuff - 1) * 100}% Reload \n +{(1 - _speedReduce) * 100}% Speed Reduce";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Ледяные Пули \n -{(_cdDebuff - 1) * 100}% Перез-ка \n +{(1 - _speedReduce) * 100}% Замедления";
    }

    public override void GivePrize()
    {
        base.GivePrize();

        StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out BulletsComponents components);
        components.SetComponent(typeof(ColdBullets));

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
