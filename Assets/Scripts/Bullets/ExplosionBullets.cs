using UnityEngine;

public class ExplosionBullets : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;

    private void Start()
    {
        if (!isActiveAndEnabled)
            return;
    }

    private void Boom()
    {
        Transform explosion = Instantiate(_explosion, transform.position, Quaternion.identity).transform;
        explosion.localScale = transform.localScale;
        explosion.GetComponent<Explosion>().radius *= transform.localScale.x;
    }

    private void Reset() => _explosion = null;

    private void OnDisable() => Reset();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger || !isActiveAndEnabled) return;

        Boom();
        //Instantiate(_explosion, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.isTrigger || !isActiveAndEnabled) return;

        Boom();
        //Instantiate(_explosion, transform.position, Quaternion.identity);
    }
}
