using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ValueSystem
{
    [SerializeField] private ValueEvent ValueChanged = new();
    private float _value;
    private float _valueMax;

    public void Setup(float value)
    {
        _value = _valueMax = value;
        SayChanged();
    }

    public void SetupMax(float max)
    {
        _valueMax = max;
        SayChanged();
    }

    public void AddValue(float value)
    {
        _value += value;
        if (_value > _valueMax) _value = _valueMax;
        SayChanged();
    }

    public void RemoveValue(float value)
    {
        _value -= value;
        if (_value < 0) _value = 0;
        SayChanged();
    }

    public void SayChanged() => ValueChanged.Invoke(_value / _valueMax);
}

[System.Serializable]
public class ValueEvent : UnityEvent<float> { }