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
        StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out BulletsComponents components);

        components.SetActiveScripts();
        StaticValues.PlayerAttackList[0].bullet.TryGetComponent(out Dazzle component);

        if (component.time == 0)
        {
            component.time = _time;
            component.cd = _cd;
        }
        else
            StaticValues.PlayerAttackList.ForEach(x => x.bullet.GetComponent<Dazzle>().time *= _increaseParam);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
