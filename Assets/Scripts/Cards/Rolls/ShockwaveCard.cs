using UnityEngine;

public class ShockwaveCard : Card
{
    [SerializeField] private float _hpBuff, _rollCdDebuff, _radiusBuff, _radius, _damage;
    [SerializeField] private GameObject _explosion;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Shockwave Roll \n +{_rollCdDebuff} Roll CD \n +{(_hpBuff - 1) * 100}% HP";
    }

    public void GivePrize()
    {
        if (!StaticValues.PlayerMovementObj.TryGetComponent(out ShockwaveRoll shockwaveRoll))
        {
            shockwaveRoll = StaticValues.PlayerObj.gameObject.AddComponent<ShockwaveRoll>();
            shockwaveRoll.damage = _damage;
            shockwaveRoll.radius = _radius;
            shockwaveRoll.explosionObj = _explosion;
        }
        else
        {
            shockwaveRoll.damage *=_radiusBuff;
            shockwaveRoll.radius *=_radiusBuff;
            shockwaveRoll.explosionObj.transform.localScale *=_radiusBuff;
        }

        StaticValues.PlayerObj.ChangeMxHp(_hpBuff);
        SetRollParam(_rollCdDebuff);
    }
}
