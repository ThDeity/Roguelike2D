using NavMeshPlus.Components;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject _buttonIcon;
    [SerializeField] private int _roomsPerArea, _roomsCount, _index;
    [SerializeField] private Transform _pointForPrize, _pointForButton;

    [Tooltip("0 - Parametres, 1 - ActiveSkills, 2 - PassiveSkills, 3 - Enemy, 4 - Default, 5 - Boss")]
    [SerializeField] private List<GameObject> _prizes;

    [SerializeField] private List<GameObject> _areas0;
    [SerializeField] private List<GameObject> _areas1;
    [SerializeField] private List<GameObject> _areas2;
    [SerializeField] private List<GameObject> _bosses;

    private List<List<GameObject>> _areas = new List<List<GameObject>>();
    private GameObject _icon, _buttonE, _currentArea;

    private static int NumOfArea;
    private void Awake()
    {
        _currentArea = FindObjectOfType<SpawnPrize>().gameObject;

        _areas.Add(_areas0);
        _areas.Add(_areas1);
        _areas.Add(_areas2);

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

    private void OnDestroy() => Destroy(_icon);

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            var _surface = FindObjectOfType<NavMeshSurface>();
            _surface.RemoveData();

            Destroy(_currentArea.gameObject);
            StaticValues.RoomsBeforeBoss += 1;
            
            if (StaticValues.RoomsBeforeBoss % _roomsPerArea == 0 && StaticValues.RoomsBeforeBoss != 0)
            {
                StaticValues.CurrentRoomType = StaticValues.RoomTypes[5];

                _currentArea = Instantiate(_bosses[NumOfArea]);
                StaticValues.RoomsBeforeBoss = 0;
                NumOfArea += 1;
            }
            else if (StaticValues.RoomsBeforeBoss % _roomsPerArea != 0 || StaticValues.RoomsBeforeBoss == 0)
            {
                StaticValues.CurrentRoomType = StaticValues.RoomTypes[_index];

                int index = Random.Range(0, _areas[NumOfArea].Count);
                _currentArea = Instantiate(_areas[NumOfArea][index]);
            }

            GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
            StaticValues.EnemiesPoint.Clear();
            foreach (GameObject p in points)
                StaticValues.EnemiesPoint.Add(p.transform);

            if (_currentArea.TryGetComponent(out SpawnPrize component))
                collision.transform.position = component.playerPointSpawn == null ? Vector2.zero : component.playerPointSpawn.position;
        }
    }
}