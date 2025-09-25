using HeneGames.Sceneloader;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayersUI : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private AudioClip _click;
    [SerializeField] private bool _isMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isMenu)
            PauseOrResume();

        if (Time.timeScale == 0 && Input.GetMouseButtonDown(0))
            StaticValues.PlayerObj.effectsSource.PlayOneShot(_click);
    }

    public void PauseOrResume()
    {
        _pausePanel.SetActive(!_pausePanel.activeSelf);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void Menu() => Camera.main.GetComponent<LoadingScreen>().LoadScene(0);//SceneManager.LoadScene(0);

    public void Quit() => Application.Quit();
}
