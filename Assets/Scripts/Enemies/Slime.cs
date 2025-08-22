using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private List<Transform> _pointsToSeparate;
    [SerializeField] private float _decreaseSize;
    [SerializeField] private GameObject _slime;
    [SerializeField] private AudioClip _separionSound;

    public int countOfSeparates;

    public override void TakeDamage(float damage, float time, bool isLifesteal, float lifesteal)
    {
        if (time == 0)
        {
            _currentHp -= damage;
            _bar.RemoveValue(damage);

            if (isLifesteal)
                StaticValues.PlayerObj.TakeDamage(-damage * lifesteal, 0, false, 0);

            if (_currentHp <= 0)
            {
                if (countOfSeparates > 0)
                {
                    if (EffectsSource != null)
                        EffectsSource.PlayOneShot(_separionSound);

                    _animator.Play("Separate");
                }
                else
                    Destroy(gameObject);
            }
            else if (_currentHp > maxHp)
                _currentHp = maxHp;
        }
        else
        {
            _isTakingDmg = true;
            _damageTaking += damage;
            _timeTaking += time;

            if (isLifesteal)
            {
                _isLifesteal = true;
                _lifestealToPlayer = lifesteal;
            }

            if (_timeTaking > 0)
                StopCoroutine(TakingDamage(time));

            StartCoroutine(TakingDamage(_timeTaking));
        }

        if (!_isPlayerNear)
        {
            _isPlayerNear = true;
            _rotateToObj = target;
        }

        if (_hpBar != null && !_hpBar.activeInHierarchy)
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