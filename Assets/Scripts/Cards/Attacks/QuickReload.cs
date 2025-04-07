using UnityEngine;

public class QuickReload : Card
{
    [Tooltip("Кд умножается на бафф")]
    [SerializeField] private float _cdBuff;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Quick Reload \n +{(1 - _cdBuff) * 100}% Reload";
    }

    public void GivePrize()
    {
        SetAttackParam(1, 0, 1, _cdBuff, 0);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
