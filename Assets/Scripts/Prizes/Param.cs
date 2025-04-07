using UnityEngine;

public class Param : Prize
{
    private GameObject _paramPanel;

    private void Start() => _paramPanel = StaticValues.ParamPanel;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            StaticValues.WasPrizeGotten = true;
            _paramPanel.SetActive(true);
            Destroy(gameObject);
        }
    }

    public void ImproveDamage(float damagePersent)
    {
        StaticValues.PlayerAttackList.ForEach(x => x.bullet.GetComponent<Bullet>().damage *= 1 + damagePersent / 100);
        _paramPanel.SetActive(false);
    }

    public void ImproveSpeed(float persentPlus)
    {
        StaticValues.PlayerMovementObj.speed *= 1.0f + persentPlus / 100;
        _paramPanel.SetActive(false);
    }

    public void ImproveCrit(int persentPlus)
    {
        StaticValues.PlayerAttackList.ForEach(x => x.bullet.GetComponent<Bullet>().critChance += persentPlus);
        _paramPanel.SetActive(false);
    }

    public void ImproveDashCd(float timeReducePersent)
    {
        StaticValues.PlayerMovementObj.dashCd *= 1 - timeReducePersent / 100;
        _paramPanel.SetActive(false);
    }

    public void ImproveReloadCd(float timeReducePersent)
    {
        StaticValues.PlayerAttackList.ForEach(x => x.reloadTime *= 1 - timeReducePersent / 100);
        _paramPanel.SetActive(false);
    }

    public void ImproveHealth(float hpPlusPersent)
    {
        StaticValues.PlayerObj.ChangeMxHp(1 + hpPlusPersent / 100);
        _paramPanel.SetActive(false);
    }
}
