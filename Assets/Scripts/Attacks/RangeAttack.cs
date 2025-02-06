using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    public float reloadTime;
    public GameObject bullet;
    public float _time { protected set; get; }
    public Transform _point;
    protected List<GameObject> _bullets = new List<GameObject>();

    protected virtual void Start()
    {
        _time = reloadTime;

        Bullet projectile = bullet.GetComponent<Bullet>();
        if (transform.tag == "Enemy")
        {
            projectile.damage = projectile.damage * StaticValues.EnemyDamage;
            projectile.critChance = (int)Mathf.Round((float)projectile.critChance * StaticValues.EnemyCrit);
        }
    }

    public virtual void Shot() => Instantiate(bullet, _point.position, transform.rotation);

    protected virtual void FixedUpdate() => _time -= Time.deltaTime;
}
