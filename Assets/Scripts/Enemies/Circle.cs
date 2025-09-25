using DG.Tweening;
using UnityEngine;

public class Circle : Enemy
{
    [SerializeField] protected GameObject _explosion;
    [SerializeField] protected float _damage;

    protected float _currentTime;

    protected override void Start()
    {
        base.Start();

        _damage *= StaticValues.EnemyDamage;
    }

    protected void Explosion()
    {
        StopAllCoroutines();
        DOTween.KillAll();

        int chance = Random.Range(0, 101);
        if (chance < _chanceFallingOut)
            Instantiate(_medkit, _transform.position, Quaternion.identity);

        SpawnEnemies spawn = FindObjectOfType<SpawnEnemies>();
        if (spawn != null && gameObject != null)
            spawn.RemoveEnemy(gameObject);

        GameObject exp = Instantiate(_explosion, _transform.position, _transform.rotation);
        exp.GetComponent<Explosion>().damage = _damage;

        exp.tag = tag;
        exp.layer = gameObject.layer;

        AudioSource.PlayClipAtPoint(_attackSound, _transform.position);
        Destroy(gameObject);
    }

    public override void TakeDamage(float damage, float time, bool isLifesteal, float lifesteal)
    {
        Explosion();
        Destroy(gameObject);
    }
}
