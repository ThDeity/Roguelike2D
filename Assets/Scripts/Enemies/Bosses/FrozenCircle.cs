using DG.Tweening;
using UnityEngine;

public class FrozenCircle : MonoBehaviour
{
    [SerializeField] private float _debuffSpeed, _time, _scale, _timeOfScale;
    private float _startScale, _currentTime;

    private void Start()
    {
        _currentTime = _timeOfScale;
        _startScale = transform.localScale.x;

        transform.DOScale(_scale * transform.localScale, _timeOfScale);
        Destroy(gameObject, _timeOfScale * 2);
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime < 0)
            transform.DOScale(_startScale, _timeOfScale);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        if (collision.tag != tag && collision.TryGetComponent(out DebuffsEffects effects))
            effects.Freezing(_time, _debuffSpeed);
    }
}