using DG.Tweening;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class BossOfDarkness : Enemy
{
    [SerializeField] protected GameObject _spikes, _trace, _tentacle, _arrow, _bomb, _frozenCircle;
    [SerializeField] protected float _timeOfStopping, _maxSpeed, _minSpeed, _timeOfCocoon, _radius, _timeOfPuls, _timeBtwPuls, _scale, _timeOfShadowCapture,
    _timeOfTrace, _minIntensity, _timeOfLightIntensity, _timeOfLight, _angleBtwArrows, _timeBtwShots, _vignetteScaleAfterFurious, _furiousScale, _timeBtw, _timeBtwCircles;

    [SerializeField] protected int _countOfPoints, _countOfSpikes, _indexOfAttack, _countOfPulses, _countOfTraces, _countOfTentacles, _arrows, _countOfBombs,
                                                                                                _countOfWaves, _countOfBullets, _countOfCircles;

    [Tooltip("Названия анимаций атак до ярости и после")]
    [SerializeField] private List<string> _beforeFurious, _afterFurious;
    [SerializeField] private WaveBullet _shadowBullet;
    [SerializeField] private Light2D _vignette;
    [SerializeField] private Color _pulsColor;

    protected float _timeOfAcceleration, _currentTimeBtwShots;
    protected Light2D _currentLight, _globalLight;
    protected SpriteRenderer _renderer;
    protected List<Enemy> _enemyList;
    protected int _currentCount;

    bool _isFurious;
    public override void TakeDamage(float damage, float time)
    {
        if (time == 0)
        {
            _currentHp -= damage;
            _bar.RemoveValue(damage);

            if (_currentHp <= 0)
                Destroy(gameObject);
            else if (_currentHp > maxHp)
                _currentHp = maxHp;
        }
        else
        {
            _isTakingDmg = true;
            _damageTaking = damage;
            _timeTaking = time;

            StartCoroutine(TakingDamage(time));
        }

        if (_currentHp <=  maxHp * 0.5f && !_isFurious)
        {
            _isFurious = true;
            _indexOfAttack = 0;
            Destroy(_globalLight.gameObject);

            _minSpeed *= _furiousScale;
            _maxSpeed *= _furiousScale;
            _reloadTime *= 2 - _furiousScale;

            _currentLight = Instantiate(_vignette.gameObject, StaticValues.PlayerTransform).GetComponent<Light2D>();
            _currentLight.transform.localScale = Vector2.one;
            _currentLight.transform.DOScale(_vignetteScaleAfterFurious, 3);
        }
    }

    public void ResumeBoss()
    {
        _agent.isStopped = false;
        _renderer.enabled = true;
    }

    protected IEnumerator FrozenCircles()
    {
        _time = _time > 0 ? _time + _countOfCircles * _timeBtwCircles : _countOfCircles * _timeBtwCircles;

        for (int i = 0; i < _countOfCircles; i++)
        {
            Instantiate(_frozenCircle, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(_timeBtwCircles);
        }
    }

    protected IEnumerator WaveShooting(float amplitude, float diff)
    {
        _agent.isStopped = true;
        float roatZ = 0;

        for (int i = 0; i < _countOfBullets; i++)
        {
            if (target != null)
            {
                Vector2 difference = target.position - _transform.position;
                roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            }
            float angle = diff + roatZ + _offset;

            WaveBullet bullet = Instantiate(_shadowBullet, _transform.position, Quaternion.identity);
            bullet.amplitude = amplitude;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            yield return new WaitForSeconds(_timeBtw);
        }

        _agent.isStopped = false;
    }

    protected void FuriousShot()
    {
        _time = _time > 0 ? _time + _countOfBullets * _timeBtw : _countOfBullets * _timeBtw;

        for (int y = 0; y < _countOfWaves; y++)
        {
            float diff;
            if (y % 2 == 0 || y == 0)
                diff = -(_angleBtwArrows * 0.5f * y);
            else
                diff = _angleBtwArrows * Mathf.CeilToInt(y * 0.5f);

            float amplitude = Random.Range(0.025f, 0.1f);
            StartCoroutine(WaveShooting(amplitude, diff));
        }
    }

    protected void LightningBombs()
    {
        for (int i = 0; i < _countOfBombs;  i++)
        {
            Transform point = _points[Random.Range(0, _points.Count)];
            _randomDistance = Random.Range(-_radius, _radius);
            Vector2 pos = new Vector2(_randomDistance + point.position.x, _randomDistance + point.position.y);

            Transform t = Instantiate(_bomb, pos, Quaternion.identity).transform;
            t.localScale *= Random.Range(1, _scale);
        }
    }

    protected void Shot()
    {
        float roatZ = 0;
        if (_rotateToObj != null)
        {
            Vector2 difference = target.position - _transform.position;
            roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }

        for (int i = 0; i < _arrows; i++)
        {
            Transform b = Instantiate(_arrow, _transform.position, Quaternion.identity).transform;

            if (i % 2 == 0 || i == 0)
                b.rotation = Quaternion.Euler(0, 0, roatZ + _offset -  (_angleBtwArrows * 0.5f * i));
            else
                b.rotation = Quaternion.Euler(0, 0, roatZ + _offset + _angleBtwArrows * Mathf.CeilToInt(i * 0.5f));
        }
    }

    protected IEnumerator ShadowCaptivity()
    {
        _agent.isStopped = true;
        _renderer.enabled = false;
        _animator.enabled = false;

        _time = _time > 0 ? _time + _timeOfShadowCapture : _timeOfShadowCapture;

        Tentacle t = Instantiate(_tentacle, target.position, Quaternion.identity).GetComponent<Tentacle>();
        t.boss = gameObject;

        for (int i = 0; i < _countOfTentacles;  i++)
        {
            float randX = Random.Range(-_radius, _radius);
            float randY = Random.Range(-_radius, _radius);
            
            t = Instantiate(_tentacle, new Vector2(randX + target.position.x, randY + target.position.y), Quaternion.identity).GetComponent<Tentacle>();
            t.boss = gameObject;
        }

        yield return new WaitForSeconds(_timeOfShadowCapture);

        _agent.isStopped = false;
        _renderer.enabled = true;
        _animator.enabled = true;
    }

    protected IEnumerator SetVignette()
    {
        if (StaticValues.PlayerTransform != null)
        {
            _time = 0;
            StaticValues.PlayerMovementObj.ChangeSpeed(0.6f);

            _enemyList.ForEach(x => x.gameObject.SetActive(true));

            if (_currentLight != null)
                Destroy(_currentLight.gameObject);

            _agent.isStopped = false;
            if (_globalLight != null)
                _globalLight.enabled = false;

            _currentLight = Instantiate(_vignette.gameObject, StaticValues.PlayerTransform).GetComponent<Light2D>();
            _currentLight.transform.DOScale(_minIntensity, _timeOfLightIntensity);

            yield return new WaitForSeconds(_timeOfLight);

            StaticValues.PlayerMovementObj.ChangeSpeed(1, false);
            _enemyList.ForEach(x => x.gameObject.SetActive(false));
            Destroy(_currentLight.gameObject);
            _globalLight.enabled = true;
        }
        else
        {
            Debug.Log("Not hehe");
            yield return null;
        }
    }

    protected virtual Vector2 GeneratePos()
    {
        Transform point = _points[Random.Range(0, _points.Count)];
        float rand = Random.Range(-_radius, _radius);

        return new Vector2(rand + point.position.x, rand + point.position.y);
    }

    protected virtual IEnumerator SetCell(GameObject _zone, Vector2 pos, float time)
    {
        Transform zone = Instantiate(_zone, pos, Quaternion.identity).transform;
        zone.localScale = new Vector2(zone.localScale.x * Random.Range(1, _scale), zone.localScale.y * Random.Range(1, _scale));
        zone.localRotation = Quaternion.Euler(0,0, Random.Range(0, 360));

        yield return new WaitForSeconds(time);

        if (zone != null && zone.TryGetComponent(out Collider2D collider2D))
            collider2D.enabled = true;

        if (zone != null && zone.TryGetComponent(out Animator anim))
            anim.enabled = true;
    }

    protected virtual void Cells(GameObject _zone, float time, int count)
    {
        if (target != null)
            StartCoroutine(SetCell(_zone, target.position, time));

        for (int i = 0; i < count; i++)
            StartCoroutine(SetCell(_zone, GeneratePos(), time));
    }

    public virtual void Traces()
    {
        _time = _time > 0 ? _time + _timeOfTrace : _timeOfTrace;

        Cells(_trace, _timeOfTrace, _countOfTraces);
    }

    protected virtual void Spikes()
    {
        for (int i = 0; i < _countOfSpikes; i++)
        {
            Vector2 pos = new Vector2(_transform.position.x - Random.Range(-_radius, _radius),
                                        _transform.position.y - Random.Range(-_radius, _radius));

            Transform spike = Instantiate(_spikes, pos, Quaternion.identity).transform;
            spike.localScale = Vector2.one * Random.Range(0.9f, 1.3f);
        }
    }

    protected virtual IEnumerator ShadowCocoon()
    {
        _animator.enabled = false;
        _time = (_timeBtwPuls + _timeOfPuls) * _countOfPulses + _timeOfCocoon;
        for (int i = 0; i < _countOfPulses; i++)
        {
            _renderer.DOColor(_pulsColor, _timeOfPuls);
            yield return new WaitForSeconds(_timeOfPuls);

            _renderer.color = Color.white;
            yield return new WaitForSeconds(_timeBtwPuls);
        }

        _renderer.enabled = false;
        _agent.isStopped = true;

        Spikes();

        yield return new WaitForSeconds(_timeOfCocoon);

        _animator.enabled = true;
        _agent.isStopped = false;
        _renderer.enabled = true;
    }

    protected override void FindPoint()
    {
        if (_points[0] == null)
        {
            GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
            StaticValues.EnemiesPoint.Clear();
            foreach (GameObject p in points)
                StaticValues.EnemiesPoint.Add(p.transform);
        }

        base.FindPoint();
        _agent.speed = _minSpeed;
        _timeOfAcceleration = _randomDistance / _minSpeed;

        _randomDistance *= _randomDistance;
        _agent.SetDestination(_currentPos);

        StartCoroutine(Acceleration());
    }

    protected virtual IEnumerator Acceleration()
    {
        DOTween.To(() => _agent.speed, x => _agent.speed = x, _maxSpeed, _timeOfAcceleration);

        yield return new WaitForSeconds(_timeOfAcceleration);

        DOTween.To(() => _agent.speed, x => _agent.speed = x, _minSpeed, _timeOfAcceleration);
    }

    protected virtual IEnumerator Stop()
    {
        _agent.isStopped = true;

        yield return new WaitForSeconds(_timeOfStopping);

        _agent.isStopped = false;
        FindPoint();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (_currentLight != null )
            Destroy(_currentLight);

        GameObject.FindGameObjectsWithTag("Enemy").ToList().ForEach(x => Destroy(x.gameObject));
        GameObject.FindGameObjectsWithTag(tag).ToList().ForEach(x => Destroy(x.gameObject));

        if (_currentHp <= 0)
            FindObjectOfType<SpawnPrize>().GivePrize();
        StaticValues.WasPrizeGotten = true;

        StaticValues.PlayerMovementObj.ChangeSpeed(1, false);
    }

    private void Awake() => StaticValues.WasPrizeGotten = false;

    protected override void Start()
    {
        base.Start();

        _enemyList = FindObjectsOfType<Enemy>().ToList();
        _enemyList.Remove(this);
        _enemyList.ForEach(enemy => enemy.gameObject.SetActive(false));

        _maxSpeed *= StaticValues.EnemySpeed;
        _minSpeed *= StaticValues.EnemySpeed;
        StaticValues.CurrentRoomType = "Boss";

        _globalLight = GameObject.FindGameObjectWithTag("Finish").GetComponent<Light2D>();
        _renderer = _transform.GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Update()
    {
        _time -= Time.deltaTime;
        _currentTimeBtwShots -= Time.deltaTime;

        if (_isTakingDmg)
            TakeDamage(_damageTaking / _timeTaking * Time.deltaTime, 0);
    }

    protected override void FixedUpdate()
    {
        if (_currentTimeBtwShots <= 0 && !_agent.isStopped)
        {
            Shot();
            _currentTimeBtwShots = _timeBtwShots;
        }

        if (_time <= 0)
        {
            if (_beforeFurious.Count > 0 && !_isFurious)
                _animator.Play(_beforeFurious[_indexOfAttack]);
            else if (_afterFurious.Count > 0)
                _animator.Play(_afterFurious[_indexOfAttack]);

            if (_time <= 0)
                _time = _reloadTime * Random.Range(0.6f, 1);
            else
                _time += _reloadTime * Random.Range(0.6f, 1);

            if (!_isFurious)
                _indexOfAttack = _indexOfAttack > _beforeFurious.Count - 2 ? 0 : _indexOfAttack + 1;
            else
                _indexOfAttack = _indexOfAttack > _afterFurious.Count - 2 ? 0 : _indexOfAttack + 1;
        }

        if (!_agent.isStopped && ((Vector2)_transform.position - _currentPos).sqrMagnitude <= _randomDistance)
        {
            FindPoint();
            _currentCount++;

            if (_currentCount == _countOfPoints)
            {
                _currentCount = 0;
                StartCoroutine(Stop());
            }
        }
    }
}
