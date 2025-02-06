using UnityEngine;

public class Collapse : MonoBehaviour
{
    public GameObject bullet;
    public int bulletsCount;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == transform.tag || collision.collider.isTrigger) return;

        float angle = 0, delta = 360f / bulletsCount;

        for (int i = 0; i < bulletsCount; i++)
        {
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, angle));
            angle += delta;
        }
    }
}
