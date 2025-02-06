using UnityEngine;

public class Spikes : MonoBehaviour, OnTakeDmg
{
    public Bullet spike;
    public int spikesCount;
    public float interval, radius;

    private float _currentTime;

    private void Update() => _currentTime -= Time.deltaTime;

    public void OnTakeDmg()
    {
        if (_currentTime > 0) return;

        float angle = 0, delta = 360f / spikesCount;

        for (int i = 0; i < spikesCount; i++)
        {
            Vector2 pos = new Vector2(transform.position.x + radius * Mathf.Cos(angle), transform.position.y + radius * Mathf.Sin(angle));

            Instantiate(spike.gameObject, pos, Quaternion.Euler(0, 0, angle));
            angle += delta;
        }

        _currentTime = interval;
    }
}
