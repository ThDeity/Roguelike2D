using Unity.VisualScripting;
using UnityEngine;

public class ChargeCard : Card
{
    [SerializeField] private float _timeToMaximize, _debuffCd, _dmgBuff, _maxDmgIncrease, _speedDebuff;
    [SerializeField] private GameObject _chargingBullet;
    [SerializeField] private Vector2 _maxSize;
    [SerializeField] private KeyCode _keyCode;

    public void GivePrize()
    {
        Charge charge = StaticValues.PlayerAttackList[0].AddComponent<Charge>();

        charge.chargingBullet = _chargingBullet;
        charge.timeToMaximize = _timeToMaximize;
        charge._buffDmg = _maxDmgIncrease;
        charge.speedDebuff = _speedDebuff;
        charge.maxSize = _maxSize;
        charge.key = _keyCode;

        SetAttackParam(_dmgBuff, 0, 1, _debuffCd);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
