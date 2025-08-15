using UnityEngine;

public class LeachCard : Card
{
    [SerializeField] private float _lifeSteal, _mxHpBuff;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Leach \n +{_lifeSteal * 100}% Lifesteal \n +{(_mxHpBuff - 1) * 100}% HP";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Лич \n +{_lifeSteal * 100}% Кража ХП \n +{(_mxHpBuff - 1) * 100}% ХП";
    }

    public void GivePrize()
    {
        SetAttackParam(1, _lifeSteal, 1, 1);
        StaticValues.PlayerObj.ChangeMxHp(_mxHpBuff);
        FindObjectOfType<StaticValues>().playerPrefab.GetComponent<Player>().ChangeMxHp(_mxHpBuff);

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
