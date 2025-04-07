using UnityEngine;

public class LifestealerCard : Card
{
    [SerializeField] private Lifesteal _lifesteal;
    [SerializeField] private float _sizeBuff, _intervalBuff;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Lifestealer \n\n";
    }

    public void GivePrize()
    {
        FindObjectOfType<StaticValues>().playerPrefab.TryGetComponent(out Lifesteal steal);

        if (steal == null)
        {
            steal = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<Lifesteal>();
            steal.lifesteal = _lifesteal;
        }
        else
        {
            steal.damage *= _sizeBuff;
            steal.transform.localScale *= _sizeBuff;
            steal.intervalBtwDmg *= _intervalBuff;
        }

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}