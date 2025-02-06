using UnityEngine;

public class Shuriken : RangeAttack
{
    [SerializeField] protected Transform[] _points;
    [SerializeField] protected float _increaseSpeed;

    public void SpecialShot()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            Bullet bull = Instantiate(bullet, _points[i].position, _points[i].rotation).GetComponent<Bullet>();
            bull.speed *= _increaseSpeed;
        }
    }
}