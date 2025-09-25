using UnityEngine;

public class DrillProjectiles : Card
{
    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Drill Bullets\n Bullets go through enemies";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Сквозные Пули\n Пули проходят сквозь врагов";
    }

    public override void GivePrize()
    {
        base.GivePrize();

        StaticValues.PlayerAttackList.ForEach(x => x.bullet.GetComponent<Bullet>().isDrillAmmo = true);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
