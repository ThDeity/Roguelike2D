using UnityEngine;

public class PlayersUI : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;

    public void PauseOrResume()
    {
        _pausePanel.SetActive(!_pausePanel.activeSelf);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void Quit() => Application.Quit();
}
