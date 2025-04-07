using System.Collections.Generic;
using UnityEngine;

public class PassiveSkill : Prize
{
    [SerializeField] private List<Transform> _slots = new List<Transform>();
    private GameObject _passiveSkillsPanel;

    [SerializeField] private List<GameObject> _usualSkills = new List<GameObject>(), _rareSkills = new List<GameObject>(), 
        _epicSkills = new List<GameObject>(), _legendarySkills = new List<GameObject>();
    [SerializeField] private int _usualChance, _rareChance, _epicChance, _legendaryChance;

    private void Start() => _passiveSkillsPanel = StaticValues.PassiveSkillsPanel;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            StaticValues.WasPrizeGotten = true;
            _passiveSkillsPanel.SetActive(true);
            _passiveSkillsPanel.GetComponent<PassiveSkill>().ShowSkills();

            Destroy(gameObject);
        }
    }

    public void ShowSkills()
    {
        List<GameObject> usual = new List<GameObject>(), rare = new List<GameObject>(), epic = new List<GameObject>(), legendary = new List<GameObject>();
        var cards = new List<GameObject>();

        for (int i = 0; i < _slots.Count; i++)
        {
            int allChances = _usualChance + _rareChance + _epicChance + _legendaryChance;
            int number = Random.Range(0, allChances);

            if (number <= _usualChance)
                cards = _usualSkills;
            else if (number <= _rareChance)
                cards = _rareSkills;
            else if (number <= _epicChance)
                cards = _epicSkills;
            else
                cards = _legendarySkills;

            int index = Random.Range(0, cards.Count - 1);
            Instantiate(cards[index], _slots[i]);

            if (number <= _usualChance)
            {
                usual.Add(cards[index]);
                _usualSkills.RemoveAt(index);
            }
            else if (number <= _rareChance)
            {
                rare.Add(cards[index]);
                _rareSkills.RemoveAt(index);
            }
            else if (number <= _epicChance)
            {
                epic.Add(cards[index]);
                _epicSkills.RemoveAt(index);
            }
            else
            {
                legendary.Add(cards[index]);
                _legendarySkills.RemoveAt(index);
            }
        }

        _usualSkills.AddRange(usual);
        _rareSkills.AddRange(rare);
        _epicSkills.AddRange(epic);
        _legendarySkills.AddRange(legendary);
    }
}
