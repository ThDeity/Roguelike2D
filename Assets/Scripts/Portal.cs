using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] private List<string> _sceneList = new List<string>();

    [Tooltip("0 - Parametres, 1 - ActiveSkills, 2 - PassiveSkills, 3 - Enemy, 4 - Default")]
    [SerializeField] private List<GameObject> _prizes = new List<GameObject>();
    
    int index;
    GameObject icon;
    private void Awake()
    {
        index = Random.Range(0, StaticValues.RoomTypes.Count - 1);
        icon = Instantiate(_prizes[index], _point.position, Quaternion.identity);
        gameObject.SetActive(false);
        icon.SetActive(false);
    }

    private void OnEnable() => icon.SetActive(true);

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            StaticValues.CurrentRoomType = StaticValues.RoomTypes[index];
            SceneManager.LoadScene(_sceneList[Random.Range(0, _sceneList.Count - 1)]);
        }
    }
}
