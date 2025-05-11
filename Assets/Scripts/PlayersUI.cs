using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayersUI : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private bool _isMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isMenu)
            PauseOrResume();
    }

    public void PauseOrResume()
    {
        _pausePanel.SetActive(!_pausePanel.activeSelf);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void Menu() => SceneManager.LoadScene(0);

    public void Quit() => Application.Quit();
}
