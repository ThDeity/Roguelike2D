using UnityEngine;

public class SlideCard : Card
{
    [SerializeField] private float _buffSpeed, _debuffSpeed, _buffMxHp, _reloadCd, _buff;

    public void GivePrize()
    {
        StaticValues.PlayerObj.ChangeMxHp(_buffMxHp);
        FindObjectOfType<StaticValues>().playerPrefab.TryGetComponent(out Slide steal);

        if (steal == null)
        {
            steal = FindObjectOfType<StaticValues>().playerPrefab.AddComponent<Slide>();
            steal.increaseSpeed = _buffSpeed;
            steal.decreaseSpeed = _debuffSpeed;
            steal.reloadTime = _reloadCd;

            StaticValues.PlayerAttackList[ 0].bullet.AddComponent<ShotDetector>();
        }
        else
        {
            steal.increaseSpeed *= _buff;
            steal.decreaseSpeed *= _buff;
            steal.reloadTime *= _buff;
        }

        StaticValues.PassiveSkillsPanel.SetActive(false);
    }
}