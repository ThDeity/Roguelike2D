using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

public class DebuffsEffects : MonoBehaviour
{
    [SerializeField] private Vector2 _effectsScale;
    private List<RangeAttack> _rangeAttacks;
    [SerializeField] private bool _isEnemy;
    private Animator _animator;
    private Enemy _enemy;
    NavMeshAgent agent;

    [Tooltip("0 - дазл, 1 - заморозка, 2 - оглушение, 3 - щит")]
    [SerializeField] private List<GameObject> _effects;

    private bool _isClear;

    private void Start()
    {
        if(_isEnemy)
        {
            _enemy = GetComponent<Enemy>();
            agent = GetComponent<NavMeshAgent>();
        }

        _animator = GetComponent<Animator>();
        _rangeAttacks = GetComponentsInChildren<RangeAttack>().ToList();
    }

    private GameObject SetEffect(int index)
    {
        Transform t = Instantiate(_effects[index], transform).transform;
        t.localScale = _effectsScale;

        return t.gameObject;
    }

    private void OnDestroy()
    {
        if (StaticValues.PlayerObj.IsExploding)
            Instantiate(StaticValues.PlayerObj.Explosion, transform.position, Quaternion.identity);
    }

    public void FindEnemy()
    {
        float _minDistance = float.MaxValue;
        foreach (GameObject enemy in SpawnEnemies.Enemies)
        {
            if (enemy != gameObject)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);

                if (distance < _minDistance)
                {
                    _minDistance = distance;
                    _enemy.target = enemy.transform;
                }
            }
        }

        float increaseParam;
        if (_enemy.target == StaticValues.PlayerObj.transform)
        {
            agent.speed = 0;
            increaseParam = 10;
            _enemy.ChangeReloadCd(increaseParam);
        }
    }

    private bool _isShield;
    public void Shield(float time)
    {
        if (_isShield) return;

        StartCoroutine(ShieldCoroutine(time));
    }

    private IEnumerator ShieldCoroutine(float time)
    {
        GameObject effect = SetEffect(3);
        _isShield = true;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(time);

        Destroy(effect);
        _isShield = false;
    }

    public void Charming(float time, float increaseParam) => StartCoroutine(CharmingCoroutine(time, increaseParam));

    private IEnumerator CharmingCoroutine(float time, float increaseParam)
    {
        if (_isEnemy)
        {
            transform.tag = "Somebody";
            _enemy.ChangeReloadCd(increaseParam);
            _enemy.isCharmed = true;

            FindEnemy();

            yield return new WaitForSeconds(time);

            _enemy.isCharmed = false;
            transform.tag = "Enemy";
            _enemy.ChangeReloadCd(1 /  increaseParam);
            _enemy.target = StaticValues.PlayerObj.transform;
        }
    }

    private bool _isDazzled;
    public void Dazzle(float time) => StartCoroutine(DazzleCoroutine(time));

    private IEnumerator DazzleCoroutine(float time)
    {
        if (_isDazzled) yield break;
        _isDazzled = true;

        GameObject effect = SetEffect(0);

        float speed;
        if (_isEnemy)
        {
            speed = agent.speed;
            agent.speed = 0;

            yield return new WaitForSeconds(time);

            agent.speed = speed;
        }
        else
        {
            StaticValues.PlayerMovementObj.ChangeSpeed(0);
            StaticValues.PlayerMovementObj.GetComponent<Rigidbody2D>().mass *= 1000;
            StaticValues.PlayerAttackList.ForEach(x => x.enabled = false);

            yield return new WaitForSeconds(time);

            StaticValues.PlayerAttackList.ForEach(x => x.enabled = true);
            StaticValues.PlayerMovementObj.GetComponent<Rigidbody2D>().mass /= 1000;
            StaticValues.PlayerMovementObj.ChangeSpeed(0, false);
        }

        Destroy(effect);
        _isDazzled = false;
    }

    bool _isFrozen;
    public void Freezing(float time, float force, float damage = 0, bool isEnableAnim = false)
    {
        if (_isFrozen) return;

        StartCoroutine(FreezingCoroutine(time, force, damage, isEnableAnim));
    }

    private IEnumerator FreezingCoroutine(float time, float force, float damage, bool isEnableAnim = false)
    {
        GameObject effect = SetEffect(1);

        _isFrozen = true;
        float speed, cd = 1, newCd;
        gameObject.GetComponent<IDamagable>().TakeDamage(damage, 0);
        if (_isEnemy)
        {
            speed = agent.speed;
            agent.speed *= 1 - force;

            cd = _enemy.ChangeReloadCd(1);
            newCd = _enemy.ChangeReloadCd(cd / time);

            if (isEnableAnim)
                GetComponent<Animator>().enabled = false;

            yield return new WaitForSeconds(time);

            if (isEnableAnim)
                GetComponent<Animator>().enabled = true;

            _enemy.ChangeReloadCd(cd / newCd);
            agent.speed = speed;
        }
        else
        {
            StaticValues.PlayerAttackList.ForEach(value => { cd = value.reloadTime; value.reloadTime *= 1 - force; });

            speed = StaticValues.PlayerMovementObj.speed;
            StaticValues.PlayerMovementObj.speed *= 1 - force;

            yield return new WaitForSeconds(time);

            StaticValues.PlayerAttackList.ForEach(value => value.reloadTime = cd);

            StaticValues.PlayerMovementObj.speed = speed;
        }

        _isFrozen = false;
        Destroy(effect);
    }

    public void Silence(float time)
    {
        if (!_isEnemy) return;

        StartCoroutine(GetSilence(time));
    }

    private IEnumerator GetSilence(float time)
    {
        GameObject effect = SetEffect(2);

        _enemy.enabled = false;
        _rangeAttacks.ForEach(attack => attack.enabled = false);

        if (_animator != null)
            _animator.enabled = false;

        yield return new WaitForSeconds(time);

        if (_animator != null)
            _animator.enabled = true;

        _rangeAttacks.ForEach(attack => attack.enabled = true);
        _enemy.enabled = true;

        Destroy(effect);
    }
}
