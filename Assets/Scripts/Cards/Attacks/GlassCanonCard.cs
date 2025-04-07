using UnityEngine;

public class GlassCanonCard : Card
{
    [Tooltip("Урон больше 1, хп меньше")]
    [SerializeField] private float _buffDmg, _debufHp;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Glass Canon \n +{(_buffDmg - 1) * 100}% DMG \n -{(1 - _debufHp) * 100}% HP";
    }

    public void GivePrize()
    {
        SetAttackParam(_buffDmg, 0, 1, 1);

        StaticValues.PlayerObj.ChangeMxHp(_debufHp);
        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
