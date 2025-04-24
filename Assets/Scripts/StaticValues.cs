using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StaticValues : MonoBehaviour
{
    public static List<string> RoomTypes {  get; private set; }
    public static Dictionary<string, int> CurrentRoomTypes = new Dictionary<string, int>();

    public static GameObject ParamPanel, EnemyParamPanel, PassiveSkillsPanel, ActiveSkillsPanel, SkillsTimer;
    public static string CurrentRoomType = "PassiveSkills";
    public static bool WasPrizeGotten;

    public static List<Transform> EnemiesPoint = new List<Transform>();
    public static List<PlayerAttack> PlayerAttackList;
    public static PlayerMovement PlayerMovementObj;
    public static Transform PlayerTransform;
    public static Player PlayerObj;

    public GameObject playerPrefab;
    public List<PlayerAttack> attacks = new List<PlayerAttack>();

    public static float EnemyMaxHp = 1, EnemySpeed = 1, EnemyDamage = 1, EnemyCount = 1, EnemyCrit = 1;

    public bool isMenu;

    public static int RoomsBeforeBoss;
    private void Awake()
    {
        if (!isMenu)
        {
            PlayerObj = FindObjectOfType<Player>();
            PlayerTransform = PlayerObj.transform;
            PlayerMovementObj = PlayerObj.GetComponent<PlayerMovement>();
            PlayerAttackList = PlayerObj.transform.GetChild(0).GetComponentsInChildren<PlayerAttack>().ToList();
            attacks = PlayerAttackList;

            RoomTypes = new List<string>() { "Parametres", "ActiveSkills", "PassiveSkills", "Enemy", "Default", "Boss" };
            if (CurrentRoomTypes.Count == 0 || CurrentRoomType == "Boss")
            {
                foreach (var name in RoomTypes)
                {
                    if (CurrentRoomTypes.ContainsKey(name))
                        CurrentRoomTypes[name] = 3;
                    else
                        CurrentRoomTypes.Add(name, 3);
                }
            }

            if (CurrentRoomType == "Boss" && WasPrizeGotten)
            {
                EnemyMaxHp += 0.5f;
                EnemySpeed += 0.5f;
                EnemyDamage += 0.5f;
                EnemyCount += 0.5f;
                EnemyCrit += 0.5f;

                playerPrefab.transform.localScale = Vector2.one;
            }

            RoomsBeforeBoss = RoomsBeforeBoss > 0 ? RoomsBeforeBoss + 1 : 1;

            GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
            EnemiesPoint.Clear();
            foreach (GameObject p in points)
                EnemiesPoint.Add(p.transform);

            ParamPanel = FindObjectOfType<Param>().gameObject;
            ParamPanel.SetActive(false);

            EnemyParamPanel = FindObjectOfType<EnemyParam>().gameObject;
            EnemyParamPanel.SetActive(false);

            PassiveSkillsPanel = FindObjectOfType<PassiveSkill>().gameObject;
            PassiveSkillsPanel.SetActive(false);

            ActiveSkillsPanel = FindObjectOfType<ActiveSkill>().gameObject;
            SkillsTimer = ActiveSkillsPanel.GetComponent<ActiveSkill>().skillSlot.GetComponentInChildren<Text>().gameObject;
            SkillsTimer.SetActive(false);
            ActiveSkillsPanel.SetActive(false);
        }
    }

    private int mainMenuSceneIndex = 0;

    IEnumerator loadScene(int index)
    {
        // Загружаем новую сцену в аддитивном режиме
        AsyncOperation scene = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        scene.allowSceneActivation = false;

        // Ждем завершения загрузки (до 90%, так как allowSceneActivation = false)
        while (scene.progress < 0.9f)
        {
            Debug.Log("Loading scene " + index + " Progress: " + scene.progress);
            yield return null;
        }

        // Разрешаем активацию сцены
        scene.allowSceneActivation = true;

        // Ждем полной загрузки сцены
        while (!scene.isDone)
        {
            yield return null;
        }

        OnSceneLoaded(index);
    }

    void OnSceneLoaded(int loadedSceneIndex)
    {
        Debug.Log("Scene " + loadedSceneIndex + " fully loaded");

        // Получаем только что загруженную сцену
        Scene loadedScene = SceneManager.GetSceneByBuildIndex(loadedSceneIndex);

        if (loadedScene.IsValid())
        {
            Debug.Log("Setting scene " + loadedScene.name + " as active");

            // Переносим UI объект в новую сцену
            SceneManager.MoveGameObjectToScene(PlayerObj.gameObject, loadedScene);

            // Устанавливаем новую сцену как активную
            SceneManager.SetActiveScene(loadedScene);

            // Выгружаем сцену меню
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

    public void Restart()
    {
        foreach (var script in playerPrefab.GetComponents<MonoBehaviour>())
        {
            if (script == playerPrefab.GetComponent<Player>() || script == playerPrefab.GetComponent<DebuffsEffects>())
                continue;

            DestroyImmediate(script, true);
        }

        CurrentRoomType = "PassiveSkills";
        foreach (var name in RoomTypes)
        {
            if (CurrentRoomTypes.ContainsKey(name))
                CurrentRoomTypes[name] = 3;
            else
                CurrentRoomTypes.Add(name, 3);
        }

        playerPrefab.AddComponent<PlayerMovement>().Reset();
        playerPrefab.GetComponent<Player>().Reset2();
        playerPrefab.transform.localScale = Vector3.one;

        PlayerAttackList[0].bullet.GetComponents<MonoBehaviour>().ToList().ForEach(x => DestroyImmediate(x, true));
        Bullet bull = PlayerAttackList[0].bullet.AddComponent<Bullet>();
        bull.Reset();

        playerPrefab.transform.GetChild(0).GetChild(0).gameObject.GetComponents<PlayerAttack>().ToList().ForEach(x => DestroyImmediate(x, true));
        PlayerAttack attack = playerPrefab.transform.GetChild(0).GetChild(0).gameObject.AddComponent<PlayerAttack>();
        attack.Reset();

        if (playerPrefab.TryGetComponent(out Skill skill))
        {
            skill.ResetAll();
            DestroyImmediate(playerPrefab.GetComponent<Skill>(), true);
        }

        EnemyMaxHp = EnemySpeed = EnemyDamage = EnemyCount = EnemyCrit = 1;

        RoomsBeforeBoss = 0;

        StartCoroutine(loadScene(1));
    }
}
