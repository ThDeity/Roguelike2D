using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] protected ValueSystem _bar = new ValueSystem();
    [SerializeField] private float _hpMax;
    [SerializeField] private float _currentHp;

    public bool IsExploding;
    public GameObject Explosion;

    public int lifesCount;
    public float hpAfterDeath;

    public static Player PlayerObj;

    private List<OnTakeDmg> _onTakeDmgList;

    private bool _isTakingDmg;
    private float _dmg, _time;
    public void TakeDamage(float damage, float time)
    {
        if (damage > 0)
            _onTakeDmgList.ForEach(x => x.OnTakeDmg());

        if (time == 0)
        {
            _currentHp -= damage;
            _bar.RemoveValue(damage);

            if (_currentHp <= 0 && lifesCount <= 0 && gameObject != null)
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
            _isTakingDmg = true;
            _dmg = damage;
            _time = time;

            StartCoroutine(TakingDamage(time));
        }
    }

    private IEnumerator TakingDamage(float time)
    {
        yield return new WaitForSeconds(time);

        _isTakingDmg = false;
        _dmg = 0;
    }

    private void Awake()
    {
        PlayerObj = GetComponent<Player>();
        _bar.Setup(_hpMax);
        _currentHp = _hpMax;

        _onTakeDmgList = GetComponents<OnTakeDmg>().ToList();
    }

    private void Update()
    {
        if (_isTakingDmg)
            TakeDamage(_dmg / _time * Time.deltaTime, 0);
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
