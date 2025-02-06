using UnityEngine;
using System.Collections;

public class HexagonTopAttack : RangeAttack
{
    [SerializeField] protected float _longCd;
    [SerializeField] protected int _shotCount;

    private IEnumerator Attacks()
    {
        if (_time <= 0)
        {
            for (int i = 0; i < _shotCount - 1; i++)
            {
                Shot();
                yield return new WaitForSeconds(reloadTime);
            }

            Shot();
            _time = _longCd;
        }
    }
}
