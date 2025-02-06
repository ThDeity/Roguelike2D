using UnityEngine;

public class ImplodeCard : Card
{
    [SerializeField] private float _hpBuff, _rollCdDebuff, _sizeBuff;//hpBuff ��������� � ���� ����������� ����� (�� ���������� �� ��� ��������)
    [SerializeField] private Implode _implode;

    public void GivePrize()
    {
        SetRollParam(_rollCdDebuff);
        StaticValues.PlayerObj.ChangeMxHp(_hpBuff);

        if (!StaticValues.PlayerMovementObj.TryGetComponent(out ImplodeRoll implodeRoll))
        {
            implodeRoll = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<ImplodeRoll>();
            implodeRoll.implode = _implode;
        }
        else
        {
            implodeRoll.implode.radius *= _sizeBuff;
            implodeRoll.implode.damage *= _sizeBuff;
        }
    }
}
