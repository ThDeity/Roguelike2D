using UnityEngine;

public class TricksterCard : Card
{
    [SerializeField] private float _debuffCd, _bonusDmgPerBounce;
    [SerializeField] private int _bonusBounces;

    public void GivePrize()
    {
        StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out Ricoshet card);
        if (card == null)
            StaticValues.PlayerAttackList.ForEach(x => Destroy(x.bullet.GetComponent<Bullet>()));

        foreach(var x  in StaticValues.PlayerAttackList)
        {
            x.bullet.AddComponent<Ricoshet>().riccochetsCount += _bonusBounces;
            x.bullet.GetComponent<Ricoshet>().damageIncreasePerBounce += _bonusDmgPerBounce;
        }

        SetAttackParam(1, 0, 1, _debuffCd, 0, 1);
        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
