using UnityEngine;

public class PhoenixCard : MonoBehaviour
{
    [SerializeField] private float _hpDebuff, _hpPersentAfterDeath;

    public void GivePrize()
    {
        Player player = FindObjectOfType<StaticValues>().playerPrefab.GetComponent<Player>();

        player.ChangeMxHp(_hpDebuff);
        player.hpAfterDeath = _hpDebuff;
        player.lifesCount++;

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
