using UnityEngine;

public class BallistaCard : Skill
{
    [SerializeField] private Ballista _ballista;
    [SerializeField] private GameObject _zone;
    [SerializeField] private float _reloadTime, _increaseParam, _hpOfPlayer;

    public override void GivePrize(Sprite skillSlot)
    {
        base.GivePrize(skillSlot);
        StaticValues values = FindObjectOfType<StaticValues>();

        if (values.playerPrefab.TryGetComponent(out BallistaSkill skill))
        {
            skill.ballista.lifeTime *= _increaseParam;
            skill.ballista.maxHp *= _increaseParam;
            skill.ballista.radius *= _increaseParam;
            skill.ballista.persentOfDmg *= _increaseParam;

            skill.hpOfPlayer *= _increaseParam;

            return;
        }
        else if (values.playerPrefab.TryGetComponent(out Skill component))
        {
            component.ResetAll();
            DestroyImmediate(component, true);
        }

        BallistaSkill ballista = values.playerPrefab.AddComponent<BallistaSkill>();
        ballista.reloadTime = _reloadTime;
        ballista.hpOfPlayer = _hpOfPlayer;
        ballista.skillSprite = skillSlot;
        ballista.ballista = _ballista;
        ballista.zone = _zone;
    }
}