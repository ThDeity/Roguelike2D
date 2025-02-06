using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActiveSkill : MonoBehaviour
{
    [SerializeField] private List<Transform> _slots = new List<Transform>();
    private GameObject _activeSkillsPanel;

    public Image skillSlot;

    [SerializeField] private List<GameObject> _skills = new List<GameObject>();

    private void Start() => _activeSkillsPanel = StaticValues.ActiveSkillsPanel;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E) && !StaticValues.WasPrizeGotten)
        {
            StaticValues.WasPrizeGotten = true;
            _activeSkillsPanel.SetActive(true);
            _activeSkillsPanel.GetComponent<ActiveSkill>().ShowSkills();

            Destroy(gameObject);
        }
    }

    public void ShowSkills()
    {
        if (_skills.Count < _slots.Count)
        {
            Debug.LogError("Не хватает заклинаний, их количество должно быть больше, либо равно количеству слотов");
            return;
        }

        List<GameObject> list = _skills;

        foreach (Transform slot in _slots)
        {
            int index = Random.Range(0, list.Count - 1);
            Instantiate(list[index], slot);

            list.RemoveAt(index);
        }
    }

    public void Escape() => _activeSkillsPanel.SetActive(false);
}
