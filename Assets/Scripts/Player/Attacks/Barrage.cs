using UnityEngine;

public class Barrage : PlayerAttack
{
    public int bulletsCount;
    public float anglesOffset, speedRange, minSpeed;

    public void CreateBullet(int count)
    {
        for (int i = 0; i < bulletsCount; i++)
        {
            Vector3 angle = _player.eulerAngles;
            angle.z += Random.Range(-anglesOffset, anglesOffset);

            Bullet bullet2 = Instantiate(bullet, _point.position, Quaternion.Euler(angle)).GetComponent<Bullet>();
            bullet2.speed += Random.Range(-speedRange, speedRange);

            if (bullet2.speed < minSpeed)
                bullet2.speed = minSpeed;
        }
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0) && _time <= 0)
            OnMouseButtonDown();
    }

    public override void OnMouseButtonDown()
    {
        if (_time > 0) return;

        CreateBullet(bulletsCount);
        _time = reloadTime;
    }
}
