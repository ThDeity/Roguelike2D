using UnityEngine;

public class PoisonCard : Card
{
    [SerializeField] private float _dmgBuff, _cdDebuff, _timeOfTakingDmg;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Poison \n +{(_dmgBuff - 1) * 100}% DMG\n -{(_cdDebuff - 1) * 100}% Reload\n {_timeOfTakingDmg}s Time of taking dmg";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"яд \n +{(_dmgBuff - 1) * 100}% ƒћ√\n -{(_cdDebuff - 1) * 100}% ѕерез-ка\n {_timeOfTakingDmg}с ƒлительность отравлени€";
    }

    public override void GivePrize()
    {
        base.GivePrize();

        SetAttackParam(_dmgBuff,0,1,_cdDebuff,_timeOfTakingDmg);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
