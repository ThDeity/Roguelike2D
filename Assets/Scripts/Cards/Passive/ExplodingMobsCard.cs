using UnityEngine;

public class ExplodingMobsCard : Card
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private float _explosionBuff;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Exploding Mobs \n";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Exploding Mobs \n";
    }

    public override void GivePrize()
    {
        base.GivePrize();

        Player player = StaticValues.PlayerObj;

        if (!player.IsExploding)
        {
            player.IsExploding = true;
            player.explosion = _explosion;
        }
        else
        {
            Explosion explosion = player.explosion.GetComponent<Explosion>();

            Debug.Log(player.explosion.GetComponent<Explosion>().radius);

            explosion.radius *= _explosionBuff;
            explosion.damage *= _explosionBuff;

            Debug.Log(player.explosion.GetComponent<Explosion>().radius);
        }

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}