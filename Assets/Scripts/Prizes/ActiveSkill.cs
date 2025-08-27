using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActiveSkill : Prize
{
    [SerializeField] private List<Transform> _slots = new List<Transform>();
    private GameObject _activeSkillsPanel;

    public Image skillSlot;

    [SerializeField] private List<GameObject> _skills = new List<GameObject>();

    private void Start() => _activeSkillsPanel = StaticValues.ActiveSkillsPanel;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E))// && !StaticValues.WasPrizeGotten)
        {
            StaticValues.PlayerObj.effectsSource.PlayOneShot(_sound);
            Time.timeScale = 0;

            _activeSkillsPanel.SetActive(true);
            _activeSkillsPanel.GetComponent<ActiveSkill>().ShowSkills();

            StaticValues.WasPrizeGotten = true;
            Destroy(gameObject);
        }
    }

    public void ShowSkills()
    {
        Time.timeScale = 0;

        if (_skills.Count < _slots.Count)
        {
            Debug.LogError("�� ������� ����������, �� ���������� ������ ���� ������, ���� ����� ���������� ������");
            return;
        }

        List<GameObject> list = new List<GameObject>(_skills);

        foreach (Transform slot in _slots)
        {
            int index = Random.Range(0, list.Count);
            Instantiate(list[index], slot);

            list.RemoveAt(index);
        }
    }

    public void Escape() => _activeSkillsPanel.SetActive(false);

    Button _rerollButton;
    public void Reroll(Button button)
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