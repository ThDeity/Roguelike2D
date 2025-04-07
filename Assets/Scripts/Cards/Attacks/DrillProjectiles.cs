using UnityEngine;

public class DrillProjectiles : Card
{
    protected override void Start()
    {
        base.Start();
        _description.text = $"Drill Bullets \n Bullets go through enemies";
    }

    public void GivePrize()
    {
        StaticValues.PlayerAttackList.ForEach(x => x.bullet.GetComponent<Bullet>().isDrillAmmo = true);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
