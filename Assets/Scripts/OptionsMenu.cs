using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio Stuff")]
    public AudioMixer audioMixer;
    
    [Header("Resolution Set")]
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public Toggle fullscreenToggle;

    [Header("Graphic Set")] 
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown aaDropdown;
    public TMP_Dropdown textureDropdown;
    
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
        
        LoadSettings(currentResolutionIndex);
        #endregion
    }
    
    
    #region Volume Stuff

    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        volume = VolumeRemapping(volume);
        audioMixer.SetFloat("masterVolume",volume);
    }

    /// <summary>
    /// SFXVolume setting!
    /// </summary>
    /// <param name="volume"> for decibel point value</param>
    public void SFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume",volume);
        volume = VolumeRemapping(volume);
        audioMixer.SetFloat("SFXVolume", volume);
    }

    /// <summary>
    /// Volume setting!
    /// </summary>
    /// <param name="_value"></param>
    /// <returns>This will return the ACCURATE decibel amount for volume.</returns>
    private float VolumeRemapping(float _value)
    {
        return -40 + (_value - 0) * (20 - -40) / (1 - 0);
    }

    #endregion
   

    /// <summary>
    /// This function will Ternary operator the bool! 
    /// </summary>
    /// <param name="isFullscreen"> This is what will decide the game will be fullscreened or not</param>
    public void SetFullScreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt("Fullscreen",(isFullscreen ? 1 : 0));
        Screen.fullScreen = isFullscreen;
    }

    /// <summary>
    /// This function will run on Awake and will grab and list resolutions that are available for the user.
    /// </summary>
    /// <param name="resolutionIndex"> How much resolution does your PC can output (Monitor Dependant/Unity) </param>
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
    
    /// <summary>
    ///  The method will be called by the dropdown Int32 method inside Unity.
    /// </summary>
    /// <param name="qualityIndex"> How much quality exists!</param>
    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != 6) // if the user is not using 
            //any of the presets
            QualitySettings.SetQualityLevel(qualityIndex);
        switch (qualityIndex)
        {
            case 0: // quality level - very low
                textureDropdown.value = 3;
                aaDropdown.value = 0;
                break;
            case 1: // quality level - low
                textureDropdown.value = 2;
                aaDropdown.value = 0;
                break;
            case 2: // quality level - medium
                textureDropdown.value = 1;
                aaDropdown.value = 0;
                break;
            case 3: // quality level - high
                textureDropdown.value = 0;
                aaDropdown.value = 0;
                break;
            case 4: // quality level - very high
                textureDropdown.value = 0;
                aaDropdown.value = 1;
                break;
            case 5: // quality level - ultra
                textureDropdown.value = 0;
                aaDropdown.value = 2;
                break;
        }
        
        qualityDropdown.value = qualityIndex;
    }
    
    /// <summary>
    /// Method that will find all the AA settings and place it in the Dropdown!
    /// </summary>
    /// <param name="aaIndex"> How many AA settings </param>
    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityDropdown.value = 6;
    }

    /// <summary>
    /// Method is called inside Awake and will load inside PlayerPrefs!
    /// </summary>
    /// <param name="currentResolutionIndex"> Self explanatory. </param>
    public void LoadSettings(int currentResolutionIndex)
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
        if (PlayerPrefs.HasKey("TextureQualityPreference"))
            textureDropdown.value = 
                PlayerPrefs.GetInt("TextureQualityPreference");
        else
            textureDropdown.value = 0;
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
    
    /// <summary>
    /// This function gets called by the Unity Button and will save into P-p-player prefs!
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", 
            qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", 
            resolutionDropdown.value);
        PlayerPrefs.SetInt("TextureQualityPreference", 
            textureDropdown.value);
        PlayerPrefs.SetInt("AntiAliasingPreference", 
            aaDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", 
            Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("VolumePreference", 
            currentVolume); 
    }
}