using UnityEngine;

public class BallistaCard : Skill
{
    [SerializeField] private Ballista _ballista;
    [SerializeField] private GameObject _zone;
    [SerializeField] private float _reloadTime, _increaseParam;

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

            return;
        }
        else if (values.playerPrefab.TryGetComponent(out Skill component))
            DestroyImmediate(component, true);

        BallistaSkill ballista = values.playerPrefab.AddComponent<BallistaSkill>();
        ballista.ballista = _ballista;
        ballista.zone = _zone;
        ballista.reloadTime = _reloadTime;
        ballista.skillSprite = skillSlot;
    }
}