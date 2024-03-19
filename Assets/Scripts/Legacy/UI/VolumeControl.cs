using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] string volumeParameter;
    private float currentVolume;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle muteToggle;
    private bool disableToggleEvent;

    private void Awake()
    {
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
        muteToggle.onValueChanged.AddListener(ToggleMute);
    }

    private void Start()
    {
        UpdateVolume(PlayerPrefs.GetFloat(volumeParameter, volumeSlider.value));
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, volumeSlider.value); 
    }
    public void UpdateVolume(float sliderValue)
    {
        //this turns the value in decibel(from logarithmic) so slider volume is changed evenly
        AudioManager.Instance.mainMixer.SetFloat(volumeParameter, Mathf.Log10(sliderValue) * 20);
        disableToggleEvent = true;
        muteToggle.isOn = volumeSlider.value > volumeSlider.minValue;
        disableToggleEvent = false;
        
    }
    private void ToggleMute(bool soundOn)
    {
        if (disableToggleEvent)
            return;

        if (soundOn)
            volumeSlider.value = currentVolume;
        else
        {
            currentVolume = volumeSlider.value;
            volumeSlider.value = volumeSlider.minValue;
        }
    }

}
