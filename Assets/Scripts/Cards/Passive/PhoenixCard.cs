using UnityEngine;

public class PhoenixCard : Card
{
    [SerializeField] private float _hpDebuff, _hpPersentAfterDeath;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Phoenix\n {(1 - _hpPersentAfterDeath) * 100}% Hp after death\n -{(1 - _hpDebuff) * 100}% HP\n +1 Life";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Феникс\n {(1 - _hpPersentAfterDeath) * 100}% ХП после смерти\n -{(1 - _hpDebuff) * 100}% ХП\n +1 Жизнь";
    }

    public override void GivePrize()
    {
        base.GivePrize();

        Player player = StaticValues.PlayerObj;

        player.ChangeMxHp(_hpDebuff);
        player.hpAfterDeath = _hpPersentAfterDeath;
        player.lifesCount++;

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
