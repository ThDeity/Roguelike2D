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
                EnemyMaxHp = EnemySpeed = EnemyDamage = EnemyCount = EnemyCrit += 1;

                playerPrefab.transform.localScale = Vector2.one;
            }

            RoomsBeforeBoss = RoomsBeforeBoss > 0 ? RoomsBeforeBoss + 1 : 0;

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
                CurrentRoomTypes[name] = 3;
            else
                CurrentRoomTypes.Add(name, 3);
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

        EnemyMaxHp = EnemySpeed = EnemyDamage = EnemyCount = EnemyCrit = 1;

        RoomsBeforeBoss = 0;

        SceneManager.LoadScene(1);
    }
}
