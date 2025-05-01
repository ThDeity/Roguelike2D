using UnityEngine;

public class Dazzle : MonoBehaviour
{
    public float time, cd;
    private float _currentCd;

    private void Start() => _currentCd = cd;

    private void Update() => _currentCd -= Time.deltaTime;

    private void Reset() => _currentCd = time = cd = 0;

    private void OnDisable() => Reset();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DebuffsEffects effect) && _currentCd <= 0 && isActiveAndEnabled)
        {
            effect.Dazzle(time);
            _currentCd = cd;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DebuffsEffects effect) && _currentCd <= 0 && isActiveAndEnabled)
        {
            effect.Dazzle(time);
            _currentCd = cd;
        }
    }
}
