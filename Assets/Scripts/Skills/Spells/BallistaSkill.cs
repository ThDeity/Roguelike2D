using UnityEngine;

public class BallistaSkill : Skill
{
    public GameObject zone;
    public Ballista ballista;
    public float reloadTime;

    private float _currentTime;
    private Transform _zone;

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if (Input.GetMouseButton(1) && _currentTime <= 0 && _zone == null)
            _zone = Instantiate(zone, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity).transform;

        if (_zone != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _zone.position = new Vector3(pos.x, pos.y, 0);
        }

        if (Input.GetMouseButtonUp(1) && _currentTime <= 0)
        {
            Instantiate(ballista.gameObject, _zone.position, Quaternion.identity);

            StartCoroutine(StartTimer((int)reloadTime));
            _currentTime = reloadTime;

            Destroy(_zone.gameObject);
        }
    }
}
