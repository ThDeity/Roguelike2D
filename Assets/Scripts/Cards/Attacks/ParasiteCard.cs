using UnityEngine;

public class ParasiteCard : Card
{
    [SerializeField] private float _lifeSteal, _mxHpBuff, _dmgBuff, _cdDebuff, _timeOfTakingDmg;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Parasite\n +{_lifeSteal * 100}% Lifesteal\n +{(_mxHpBuff - 1) * 100}% HP\n +{(_dmgBuff - 1) * 100}% DMG\n -{(_cdDebuff - 1) * 100}% Reload";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Паразит\n +{_lifeSteal * 100}% Кражи здоровья\n +{(_mxHpBuff - 1) * 100}% ХП\n +{(_dmgBuff - 1) * 100}% ДМГ\n -{(_cdDebuff - 1) * 100}% Перез-ки";
    }

    public override void GivePrize()
    {
        base.GivePrize();

        SetAttackParam(_dmgBuff,_lifeSteal,1,_cdDebuff, _timeOfTakingDmg);
        StaticValues.PlayerObj.ChangeMxHp(_mxHpBuff);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
