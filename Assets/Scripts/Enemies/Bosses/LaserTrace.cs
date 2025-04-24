using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class LaserTrace : MonoBehaviour
{
    [SerializeField] private float _areaRadius, _timeBtwActivation, _lifeTime;
    [SerializeField] private Transform _point;

    Transform laser, point;
    private void Start()
    {
        float x = Random.Range(-_areaRadius, _areaRadius);
        float y = Mathf.Sqrt(_areaRadius * _areaRadius - x * x);
        y = Random.Range(0, 2) == 0 ? -y : y;

        point = Instantiate(_point, new Vector2(x, y), Quaternion.identity);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(point.position.y - transform.position.y, point.position.x - transform.position.x) * Mathf.Rad2Deg);

        laser = transform.GetChild(0);
        laser.localScale = new Vector2(laser.localScale.x * Vector2.Distance(transform.position, point.position), laser.localScale.y);

        StartCoroutine(Activation());
        Destroy(gameObject, _lifeTime);
    }

    private void OnDestroy()
    {
        if (point != null)
            Destroy(point.gameObject);
    }

    private IEnumerator Activation()
    {
        if (laser == null) yield break;

        laser.GetComponentInChildren<Collider2D>().enabled = false;

        yield return new WaitForSeconds(_timeBtwActivation);

        laser.GetComponentInChildren<Collider2D>().enabled = true;
        laser.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        laser.GetComponentInChildren<Light2D>().enabled = true;
    }
}
