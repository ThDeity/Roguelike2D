using UnityEngine;

public class CharmCard : Skill
{
    [SerializeField] private CharmSector _zone;
    [SerializeField] private float _reloadTime, _timeOfCharming, _increaseParamEnemy, _increaseParam;
    [SerializeField] private int _maxEnemies;

    public override void GivePrize(Sprite skillSlot)
    {
        base.GivePrize(skillSlot);


        if (StaticValues.PlayerObj.gameObject.TryGetComponent(out Charm skill))
        {
            skill.timeOfCharming *= _increaseParam;
            skill.maxEnemies++;
            skill.increaseParam *= _increaseParam;
            skill.zone.transform.localScale *= _increaseParam;

            skill.reloadTime *= 0.9f;
            return;
        }
        else if (StaticValues.PlayerObj.gameObject.TryGetComponent(out Skill component))
        {
            component.ResetAll();
            Destroy(component);
        }

        Charm charm = StaticValues.PlayerObj.gameObject.AddComponent<Charm>();
        charm.zone = _zone;
        charm.reloadTime = _reloadTime;
        charm.timeOfCharming = _timeOfCharming;
        charm.increaseParam = _increaseParam;
        charm.maxEnemies = _maxEnemies;
        charm.skillSprite = skillSlot;
    }
}
