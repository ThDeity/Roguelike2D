using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Shield : Skill, Roll
{
    public float reloadTime, activeTime;
    public GameObject shield;

    private bool _isActive;
    private float _currentTime;

    Collider2D _shield, _collider;

    protected override void Start()
    {
        _collider = GetComponent<Collider2D>();
        shield.transform.localScale *= transform.localScale.x;
    }

    private void OnRoll(bool isStarted = true)
    {
        if (_isActive && _shield != null)
        {
            _shield.enabled = !isStarted;
            _collider.enabled = false;
        }
    }

    public void OnRollStarted() => OnRoll();

    public void OnRollFinished() => OnRoll(false);

    private void Update()
    {
        _currentTime -= Time.deltaTime;
        if (Input.GetMouseButton(1) && _currentTime <= 0)
        {
            _currentTime = reloadTime;
            StartCoroutine(StartTimer((int)reloadTime));

            StartCoroutine(SetShield());
        }
    }

    private IEnumerator SetShield()
    {
        GameObject s = Instantiate(shield, transform);
        _isActive = true;
        _collider.enabled = false;

        yield return new WaitForSeconds(activeTime / 2);
        s.GetComponent<SpriteRenderer>().DOFade(0.1f, activeTime / 2);
        yield return new WaitForSeconds(activeTime / 2);

        _collider.enabled = true;
        _isActive = false;
        Destroy(s);
    }
}
