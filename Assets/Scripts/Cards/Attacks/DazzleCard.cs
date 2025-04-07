using UnityEngine;

public class DazzleCard : Card
{
    [SerializeField] private float _cdDebuff, _time, _increaseParam, _cd;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Dazzle \n -{(_cdDebuff - 1) * 100}% Reload \n {_time}s Of Dazzle";
    }

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
