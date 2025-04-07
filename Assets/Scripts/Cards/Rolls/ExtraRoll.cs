using UnityEngine;

public class ExtraRoll : Card
{
    [SerializeField] private float _rollCdDebuff;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Extra Roll \n +{_rollCdDebuff} Roll CD";
    }

    public void GivePrize()
    {
        FindObjectOfType<StaticValues>().playerPrefab.GetComponent<PlayerMovement>().rollsCount++;

        SetRollParam(_rollCdDebuff);
    }
}
