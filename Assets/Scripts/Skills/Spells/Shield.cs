using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Shield : Skill, Roll
{
    public float reloadTime, activeTime;
    public GameObject shield;

    private bool _isActive;
    private float _currentTime;

    protected static float ReloadTime, ActiveTime;
    public override void ResetAll()
    {
        reloadTime = ReloadTime;
        activeTime = ActiveTime;
    }

    protected override void Start()
    {
        base.Start();

        if (ReloadTime == 0)
        {
            ReloadTime = reloadTime;
            ActiveTime = activeTime;
        }

        shield.transform.localScale *= transform.localScale.x;
    }

    private void OnRoll(bool isStarted = true)
    {
        if (_isActive && _shieldObj != null)
        {
            if (isStarted)
                _shieldObj.layer = LayerMask.NameToLayer("Void");
            else
                _shieldObj.layer = LayerMask.NameToLayer("Player");
        }
    }

    public override void OnRollStarted() => OnRoll();

    public void OnRollFinished() => OnRoll(false);

    private void Update()
    {
        _currentTime -= Time.deltaTime;
        if ((Input.GetMouseButtonUp(1) || (joystick != null && joystick.isDragging)) && _currentTime <= 0)
        {
            _currentTime = reloadTime;
            StartCoroutine(StartTimer((int)reloadTime));

            StaticValues.PlayerObj.effectsSource.PlayOneShot(skillSound);
            StartCoroutine(SetShield());
        }
    }

    GameObject _shieldObj;
    private IEnumerator SetShield()
    {
        _shieldObj = Instantiate(shield, transform);
        _isActive = true;

        yield return new WaitForSeconds(activeTime / 2);
        _shieldObj.GetComponent<SpriteRenderer>().DOFade(0.1f, activeTime / 2);
        yield return new WaitForSeconds(activeTime / 2);

        _isActive = false;
        Destroy(_shieldObj);
    }
}
