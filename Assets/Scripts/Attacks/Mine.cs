using UnityEngine;

public class Mine : Explosion
{
    public float timeDetonation, cardDamage, sizeBuff;
    private float _currentTime;

    private void Start()
    {
        _currentTime = timeDetonation;
        Explosion explosion = explosionObj.GetComponent<Explosion>();
        explosion.damage = cardDamage * sizeBuff;
        transform.localScale *= sizeBuff;
        explosion.radius *= sizeBuff;
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0)
            MakeExplosion();
    }

    private void MakeExplosion()
    {
        Instantiate(explosionObj, transform.position, Quaternion.identity);
        SelfDestroy();
    }

    protected override void OnTriggerEnter2D(Collider2D collision) => MakeExplosion();

    protected override void OnCollisionEnter2D(Collision2D collision) => MakeExplosion();
}
