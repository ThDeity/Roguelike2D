using UnityEngine;

public class FireRollCard : Card
{
    [SerializeField] private TrailOfFire _trail;
    [SerializeField] private float _rollCdDebuff, _radiusBuff;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Fire Roll \n +{_rollCdDebuff} Roll CD";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Огненный Рывок\n +{_rollCdDebuff} КД Рывка";
    }

    public override void GivePrize()
    {
        base.GivePrize();

        if (!StaticValues.PlayerMovementObj.TryGetComponent(out FireRoll fireRoll))
        {
            fireRoll = StaticValues.PlayerObj.gameObject.AddComponent<FireRoll>();
            fireRoll.trail = _trail;
        }
        else
        {
            fireRoll.trail.damage *= _radiusBuff;
            fireRoll.trail.timeBtwDamage *= 2 - _radiusBuff;
            fireRoll.trail.lifeTime *= 2 - _radiusBuff;
        }

        SetRollParam(_rollCdDebuff);
    }
}
