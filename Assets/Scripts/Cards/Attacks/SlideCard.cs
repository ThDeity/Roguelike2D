using UnityEngine;

public class SlideCard : Card
{
    [SerializeField] private float _buffSpeed, _debuffSpeed, _buffMxHp, _reloadCd, _buff;

    protected override void Start()
    {
        base.Start();
        _description.text = $"Slide \n +{(_buffSpeed - 1) * 100}% Speed \n +{(_buffMxHp - 1) * 100}% HP \n -{(_debuffSpeed - 1) * 100}% Speed while attacking\n -{(_reloadCd - 1) * 100}% Reload";
    }

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