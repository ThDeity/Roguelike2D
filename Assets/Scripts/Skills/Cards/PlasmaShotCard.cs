using UnityEngine;

public class PlasmaShotCard : Skill
{
    [SerializeField] private float _reloadTime, _dmgIncrease, _improveSkill, _improveCd;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private GameObject _zone;

    public override void GivePrize(Sprite skillSlot)
    {
        base.GivePrize(skillSlot);

        StaticValues values = FindObjectOfType<StaticValues>();

        if (values.playerPrefab.TryGetComponent(out PlasmaShot spell))
        {
            spell.dmgIncrease *= _improveSkill;
            spell.reloadTime *= _improveCd;
            spell.zone.transform.localScale *= _improveSkill;
            spell.bullet.transform.localScale *= _improveSkill;

            return;
        }
        else if (values.playerPrefab.TryGetComponent(out Skill component))
        {
            component.ResetAll();
            DestroyImmediate(component, true);
        }

        PlasmaShot shot = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<PlasmaShot>();
        shot.dmgIncrease = _dmgIncrease;
        shot.reloadTime = _reloadTime;
        shot.bullet = _bullet;
        shot.zone = _zone;
        shot.skillSprite = skillSlot;
    }
}
