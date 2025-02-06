using UnityEngine;

public class Lifesteal : MonoBehaviour
{
    [Tooltip("Dmg - процент от максимального ХП игрока")]
    public float damage, intervalBtwDmg;
    private float _currentTime = 0;
    public Lifesteal lifesteal;

    protected void Start()
    {
        if (lifesteal != null)
            Instantiate(lifesteal, transform);

        damage = StaticValues.PlayerObj.ChangeMxHp(1) * damage;
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable enemy))
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
