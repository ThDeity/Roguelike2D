using UnityEngine;

public class GlassCanonCard : Card
{
    [Tooltip("Урон больше 1, хп меньше")]
    [SerializeField] private float _buffDmg, _debufHp;

    public void GivePrize()
    {
        SetAttackParam(_buffDmg, 0, 1, 1);

        StaticValues.PlayerObj.ChangeMxHp(_debufHp);
        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
