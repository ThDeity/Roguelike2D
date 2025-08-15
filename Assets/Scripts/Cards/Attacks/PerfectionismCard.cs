using UnityEngine;

public class PerfectionismCard : Card
{
    [Tooltip("Урон умножается 1, кд прибавляется")]
    [SerializeField] private float _buffDmg, _debuffCd;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Perfectionism \n +{(_buffDmg - 1) * 100}% DMG\n -{(_debuffCd - 1) * 100}% Reload";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Перфекционист \n +{(_buffDmg - 1) * 100}% ДМГ\n -{(_debuffCd - 1) * 100}% Перез-ка";
    }

    public void GivePrize()
    {
        SetAttackParam(_buffDmg, 1, 1, _debuffCd);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
