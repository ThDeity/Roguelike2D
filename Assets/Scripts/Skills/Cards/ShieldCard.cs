using UnityEngine;

public class ShieldCard : Skill
{
    [SerializeField] private float _reloadTime, _activeTime, _improveSkill, _improveCd;
    [SerializeField] private GameObject _shield;

    public override void GivePrize(Sprite skillSlot)
    {
        base.GivePrize(skillSlot);

        if (StaticValues.PlayerObj.TryGetComponent(out Shield spell))
        {
            spell.activeTime *= _improveSkill;
            spell.reloadTime *= _improveCd;

            return;
        }
        else if (StaticValues.PlayerObj.TryGetComponent(out Skill component))
        {
            component.ResetAll();
            DestroyImmediate(component, true);
        }

        Shield shield = StaticValues.PlayerObj.gameObject.AddComponent<Shield>();
        shield.reloadTime = _reloadTime;
        shield.activeTime = _activeTime;
        shield.shield = _shield;
        shield.skillSprite = skillSlot;
    }
}