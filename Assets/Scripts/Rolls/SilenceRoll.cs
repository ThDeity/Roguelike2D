using UnityEngine;

public class SilenceRoll : MonoBehaviour, Roll
{
    public Silence silence;

    public virtual void OnRollFinished() => Instantiate(silence, transform.position, Quaternion.identity);
}
