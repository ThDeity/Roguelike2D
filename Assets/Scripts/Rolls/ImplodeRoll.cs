using UnityEngine;

public class ImplodeRoll : MonoBehaviour, Roll
{
    public Implode implode;

    public virtual void OnRollStarted() => Instantiate(implode, transform.position, Quaternion.identity);
}
