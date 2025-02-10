using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform _playerTransform;
    private Vector3 _playerVector;
    [SerializeField] private float speed;

    private void Start() { _playerTransform = FindObjectOfType<PlayerMovement>().transform; }

    private void FixedUpdate()
    {
        if (_playerTransform != null)
        {
            _playerVector = _playerTransform.position;
            _playerVector.z = -10;
            transform.position = Vector3.Lerp(transform.position, _playerVector, speed * Time.deltaTime);
        }
    }
}
