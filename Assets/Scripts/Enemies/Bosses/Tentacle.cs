using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tentacle : MonoBehaviour
{
    [SerializeField] protected float _timeOfStun, _damage, _lifeTime, _timeBtwStart;

    protected static List<Tentacle> Tentacles = new List<Tentacle>();

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;
    private Animator _animator;
    public GameObject boss;

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _collider2D.enabled = false;

        _animator = GetComponent<Animator>();
        _animator.enabled = false;

        _damage *= StaticValues.EnemyDamage;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = Color.black;

        if (!Tentacles.Contains(this))
            Tentacles.Add(this);

        Destroy(gameObject, _lifeTime);
        StartCoroutine(EnableCollider());
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(_timeBtwStart);

        _spriteRenderer.color = Color.white;
        _animator.enabled = true;
        _collider2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.tag != tag && collision.TryGetComponent(out DebuffsEffects component))
        {
            collision.TryGetComponent(out IDamagable damagable);
            if (damagable != null)
                damagable.TakeDamage(_damage, 0);

            component.Dazzle(_timeOfStun);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Tentacles.Remove(this);

        if (boss != null && boss.GetComponent<NavMeshAgent>().isStopped == true && Tentacles.Count == 0)
        {
            boss.transform.position = transform.position;
            boss.GetComponent<BossOfDarkness>().ResumeBoss();
        }
    }
}
