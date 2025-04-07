using UnityEngine;

public class PerfectionismCard : Card
{
    [Tooltip("Урон умножается 1, кд прибавляется")]
    [SerializeField] private float _buffDmg, _debuffCd;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Perfectionism \n +{(_buffDmg - 1) * 100}% DMG\n -{(_debuffCd - 1) * 100}% Reload";
    }

    public void GivePrize()
    {
        SetAttackParam(_buffDmg, 1, 1, _debuffCd);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
