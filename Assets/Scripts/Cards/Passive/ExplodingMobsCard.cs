using UnityEngine;

public class ExplodingMobsCard : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private float _explosionBuff;

    public void GivePrize()
    {
        Player player = FindObjectOfType<StaticValues>().playerPrefab.GetComponent<Player>();

        if (!player.IsExploding)
        {
            player.IsExploding = true;
            player.Explosion = _explosion;
        }
        else
        {
            Explosion explosion = player.Explosion.GetComponent<Explosion>();

            Debug.Log(player.Explosion.GetComponent<Explosion>().radius);

            explosion.radius *= _explosionBuff;
            explosion.damage *= _explosionBuff;

            Debug.Log(player.Explosion.GetComponent<Explosion>().radius);
        }

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}