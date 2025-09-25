using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using OneClickLocalization;
using UnityEngine.EventSystems;

public class Skill : MonoBehaviour, Roll, IPointerEnterHandler, IPointerExitHandler
{
    private Text _timer;
    public Sprite skillSprite;

    protected bool _isSkillCharged;
    protected Text _mechanicDescription;
    protected SystemLanguage _currentLanguage;
    [SerializeField] protected string _titleEng, _titleRus;

    [SerializeField] protected bool _isCard;

    public SkillJoystick joystick;
    public AudioClip skillSound;

    public virtual void OnRollStarted() { _isSkillCharged = false; }

    protected virtual void ShowDescription()
    {
        _mechanicDescription.enabled = true;
        _isChosen = true;

        if (_currentLanguage == SystemLanguage.English)
            _mechanicDescription.text = _titleEng;
        else if (_currentLanguage == SystemLanguage.Russian)
            _mechanicDescription.text = _titleRus;
    }

    protected virtual void HideDescription()
    {
        _isChosen = false;

        if (_mechanicDescription != null && _mechanicDescription.isActiveAndEnabled)
        {
            _mechanicDescription.enabled = false;
            _mechanicDescription.text = "";
        }
    }    

    public void OnPointerEnter(PointerEventData eventData) => ShowDescription();

    public void OnPointerExit(PointerEventData eventData) => HideDescription();

    public void Chosen()
    {
        StaticValues.ActiveSkillsPanel.GetComponent<ActiveSkill>().skills.ForEach(x => x.Unchosen());

        ShowDescription();
    }
    public void Unchosen() => HideDescription();

    protected bool _isChosen;
    public virtual void GivePrize(Sprite skillSklot)
    {
        if (!_isChosen)
        {
            Chosen();
            return;
        }

        StaticValues.ActiveSkillsPanel.GetComponent<ActiveSkill>().skillSlot.sprite = skillSklot;
        StaticValues.ActiveSkillsPanel.SetActive(false);
    }

    public virtual void ResetAll() { }

    protected virtual void Start()
    {
        _currentLanguage = OCL.GetLanguage();

        _timer = StaticValues.SkillsTimer.GetComponent<Text>();
        _mechanicDescription = StaticValues.ActiveSkillsPanel.GetComponentInChildren<Text>();
        StaticValues.PlayerMovementObj.CheckComponents();

        if (skillSprite != null)
            StaticValues.ActiveSkillsPanel.GetComponent<ActiveSkill>().skillSlot.sprite = skillSprite;
    }

    protected virtual void OnDisable()
    {
        if (_isCard)
            Destroy(gameObject);
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