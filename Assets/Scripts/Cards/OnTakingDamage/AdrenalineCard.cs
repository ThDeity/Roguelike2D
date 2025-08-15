using UnityEngine;

public class AdrenalineCard : Card
{
    [SerializeField] private float _buffTime, _interval, _buff, _buffCd, _cardBuff;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Adrenaline\n {(_buffTime - 1) * 100}s Time of buffs\n {_interval}s Interval\n +{(_buff - 1) * 100}% Speed, Dash, DMG\n -{(_buffCd - 1) * 100}% Reload";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Адреналин\n {(_buffTime - 1) * 100}с Время бафа\n {_interval}с Интервал\n +{(_buff - 1) * 100}% Скорости, ДМГ, Дэша\n -{(_buffCd - 1) * 100}% Перез-ка";
    }

    public void GivePrize()
    {
        if (!StaticValues.PlayerObj.TryGetComponent(out Adrenaline component))
        {
            component = StaticValues.PlayerObj.gameObject.AddComponent<Adrenaline>();

            component.buff = _buff;
            component.buffCd = _buffCd;
            component.buffTime = _buffTime;
            component.interval = _interval;
        }
        else
        {
            component.buffTime *= _cardBuff;
            component.buff *= _cardBuff;
            component.interval *= 2 - _cardBuff;
            component.buffCd *= 2 - _cardBuff;
        }

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}
