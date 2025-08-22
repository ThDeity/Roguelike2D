using UnityEngine;

public class HealthVignetteEffect : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Material _effectMaterial;
    [SerializeField][Range(0, 1)] private float _healthThreshold = 0.3f;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_effectMaterial != null && _player != null)
        {
            // Добавляем отладочный вывод
            Debug.Log($"Applying effect. Health: {_player.currentHp}/{_player.GetMaxHp()}");

            // Рассчитываем текущее здоровье в диапазоне 0-1
            float healthPercent = Mathf.Clamp01(_player.currentHp / _player.GetMaxHp());
            _effectMaterial.SetFloat("_CurrentHealth", healthPercent);
            _effectMaterial.SetFloat("_HealthThreshold", _healthThreshold);

            // Применяем эффект
            Graphics.Blit(source, destination, _effectMaterial);
        }
        else
        {
            Debug.Log("hehe");

            // Если эффект отключен, просто передаём изображение
            Graphics.Blit(source, destination);
        }
    }

    private void Update()
    {
        // Обновляем параметры в реальном времени для отладки
        if (_effectMaterial != null)
        {
            _effectMaterial.SetFloat("_VignetteIntensity", 0.5f);
            _effectMaterial.SetFloat("_VignetteSoftness", 0.5f);
            _effectMaterial.SetFloat("_GrayscaleIntensity", 0.8f);
        }
    }
}