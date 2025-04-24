using UnityEngine;

public class ParasiteCard : Card
{
    [SerializeField] private float _lifeSteal, _mxHpBuff, _dmgBuff, _cdDebuff;
    [SerializeField] private int _timeOfTakingDmg;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Parasite \n +{_lifeSteal * 100}% Lifesteal \n +{(_mxHpBuff - 1) * 100}% HP \n +{(_dmgBuff - 1) * 100}% DMG\n -{(_cdDebuff - 1) * 100}% Reload";
    }

    public void GivePrize()
    {
        SetAttackParam(_dmgBuff,_lifeSteal,1,_cdDebuff, _timeOfTakingDmg);
        StaticValues.PlayerObj.ChangeMxHp(_mxHpBuff);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
