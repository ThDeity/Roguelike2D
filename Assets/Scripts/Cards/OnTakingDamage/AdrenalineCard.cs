using UnityEngine;

public class AdrenalineCard : Card
{
    [SerializeField] private float _buffTime, _interval, _buff, _buffCd, _cardBuff;

    public void GivePrize()
    {
        if (!StaticValues.PlayerObj.TryGetComponent(out Adrenaline component))
        {
            component = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<Adrenaline>();

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
