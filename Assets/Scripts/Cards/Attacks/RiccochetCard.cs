using UnityEngine;

public class RiccochetCard : Card
{
    [SerializeField] private float _buffBulletSpeed, _buffMaxDistance, _debuffDmg, _debuffCd;
    [SerializeField] private int _bounceCount;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Riccochet \n +{(_buffBulletSpeed - 1) * 100}% Bullet Speed \n +{(_buffMaxDistance - 1) * 100}% Max distance \n -{(1 - _debuffDmg) * 100}% DMG\n -{(_debuffCd - 1) * 100}% Reload \n +{_bounceCount} Riccochets";
    }

    public void GivePrize()
    {
        StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out BulletsComponents components);
        components.SetComponent(typeof(Ricoshet));

        StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out Ricoshet rico);

        if (rico.riccochetsCount == 0)
        {
            foreach (var a in StaticValues.PlayerAttackList)
            {
                a.bullet.TryGetComponent(out Bullet bull);

                if (bull != null)
                {
                    rico.speed = bull.speed;
                    rico.damage = bull.damage;
                    rico.lifeSteal = bull.lifeSteal;
                    rico.critChance = bull.critChance;
                    rico.maxDistance = bull.maxDistance;
                    rico.isDrillAmmo = bull.isDrillAmmo;
                    rico.lifeTimeRange = bull.lifeTimeRange;
                    rico.timeTakingDmg = bull.timeTakingDmg;
                    rico.riccochetsCount = _bounceCount;
                    rico.GetComponent<Collider2D>().isTrigger = false;

                    bull.enabled = false;
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
