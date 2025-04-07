using UnityEngine;
using System.Collections;

public class HexagonTopAttack : RangeAttack
{
    [SerializeField] protected float _longCd;
    [SerializeField] protected int _shotCount;

    public IEnumerator Attacks()
    {
        for (int i = 0; i < _shotCount - 1; i++)
        {
            Shot();
            yield return new WaitForSeconds(reloadTime);
        }

        Shot();
    }
}
