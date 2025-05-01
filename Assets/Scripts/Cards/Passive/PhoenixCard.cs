using UnityEngine;

public class PhoenixCard : Card
{
    [SerializeField] private float _hpDebuff, _hpPersentAfterDeath;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Phoenix \n {(1 - _hpPersentAfterDeath) * 100}% Hp after death \n -{(1 - _hpDebuff) * 100}% HP \n +1 Life";
    }

    public void GivePrize()
    {
        Player player = StaticValues.PlayerObj;

        player.ChangeMxHp(_hpDebuff);
        player.hpAfterDeath = _hpPersentAfterDeath;
        player.lifesCount++;

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
