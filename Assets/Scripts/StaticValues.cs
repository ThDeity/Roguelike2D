using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StaticValues : MonoBehaviour
{
    public static List<string> RoomTypes {  get; private set; }
    public static GameObject ParamPanel, EnemyParamPanel, PassiveSkillsPanel, ActiveSkillsPanel, SkillsTimer;
    public static string CurrentRoomType = "ActiveSkills";
    public static bool WasPrizeGotten;

    public static List<Transform> EnemiesPoint = new List<Transform>();
    public static List<PlayerAttack> PlayerAttackList;
    public static PlayerMovement PlayerMovementObj;
    public static Transform PlayerTransform;
    public static Player PlayerObj;

    public static float PlayerSize;
    public GameObject playerPrefab;
    public List<PlayerAttack> attacks = new List<PlayerAttack>();

    public static float EnemyMaxHp = 1, EnemySpeed = 1, EnemyDamage = 1, EnemyCount = 1, EnemyCrit = 1;

    public static int RoomsBeforeBoss;
    private void Awake()
    {
        PlayerObj = FindObjectOfType<Player>();
        PlayerTransform = PlayerObj.transform;
        PlayerMovementObj = PlayerObj.GetComponent<PlayerMovement>();
        PlayerAttackList = PlayerObj.transform.GetChild(0).GetComponentsInChildren<PlayerAttack>().ToList();
        attacks = PlayerAttackList;

        RoomTypes = new List<string>() { "Parametres", "ActiveSkills", "PassiveSkills", "Enemy", "Default", "Boss" };
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
