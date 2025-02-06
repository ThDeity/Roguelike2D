using UnityEngine;

public class DazzleCard : Card
{
    [SerializeField] private float _cdDebuff, _time, _increaseParam, _cd;

    public void GivePrize()
    {
        SetAttackParam(1,0,1,_cdDebuff);

        if (!StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out Dazzle component))
            StaticValues.PlayerAttackList.ForEach(x => { x.bullet.AddComponent<Dazzle>().time = _time; x.bullet.AddComponent<Dazzle>().cd = _cd; });
        else
            StaticValues.PlayerAttackList.ForEach(x => x.bullet.GetComponent<Dazzle>().time *= _increaseParam);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
