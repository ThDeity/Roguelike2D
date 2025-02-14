using System.Collections;
using UnityEngine;

public class BlinkingEnemy : Enemy
{
    [SerializeField] private float _timeBtwBlinks, _timeOfBlinking;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _color;

    private Collider2D _collider;
    private float _currentCd;
    private bool _isBlinking;
    private Color _realColor;

    protected override void Start()
    {
        base.Start();

        _collider = GetComponent<Collider2D>();
        _currentCd = _timeBtwBlinks;
        _realColor = _spriteRenderer.color;
    }

    protected override void Update()
    {
        base.Update();

        _currentCd -= Time.deltaTime;
        if (!_isBlinking && _currentCd <= 0)
            StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        _collider.enabled = false;
        _spriteRenderer.color = _color;
        _isBlinking = true;

        yield return new WaitForSeconds(_timeOfBlinking);

        _isBlinking = false;
        _collider.enabled = true;
        _spriteRenderer.color = _realColor;
        _currentCd = _timeBtwBlinks;
    }
}
