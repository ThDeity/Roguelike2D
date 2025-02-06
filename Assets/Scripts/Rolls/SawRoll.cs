using UnityEngine;

public class SawRoll : MonoBehaviour, Roll
{
    public Saw saw;

    public virtual void OnRollFinished() => Instantiate(saw, transform);
}
