using UnityEngine;

public class SlideCard : Card
{
    [SerializeField] private float _buffSpeed, _debuffSpeed, _buffMxHp, _reloadCd, _buff;

    protected override void Start()
    {
        base.Start();

        if (_currentLanguage == SystemLanguage.English)
            _description.text = $"Slide \n +{(_buffSpeed - 1) * 100}% Speed \n +{(_buffMxHp - 1) * 100}% HP \n -{(_debuffSpeed - 1) * 100}% Speed while attacking\n -{(_reloadCd - 1) * 100}% Reload";
        else if (_currentLanguage == SystemLanguage.Russian)
            _description.text = $"Слайд \n +{(_buffSpeed - 1) * 100}% Скорость \n +{(_buffMxHp - 1) * 100}% ХП \n -{(_debuffSpeed - 1) * 100}% Скорость после атаки\n -{(_reloadCd - 1) * 100}% Перез-ка";
    }

    public void GivePrize()
    {
        StaticValues.PlayerObj.ChangeMxHp(_buffMxHp);
        StaticValues.PlayerTransform.TryGetComponent(out Slide steal);

        if (steal == null)
        {
            steal = StaticValues.PlayerTransform.gameObject.AddComponent<Slide>();
            steal.increaseSpeed = _buffSpeed;
            steal.decreaseSpeed = _debuffSpeed;
            steal.reloadTime = _reloadCd;

            StaticValues.PlayerAttackList[ 0].bullet.GetComponent<BulletsComponents>().SetComponent(typeof(ShotDetector));
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