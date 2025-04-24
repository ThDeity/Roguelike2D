using System.Collections.Generic;
using UnityEngine;

public class SpawnPrize : MonoBehaviour
{
    [SerializeField] private Transform _prizePoint;
    [Tooltip("0 - Parametres, 1 - ActiveSkills, 2 - PassiveSkills, 3 - Enemy, 4 - Default, 5 - Boss")]
    [SerializeField] private List<GameObject> _prizes;
    protected static int Rooms = 2;

    [SerializeField] private GameObject _portal;
    [Tooltip("At least 3 points")]
    [SerializeField] private List<Transform> _portalsPoints;
    private List<GameObject> _portals = new List<GameObject>();

    private HashSet<int> _types = new HashSet<int>();
    int _index;
    private void Start()
    {
        if (StaticValues.RoomsBeforeBoss % Rooms != Rooms - 1 || StaticValues.RoomsBeforeBoss % Rooms != 0)
        {
            int numberOfPortals = Random.Range(1, 3);
            for (int y = 0; y < numberOfPortals; y++)
            {
                GameObject portal = Instantiate(_portal, _portalsPoints[y]);
                _portals.Add(portal);
            }
        }
        else
        {
            GameObject portal = Instantiate(_portal, _portalsPoints[0]);
            _portals.Add(portal);
        }

        int i = 0;
        while (_types.Count != _portals.Count)
        {
            _index = StaticValues.RoomsBeforeBoss % Rooms != 0 ? Random.Range(0, StaticValues.RoomTypes.Count - 2) : 5;

            if (_types.Contains(_index) || StaticValues.CurrentRoomTypes[StaticValues.RoomTypes[_index]] == 0)
                continue;

            _portals[i].GetComponent<Portal>().SetPrize(_index);
            _portals[i].SetActive(false);
            i += 1;

            _types.Add(_index);
            StaticValues.CurrentRoomTypes[StaticValues.RoomTypes[_index]] -= 1;
        }
    }

    private void Update()
    {
        if (StaticValues.WasPrizeGotten)
            LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        for (int i = 0; i < _portals.Count; i++)
            _portals[i].SetActive(true);
    }

    private void InstantiatePrize(int i) => Instantiate(_prizes[i], _prizePoint);

    public void GivePrize()
    {
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
                InstantiatePrize(4);
                break;
            default:
                StaticValues.WasPrizeGotten = true;
                break;
        }
    }
}
