using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeMaster : MonoBehaviour
{
    private const float DisabledVolume = -80;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _mixerParameter;
    [SerializeField] private float _minimumVolume;

    private void Start()
    {
        _volumeSlider.SetValueWithoutNotify(GetMixerVolume());
    }

    public void UpdateMixerVolume(float volume) => SetMixerVolume(volume);

    private void SetMixerVolume(float volumeValue)
    {
        float mixerVolume;
        if (volumeValue == 0)
            mixerVolume = DisabledVolume;
        else
            mixerVolume = Mathf.Lerp(_minimumVolume, 0, volumeValue);

        _audioMixer.SetFloat(_mixerParameter, mixerVolume);
    }

    private float GetMixerVolume()
    {
        _audioMixer.GetFloat(_mixerParameter, out float mixerVolume);
        if (mixerVolume == DisabledVolume)
            return 0;
        else
            return Mathf.Lerp(1, 0, mixerVolume / _minimumVolume);
    }
}