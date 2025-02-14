using UnityEngine;

public class Slide : MonoBehaviour
{
    public float reloadTime, increaseSpeed, decreaseSpeed;
    private PlayerMovement _player;
    private float _currentTime;

    public void Start()
    {
        _player = GetComponent<PlayerMovement>();
        //_player.ChangeSpeed(increaseSpeed);
        _currentTime = 0;
    }

    public void ChangeTime() => _currentTime = reloadTime;

    private void Update()
    {
        if (_currentTime > 0)
            _player.ChangeSpeed(decreaseSpeed);
        else
            _player.ChangeSpeed(increaseSpeed);

        _currentTime -= Time.deltaTime;
    }
}
