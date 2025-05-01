using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private float _xLeft, _xRight, _yUp, _yDown;

    [SerializeField] private List<GameObject> _enemiesObj;
    [SerializeField] private int _waves, _enemiesPerWave;
    [SerializeField] private Vector2 _boxSize;

    public static List<GameObject> Enemies = new List<GameObject>();

    private void Start()
    {
        Spawn();
        StaticValues.WasPrizeGotten = false;
        _enemiesPerWave = Mathf.CeilToInt(StaticValues.EnemyCount * _enemiesPerWave);
        _waves = Mathf.CeilToInt(StaticValues.EnemyCount * _waves);
    }

    private Vector2 RandomPos(int count = 0)
    {
        count += 1;
        Vector2 pos = new Vector2(Random.Range(_xLeft + transform.position.x, _xRight + transform.position.x),
                                        Random.Range(_yUp + transform.position.y, _yDown + transform.position.y));
        Collider2D collider = Physics2D.OverlapBox(pos, _boxSize, 0);
        if (collider == null || collider.isTrigger || count >= 5)
            return pos;
        else
            return RandomPos(count);
    }

    private void RandomCreation(List<GameObject> list, GameObject smth)
    {
        Vector2 pos = RandomPos();
        GameObject obj = Instantiate(smth, pos, Quaternion.identity);

        list.Add(obj);
    }

    private void Spawn()
    {
        int enemiesCount = (int)(_enemiesPerWave * StaticValues.EnemyCount);
        foreach (var enemy in _enemiesObj)
        {
            int count = Random.Range(1, enemiesCount);
            enemiesCount -= count;

            for (int i = 0; i < count; i++)
                RandomCreation(Enemies, enemy);
        }

        _waves--;
    }

    bool _wasPrizeGotten = false;
    private void Update()
    {
        if (Enemies.Count == 0)
        {
            if (_waves > 0)
                Spawn();
        }
    }

    private void LateUpdate()
    {
        if (Enemies.Count <= 0 && _waves <= 0 && !_wasPrizeGotten && !StaticValues.WasPrizeGotten)
        {
            FindObjectOfType<SpawnPrize>().GivePrize();
            _wasPrizeGotten = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(_xRight - _xLeft, _yUp - _yDown, 0));
    }
}
