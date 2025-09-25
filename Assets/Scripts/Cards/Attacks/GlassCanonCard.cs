using UnityEngine;

public class GlassCanonCard : Card
{
    [Tooltip("Урон больше 1, хп меньше")]
    [SerializeField] private float _buffDmg, _debufHp;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Glass Canon\n +{(_buffDmg - 1) * 100}% DMG\n -{(1 - _debufHp) * 100}% HP";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Стеклянная Пушка\n +{(_buffDmg - 1) * 100}% ДМГ\n -{(1 - _debufHp) * 100}% ХП";
    }

    public override void GivePrize()
    {
        base.GivePrize();

        SetAttackParam(_buffDmg, 0, 1, 1);

        StaticValues.PlayerObj.ChangeMxHp(_debufHp);
        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
