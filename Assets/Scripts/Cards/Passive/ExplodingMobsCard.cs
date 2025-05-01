using UnityEngine;

public class ExplodingMobsCard : Card
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private float _explosionBuff;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Exploding Mobs \n";
    }

    public void GivePrize()
    {
        Player player = StaticValues.PlayerObj;

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