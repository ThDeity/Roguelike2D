using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

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

    private Transform[] areas0 = new Transform[5], areas1 = new Transform[5], areas2 = new Transform[5];
    public static Transform[][] Areas = new Transform[3][];
    public static Transform[] Bosses = new Transform[3];

    public GameObject playerPrefab;
    public List<PlayerAttack> attacks = new List<PlayerAttack>();

    public static float EnemyMaxHp, EnemySpeed, EnemyDamage, EnemyCount, EnemyCrit;

    public bool isMenu;

    public static int RoomsBeforeBoss;
    private void Awake()
    {
        if (!isMenu)
        {
            if (EnemyMaxHp == 0)
                EnemyMaxHp = EnemySpeed = EnemyDamage = EnemyCount = EnemyCrit = 1;

            PlayerObj = FindObjectOfType<Player>();
            PlayerTransform = PlayerObj.transform;
            PlayerMovementObj = PlayerObj.GetComponent<PlayerMovement>();
            PlayerAttackList = PlayerObj.transform.GetChild(0).GetComponentsInChildren<PlayerAttack>().ToList();
            attacks = PlayerAttackList;

            GameObject[] objects = GameObject.FindGameObjectsWithTag("Area");
            for (int y = 0; y < objects.Length; y++)
            {
                for (int i = 0; i < objects[y].transform.childCount; i++)
                {
                    if (objects[y].name == "0")
                        areas0[i] = objects[y].transform.GetChild(i);
                    else if (objects[y].name == "1")
                        areas1[i] = objects[y].transform.GetChild(i);
                    else if (objects[y].name == "2")
                        areas2[i] = objects[y].transform.GetChild(i);
                    else
                        Bosses[i] = objects[y].transform.GetChild(i);

                    objects[y].transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            Areas = new Transform[][] {  areas0, areas1, areas2 };
            areas0 = areas1 = areas2 = null;

            RoomTypes = new List<string>() { "Parametres", "ActiveSkills", "PassiveSkills", "Enemy", "Default", "Boss" };
            if (CurrentRoomTypes.Count == 0 || CurrentRoomType == "Boss")
            {
                foreach (var name in RoomTypes)
                {
                    if (CurrentRoomTypes.ContainsKey(name))
                        CurrentRoomTypes[name] = 100;
                    else
                        CurrentRoomTypes.Add(name, 100);
                }
            }

            if (CurrentRoomType == "Boss" && WasPrizeGotten)
            {
                EnemyMaxHp = EnemyDamage += 1;
                EnemyCount += 0.5f;
                EnemySpeed += 0.1f;

                playerPrefab.transform.localScale = Vector2.one;
            }

            RoomsBeforeBoss = 0;

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

    private void Start()
    {
        if (isMenu) return;

        if (Areas != null && Areas.Length > 0 && Areas[0].Length > 0 && Areas[0] != null && Areas[0][0] != null)
            Areas[0][0].gameObject.SetActive(true);
        else
            Areas = null;
    }

    public void SetDifficulty(float value)
    {
        EnemyMaxHp = EnemySpeed = EnemyDamage = EnemyCount = EnemyCrit = value;
        Debug.Log(EnemyMaxHp);
    }

    public void Restart()
    {
        foreach (var script in playerPrefab.GetComponents<MonoBehaviour>())
        {
            if (script == playerPrefab.GetComponent<Player>() || script == playerPrefab.GetComponent<DebuffsEffects>())
                continue;

            DestroyImmediate(script, true);
        }

        CurrentRoomType = "ActiveSkills";
        foreach (var name in RoomTypes)
        {
            if (CurrentRoomTypes.ContainsKey(name))
                CurrentRoomTypes[name] = 100;
            else
                CurrentRoomTypes.Add(name, 100);
        }

        playerPrefab.AddComponent<PlayerMovement>().Reset();
        playerPrefab.GetComponent<Player>().Reset2();
        playerPrefab.transform.localScale = Vector2.one;

        PlayerAttackList[0].bullet.GetComponent<BulletsComponents>().Reset2();
        Bullet bull = PlayerAttackList[0].bullet.GetComponent<Bullet>();
        bull.Reset();

        playerPrefab.transform.GetChild(0).GetChild(0).gameObject.GetComponents<PlayerAttack>().ToList().ForEach(x => DestroyImmediate(x, true));
        PlayerAttack attack = playerPrefab.transform.GetChild(0).GetChild(0).gameObject.AddComponent<PlayerAttack>();
        attack.Reset();

        if (playerPrefab.TryGetComponent(out Skill skill))
        {
            skill.ResetAll();
            DestroyImmediate(playerPrefab.GetComponent<Skill>(), true);
        }

        RoomsBeforeBoss = 0;
        Portal.NumOfArea = 0;

        SceneManager.LoadScene(1);
    }

    public static void ResetStatics() =>
        EnemyMaxHp = EnemySpeed = EnemyDamage = EnemyCount = EnemyCrit = 1;
}
