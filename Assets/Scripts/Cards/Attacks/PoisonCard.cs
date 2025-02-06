using UnityEngine;

public class PoisonCard : Card
{
    [SerializeField] private float _dmgBuff, _cdDebuff;
    [SerializeField] private int _timeOfTakingDmg;

    public void GivePrize()
    {
        SetAttackParam(_dmgBuff,0,1,_cdDebuff,_timeOfTakingDmg);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
