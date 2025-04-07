using UnityEngine;

public class CollapseCard : Card
{
    [SerializeField] private float _buffDmg, _debuffSpeed, _debuffCd, _debuffMaxDistance, _buffSize;
    [SerializeField] private int _bulletsCount;
    [SerializeField] private GameObject _bullet;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Collapse \n +{(_buffDmg - 1) * 100}% DMG \n -{(_debuffCd - 1) * 100}% Reload \n +{_bulletsCount} Bullets\n -{_debuffSpeed - 1}% Bullet Speed \n +{(_buffSize - 1) * 100}% Size";
    }

    public void GivePrize()
    {
        if (!StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out Collapse collapse))
        {
            foreach(var x in StaticValues.PlayerAttackList)
            {
                Collapse c = x.bullet.AddComponent<Collapse>();
                c.bullet = _bullet;
                c.bulletsCount = _bulletsCount;
            }
        }
        else
            StaticValues.PlayerAttackList.ForEach(x => x.bullet.GetComponent<Collapse>().bulletsCount += _bulletsCount);

        SetAttackParam(_buffDmg,0,_debuffSpeed,_debuffCd, 0,_debuffMaxDistance,_buffSize);
        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
