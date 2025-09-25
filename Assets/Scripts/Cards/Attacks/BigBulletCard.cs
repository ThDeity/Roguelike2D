using UnityEngine;

public class BigBulletCard : Card
{
    [SerializeField] private float _cdDebuff, _sizeBuff;

    protected override void Start()
    {
        base.Start();
        
        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Big Bullet \n -{(_cdDebuff - 1) * 100}% Reload \n +{(_sizeBuff - 1) * 100} Size";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Большие Пули \n -{(_cdDebuff - 1) * 100}% Перез-ка \n +{(_sizeBuff - 1) * 100} Размер";
    }

    public override void GivePrize()
    {
        base.GivePrize();

        SetAttackParam(1,0,1,_cdDebuff,0,1, _sizeBuff);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
