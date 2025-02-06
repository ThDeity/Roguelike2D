using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] protected ValueSystem _bar = new ValueSystem();
    [SerializeField] private float _hpMax;
    private float _currentHp;

    public bool IsExploding;
    public GameObject Explosion;

    public int lifesCount;
    public float hpAfterDeath;

    public static Player PlayerObj;

    private List<OnTakeDmg> _onTakeDmgList;

    protected float _damageTaking, _timeTaking;
    public void TakeDamage(float damage, int time)
    {
        if (damage > 0)
            _onTakeDmgList.ForEach(x => x.OnTakeDmg());

        if (time == 0)
        {
            _currentHp -= damage;
            _bar.RemoveValue(damage);

            if (_currentHp <= 0 && lifesCount <= 0)
                Destroy(gameObject);
            else if (_currentHp <= 0 && lifesCount > 0)
            {
                _currentHp = _hpMax * hpAfterDeath;
                _bar.AddValue(_currentHp);
                lifesCount--;

                FindObjectOfType<StaticValues>().playerPrefab.GetComponent<Player>().lifesCount = lifesCount;
            }
            else if (_currentHp > _hpMax)
                _currentHp = _hpMax;
        }
        else
        {
            _damageTaking = damage;
            _timeTaking = time;
            StartCoroutine(TakingDamage());

            _damageTaking = 0;
            _timeTaking = 0;
        }
    }

    private IEnumerator TakingDamage()
    {
        for (int i = 0; i < (int) _timeTaking; i++)
        {
            float damage = _damageTaking / _timeTaking;
            TakeDamage(damage, 0);
            yield return new WaitForSeconds(1);
        }
    }

    private void Awake()
    {
        PlayerObj = GetComponent<Player>();
        _bar.Setup(_hpMax);
        _currentHp = _hpMax;

        _onTakeDmgList = GetComponents<OnTakeDmg>().ToList();
    }

    public float ChangeMxHp(float hpChange)
    {
        _hpMax *= hpChange;
        transform.localScale *= hpChange;
        _currentHp *= hpChange;
        _bar.SetupMax(_hpMax);

        return _hpMax;
    }
}
