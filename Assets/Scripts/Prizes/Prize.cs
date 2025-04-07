using UnityEngine;

public class Prize : MonoBehaviour
{
    [SerializeField] protected GameObject _buttonE;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            _buttonE.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            _buttonE.gameObject.SetActive(false);
    }
}
