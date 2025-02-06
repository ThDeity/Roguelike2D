using System.Collections.Generic;
using UnityEngine;

public class SpawnPrize : MonoBehaviour
{
    [SerializeField] private Transform _prizePoint;
    [Tooltip("0 - parametres, 1 - active skills, 2 - passive skills, 3 - enemy's param")]
    [SerializeField] private List<GameObject> _prizes;

    [SerializeField] private GameObject _portal;
    [Tooltip("At least 3 points")]
    [SerializeField] private List<Transform> _portalsPoints;
    private List<GameObject> _portals = new List<GameObject>();

    private void Start()
    {
        int numberOfPortals = Random.Range(1, 3);
        for (int i = 0; i < numberOfPortals; i++)
        {
            GameObject portal = Instantiate(_portal, _portalsPoints[i]);
            _portals.Add(portal);
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
            default:
                StaticValues.WasPrizeGotten = true;
                break;
        }
    }
}
