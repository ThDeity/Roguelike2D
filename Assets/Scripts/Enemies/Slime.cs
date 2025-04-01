using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class Slime : Enemy
{
    [SerializeField] private List<Transform> _pointsToSeparate;
    [SerializeField] private float _decreaseSize;
    [SerializeField] private GameObject _slime;

    public int countOfSeparates;

    public override void TakeDamage(float damage, float time)
    {
        if (time == 0)
        {
            _currentHp -= damage;
            _bar.RemoveValue(damage);

            if (_currentHp <= 0)
            {
                if (countOfSeparates > 0)
                    _animator.Play("Separate");
                else
                    Destroy(gameObject);
            }
            else if (_currentHp > maxHp)
                _currentHp = maxHp;
        }
        else
        {
            _isTakingDmg = true;
            _damageTaking = damage;
            _timeTaking = time;

            StartCoroutine(TakingDamage(time));
        }

        if (!_isPlayerNear)
        {
            _isPlayerNear = true;
            _rotateToObj = target;
        }

        if (!_hpBar.activeInHierarchy)
            _hpBar.SetActive(true);
    }

    public void SplitUp()
    {
        for (int i = 0; i < _pointsToSeparate.Count; i++)
        {
            Slime slime = Instantiate(_slime, _pointsToSeparate[i].position, Quaternion.identity).GetComponent<Slime>();

            slime.countOfSeparates = countOfSeparates - 1;
            slime.GetComponent<NavMeshAgent>().speed *= 2 - _decreaseSize;
            slime.transform.localScale *= _decreaseSize;
            slime.maxHp *= _decreaseSize;

            slime.ChangeReloadCd(_decreaseSize);
        }

        Destroy(gameObject);
    }
}