using UnityEngine;

public class LeachCard : Card
{
    [SerializeField] private float _lifeSteal, _mxHpBuff;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Leach \n +{(_lifeSteal - 1) * 100}% Lifesteal \n +{(_mxHpBuff - 1) * 100}% HP";
    }

    public void GivePrize()
    {
        SetAttackParam(1, _lifeSteal, 1, 1);
        StaticValues.PlayerObj.ChangeMxHp(_mxHpBuff);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
