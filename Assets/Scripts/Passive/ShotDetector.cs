using UnityEngine;

public class ShotDetector : MonoBehaviour
{
    private void Start() => StaticValues.PlayerObj.GetComponent<Slide>().ChangeTime();
}
