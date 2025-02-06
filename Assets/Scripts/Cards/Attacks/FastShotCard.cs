using UnityEngine;

public class FastShotCard : Card
{
    [Tooltip("Скорость и кд больше 1")]
    [SerializeField] private float _buffSpeed, _debuffCd, _maxDistanceBuff;

    public void GivePrize()
    {
        SetAttackParam(1,1,_buffSpeed, _debuffCd,0, _maxDistanceBuff);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
