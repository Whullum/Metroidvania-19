using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider soundEffectVolumeSlider;

    [Header("Wwise RTPC")]
    public AK.Wwise.RTPC masterRTPC;
    public AK.Wwise.RTPC musicRTPC;
    public AK.Wwise.RTPC sfxRTPC;

    private void Start()
    {
        SetVolumeSliderValues();
    }

    public void VolumeSlidersBehavior(string value)
    {
        switch (value)
        {
            case "master":
                masterRTPC.SetGlobalValue(masterVolumeSlider.value);
                break;
            case "music":
                musicRTPC.SetGlobalValue(musicVolumeSlider.value);
                break;
            case "sfx":
                sfxRTPC.SetGlobalValue(soundEffectVolumeSlider.value);
                break;
        }
    }

    public void SetVolumeSliderValues()
    {
        masterVolumeSlider.value = masterRTPC.GetValue(gameObject);
        musicVolumeSlider.value = musicRTPC.GetValue(gameObject);
        soundEffectVolumeSlider.value = sfxRTPC.GetValue(gameObject);
    }
}
