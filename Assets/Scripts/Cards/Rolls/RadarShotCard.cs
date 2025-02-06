using UnityEngine;

public class RadarShotCard : Card
{
    [SerializeField] private float _hpBuff, _rollCdDebuff, _radiusBuff;
    [SerializeField] private RadarShot _radarShot;

    public void GivePrize()
    {
        if (!StaticValues.PlayerMovementObj.TryGetComponent(out RadarShotRoll radarShotRoll))
        {
            radarShotRoll = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<RadarShotRoll>();
            radarShotRoll.radarShot = _radarShot;
        }
        else
        {
            radarShotRoll.radarShot.radius *= _radiusBuff;
            radarShotRoll.radarShot.damageBoost *= _radiusBuff;
            radarShotRoll.radarShot.transform.localScale *= _radiusBuff;
            radarShotRoll.radarShot.maxEnemiesCount++;
        }

        StaticValues.PlayerObj.ChangeMxHp(_hpBuff);
        SetRollParam(_rollCdDebuff);
    }
}
