using DigitalRuby.LightningBolt;
using UnityEngine;

public class LightningBulletsCard : Card
{
    [SerializeField] private int _maxEnemies;
    [SerializeField] private float _partOfDmg, _dazzleTime, _maxDistance, _timeOfExistingBolt, _buffCard, _debuffCd;
    [SerializeField] private LightningBoltScript _bolt;

    public void GivePrize()
    {
        if (!StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out LightningBullets component))
        {
            component = StaticValues.PlayerAttackList[0].bullet.AddComponent<LightningBullets>();

            component.maxEnemies = _maxEnemies;
            component.damage = StaticValues.PlayerAttackList[0].bullet.GetComponent<Bullet>().damage * _partOfDmg;
            component.dazzleTime = _dazzleTime;
            component.maxDistance = _maxDistance;
            component.timeOfExistingBolt = _timeOfExistingBolt;
            component.bolt = _bolt;
        }
        else
        {
            component.maxEnemies++;
            component.damage *= _buffCard;
            component.dazzleTime *= _buffCard;
            component.maxDistance *= _buffCard;
        }

        SetAttackParam(1,0,1,_debuffCd);
        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
