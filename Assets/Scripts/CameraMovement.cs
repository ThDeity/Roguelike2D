using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform _playerPosition;
    private Vector3 _playerVector;
    [SerializeField] private float speed;

    private void Start() { _playerPosition = FindObjectOfType<PlayerMovement>().transform; }

    private void FixedUpdate()
    {
        _playerVector = _playerPosition.position;
        _playerVector.z = -10;
        transform.position = Vector3.Lerp(transform.position, _playerVector, speed * Time.deltaTime);
    }
}
