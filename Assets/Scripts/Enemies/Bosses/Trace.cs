using DG.Tweening;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class Trace : MeleeAttack
{
    [SerializeField] private float _timeOfChangingColor, _maxIntensity, _lifeTime;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Light2D _light2D;
    [SerializeField] private Color _color;

    private Collider2D _collider;
    protected override void Start()
    {
        base.Start();

        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
        StartCoroutine(Boom());
    }

    private IEnumerator Boom()
    {
        _spriteRenderer.DOColor(_color, _timeOfChangingColor);
        DOTween.To(() => _light2D.intensity, x => _light2D.intensity = x, _maxIntensity, _timeOfChangingColor);
        yield return new WaitForSeconds(_timeOfChangingColor);
        
        _collider.enabled = true;
        yield return new WaitForSeconds(_lifeTime);

        Destroy(gameObject);
    }
}
