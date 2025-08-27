using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class RevivalPanel : MonoBehaviour
{
    [SerializeField] private Text _timer;

    [Tooltip("Must be non negative & integer")]
    [SerializeField] private float _timeToThink;

    [SerializeField] private string _idAdv;

    private void Update()
    {
        _timeToThink -= Time.deltaTime;
        _timer.text = ((int) _timeToThink).ToString();

        if (_timeToThink <= 0)
            Discard();
    }

    public void Discard()
    {
        StaticValues.ResetStatics();

        SceneManager.LoadScene("Menu");
    }

    public void Revive()
    {
        YG2.RewardedAdvShow(_idAdv, () =>
        {
            // Получение вознаграждения
            SetReward();
        });
    }

    protected void SetReward()
    {
        StaticValues.PlayerObj.lifesCount += 1;

        gameObject.SetActive(false);
    }
}
