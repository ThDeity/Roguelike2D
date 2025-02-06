using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

public class DebuffsEffects : MonoBehaviour
{
    [SerializeField] private bool _isEnemy;
    private Enemy _enemy;
    private List<RangeAttack> _rangeAttacks;
    private Animator _animator;
    NavMeshAgent agent;
    private bool _isCharmed;

    [Tooltip("0 - дазл, 1 - заморозка, 2 - оглушение, 3 - щит")]
    [SerializeField] private List<GameObject> _effects;

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
        t.localScale *= transform.localScale.x;

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

    public void Dazzle(float time) => StartCoroutine(DazzleCoroutine(time));

    private IEnumerator DazzleCoroutine(float time)
    {
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
            speed = StaticValues.PlayerMovementObj.speed;
            StaticValues.PlayerMovementObj.speed = 0;

            yield return new WaitForSeconds(time);

            StaticValues.PlayerMovementObj.speed = speed;
        }
        Destroy(effect);
    }

    public void Freezing(float time, float force, float damage = 0, bool isEnableAnim = false) => StartCoroutine(FreezingCoroutine(time, force, damage, isEnableAnim));

    private IEnumerator FreezingCoroutine(float time, float force, float damage, bool isEnableAnim = false)
    {
        GameObject effect = SetEffect(1);

        float speed, cd = 1, newCd;
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

        gameObject.GetComponent<IDamagable>().TakeDamage(damage, 0);
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
        _animator.enabled = false;

        yield return new WaitForSeconds(time);

        _animator.enabled = true;
        _rangeAttacks.ForEach(attack => attack.enabled = true);
        _enemy.enabled = true;

        Destroy(effect);
    }
}
