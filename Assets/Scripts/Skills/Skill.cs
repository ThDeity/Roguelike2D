using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour, Roll
{
    private Text _timer;
    public Sprite skillSprite;
    protected bool _isSkillCharged;

    public virtual void OnRollStarted() { _isSkillCharged = false; }

    public virtual void GivePrize(Sprite skillSklot)
    {
        StaticValues.ActiveSkillsPanel.GetComponent<ActiveSkill>().skillSlot.sprite = skillSklot;
        StaticValues.ActiveSkillsPanel.SetActive(false);
    }

    public virtual void ResetAll() { }

    protected virtual void Start()
    {
        _timer = StaticValues.SkillsTimer.GetComponent<Text>();

        if (skillSprite != null)
            StaticValues.ActiveSkillsPanel.GetComponent<ActiveSkill>().skillSlot.sprite = skillSprite;
    }

    protected IEnumerator StartTimer(int time)
    {
        _timer.gameObject.SetActive(true);
        for (int i = time; i > 0; i--)
        {
            _timer.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        _timer.gameObject.SetActive(false);
    }
}