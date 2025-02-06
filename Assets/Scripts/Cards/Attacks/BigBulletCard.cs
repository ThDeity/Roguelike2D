using UnityEngine;

public class BigBulletCard : Card
{
    [SerializeField] private float _cdDebuff, _sizeBuff;

    public void GivePrize()
    {
        SetAttackParam(1,0,1,_cdDebuff,0,1, _sizeBuff);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
