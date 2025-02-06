using UnityEngine;

public class QuickReload : Card
{
    [Tooltip("Кд умножается на бафф")]
    [SerializeField] private float _cdBuff;

    public void GivePrize()
    {
        SetAttackParam(1, 0, 1, _cdBuff, 0);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
