using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private float _radius, _offset;
    private GameObject _pointer;
    [SerializeField] private Transform _target;

    private void Start()
    {
        _pointer = transform.GetChild(0).gameObject;
        _pointer.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_target != null && !StaticValues.WasPrizeGotten && _target.gameObject.activeSelf && SpawnEnemies.Enemies.Count < 3)
        {
            Vector3 difference = _target.position - transform.position;
            float roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, roatZ + _offset);

            if ((_target.position - transform.position).sqrMagnitude < _radius * _radius)
                _pointer.SetActive(false);
            else
                _pointer.SetActive(true);
        }
        else
            _pointer.SetActive(false);
    }

    public void GetTarget()
    {
        _target = null;
        float minDistance = 10000f;

        foreach (var enemy in SpawnEnemies.Enemies)
        {
            if (enemy != null && Vector2.Distance(enemy.transform.position, transform.position) < minDistance)
            {
                minDistance = Vector2.Distance(enemy.transform.position, transform.position);
                _target = enemy.transform;
            }
        }

        if (_target != null)
            _pointer.SetActive(true);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _pointer.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Color color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
