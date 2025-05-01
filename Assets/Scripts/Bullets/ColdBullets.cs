using UnityEngine;

public class ColdBullets : MonoBehaviour
{
    public float time, speedReduce, cd;
    private float _currentCd;

    public void Reset() => time = speedReduce = cd = _currentCd = 0;

    private void OnDisable() => Reset();

    private void Update() => _currentCd -= Time.deltaTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DebuffsEffects effect) && _currentCd <= 0 && cd > 0 && isActiveAndEnabled)
        {
            effect.Freezing(time, speedReduce);
            _currentCd = cd;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DebuffsEffects effect) && _currentCd <= 0 && cd > 0 && isActiveAndEnabled)
        {
            effect.Freezing(time, speedReduce);
            _currentCd = cd;
        }
    }
}