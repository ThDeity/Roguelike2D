using UnityEngine;

public class SawCard : Card
{
    [SerializeField] private Saw _saw;
    [SerializeField] private float _rollCdDebuff, _sizeBuff, _intervalBuff;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Saw Roll \n +{_rollCdDebuff} Roll CD";
    }

    public void GivePrize()
    {
        if (!StaticValues.PlayerMovementObj.TryGetComponent(out SawRoll sawRoll))
        {
            sawRoll = StaticValues.PlayerObj.gameObject.AddComponent<SawRoll>();

            sawRoll.saw = _saw;
            SetRollParam(_saw.lifeTime);
        }
        else
        {
            sawRoll.saw.damage *= _sizeBuff;
            sawRoll.saw.transform.localScale *= _sizeBuff;
            sawRoll.saw.intervalBtwDmg *= _intervalBuff;
        }

        SetRollParam(_rollCdDebuff);
    }
}