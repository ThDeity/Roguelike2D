using DigitalRuby.LightningBolt;
using System.Collections;
using UnityEngine;

public class LightningBullets : MonoBehaviour
{
    public int maxEnemies;
    public float damage, dazzleTime, maxDistance, timeOfExistingBolt;
    public LightningBoltScript bolt;

    private IEnumerator Lightning(GameObject collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamagable component) && collision.gameObject.tag != "Player")
        {
            Debug.Log("Hello");

            component.TakeDamage(damage, 0);
            collision.gameObject.GetComponent<DebuffsEffects>().Dazzle(dazzleTime);
            maxEnemies -= 1;

            if (maxEnemies > 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(collision.transform.position, maxDistance);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].tag != "Player" && colliders[i].TryGetComponent(out IDamagable damagable))
                    {
                        maxEnemies -= 1;

                        LightningBoltScript lightning = Instantiate(bolt, collision.transform.position, Quaternion.identity);
                        lightning.StartPosition = collision.transform.position;
                        lightning.EndPosition = colliders[i].transform.position;

                        yield return new WaitForSeconds(timeOfExistingBolt);
                        Destroy(lightning.gameObject);

                        Lightning(colliders[i].gameObject);
                        break;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) => Lightning(collision.gameObject);

    private void OnTriggerEnter2D(Collider2D collision) => Lightning(collision.gameObject);
}