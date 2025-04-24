using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject _buttonIcon;
    [SerializeField] private int _roomsPerArea, _roomsCount, _index;
    [SerializeField] private Transform _pointForPrize, _pointForButton;

    [Tooltip("0 - Parametres, 1 - ActiveSkills, 2 - PassiveSkills, 3 - Enemy, 4 - Default, 5 - Boss")]
    [SerializeField] private List<GameObject> _prizes = new List<GameObject>();
    
    GameObject _icon, _buttonE;
    private void Awake()
    {
        mainMenuSceneIndex = SceneManager.GetActiveScene().buildIndex;
        _buttonE = Instantiate(_buttonIcon, _pointForButton.position, Quaternion.identity);
        _buttonIcon.SetActive(false);
    }

    public void SetPrize(int index)
    {
        _index = index;

        _icon = Instantiate(_prizes[index], _pointForPrize.position, Quaternion.identity);
        gameObject.SetActive(false);
        _icon.SetActive(false);
    }

    private void OnEnable()
    {
        if (_icon != null)
            _icon.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            _buttonE.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _buttonE != null)
            _buttonE.gameObject.SetActive(false);
    }

    private int mainMenuSceneIndex;

    IEnumerator loadScene(int index)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        scene.allowSceneActivation = false;

        while (scene.progress < 0.9f)
        {
            Debug.Log("Loading scene " + index + " Progress: " + scene.progress);
            yield return null;
        }

        scene.allowSceneActivation = true;

        while (!scene.isDone)
            yield return null;

        OnSceneLoaded(index);
    }

    void OnSceneLoaded(int loadedSceneIndex)
    {
        Debug.Log("Scene " + loadedSceneIndex + " fully loaded");

        Scene loadedScene = SceneManager.GetSceneByBuildIndex(loadedSceneIndex);

        if (loadedScene.IsValid())
        {
            Debug.Log("Setting scene " + loadedScene.name + " as active");

            SceneManager.MoveGameObjectToScene(StaticValues.PlayerObj.gameObject, loadedScene);

            SceneManager.SetActiveScene(loadedScene);

            StartCoroutine(UnloadMenuScene());
        }
        else
        {
            Debug.LogError("Loaded scene is not valid!");
        }
    }

    IEnumerator UnloadMenuScene()
    {
        // Даем время на стабилизацию
        yield return new WaitForSeconds(0.1f);

        Scene menuScene = SceneManager.GetSceneByBuildIndex(mainMenuSceneIndex);
        if (menuScene.IsValid() && menuScene.isLoaded)
        {
            Debug.Log("Unloading menu scene");
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(mainMenuSceneIndex);

            while (!unloadOperation.isDone)
            {
                yield return null;
            }

            Debug.Log("Menu scene unloaded successfully");

            // Очистка ресурсов после выгрузки сцены
            //Resources.UnloadUnusedAssets();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            if (StaticValues.RoomsBeforeBoss % _roomsPerArea != 0)
            {
                StaticValues.CurrentRoomType = StaticValues.RoomTypes[_index];
                
                StartCoroutine(loadScene(Random.Range(1, _roomsCount + 1) + 6 * (StaticValues.RoomsBeforeBoss / _roomsPerArea)));
            }
            else
            {
                StaticValues.CurrentRoomType = StaticValues.RoomTypes[5];
                Scene s = SceneManager.GetSceneByName(((StaticValues.RoomsBeforeBoss / _roomsPerArea).ToString()) + "_Boss");
                //StartCoroutine(loadScene(s));
            }
        }
    }
}
