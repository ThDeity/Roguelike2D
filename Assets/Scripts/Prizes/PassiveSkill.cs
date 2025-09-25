using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSkill : Prize
{
    [SerializeField] private List<Transform> _slots = new List<Transform>();
    [SerializeField] private Image _cardsRarity;
    private GameObject _passiveSkillsPanel;

    [SerializeField] private List<Color> _colors;

    [SerializeField] private List<GameObject> _usualSkills = new List<GameObject>(), _rareSkills = new List<GameObject>(), 
        _epicSkills = new List<GameObject>(), _legendarySkills = new List<GameObject>();
    [SerializeField] private int _usualChance, _rareChance, _epicChance, _legendaryChance;

    GameObject _rerollButton;
    public void Reroll(GameObject button)
    {
        if (StaticValues.CurrentCountOfRerolls < StaticValues.CountOfRerolls)
        {
            _rerollButton = button;

            ShowRewardAdv_UseCallback();
        }
    }

    protected override void SetReward()
    {
        StaticValues.CurrentCountOfRerolls += 1;
        foreach (Transform slot in _slots)
        {
            for (int i = 0; i < slot.childCount; i++)
                Destroy(slot.GetChild(i).gameObject);
        }
        ShowSkills();

        _rerollButton.gameObject.SetActive(false);
    }

    private void Start() => _passiveSkillsPanel = StaticValues.PassiveSkillsPanel;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E))
            Interact();
    }

    public override void Interact()
    {
        base.Interact();

        StaticValues.PlayerObj.effectsSource.PlayOneShot(_sound);
        Time.timeScale = 0;

        StaticValues.WasPrizeGotten = true;
        _passiveSkillsPanel.SetActive(true);
        _passiveSkillsPanel.GetComponent<PassiveSkill>().ShowSkills();

        Destroy(gameObject);
    }

    public List<Card> currentCards {  get; private set; }
    public void ShowSkills()
    {
        List<GameObject> usual = new List<GameObject>(), rare = new List<GameObject>(), epic = new List<GameObject>(), legendary = new List<GameObject>();
        var cards = new List<GameObject>();
        currentCards = new List<Card>();

        for (int i = 0; i < _slots.Count; i++)
        {
            int allChances = _usualChance + _rareChance + _epicChance + _legendaryChance;
            int number = Random.Range(0, allChances + 1);

            if (number <= _usualChance)
                cards = new List<GameObject>(_usualSkills);
            else if (number <= _usualChance + _rareChance)
                cards = new List<GameObject>(_rareSkills);
            else if (number <= _usualChance + _rareChance + _epicChance)
                cards = new List<GameObject>(_epicSkills);
            else
                cards = new List<GameObject>(_legendarySkills);

            int index = Random.Range(0, cards.Count);
            Image image = Instantiate(_cardsRarity, _slots[i]);
            GameObject c = Instantiate(cards[index], _slots[i]);
            currentCards.Add(c.GetComponent<Card>());

            if (number <= _usualChance)
            {
                image.color = _colors[0];
                usual.Add(cards[index]);
                _usualSkills.RemoveAt(index);
            }
            else if (number <= _usualChance + _rareChance)
            {
                image.color = _colors[1];
                rare.Add(cards[index]);
                _rareSkills.RemoveAt(index);
            }
            else if (number <= _usualChance + _rareChance + _epicChance)
            {
                image.color = _colors[2];
                epic.Add(cards[index]);
                _epicSkills.RemoveAt(index);
            }
            else
            {
                image.color = _colors[3];
                legendary.Add(cards[index]);
                _legendarySkills.RemoveAt(index);
            }
        }

        _usualSkills.AddRange(usual);
        _rareSkills.AddRange(rare);
        _epicSkills.AddRange(epic);
        _legendarySkills.AddRange(legendary);

        usual.Clear();
        rare.Clear();
        epic.Clear();
        legendary.Clear();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (StaticValues.CountOfRerolls <= 0)
            _rerollButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (_rerollButton != null && StaticValues.CurrentCountOfRerolls < StaticValues.CountOfRerolls)
            _rerollButton.gameObject.SetActive(true);

        if (_buttonE == null)
            Time.timeScale = 1;
    }
}
