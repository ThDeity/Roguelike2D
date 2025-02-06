using UnityEditor;
using UnityEngine;

public class RiccochetCard : Card
{
    [SerializeField] private float _buffBulletSpeed, _buffMaxDistance, _debuffDmg, _debuffCd;
    [SerializeField] private int _bounceCount;

    public void GivePrize()
    {
        if (!StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out Ricoshet rico))
        {
            foreach (var a in StaticValues.PlayerAttackList)
            {
                a.bullet.TryGetComponent(out Bullet bull);
                Ricoshet x = a.bullet.AddComponent<Ricoshet>();

                if (bull != null)
                {
                    x.speed = bull.speed;
                    x.damage = bull.damage;
                    x.lifeSteal = bull.lifeSteal;
                    x.critChance = bull.critChance;
                    x.maxDistance = bull.maxDistance;
                    x.isDrillAmmo = bull.isDrillAmmo;
                    x.lifeTimeRange = bull.lifeTimeRange;
                    x.timeTakingDmg = bull.timeTakingDmg;
                    x.riccochetsCount = _bounceCount;
                    x.GetComponent<Collider2D>().isTrigger = false;

                    DestroyImmediate(bull, true);
                }
            }
        }
        else
        {
            foreach (var a in StaticValues.PlayerAttackList)
                a.GetComponent<Ricoshet>().riccochetsCount += _bounceCount;
        }

        SetAttackParam(_debuffDmg,0,_buffBulletSpeed,_debuffCd, 0,_buffMaxDistance);
        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
