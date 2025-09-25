using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, Interactive
{
    [SerializeField] private GameObject _buttonIcon;
    [SerializeField] private int _roomsPerArea, _index;
    [SerializeField] private Transform _pointForPrize, _pointForButton;

    [Tooltip("0 - Parametres, 1 - ActiveSkills, 2 - PassiveSkills, 3 - Enemy, 4 - Default, 5 - Boss")]
    [SerializeField] private List<GameObject> _prizes;

    private GameObject _icon, _buttonE, _currentArea;
    private bool _isPlayerNear, _wasPortal;

    public static int NumOfArea;
    private void Start()
    {
        _currentArea = FindObjectOfType<SpawnPrize>().gameObject;

        _buttonE = Instantiate(_buttonIcon, _pointForButton.position, Quaternion.identity);
        _buttonIcon.SetActive(false);
    }

    private void Update()
    {
        if (_isPlayerNear && Input.GetKeyDown(KeyCode.E) && !_wasPortal)
        {
            Interact();
        }
    }

    public void SetPrize(int index)
    {
        _index = index;

        _icon = Instantiate(_prizes[index], _pointForPrize.position, Quaternion.identity);
        _icon.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (_icon != null)
            _icon.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == StaticValues.PlayerTransform)
        {
            if (!PlatformController.IsMobile)
                _buttonE.SetActive(true);
            else
            {
                StaticValues.PlayerObj.interactionButton.SetActive(true);
                StaticValues.InteractButtonObj.interactiveObj = this;
            }
            _isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform == StaticValues.PlayerTransform && _buttonE != null)
        {
            if (!PlatformController.IsMobile)
                _buttonE.SetActive(false);
            else
                StaticValues.PlayerObj.interactionButton.SetActive(false);
            _isPlayerNear = false;
        }
    }

    private void OnDisable()
    {
        _wasPortal = false;

        if (_icon != null) _icon.SetActive(false);
        if (_buttonE != null) _buttonE.SetActive(false);
    }

    private void OnDestroy()
    {
        Destroy(_icon);
        Destroy(_buttonE);
    }

    public void Interact()
    {
        if (_wasPortal) return;

        _wasPortal = true;

        foreach (Portal p in FindObjectsOfType<Portal>())
        {
            if (p != this)
                Destroy(p.gameObject);
            //p.gameObject.SetActive(false);
        }

        _currentArea.SetActive(false);
        StaticValues.RoomsBeforeBoss += 1;

        if (StaticValues.RoomsBeforeBoss % _roomsPerArea == 0 && StaticValues.RoomsBeforeBoss != 0)
        {
            StaticValues.CurrentRoomType = StaticValues.RoomTypes[5];

            _currentArea = StaticValues.Bosses[NumOfArea].gameObject;

            StaticValues.RoomsBeforeBoss = 0;
            NumOfArea += 1;
        }
        else// if (StaticValues.RoomsBeforeBoss % _roomsPerArea != 0 || StaticValues.RoomsBeforeBoss == 0)
        {
            StaticValues.CurrentRoomType = StaticValues.RoomTypes[_index];

            int index = StaticValues.RoomsBeforeBoss % StaticValues.Areas[NumOfArea].Length;
            if (StaticValues.Areas[NumOfArea][index] != null)
                _currentArea = StaticValues.Areas[NumOfArea][index].gameObject;
            else
                _currentArea = StaticValues.Bosses[StaticValues.Bosses.Length - 1].gameObject;
        }

        _currentArea.SetActive(true);
        if (_currentArea.TryGetComponent(out SpawnPrize component))
            StaticValues.PlayerTransform.position = component.playerPointSpawn == null ? Vector2.zero : component.playerPointSpawn.position;

        StaticValues.PlayerObj.StartCoroutine(StaticValues.PlayerObj.SetImmortal(1f));
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }
}