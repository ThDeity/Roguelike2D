using UnityEngine;

public class SilenceCard : Card
{
    [SerializeField] private float _hpDebuff, _rollCdDebuff, _radiusBuff;
    [SerializeField] private Silence _silence;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Silence Roll \n +{_rollCdDebuff} Roll CD \n -{(1 - _hpDebuff) * 100}% HP";
    }

    public void GivePrize()
    {
        if (!StaticValues.PlayerMovementObj.TryGetComponent(out SilenceRoll silenceRoll))
        {
            silenceRoll = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<SilenceRoll>();
            silenceRoll.silence = _silence;
        }
        else
        {
            silenceRoll.silence.radius *= _radiusBuff;
            silenceRoll.silence.timeOfSilence *= _radiusBuff;
            silenceRoll.silence.transform.localScale *= _radiusBuff;
        }

        StaticValues.PlayerObj.ChangeMxHp(_hpDebuff);
        SetRollParam(_rollCdDebuff);
    }
}
