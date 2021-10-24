using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [Header("Audio Stuff")]
    public AudioMixer audioMixer;
    
    [Header("Resolution Set")]
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public Toggle fullscreenToggle;

    [Header("Anti-Aliasing Set")] 
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown aaDropdown;
    
    [Header("Volume Set")]
    public Slider masterVolume;
    public Slider sfxVolumeSlider;
    float currentVolume;
    
    
    public static bool loadData = false;

    public void Awake()
    {
        #region Resolution Start
        resolutionDropdown.ClearOptions();
        List<string> resOptions = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " +
                            resolutions[i].height;
            resOptions.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i; 
            }
        }

        resolutionDropdown.AddOptions(resOptions);
        if (PlayerPrefs.HasKey("Resolution"))
        {
            int resIndex = PlayerPrefs.GetInt("Resolution");
            resolutionDropdown.value = resIndex;
            resolutionDropdown.RefreshShownValue();
            SetResolution(resIndex);
        }
        else
        {
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
        #endregion
    }

    private void Start()
    {
        LoadSettings();
    }

    #region Volume Stuff

    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        volume = VolumeRemapping(volume);
        audioMixer.SetFloat("masterVolume",volume);
    }

    public void SFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume",volume);
        volume = VolumeRemapping(volume);
        audioMixer.SetFloat("SFXVolume", volume);
    }

    private float VolumeRemapping(float _value)
    {
        return -40 + (_value - 0) * (20 - -40) / (1 - 0);
    }

    #endregion
   

    public void SetFullScreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt("Fullscreen",(isFullscreen ? 1 : 0));
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.value = 6;
    }
    
    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityDropdown.value = 6;
    }

    private void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropdown.value = 
                PlayerPrefs.GetInt("QualitySettingPreference");
        else
            qualityDropdown.value = 3;
        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value = 
                PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;
        if (PlayerPrefs.HasKey("AntiAliasingPreference"))
            aaDropdown.value = 
                PlayerPrefs.GetInt("AntiAliasingPreference");
        else
            aaDropdown.value = 1;
        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen = 
                Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;
    }
}