using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Text _description, _realDescription;
    [SerializeField] protected string _title;
    protected Button _button;

    public Text count;

    protected virtual void Start()
    {
        _description = GetComponentInChildren<Text>();
        _realDescription = StaticValues.PassiveSkillsPanel.GetComponentInChildren<Text>();
    }

    protected virtual void SetAttackParam(float dmg = 1, float lifeSteal = 0, float bulletSpeed = 1, float cd = 1, int timeOfTakingDmg = 0, float maxDistance = 1, float changeSize = 1)
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        _realDescription.enabled = true;
        _realDescription.text = _title;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _realDescription.enabled = false;
        _realDescription.text = "";
    }

    public void OnDisable()
    {
        StaticValues.PlayerObj.CheckComponents();
        StaticValues.PlayerMovementObj.CheckComponents();
    }
}
