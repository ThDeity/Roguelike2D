using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HealthPostProcessing : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private Player player;

    [Header("Effect Settings")]
    [SerializeField][Range(0, 1)] private float healthThreshold = 0.3f;
    [SerializeField] private float transitionSpeed = 2f;

    [Header("Effect Intensity")]
    [SerializeField][Range(0, 1)] private float maxVignetteIntensity = 0.5f;
    [SerializeField][Range(-100, 100)] private float minSaturation = -100f;

    private PostProcessVolume volume;
    private Vignette vignette;
    private ColorGrading colorGrading;

    private float currentVignetteIntensity = 0f;
    private float currentSaturation = 0f;

    void Start()
    {
        // Получаем компонент PostProcessVolume
        volume = GetComponent<PostProcessVolume>();

        // Получаем настройки эффектов из профиля
        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGetSettings(out vignette);
            volume.profile.TryGetSettings(out colorGrading);
        }

        // Проверяем, что все компоненты на месте
        if (player == null)
        {
            Debug.LogError("Player reference is not set in HealthPostProcessing!");
            enabled = false;
            return;
        }

        if (vignette == null || colorGrading == null)
        {
            Debug.LogError("Vignette or ColorGrading effects are missing from PostProcessVolume!");
            enabled = false;
            return;
        }

        // Изначально выключаем эффекты
        vignette.active = false;
        colorGrading.active = false;
    }

    public bool forceEnableEffects = false;
    void Update()
    {
        if (forceEnableEffects)
        {
            // Принудительно включаем эффекты для тестирования
            if (vignette != null)
            {
                vignette.active = true;
                vignette.intensity.value = maxVignetteIntensity;
            }

            if (colorGrading != null)
            {
                colorGrading.active = true;
                colorGrading.saturation.value = minSaturation;
            }
            return;
        }

        if (player == null) return;

        // Рассчитываем процент здоровья
        float healthPercent = Mathf.Clamp01(player.currentHp / player.GetMaxHp());
        bool isLowHealth = healthPercent <= healthThreshold;

        // Рассчитываем интенсивность эффектов
        float targetVignetteIntensity = isLowHealth ?
            Mathf.Lerp(0, maxVignetteIntensity, 1 - (healthPercent / healthThreshold)) : 0;

        float targetSaturation = isLowHealth ?
            Mathf.Lerp(0, minSaturation, 1 - (healthPercent / healthThreshold)) : 0;

        // Плавно изменяем интенсивность эффектов
        currentVignetteIntensity = Mathf.Lerp(currentVignetteIntensity, targetVignetteIntensity, Time.deltaTime * transitionSpeed);
        currentSaturation = Mathf.Lerp(currentSaturation, targetSaturation, Time.deltaTime * transitionSpeed);

        // Применяем эффекты
        if (vignette != null)
        {
            Debug.Log("vignette");

            vignette.active = currentVignetteIntensity > 0.01f;
            vignette.intensity.value = currentVignetteIntensity;
        }

        if (colorGrading != null)
        {
            colorGrading.active = Mathf.Abs(currentSaturation) > 1f;
            colorGrading.saturation.value = currentSaturation;
        }

        // Отладочная информация
        Debug.Log($"Health: {healthPercent:P0}, Vignette: {currentVignetteIntensity:F2}, Saturation: {currentSaturation:F0}");
    }

    // Метод для принудительного сброса эффектов (например, при возрождении)
    public void ResetEffects()
    {
        currentVignetteIntensity = 0f;
        currentSaturation = 0f;

        if (vignette != null)
        {
            vignette.active = false;
            vignette.intensity.value = 0f;
        }

        if (colorGrading != null)
        {
            colorGrading.active = false;
            colorGrading.saturation.value = 0f;
        }
    }
}