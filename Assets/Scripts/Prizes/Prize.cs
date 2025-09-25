using UnityEngine;
using UnityEngine.UI;
using YG;
using static UnityEngine.AudioSettings;

public class Prize : MonoBehaviour, Interactive
{
    [SerializeField] protected GameObject _buttonE;
    [SerializeField] protected AudioClip _sound;
    [SerializeField] protected Text _countOfRerolls;
    [SerializeField] protected string _idAdv;

    public virtual void Interact() { }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!PlatformController.IsMobile)
                _buttonE.SetActive(true);
            else
            {
                Debug.Log("hehehehehe");

                StaticValues.PlayerObj.interactionButton.SetActive(true);
                StaticValues.InteractButtonObj.interactiveObj = this;
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!PlatformController.IsMobile)
                _buttonE.SetActive(false);
            else
                StaticValues.PlayerObj.interactionButton.SetActive(false);
        }
    }

    protected virtual void OnEnable()
    {
        if (_countOfRerolls != null)
        {
            if (StaticValues.CountOfRerolls > StaticValues.CurrentCountOfRerolls)
                _countOfRerolls.text = $"x{StaticValues.CountOfRerolls - StaticValues.CurrentCountOfRerolls}";
            else
                _countOfRerolls.enabled = false;
        }
    }

    public void ShowRewardAdv_UseCallback()
    {
        YG2.RewardedAdvShow(_idAdv, () =>
        {
            // Получение вознаграждения
            SetReward();
        });
    }

    protected virtual void SetReward() { }
}
