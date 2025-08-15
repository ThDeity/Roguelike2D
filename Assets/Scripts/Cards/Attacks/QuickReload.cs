using UnityEngine;

public class QuickReload : Card
{
    [Tooltip("Кд умножается на бафф")]
    [SerializeField] private float _cdBuff;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Quick Reload \n +{(1 - _cdBuff) * 100}% Reload";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Быстрая Перезарядка \n +{(1 - _cdBuff) * 100}% Перез-ка";
    }

    public void GivePrize()
    {
        SetAttackParam(1, 0, 1, _cdBuff, 0);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
