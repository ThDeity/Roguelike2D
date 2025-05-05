using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _offset, _dashSpeed;
    private float _time, _dashTimeCd, _currentSpeed;
    private Vector2 _velocity, _dashVector;
    private Rigidbody2D _rigidbody2D;
    public float speed, dashCd, dashTime;

    public int rollsCount;

    private  List<Roll> _rolls = new List<Roll>();

    protected static float Speed, DashCd, DashTime, Offset, DashSpeed;
    protected static int RollsCount;
    public void Reset()
    {
        _offset = Offset;
        _dashSpeed = DashSpeed;
        speed = Speed;
        dashCd = DashCd;
        dashTime = DashTime;
        rollsCount = RollsCount;
    }

    public void CheckComponents() => _rolls = GetComponents<Roll>().ToList();

    private void Awake()
    {
        if (DashSpeed == 0)
        {
            Offset = _offset;
            DashSpeed = _dashSpeed;
            Speed = speed;
            DashCd = dashCd;
            DashTime = dashTime;
            RollsCount = 1;
        }
    }

    private void Start()
    {
        _rolls = GetComponents<Roll>().ToList();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _time = 0;
        _currentSpeed = speed;
        _dashTimeCd = 0;
    }

    public void ChangeSpeed(float newSpeed, bool isChange = true) => speed = isChange ? _currentSpeed * newSpeed : _currentSpeed;

    public void PlusSpeed(float newSpeed, bool isChange = true) => speed = isChange ? _currentSpeed + newSpeed : _currentSpeed;

    private void FixedUpdate()
    {
        _velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        _rigidbody2D.velocity = _velocity * speed;

        _dashTimeCd -= Time.deltaTime;

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float roatZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, roatZ + _offset);

        if (Input.GetKey(KeyCode.Space))
            Dash();

        if (_time > 0)
        {
            _rigidbody2D.AddForce(_dashVector * _dashSpeed, ForceMode2D.Impulse);
            _time -= Time.deltaTime;
        }
    }

    private float _speedBeforeDash;
    private IEnumerator EnableCollider()
    {
        for (int i = 0; i < rollsCount; i++)
        {
            speed = 0f;
            _time = dashTime;
            _rolls.ForEach(r => r.OnRollStarted());

            gameObject.layer = LayerMask.NameToLayer("Void");
            yield return new WaitForSeconds(dashTime);

            if (i != rollsCount - 1)
            {
                _rolls.ForEach(r => r.OnRollFinished());
                speed = 0.1f;
                yield return new WaitForSeconds(0.05f);
            }
        }

        speed = _speedBeforeDash;
        gameObject.layer = LayerMask.NameToLayer("Player");

        _rolls.ForEach(r => r.OnRollFinished());
    }

    private void Dash()
    {
        if (_time > 0 || _dashTimeCd > 0 || speed == 0) return;

        _dashTimeCd = dashCd;
        _speedBeforeDash = speed;

        if (_velocity.magnitude != 0)
            _dashVector = _velocity.normalized;
        else
            _dashVector = Vector2.up;

        StartCoroutine(EnableCollider());
    }
}
