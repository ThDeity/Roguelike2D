using UnityEngine;

public class EnemyParam : MonoBehaviour
{
    private GameObject _paramPanel;

    private void Start() => _paramPanel = StaticValues.EnemyParamPanel;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            _paramPanel = StaticValues.EnemyParamPanel;
            StaticValues.WasPrizeGotten = true;
            _paramPanel.SetActive(true);
            Destroy(gameObject);
        }
    }

    public void ChangeDamage(float damagePersent)
    {
        StaticValues.EnemyDamage *= Random.Range(0, 1) == 0 ? 1 + damagePersent / 100 : 1 - damagePersent / 100;
        _paramPanel.SetActive(false);
    }

    public void ChangeSpeed(float speedPersent)
    {
        StaticValues.EnemySpeed *= Random.Range(0, 1) == 0 ? 1 + speedPersent / 100 : 1 - speedPersent / 100;
        _paramPanel.SetActive(false);
    }

    public void ChangeCrit(int critPersent)
    {
        StaticValues.EnemyCrit += Random.Range(0, 1) == 0 ? critPersent : -critPersent;
        _paramPanel.SetActive(false);
    }

    public void ChangeEnemyCount(float countChange)
    {
        StaticValues.EnemyCount *= Random.Range(0, 1) == 0 ? 1 + countChange / 100 : 1 - countChange / 100;
        _paramPanel.SetActive(false);
    }

    public void ChangeMaxHp(float hpChange)
    {
        StaticValues.EnemyCount *= Random.Range(0, 1) == 0 ? 1 + hpChange / 100 : 1 - hpChange / 100;
        _paramPanel.SetActive(false);
    }
}
