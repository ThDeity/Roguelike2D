using UnityEngine;
using DG.Tweening;

public class Charge : MonoBehaviour
{
    public float timeToMaximize, _buffDmg, _Dmg, speedDebuff;
    public GameObject chargingBullet;
    public Vector2 maxSize;
    public KeyCode key;

    private Vector2 _minSize, _currentSize;
    private Transform _point;
    float _minDmg, _maxDmg;
    GameObject bullet;

    private void Start()
    {
        _minSize = _currentSize = StaticValues.PlayerAttackList[0].bullet.transform.localScale;
        _minDmg = StaticValues.PlayerAttackList[0].bullet.GetComponent<Bullet>().damage;
        _maxDmg = _minDmg * _buffDmg;
        _Dmg = _minDmg;

        _point = StaticValues.PlayerAttackList[0]._point;
        bullet = Instantiate(chargingBullet, _point);
        bullet.transform.localScale = _minSize;
        bullet.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(key))
        {
            _currentSize = bullet.transform.localScale;
            bullet.SetActive(true);
            bullet.transform.DOScale(maxSize, timeToMaximize).SetEase(Ease.Linear);

            StaticValues.PlayerMovementObj.ChangeSpeed(speedDebuff);

            DOTween.To(() => _Dmg, x => _Dmg = x, _maxDmg, timeToMaximize).SetEase(Ease.Linear);
        }
        else if (Input.GetKeyUp(key))
        {
            bullet.SetActive(false);
            DOTween.Clear();
            _Dmg = _minDmg;
            bullet.transform.localScale = _minSize;
            StaticValues.PlayerMovementObj.ChangeSpeed(0, false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            bullet.SetActive(false);
            DOTween.CompleteAll();
            Debug.Log(_currentSize);

            foreach(var attack in StaticValues.PlayerAttackList)
            {
                attack.bullet.transform.localScale = _currentSize;
                Debug.Log(Equals(attack.bullet.transform.localScale, _currentSize));
                attack.bullet.GetComponent<Bullet>().damage = _Dmg;
            }

            //Debug.Log(Equals(StaticValues.PlayerAttackList[0].bullet.transform.localScale, _currentSize));
            StaticValues.PlayerMovementObj.ChangeSpeed(0, false);
            bullet.transform.localScale = _minSize;
            _Dmg = _minDmg;
            DOTween.Clear();

            foreach (var attack in StaticValues.PlayerAttackList)
            {
                attack.bullet.transform.localScale = _minSize;
                attack.bullet.GetComponent<Bullet>().damage = _minDmg;
            }
        }
    }
}
