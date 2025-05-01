using UnityEngine;

public class PlasmaShotCard : Skill
{
    [SerializeField] private float _reloadTime, _dmgIncrease, _improveSkill, _improveCd;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private GameObject _zone;

    public override void GivePrize(Sprite skillSlot)
    {
        base.GivePrize(skillSlot);

        if (StaticValues.PlayerObj.TryGetComponent(out PlasmaShot spell))
        {
            spell.dmgIncrease *= _improveSkill;
            spell.reloadTime *= _improveCd;
            spell.zone.transform.localScale *= _improveSkill;
            spell.bullet.transform.localScale *= _improveSkill;

            return;
        }
        else if (StaticValues.PlayerObj.TryGetComponent(out Skill component))
        {
            component.ResetAll();
            Destroy(component);
        }

        PlasmaShot shot = StaticValues.PlayerObj.gameObject.AddComponent<PlasmaShot>();
        shot.dmgIncrease = _dmgIncrease;
        shot.reloadTime = _reloadTime;
        shot.bullet = _bullet;
        shot.zone = _zone;
        shot.skillSprite = skillSlot;
    }
}
