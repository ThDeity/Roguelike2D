using UnityEngine;

public class ExtraRoll : Card
{
    [SerializeField] private float _rollCdDebuff;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Extra Roll \n +{_rollCdDebuff} Roll CD";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Дополнительный Рывок\n +{_rollCdDebuff} КД Рывка";
    }

    public void GivePrize()
    {
        StaticValues.PlayerMovementObj.rollsCount++;

        SetRollParam(_rollCdDebuff);
    }
}
