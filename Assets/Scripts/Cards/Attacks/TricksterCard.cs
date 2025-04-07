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
        StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out Ricoshet card);
        if (card == null)
            StaticValues.PlayerAttackList.ForEach(x => DestroyImmediate(x.bullet.GetComponent<Bullet>(), true));

        foreach(var x  in StaticValues.PlayerAttackList)
        {
            x.bullet.AddComponent<Ricoshet>().riccochetsCount += _bonusBounces;
            x.bullet.GetComponent<Ricoshet>().damageIncreasePerBounce += _bonusDmgPerBounce;
        }

        SetAttackParam(1, 0, 1, _debuffCd, 0, 1);
        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
