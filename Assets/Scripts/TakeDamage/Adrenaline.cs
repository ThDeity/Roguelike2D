using System.Collections;
using UnityEngine;

public class Adrenaline : MonoBehaviour, OnTakeDmg
{
    public float buffTime, interval, buff, buffCd;
    private float _currentTime;

    private PlayerMovement player;

    private void Start() => player = GetComponent<PlayerMovement>();

    private void Update() => _currentTime -= Time.deltaTime;

    public void OnTakeDmg()
    {
        if (_currentTime > 0) return;

        StartCoroutine(Buff());
        _currentTime = interval;
    }

    private IEnumerator Buff()
    {
        player.speed *= buff;
        player.dashCd *= buffCd;
        player.dashTime *= buff;
        StaticValues.PlayerAttackList.ForEach(x => { x.bullet.GetComponent<Bullet>().damage *= buff; x.reloadTime *= buffCd; });

        yield return new WaitForSeconds(buffTime);

        player.speed /= buff;
        player.dashCd /= buffCd;
        player.dashTime /= buff;
        StaticValues.PlayerAttackList.ForEach(x => { x.bullet.GetComponent<Bullet>().damage /= buff; x.reloadTime /= buffCd; });
    }
}
