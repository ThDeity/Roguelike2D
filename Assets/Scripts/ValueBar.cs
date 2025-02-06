using UnityEngine;

public class ValueBar : MonoBehaviour
{
    [SerializeField] private Transform _lineBar;

    public void SetValue(float value)
    {
        if (value < 0) value = 0f;
        if (value > 1f) value = 1f;
        _lineBar.localScale = new Vector2(value, 1f);
    }
}