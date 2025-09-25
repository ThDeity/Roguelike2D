using DG.Tweening;
using UnityEngine;

public class Medkit : MonoBehaviour, Interactive
{
    [SerializeField] private float _lifetime, _healPersent;
    [SerializeField] protected GameObject _buttonE;

    private void Start()
    {
        GetComponent<SpriteRenderer>().DOFade(0.05f, _lifetime);
        Destroy(gameObject, _lifetime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Interact();
    }

    public void Interact()
    {
        StaticValues.PlayerObj.TakeDamage(-StaticValues.PlayerObj.GetMaxHp() * _healPersent, 0, false, 0);
        Destroy(gameObject);
    }
}
