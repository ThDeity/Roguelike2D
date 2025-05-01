using UnityEngine;

public class TricksterCard : Card
{
    [SerializeField] private float _debuffCd, _bonusDmgPerBounce;
    [SerializeField] private int _bonusBounces;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Trickster \n +{(_bonusDmgPerBounce - 1) * 100}% DMG per bounce\n -{(_debuffCd - 1) * 100}% Reload\n +{-_bonusBounces} Riccochets";
    }

    public void GivePrize()
    {
        StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out BulletsComponents components);
        components.SetComponent(typeof(Ricoshet));

        StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out Bullet bull);
        StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out Ricoshet card);
        if (card.riccochetsCount == 0)
        {
            card.speed = bull.speed;
            card.damage = bull.damage;
            card.lifeSteal = bull.lifeSteal;
            card.critChance = bull.critChance;
            card.maxDistance = bull.maxDistance;
            card.isDrillAmmo = bull.isDrillAmmo;
            card.lifeTimeRange = bull.lifeTimeRange;
            card.timeTakingDmg = bull.timeTakingDmg;
            card.GetComponent<Collider2D>().isTrigger = false;

            bull.enabled = false;
        }

        card.riccochetsCount += _bonusBounces;
        card.damageIncreasePerBounce += _bonusDmgPerBounce;

        SetAttackParam(1, 0, 1, _debuffCd, 0, 1);
        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
