using UnityEngine;

public class BallistaCard : Skill
{
    [SerializeField] private Ballista _ballista;
    [SerializeField] private GameObject _zone;
    [SerializeField] private float _reloadTime, _increaseParam, _hpOfPlayer;

    public override void GivePrize(Sprite skillSlot)
    {
        base.GivePrize(skillSlot);

        if (StaticValues.PlayerObj.gameObject.TryGetComponent(out BallistaSkill skill))
        {
            skill.ballista.lifeTime *= _increaseParam;
            skill.ballista.maxHp *= _increaseParam;
            skill.ballista.radius *= _increaseParam;
            skill.ballista.persentOfDmg *= _increaseParam;

            skill.hpOfPlayer *= _increaseParam;
            skill.reloadTime *= 0.9f;

            return;
        }
        else if (StaticValues.PlayerObj.gameObject.TryGetComponent(out Skill component))
        {
            component.ResetAll();
            Destroy(component);
        }

        BallistaSkill ballista = StaticValues.PlayerObj.gameObject.AddComponent<BallistaSkill>();
        ballista.reloadTime = _reloadTime;
        ballista.hpOfPlayer = _hpOfPlayer;
        ballista.skillSprite = skillSlot;
        ballista.ballista = _ballista;
        ballista.zone = _zone;
    }
}