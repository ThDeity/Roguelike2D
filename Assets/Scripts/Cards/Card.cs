using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
    protected virtual void SetAttackParam(float dmg = 1, float lifeSteal = 0, float bulletSpeed = 1, float cd = 1, int timeOfTakingDmg = 0, float maxDistance = 1, float changeSize = 1)
    {
        //FindObjectOfType<StaticValues>().attacks.ForEach(x => x.reloadTime *= cd);
        List<PlayerAttack> attacks = FindObjectOfType<StaticValues>().playerPrefab.transform.GetChild(0).GetComponentsInChildren<PlayerAttack>().ToList();
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
        FindObjectOfType<StaticValues>().playerPrefab.GetComponent<PlayerMovement>().dashCd += rollCdPlus;
        FindObjectOfType<StaticValues>().playerPrefab.GetComponent<PlayerMovement>().dashCd *= rollCdProduct;

        StaticValues.PassiveSkillsPanel.gameObject.SetActive(false);
    }
}
