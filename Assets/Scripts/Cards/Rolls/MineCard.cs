using UnityEngine;

public class MineCard : Card
{
    [SerializeField] private GameObject _mine;
    [SerializeField] private float _rollDebuff, _mineSizeBuff;

    public void GivePrize()
    {
        if (!StaticValues.PlayerMovementObj.TryGetComponent(out MineRoll mineRoll))
        {
            mineRoll = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<MineRoll>();

            mineRoll.mine = _mine;
            mineRoll.mine.GetComponent<Mine>().sizeBuff = 1;
        }
        else
        {
            Mine minePrefab = mineRoll.mine.GetComponent<Mine>();
            minePrefab.transform.localScale *= _mineSizeBuff;
            minePrefab.sizeBuff *= _mineSizeBuff;
        }

        SetRollParam(_rollDebuff);
    }
}
