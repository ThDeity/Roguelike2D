using UnityEngine;

public class DrillProjectiles : Card
{
    public void GivePrize()
    {
        StaticValues.PlayerAttackList.ForEach(x => x.bullet.GetComponent<Bullet>().isDrillAmmo = true);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
