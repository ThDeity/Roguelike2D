using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class SpikeBombs : MonoBehaviour, IDamagable
{
    [SerializeField] protected ValueSystem _bar = new ValueSystem();

    [SerializeField] private float _timeOfChangingColor, _maxIntensity;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private int _numOfBullets;
    [SerializeField] private Light2D _light2D;
    [SerializeField] private Color _color;

    private float _currentHp, _damageTaking, _timeTaking, _lifestealToPlayer;
    private GameObject _hpBar;

    public float maxHp;

    private void Start()
    {
        StartCoroutine(Boom());

        if (_hpBar == null)
        {
            _hpBar = transform.GetComponentInChildren<Canvas>().gameObject;
            _hpBar.SetActive(false);
        }

        maxHp *= StaticValues.EnemyMaxHp;
        _bar.Setup(maxHp);
        _currentHp = maxHp;
    }

    private void Update()
    {
        if (_isTakingDmg)
            TakeDamage(_damageTaking / _timeTaking * Time.deltaTime, 0, _isLifesteal, _lifestealToPlayer);
    }

    protected bool _isTakingDmg, _isLifesteal;
    public virtual void TakeDamage(float damage, float time, bool isLifesteal, float lifesteal)
    {
        if (time == 0)
        {
            _currentHp -= damage;
            _bar.RemoveValue(damage);

            if (isLifesteal)
                StaticValues.PlayerObj.TakeDamage(-damage * lifesteal, 0, false, 0);

            if (_currentHp <= 0)
                Destroy(gameObject);
            else if (_currentHp > maxHp)
                _currentHp = maxHp;
        }
        else
        {
            _isTakingDmg = true;
            _damageTaking += damage;
            _timeTaking += time;

            if (isLifesteal)
            {
                _isLifesteal = true;
                _lifestealToPlayer = lifesteal;
            }

            if (_timeTaking > 0)
                StopCoroutine(TakingDamage(time));

            StartCoroutine(TakingDamage(_timeTaking));
        }

        if (_hpBar != null && !_hpBar.activeInHierarchy)
            _hpBar.SetActive(true);
    }

    protected IEnumerator TakingDamage(float time)
    {
        yield return new WaitForSeconds(time);

        _isTakingDmg = _isLifesteal = false;
        _damageTaking = _lifestealToPlayer = 0;
    }

    private IEnumerator Boom()
    {
        _spriteRenderer.DOColor(_color, _timeOfChangingColor);
        DOTween.To(() => _light2D.intensity, x  => _light2D.intensity = x, _maxIntensity, _timeOfChangingColor);
        yield return new WaitForSeconds(_timeOfChangingColor);

        for (int i = 0; i < _numOfBullets; i++)
        {
            Transform bullet = Instantiate(_bullet, transform.position, Quaternion.identity).transform;
            bullet.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            bullet.localScale *= 2;

            bullet.tag = tag;
            bullet.gameObject.layer = gameObject.layer;
        }

        Destroy(gameObject);
    }
}
