using UnityEngine;
using UnityEngine.UI;

public class EnemyParam : Prize
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private string _happyText, _unhappyText;
    
    private GameObject _paramPanel;
    private int chance;

    private void Start() => _paramPanel = StaticValues.EnemyParamPanel;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            _paramPanel = StaticValues.EnemyParamPanel;
            _paramPanel.SetActive(true);

            StaticValues.WasPrizeGotten = true;
            Destroy(gameObject);
        }
    }

    private void OnEnable() => chance = Random.Range(0, 2);

    private void WriteResults()
    {
        _panel.SetActive(true);
        _panel.GetComponentInChildren<Text>().text = chance == 0 ? _unhappyText : _happyText;

        _paramPanel.SetActive(false);
    }

    public void ChangeDamage(float damagePersent)
    {
        StaticValues.EnemyDamage *= chance == 0 ? 1 + damagePersent / 100 : 1 - damagePersent / 100;
        WriteResults();
    }

    public void ChangeSpeed(float speedPersent)
    {
        StaticValues.EnemySpeed *= chance == 0 ? 1 + speedPersent / 100 : 1 - speedPersent / 100;
        WriteResults();
    }

    public void ChangeCrit(int critPersent)
    {
        StaticValues.EnemyCrit += chance == 0 ? critPersent : -critPersent;
        WriteResults();
    }

    public void ChangeEnemyCount(float countChange)
    {
        StaticValues.EnemyCount *= chance == 0 ? 1 + countChange / 100 : 1 - countChange / 100;
        WriteResults();
    }

    public void ChangeMaxHp(float hpChange)
    {
        StaticValues.EnemyCount *= chance == 0 ? 1 + hpChange / 100 : 1 - hpChange / 100;
        WriteResults();
    }
}
