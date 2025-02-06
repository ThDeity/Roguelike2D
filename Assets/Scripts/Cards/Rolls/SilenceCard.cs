using UnityEngine;

public class SilenceCard : Card
{
    [SerializeField] private float _hpDebuff, _rollCdDebuff, _radiusBuff;
    [SerializeField] private Silence _silence;

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
