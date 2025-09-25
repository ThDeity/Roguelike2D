using UnityEngine;

public class IceBurstCard : Skill
{
    [SerializeField] private GameObject _zone, _effects;
    [SerializeField] private float _reloadTime, _freezingTime, _radius, _force, _damage, _improveSkill, _speed;

    public override void GivePrize(Sprite skillSlot)
    {
        base.GivePrize(skillSlot);

        if (StaticValues.PlayerObj.TryGetComponent(out IceBurst spell))
        {
            spell.Improve(_improveSkill);

            spell.reloadTime *= 0.9f;
            return;
        }
        else if (StaticValues.PlayerObj.TryGetComponent(out Skill component))
        {
            component.ResetAll();
            Destroy(component);
        }

        IceBurst iceBurst = StaticValues.PlayerObj.gameObject.AddComponent<IceBurst>();
        iceBurst.skillSound = skillSound;
        iceBurst.zone = _zone;
        iceBurst.reloadTime = _reloadTime;
        iceBurst.freezingTime = _freezingTime;
        iceBurst.radius = _radius;
        iceBurst.force = _force;
        iceBurst.damage = _damage;
        iceBurst.effects = _effects;
        iceBurst.skillSprite = skillSlot;
        iceBurst.joystick = FindObjectOfType<SkillJoystick>();
        iceBurst.speed = _speed;
    }
}