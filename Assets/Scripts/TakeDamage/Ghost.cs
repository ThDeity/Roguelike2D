using System.Collections;
using UnityEngine;

public class Ghost : MonoBehaviour, OnTakeDmg, Roll
{
    public Color color;
    public float interval, timeOf;
    private float _currentTime;

    private Collider2D _collider;
    private SpriteRenderer _renderer;

    bool _isTookDmg;
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update() => _currentTime -= Time.deltaTime;

    public void OnTakeDmg()
    {
        if (_currentTime > 0) return;

        _isTookDmg = true;
        StartCoroutine(BecomeGhost());
        _isTookDmg = false;

        _currentTime = interval;
    }

    public void OnRollFinished()
    {
        if (!_isTookDmg) return;

        _renderer.color = color;
        _collider.enabled = false;
    }

    private IEnumerator BecomeGhost()
    {
        Color standartColor = _renderer.color;
        _renderer.color = color;
        _collider.enabled = false;

        yield return new WaitForSeconds(timeOf);

        _renderer.color = standartColor;
        _collider.enabled = true;
    }
}
