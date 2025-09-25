using UnityEngine;

public class MineCard : Card
{
    [SerializeField] private GameObject _mine;
    [SerializeField] private float _rollDebuff, _mineSizeBuff;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Mine Roll \n +{_rollDebuff} Roll CD";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Рывок с Миной\n +{_rollDebuff} КД Рывка";
    }

    public override void GivePrize()
    {
        base.GivePrize();

        if (!StaticValues.PlayerMovementObj.TryGetComponent(out MineRoll mineRoll))
        {
            mineRoll = StaticValues.PlayerObj.gameObject.AddComponent<MineRoll>();

            mineRoll.mine = _mine;
            mineRoll.mine.GetComponent<Mine>().sizeBuff = 1;
        }
        else
        {
            Mine minePrefab = mineRoll.mine.GetComponent<Mine>();
            minePrefab.transform.localScale *= _mineSizeBuff;
            minePrefab.sizeBuff *= _mineSizeBuff;
        }

        SetRollParam(_rollDebuff);
    }
}
