using UnityEngine;

public class Charm : Skill
{
    public CharmSector zone;
    public float reloadTime, timeOfCharming, increaseParam;
    public int maxEnemies;

    private float _currentTime;
    private CharmSector _zone;

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0 && Input.GetMouseButtonDown(1))
            _zone = Instantiate(zone.gameObject, transform).GetComponent<CharmSector>();

        if (_zone != null && Input.GetMouseButtonUp(1))
        {
            int enemies = 0;
            foreach(var effect in _zone.effects)
            {
                if (enemies < maxEnemies)
                    effect.Charming(timeOfCharming, increaseParam);

                enemies++;
            }

            StartCoroutine(StartTimer((int)reloadTime));
            _currentTime = reloadTime;

            Destroy(_zone.gameObject);
        }
    }
}
