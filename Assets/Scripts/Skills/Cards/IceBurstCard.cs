using UnityEngine;

public class IceBurstCard : Skill
{
    [SerializeField] private GameObject _zone, _effects;
    [SerializeField] private float _reloadTime, _freezingTime, _radius, _force, _damage, _improveSkill;

    public override void GivePrize(Sprite skillSlot)
    {
        base.GivePrize(skillSlot);

        StaticValues values = FindObjectOfType<StaticValues>();

        if (values.playerPrefab.TryGetComponent(out IceBurst spell))
        {
            spell.freezingTime *= _improveSkill;
            spell.radius *= _improveSkill;
            spell.force *= _improveSkill;
            spell.damage *= _improveSkill;

            return;
        }
        else if (values.playerPrefab.TryGetComponent(out Skill component))
        {
            component.ResetAll();
            DestroyImmediate(component, true);
        }

        IceBurst iceBurst = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<IceBurst>();
        iceBurst.zone = _zone;
        iceBurst.reloadTime = _reloadTime;
        iceBurst.freezingTime = _freezingTime;
        iceBurst.radius = _radius;
        iceBurst.force = _force;
        iceBurst.damage = _damage;
        iceBurst.effects = _effects;
        iceBurst.skillSprite = skillSlot;
    }
}