using UnityEngine;

public class SteadyShot : Card
{
    [Tooltip("��������, ��������� ����� ����, �� � �� ������ 1")]
    [SerializeField] private float _buffSpeed, _debuffCd, _buffMxHp, _buffMaxDistance;

    public void GivePrize()
    {
        StaticValues.PlayerObj.ChangeMxHp(_buffMxHp);
        SetAttackParam(1, 0, _buffSpeed, _debuffCd, 0, _buffMaxDistance);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
