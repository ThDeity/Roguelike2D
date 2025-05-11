using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    public float reloadTime;
    public GameObject bullet;
    public float _time { protected set; get; }
    public Transform _point;

    protected virtual void Start()
    {
        _time = reloadTime;

        bullet.TryGetComponent(out Bullet projectile);
        if (transform.tag == "Enemy" && projectile != null)
        {
            projectile.damage = projectile.damage * StaticValues.EnemyDamage;
            projectile.critChance = (int)Mathf.Round((float)projectile.critChance * StaticValues.EnemyCrit);
        }
    }

    public virtual void Shot()
    {
        GameObject b = Instantiate(bullet, _point.position, transform.rotation);
        b.layer = gameObject.layer;
    }

    public virtual void ShotFromPoint() => Instantiate(bullet, _point.position, _point.rotation);

    protected virtual void FixedUpdate() => _time -= Time.deltaTime;
}
