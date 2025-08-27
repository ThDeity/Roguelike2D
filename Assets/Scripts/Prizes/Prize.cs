using UnityEngine;
using UnityEngine.UI;
using YG;

public class Prize : MonoBehaviour
{
    [SerializeField] protected GameObject _buttonE;
    [SerializeField] protected AudioClip _sound;
    [SerializeField] protected Text _countOfRerolls;
    [SerializeField] protected string _idAdv;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            _buttonE.SetActive(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            _buttonE.gameObject.SetActive(false);
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
