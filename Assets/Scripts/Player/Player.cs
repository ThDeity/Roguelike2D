using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] protected ValueSystem _bar = new ValueSystem();
    [SerializeField] private float _hpMax;
    public float currentHp { private set; get; }

    public bool IsExploding;
    public GameObject Explosion;

    public int lifesCount;
    public float hpAfterDeath;

    public static Player PlayerObj;

    public static float DamageOnStart;

    private List<OnTakeDmg> _onTakeDmgList = new List<OnTakeDmg>();

    private bool _isTakingDmg = false;
    private float _dmg, _time;
    public void TakeDamage(float damage, float time)
    {
        if (damage > 0 && _onTakeDmgList.Count > 0)
            _onTakeDmgList.ForEach(x => x.OnTakeDmg());

        if (time <= 0)
        {
            currentHp -= damage;
            if (currentHp > _hpMax)
                currentHp = _hpMax;

            if (damage > 0)
                _bar.RemoveValue(damage);
            else
                _bar.AddValue(-damage);

            if (currentHp <= 0 && lifesCount <= 0 && gameObject != null)
            {
                Destroy(gameObject);

                SceneManager.LoadScene("Menu");
            }
            else if (currentHp <= 0 && lifesCount > 0)
            {
                currentHp = _hpMax * hpAfterDeath;
                _bar.AddValue(currentHp);
                lifesCount--;

                FindObjectOfType<StaticValues>().playerPrefab.GetComponent<Player>().lifesCount = lifesCount;
            }
            else if (currentHp > _hpMax)
                currentHp = _hpMax;
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

    protected static ValueSystem Bar;
    protected static float HpMax, HpAfterDeath;
    public void Reset2()
    {
        Debug.Log(_bar.ToString());

        currentHp = _hpMax = HpMax;
        IsExploding = false;
        Explosion = null;

        lifesCount = 0;
        hpAfterDeath = 0;

        DamageOnStart = 0;

        _bar.Setup(_hpMax);
        PlayerObj = GetComponent<Player>();
    }

    private void Awake()
    {
        if (Bar == null)
        {
            Bar = _bar;
            HpMax = _hpMax;
        }

        PlayerObj = GetComponent<Player>();
        _bar.Setup(_hpMax);
        currentHp = _hpMax;

        TakeDamage(DamageOnStart, 0);
        DamageOnStart = 0;

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
        currentHp *= hpChange;
        _bar.SetupMax(_hpMax);

        return _hpMax;
    }
}
