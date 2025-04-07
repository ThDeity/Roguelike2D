using UnityEngine;

public class FastShotCard : Card
{
    [Tooltip("Скорость и кд больше 1")]
    [SerializeField] private float _buffSpeed, _debuffCd, _maxDistanceBuff;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Fast Shot \n +{(_buffSpeed - 1) * 100}% Bullet's speed \n -{(_debuffCd - 1) * 100}% Reload \n +{(_maxDistanceBuff - 1) * 100}% Max distance";
    }

    public void GivePrize()
    {
        SetAttackParam(1,1,_buffSpeed, _debuffCd,0, _maxDistanceBuff);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
