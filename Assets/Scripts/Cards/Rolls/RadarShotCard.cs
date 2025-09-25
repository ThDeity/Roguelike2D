using UnityEngine;

public class RadarShotCard : Card
{
    [SerializeField] private float _hpBuff, _rollCdDebuff, _radiusBuff;
    [SerializeField] private RadarShot _radarShot;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Radar Shot Roll\n +{_rollCdDebuff} Roll CD \n +{(_radarShot.damageBoost - 1) * 100}% Radar DMG from your DMG";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Стреляющий Рывок\n +{_rollCdDebuff} КД Рывка\n +{(_radarShot.damageBoost - 1) * 100}% Увеличение урона от Выстрела";
    }

    public override void GivePrize()
    {
        base.GivePrize();

        if (!StaticValues.PlayerMovementObj.TryGetComponent(out RadarShotRoll radarShotRoll))
        {
            radarShotRoll = StaticValues.PlayerObj.gameObject.AddComponent<RadarShotRoll>();
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
