using UnityEngine;

public class CapsuleAttack : RangeAttack
{
    protected override void FixedUpdate() { }

    public override void Shot()
    {
        GameObject b = Instantiate(bullet, _point.position, transform.rotation);
        b.layer = gameObject.layer;

        b.GetComponent<FragileBullet>().gunner = GetComponent<Capsule>();
    }
}
