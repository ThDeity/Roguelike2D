using UnityEngine;

public class MineRoll : MonoBehaviour, Roll
{
    public GameObject mine;

    public virtual void OnRollStarted() => Instantiate(mine, transform.position, Quaternion.identity);
}
