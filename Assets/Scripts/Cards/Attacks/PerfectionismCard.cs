using UnityEngine;

public class PerfectionismCard : Card
{
    [Tooltip("���� ���������� 1, �� ������������")]
    [SerializeField] private float _buffDmg, _debuffCd;

    public void GivePrize()
    {
        SetAttackParam(_buffDmg, 1, 1, _debuffCd);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
