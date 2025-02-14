using System.Collections;
using UnityEngine;

public class Mortira : Enemy
{
    [SerializeField] private float _timeBtwShot, _timeToDmg, _damage;
    [SerializeField] private GameObject _bullet, _zone;
    private Transform _currentZone;

    public IEnumerator Attack()
    {
        _currentZone = Instantiate(_zone, target.position, Quaternion.identity).transform;
        Vector2 pos = _currentZone.position;

        yield return new WaitForSeconds(_timeBtwShot);

        Instantiate(_bullet, new Vector2(pos.x, pos.y + _zone.transform.localScale.y * 0.5f), Quaternion.identity);
        Destroy(_currentZone.gameObject);

        yield return new WaitForSeconds(_timeToDmg);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, _zone.transform.localScale.y * 0.5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.transform.tag == target.tag)
                collider.GetComponent<IDamagable>().TakeDamage(_damage, 0);
        }
    }

    protected override void Update()
    {
        _time -= Time.deltaTime;

        if (isCharmed)
            _debuffs.FindEnemy();
    }
}
