using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    private Text _timer;
    public Sprite skillSprite;

    public virtual void GivePrize(Sprite skillSklot)
    {
        StaticValues.ActiveSkillsPanel.GetComponent<ActiveSkill>().skillSlot.sprite = skillSklot;
        StaticValues.ActiveSkillsPanel.SetActive(false);
    }

    protected virtual void Start()
    {
        if (skillSprite != null)
            StaticValues.ActiveSkillsPanel.GetComponent<ActiveSkill>().skillSlot.sprite = skillSprite;

        _timer = StaticValues.ActiveSkillsPanel.GetComponent<ActiveSkill>().skillSlot.GetComponentInChildren<Text>();
        _timer.enabled = false;
    }

    protected IEnumerator StartTimer(int time)
    {
        _timer.enabled = true;
        for (int i = time; i > 0; i--)
        {
            _timer.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        _timer.enabled = false;
    }
}