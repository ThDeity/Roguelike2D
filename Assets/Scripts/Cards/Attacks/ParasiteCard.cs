using UnityEngine;

public class ParasiteCard : Card
{
    [SerializeField] private float _lifeSteal, _mxHpBuff, _dmgBuff, _cdDebuff;
    [SerializeField] private int _timeOfTakingDmg;

    public void GivePrize()
    {
        SetAttackParam(_dmgBuff,_lifeSteal,1,_cdDebuff, _timeOfTakingDmg);
        StaticValues.PlayerObj.ChangeMxHp(_mxHpBuff);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
