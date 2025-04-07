using UnityEngine;

public class PoisonCard : Card
{
    [SerializeField] private float _dmgBuff, _cdDebuff;
    [SerializeField] private int _timeOfTakingDmg;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Poison \n +{(_dmgBuff - 1) * 100}% DMG\n -{(_cdDebuff - 1) * 100}% Reload\n {_timeOfTakingDmg}s Time of taking dmg";
    }

    public void GivePrize()
    {
        SetAttackParam(_dmgBuff,0,1,_cdDebuff,_timeOfTakingDmg);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
