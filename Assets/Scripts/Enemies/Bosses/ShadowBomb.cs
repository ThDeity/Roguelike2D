using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ShadowBomb : MonoBehaviour
{
    [SerializeField] private float _timeBtwActivation, _lifeTime;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;

        StartCoroutine(Activation());
        Destroy(gameObject, _lifeTime);
    }

    private IEnumerator Activation()
    {
        _spriteRenderer.DOColor(Color.white, _timeBtwActivation);

        yield return new WaitForSeconds(_timeBtwActivation);

        _collider.enabled = true;
    }
}
