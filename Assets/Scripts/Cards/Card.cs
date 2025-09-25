using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using OneClickLocalization;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Text _description, _mechanicDescription;
    protected SystemLanguage _currentLanguage;
    [SerializeField] protected string _titleEng, _titleRus;
    protected Button _button;
    protected bool _isChosen;

    protected virtual void Start()
    {
        _currentLanguage = OCL.GetLanguage();

        _description = GetComponentInChildren<Text>();
        _mechanicDescription = StaticValues.PassiveSkillsPanel.GetComponentInChildren<Text>();
    }

    protected virtual void SetAttackParam(float dmg = 1, float lifeSteal = 0, float bulletSpeed = 1, float cd = 1, float timeOfTakingDmg = 0, float maxDistance = 1, float changeSize = 1)
    {
        List<PlayerAttack> attacks = StaticValues.PlayerTransform.GetChild(0).GetComponentsInChildren<PlayerAttack>().ToList();
        attacks.ForEach(x => x.reloadTime *= cd);

        foreach (var attack in StaticValues.PlayerAttackList)
        {
            attack.reloadTime *= cd;
            attack.bullet.transform.localScale *= changeSize;

            Bullet bullet = attack.bullet.GetComponent<Bullet>();
            bullet.timeTakingDmg = bullet.timeTakingDmg > timeOfTakingDmg ? bullet.timeTakingDmg : timeOfTakingDmg;

            bullet.speed *= bulletSpeed;
            bullet.lifeSteal += lifeSteal;
            bullet.damage *= dmg;
            bullet.maxDistance *= maxDistance;
        }
    }

    protected virtual void SetRollParam(float rollCdPlus = 0, float rollCdProduct = 1)
    {
        StaticValues.PlayerMovementObj.dashCd += rollCdPlus;
        StaticValues.PlayerMovementObj.dashCd *= rollCdProduct;

        StaticValues.PassiveSkillsPanel.gameObject.SetActive(false);
    }

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

    public virtual void GivePrize()
    {
        if (!_isChosen)
        {
            Chosen();
            return;
        }
    }

    public void Chosen()
    {
        StaticValues.ActiveSkillsPanel.GetComponent<PassiveSkill>().currentCards.ForEach(x => x.Unchosen());

        ShowDescription();
    }
    public void Unchosen() => HideDescription();

    public void OnDisable()
    {
        StaticValues.PlayerObj.CheckComponents();
        StaticValues.PlayerMovementObj.CheckComponents();

        Destroy(transform.parent.GetChild(0).gameObject);
        Destroy(gameObject);
    }
}
