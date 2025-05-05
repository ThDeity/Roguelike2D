using UnityEngine;

public class LifestealerCard : Card
{
    [SerializeField] private Lifesteal _lifesteal;
    [SerializeField] private float _sizeBuff, _intervalBuff;
    [SerializeField] private int _count;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Lifestealer \n\n";
    }

    public void GivePrize()
    {
        StaticValues.PlayerObj.gameObject.TryGetComponent(out Lifesteal steal);

        if (steal == null)
        {
            steal = StaticValues.PlayerObj.gameObject.AddComponent<Lifesteal>();
            steal.lifesteal = _lifesteal;
        }
        else
        {
            steal.enemiesCount = _count;
            steal.damage *= _sizeBuff;
            steal.transform.localScale *= _sizeBuff;
            steal.intervalBtwDmg *= _intervalBuff;
        }

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}