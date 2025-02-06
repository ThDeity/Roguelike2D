using UnityEngine;

public class PerfectionismCard : Card
{
    [Tooltip("Урон умножается 1, кд прибавляется")]
    [SerializeField] private float _buffDmg, _debuffCd;

    public void GivePrize()
    {
        SetAttackParam(_buffDmg, 1, 1, _debuffCd);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
