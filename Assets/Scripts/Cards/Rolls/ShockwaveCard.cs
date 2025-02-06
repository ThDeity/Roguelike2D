using UnityEngine;

public class ShockwaveCard : Card
{
    [SerializeField] private float _hpBuff, _rollCdDebuff, _radiusBuff, _radius, _damage;
    [SerializeField] private GameObject _explosion;

    public void GivePrize()
    {
        if (!StaticValues.PlayerMovementObj.TryGetComponent(out ShockwaveRoll shockwaveRoll))
        {
            shockwaveRoll = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<ShockwaveRoll>();
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
