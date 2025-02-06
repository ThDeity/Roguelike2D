using UnityEngine;

public class BarrageCard : Card
{
    [SerializeField] private float _dmgDebuff, _cdDebuff, _anglesOffset, _maxDistance, _speedRange, _minSpeed, _bulletSpeed;
    [SerializeField] private int _bulletsCount;

    public void GivePrize()
    {
        SetAttackParam(_dmgDebuff,0,_bulletSpeed,_cdDebuff,0,_maxDistance);

        if (!StaticValues.PlayerAttackList[0].gameObject.TryGetComponent(out Barrage barrage))
        {
            barrage = FindObjectOfType<StaticValues>().playerPrefab.transform.GetChild(0).gameObject.AddComponent<Barrage>();
            PlayerAttack attack = StaticValues.PlayerObj.GetComponent<StaticValues>().playerPrefab.transform.GetChild(0).GetComponentsInChildren<PlayerAttack>()[0];
            barrage = attack.GetComponent<Barrage>();

            barrage.bullet = attack.bullet;
            barrage.reloadTime = attack.reloadTime;
            barrage.bulletsCount = _bulletsCount;
            barrage.anglesOffset = _anglesOffset;
        }
        else
        {
            barrage.reloadTime *= _cdDebuff;
            barrage.bulletsCount += _bulletsCount;
        }

        Bullet bullet = barrage.bullet.GetComponent<Bullet>();
        barrage.minSpeed = bullet.speed * _minSpeed;
        barrage.speedRange = _speedRange;

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
