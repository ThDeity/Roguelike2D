using UnityEngine;

public class ExplosionBullets : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;

    private void Boom()
    {
        Transform explosion = Instantiate(_explosion, transform.position, Quaternion.identity).transform;
        explosion.localScale = transform.localScale;
        explosion.GetComponent<Explosion>().radius *= transform.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        Boom();
        //Instantiate(_explosion, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.isTrigger) return;

        Boom();
        //Instantiate(_explosion, transform.position, Quaternion.identity);
    }
}
