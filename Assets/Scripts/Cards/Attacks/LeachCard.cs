using UnityEngine;

public class LeachCard : Card
{
    [SerializeField] private float _lifeSteal, _mxHpBuff;

    public void GivePrize()
    {
        SetAttackParam(1, _lifeSteal, 1, 1);
        StaticValues.PlayerObj.ChangeMxHp(_mxHpBuff);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
