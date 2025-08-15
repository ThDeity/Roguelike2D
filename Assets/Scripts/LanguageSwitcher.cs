using UnityEngine;
using UnityEngine.UI;
using OneClickLocalization;

public class LanguageSwitcher : MonoBehaviour
{
    [SerializeField] private Button _englishButton, _russianButton;

    void Start()
    {
        // Назначаем обработчики кнопок
        _englishButton.onClick.AddListener(() => SwitchLanguage(SystemLanguage.English));
        _russianButton.onClick.AddListener(() => SwitchLanguage(SystemLanguage.Russian));

        // Инициализируем текущий язык
        UpdateButtonStates(OCL.GetLanguage());

        // Подписываемся на смену языка
        OCL.onLanguageChanged += OnLanguageChanged;
    }

    void OnDestroy()
    {
        OCL.onLanguageChanged -= OnLanguageChanged;
    }

    private void SwitchLanguage(SystemLanguage language)
    {
        // Отключаем автоопределение и устанавливаем язык
        OCL.setLanguageAuto(false);
        OCL.SetLanguage(language);
    }

    private void OnLanguageChanged(SystemLanguage oldLang, SystemLanguage newLang)
    {
        UpdateButtonStates(newLang);
        Debug.Log($"Language changed to: {newLang}");
    }

    private void UpdateButtonStates(SystemLanguage currentLanguage)
    {
        _englishButton.interactable = (currentLanguage != SystemLanguage.English);
        _russianButton.interactable = (currentLanguage != SystemLanguage.Russian);
    }
}