using RimuruDev;
using UnityEngine;
using YG;
using OneClickLocalization;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private DeviceTypeDetector _detector;
    [SerializeField] private GameObject _joysticksPanel, _mobileGuide, _pcGuide;
    public static bool IsMobile;

    private void Awake()
    {
        if (YG2.lang == "ru")
        {
            OCL.setLanguageAuto(false);
            OCL.SetLanguage(SystemLanguage.Russian);
        }
        else
        {
            OCL.setLanguageAuto(false);
            OCL.SetLanguage(SystemLanguage.English);
        }

        if (_detector.CurrentDeviceType == CurrentDeviceType.WebMobile)
        {
            _joysticksPanel.SetActive(true);
            IsMobile = true;

            if (_pcGuide != null)
                _pcGuide.SetActive(false);
        }
        else
        {
            _joysticksPanel.SetActive(false);
            IsMobile = false;

            if (_mobileGuide != null)
                _mobileGuide.SetActive(false);
        }
    }

    public void TurnOnTutorial()
    {
        if (_mobileGuide == null || _pcGuide == null)
            return;

        if (_detector.CurrentDeviceType == CurrentDeviceType.WebMobile)
            _mobileGuide.SetActive(true);
        else
            _pcGuide.SetActive(true);
    }
}