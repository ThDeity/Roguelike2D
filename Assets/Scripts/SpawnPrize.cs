using System.Collections.Generic;
using UnityEngine;

public class SpawnPrize : MonoBehaviour
{
    public Transform playerPointSpawn;
    [SerializeField] private Transform _prizePoint;

    [Tooltip("0 - Parametres, 1 - ActiveSkills, 2 - PassiveSkills, 3 - Enemy, 4 - Default, 5 - Boss")]
    [SerializeField] private List<GameObject> _prizes;
    protected static int Rooms = 2;

    [SerializeField] private GameObject _portal;
    [Tooltip("At least 3 points")]
    [SerializeField] private List<Transform> _portalsPoints;

    public List<GameObject> _portals = new List<GameObject>();
    private HashSet<int> _types = new HashSet<int>();

    private static bool WasPrizeSpawn;

    int _index, _countOfEnables = 0;
    private void OnEnable()
    {
        WasPrizeSpawn = false;
        _countOfEnables += 1;

        if (_countOfEnables <= 0) return;

        if (StaticValues.RoomsBeforeBoss % Rooms != Rooms - 1)
        {
            int numberOfPortals = Random.Range(1, 4);
            for (int y = 0; y < numberOfPortals; y++)
            {
                GameObject portal = Instantiate(_portal, _portalsPoints[y]);
                _portals.Add(portal);
            }

            int i = 0, times = 0;
            List<int> indexes = new List<int>{ 0, 1, 2, 3, 4 };

            while (_types.Count < _portals.Count && times < 100)
            {
                _index = indexes[Random.Range(0, indexes.Count)];

                times++;
                if (_types.Contains(_index) || StaticValues.CurrentRoomTypes[StaticValues.RoomTypes[_index]] == 0)
                    continue;

                _portals[i].GetComponent<Portal>().SetPrize(_index);
                _portals[i].SetActive(false);
                i += 1;

                _types.Add(_index);
                StaticValues.CurrentRoomTypes[StaticValues.RoomTypes[_index]] -= 1;

                indexes.Remove(_index);
            }

            if (_types.Count < _portals.Count)
            {
                for (; i < _portals.Count; i++)
                {
                    _portals[i].GetComponent<Portal>().SetPrize(4);//Default
                    _portals[i].SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("Boss!");

            GameObject portal = Instantiate(_portal, _portalsPoints[0].position, Quaternion.identity);
            _portals.Add(portal);

            portal.GetComponent<Portal>().SetPrize(5);
        }
    }

    private void Update()
    {
        if (StaticValues.WasPrizeGotten)
            LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        Debug.Log(_portals.Count);

        for (int i = 0; i < _portals.Count; i++)
            _portals[i].SetActive(true);

        StaticValues.WasPrizeGotten = false;
    }

    private void InstantiatePrize(int i) => Instantiate(_prizes[i], _prizePoint);

    public void GivePrize()
    {
        if (WasPrizeSpawn) return;
        WasPrizeSpawn = true;

        string type = StaticValues.CurrentRoomType;
        switch (type)
        {
            case "Parametres":
                InstantiatePrize(0);
                break;
            case "ActiveSkills":
                InstantiatePrize(1);
                break;
            case "PassiveSkills":
                InstantiatePrize(2);
                break;
            case "Enemy":
                InstantiatePrize(3);
                break;
            case "Boss":
                InstantiatePrize(5);
                break;
            default:
                StaticValues.WasPrizeGotten = true;
                break;
        }
    }
}
