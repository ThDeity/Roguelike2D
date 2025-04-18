using UnityEngine;

public class FireRollCard : Card
{
    [SerializeField] private TrailOfFire _trail;
    [SerializeField] private float _rollCdDebuff, _radiusBuff;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Fire Roll \n +{_rollCdDebuff} Roll CD";
    }

    public void GivePrize()
    {
        if (!StaticValues.PlayerMovementObj.TryGetComponent(out FireRoll fireRoll))
        {
            fireRoll = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<FireRoll>();
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
