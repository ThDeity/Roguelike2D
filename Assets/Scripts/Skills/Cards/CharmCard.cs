using UnityEngine;

public class CharmCard : Skill
{
    [SerializeField] private CharmSector _zone;
    [SerializeField] private float _reloadTime, _timeOfCharming, _increaseParamEnemy, _increaseParam;
    [SerializeField] private int _maxEnemies;

    public override void GivePrize(Sprite skillSlot)
    {
        base.GivePrize(skillSlot);

        StaticValues values = FindObjectOfType<StaticValues>();

        if (values.playerPrefab.TryGetComponent(out Charm skill))
        {
            skill.timeOfCharming *= _increaseParam;
            skill.maxEnemies++;
            skill.increaseParam *= _increaseParam;
            skill.zone.transform.localScale *= _increaseParam;

            return;
        }
        else if (values.playerPrefab.TryGetComponent(out Skill component))
        {
            component.ResetAll();
            DestroyImmediate(component, true);
        }

        Charm charm = values.playerPrefab.AddComponent<Charm>();
        charm.zone = _zone;
        charm.reloadTime = _reloadTime;
        charm.timeOfCharming = _timeOfCharming;
        charm.increaseParam = _increaseParam;
        charm.maxEnemies = _maxEnemies;
        charm.skillSprite = skillSlot;
    }
}
