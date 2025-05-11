using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] protected ValueSystem _bar = new ValueSystem();
    [SerializeField] private float _hpMax, _restorePersent, _immortalTime;
    [SerializeField] private Color _colorAfterDeath;
    public float currentHp { private set; get; }

    public bool IsExploding;
    public GameObject Explosion;

    public int lifesCount;
    public float hpAfterDeath;

    public static Player PlayerObj;

    private List<OnTakeDmg> _onTakeDmgList = new List<OnTakeDmg>();

    private bool _isTakingDmg = false, _isDead = false, _isImmortal = false;
    private float _dmg, _time;

    private SpriteRenderer _spriteRenderer;
    private Color _previousColor;
    private Skill _skill;

    public void TakeDamage(float damage, float time)
    {
        if (_isImmortal) return;

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

            if (currentHp <= 0 && lifesCount <= 0 && gameObject != null && !_isDead)
            {
                StaticValues.ResetStatics();

                SceneManager.LoadScene("Menu");
            }
            else if (currentHp <= 0 && lifesCount > 0 && !_isDead)
            {
                lifesCount -= 1;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Collider2D>().enabled = false;
                StaticValues.PlayerMovementObj.enabled = false;

                _spriteRenderer.color = _colorAfterDeath;
                transform.GetChild(0).gameObject.SetActive(false);

                _isDead = true;
                _bar.RemoveValue(damage);
                if (TryGetComponent(out Skill skill))
                {
                    _skill = skill;
                    _skill.enabled = false;
                }
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

    public IEnumerator SetImmortal(float time)
    {
        _isImmortal = true;

        yield return new WaitForSeconds(time);

        _isImmortal = false;
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

        transform.localScale = Vector2.one;

        _bar.Setup(_hpMax);
        PlayerObj = GetComponent<Player>();
    }

    public void CheckComponents() => _onTakeDmgList = GetComponents<OnTakeDmg>().ToList();

    private void Awake()
    {
        if (Bar == null)
        {
            Bar = _bar;
            HpMax = _hpMax;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _previousColor = _spriteRenderer.color;

        PlayerObj = GetComponent<Player>();
        _bar.Setup(_hpMax);
        currentHp = _hpMax;

        _onTakeDmgList = GetComponents<OnTakeDmg>().ToList();
    }

    float _elapsedTime1 = 0, _elapsedTime2 = 0;
    Color _currentColor;
    private void Update()
    {
        if (_isTakingDmg)
            TakeDamage(_dmg / _time * Time.deltaTime, 0);

        if (_isDead)
        {
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space))
                _currentColor = _spriteRenderer.color;

            if (Input.GetKey(KeyCode.Space) && currentHp < _hpMax)
            {
                if (_currentColor == null)
                    _currentColor = _spriteRenderer.color;

                _spriteRenderer.color = Color.Lerp(_currentColor, _previousColor, _elapsedTime1 / (_hpMax / (_restorePersent * _hpMax)));
                _elapsedTime1 += Time.deltaTime;

                if (_elapsedTime2 > 0)
                    _elapsedTime2 -= Time.deltaTime;

                TakeDamage(-_restorePersent * _hpMax * Time.deltaTime, 0);
            }
            else if (currentHp >= _hpMax)
            {
                _elapsedTime1 = _elapsedTime2 = 0;
                _currentColor = _previousColor;

                GetComponent<Collider2D>().enabled = true;
                StaticValues.PlayerMovementObj.enabled = true;

                transform.GetChild(0).gameObject.SetActive(true);

                _isDead = false;
                currentHp = hpAfterDeath * _hpMax;
                _bar.RemoveValue(_hpMax - currentHp);

                if (_skill != null)
                    _skill.enabled = true;

                StartCoroutine(SetImmortal(_immortalTime));
            }
            else if (currentHp > 0 && currentHp < _hpMax)
            {
                _spriteRenderer.color = Color.Lerp(_currentColor, _colorAfterDeath, _elapsedTime2 / (_hpMax / (_restorePersent * _hpMax)));
                _elapsedTime2 += Time.deltaTime;
                if (_elapsedTime1 > 0) 
                    _elapsedTime1 -= Time.deltaTime;

                TakeDamage(_restorePersent * _hpMax * Time.deltaTime, 0);
            }
        }
    }

    public float ChangeMxHp(float hpChange)
    {
        _hpMax *= hpChange;
        transform.localScale *= hpChange;
        _bar.SetupMax(_hpMax);
        _bar.AddValue(currentHp * hpChange - currentHp);
        currentHp *= hpChange;

        return _hpMax;
    }
}
