using System.Collections.Generic;
using UnityEngine;

public class Lifesteal : MonoBehaviour
{
    [Tooltip("Dmg - процент от максимального ХП игрока")]
    public float damage, intervalBtwDmg;
    public int enemiesCount;
    public Lifesteal lifesteal;

    private float _currentTime = 0;
    protected List<IDamagable> _enemies = new List<IDamagable>();

    private Lifesteal _currentLifesteal;
    protected void Start()
    {
        if (lifesteal != null)
            Instantiate(lifesteal, transform);

        _currentLifesteal = StaticValues.PlayerObj.GetComponent<Lifesteal>();
        damage = StaticValues.PlayerObj.ChangeMxHp(1) * damage;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (lifesteal != null)
        {
            damage = _currentLifesteal.damage;
            intervalBtwDmg = _currentLifesteal.intervalBtwDmg;
            enemiesCount = _currentLifesteal.enemiesCount;
        }

        if (collision.TryGetComponent(out IDamagable enemy) && collision.tag != tag && !_enemies.Contains(enemy) && _enemies.Count < enemiesCount)
            _enemies.Add(enemy);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable enemy) && _enemies.Contains(enemy))
            _enemies.Remove(enemy);
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable enemy) && collision.tag != tag && _enemies.Contains(enemy))
        {
            if (_currentTime <= 0)
            {
                enemy.TakeDamage(damage, 0);
                _currentTime = intervalBtwDmg;

                StaticValues.PlayerObj.TakeDamage(-damage, 0);
            }

            _currentTime -= Time.deltaTime;
        }
    }
}
