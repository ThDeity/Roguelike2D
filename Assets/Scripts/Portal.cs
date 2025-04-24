using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject _buttonIcon;
    [SerializeField] private int _roomsPerArea, _roomsCount, _index;
    [SerializeField] private Transform _pointForPrize, _pointForButton;

    [Tooltip("0 - Parametres, 1 - ActiveSkills, 2 - PassiveSkills, 3 - Enemy, 4 - Default, 5 - Boss")]
    [SerializeField] private List<GameObject> _prizes = new List<GameObject>();
    
    GameObject _icon, _buttonE;
    private void Awake()
    {
        _buttonE = Instantiate(_buttonIcon, _pointForButton.position, Quaternion.identity);
        _buttonIcon.SetActive(false);
    }

    public void SetPrize(int index)
    {
        _index = index;

        _icon = Instantiate(_prizes[index], _pointForPrize.position, Quaternion.identity);
        gameObject.SetActive(false);
        _icon.SetActive(false);
    }

    private void OnEnable()
    {
        if (_icon != null)
            _icon.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            _buttonE.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _buttonE != null)
            _buttonE.gameObject.SetActive(false);
    }

    bool _isDamaged;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            float damage = StaticValues.PlayerObj.ChangeMxHp(1) - StaticValues.PlayerObj.currentHp;
            if (damage > 0 && !_isDamaged)
            {
                Player.DamageOnStart = damage;
                _isDamaged = true;
            }

            if (StaticValues.RoomsBeforeBoss % _roomsPerArea != 0)
            {
                StaticValues.CurrentRoomType = StaticValues.RoomTypes[_index];
                SceneManager.LoadScene($"{StaticValues.RoomsBeforeBoss / _roomsPerArea}{Random.Range(0, _roomsCount)}");
            }
            else
            {
                StaticValues.CurrentRoomType = StaticValues.RoomTypes[5];
                SceneManager.LoadScene((StaticValues.RoomsBeforeBoss / _roomsPerArea).ToString() + "_Boss");
            }
        }
    }
}
