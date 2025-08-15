using UnityEngine;

public class ShockwaveCard : Card
{
    [SerializeField] private float _hpBuff, _rollCdDebuff, _radiusBuff, _radius, _damage;
    [SerializeField] private GameObject _explosion;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Shockwave Roll \n +{_rollCdDebuff} Roll CD \n +{(_hpBuff - 1) * 100}% HP";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Отталкивающий Рывок\n +{_rollCdDebuff} КД Рывка\n +{(_hpBuff - 1) * 100}% ХП";
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
