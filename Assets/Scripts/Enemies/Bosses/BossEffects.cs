using System.Collections;
using UnityEngine;

public class BossEffects : DebuffsEffects
{
    protected override IEnumerator CharmingCoroutine(float time, float increaseParam)
    {
        GameObject effect = SetEffect(4);

        transform.tag = "Somebody";
        gameObject.layer = LayerMask.NameToLayer("Water");
        _enemy.isCharmed = true;

        yield return new WaitForSeconds(time);

        if (!_enemy.isActiveAndEnabled)
            _enemy.enabled = true;

        transform.tag = "Enemy";
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        _enemy.isCharmed = false;

        Destroy(effect);
    }

    protected override IEnumerator DazzleCoroutine(float time) { yield return null; }

    protected override IEnumerator FreezingCoroutine(float time, float force, float damage, bool isEnableAnim = false)
    {
        GameObject effect = SetEffect(1);

        isFrozen = true;
        float speed, cd = 1, newCd;

        gameObject.GetComponent<IDamagable>().TakeDamage(damage, 0);

        speed = agent.speed;
        agent.speed *= 1 - force;

        cd = _enemy.ChangeReloadCd(1);
        newCd = _enemy.ChangeReloadCd(cd / time);

        yield return new WaitForSeconds(time);

        _enemy.ChangeReloadCd(cd / newCd);
        agent.speed = speed;

        isFrozen = false;
        Destroy(effect);
    }

    protected override IEnumerator GetSilence(float time)
    {
        GameObject effect = SetEffect(2);

        _enemy.enabled = false;
        _rangeAttacks.ForEach(attack => attack.enabled = false);

        float speed = _animator.speed;
        if (_animator != null)
            _animator.speed = 0.1f;

        yield return new WaitForSeconds(time);

        if (_animator != null)
            _animator.speed = speed;

        _rangeAttacks.ForEach(attack => attack.enabled = true);
        _enemy.enabled = true;

        Destroy(effect);
    }

    protected override void OnDestroy() { }
}