using UnityEngine;

public class ExtraRoll : Card
{
    [SerializeField] private float _rollCdDebuff;

    public void GivePrize()
    {
        FindObjectOfType<StaticValues>().playerPrefab.GetComponent<PlayerMovement>().rollsCount++;

        SetRollParam(_rollCdDebuff);
    }
}
