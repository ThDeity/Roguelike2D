using UnityEngine;

public class RadarShotRoll : MonoBehaviour, Roll
{
    public RadarShot radarShot;

    public void OnRollFinished() => Instantiate(radarShot, transform.position, Quaternion.identity);
}
