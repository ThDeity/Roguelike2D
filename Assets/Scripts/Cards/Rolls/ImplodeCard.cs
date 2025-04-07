using UnityEngine;

public class ImplodeCard : Card
{
    [SerializeField] private float _hpBuff, _rollCdDebuff, _sizeBuff;//hpBuff указывать в виде дес€тичного числа (хп домнажатс€ на это значение)
    [SerializeField] private Implode _implode;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Implode Roll \n +{_rollCdDebuff} Roll CD \n +{(1 - _hpBuff) * 100}% HP";
    }

    public void GivePrize()
    {
        SetRollParam(_rollCdDebuff);
        StaticValues.PlayerObj.ChangeMxHp(_hpBuff);

        if (!StaticValues.PlayerMovementObj.TryGetComponent(out ImplodeRoll implodeRoll))
        {
            implodeRoll = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<ImplodeRoll>();
            implodeRoll.implode = _implode;
        }
        else
        {
            implodeRoll.implode.radius *= _sizeBuff;
            implodeRoll.implode.damage *= _sizeBuff;
        }
    }
}
