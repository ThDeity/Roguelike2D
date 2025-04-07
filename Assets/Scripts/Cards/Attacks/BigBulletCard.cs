using UnityEngine;

public class BigBulletCard : Card
{
    [SerializeField] private float _cdDebuff, _sizeBuff;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Big Bullet \n -{(_cdDebuff - 1) * 100}% Reload \n +{(_sizeBuff - 1) * 100} Size";
    }

    public void GivePrize()
    {
        SetAttackParam(1,0,1,_cdDebuff,0,1, _sizeBuff);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
