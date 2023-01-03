using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using TMPro;


// SettingsManager and SettingsController are different
// SettingsManager controls the actual changes on the settings from the player and saves them
// SettingsController is where the player changes the settings that the player wants
public class SettingsMenu : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] AudioMixer audioMixer;

    [Header("Settings Visually")]
    [SerializeField] TMP_Dropdown localizationDropdown;

    [Space]

    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;

    [Space]

    [SerializeField] TMP_Dropdown resolutionsDropdown;
    [SerializeField] Toggle fullscreenToggle;

    // Local variables
    bool localizationActive = false;

    Resolution[] resolutions;


    void Start() 
    {
        // Getting default values

        // Language
        if (PlayerPrefs.HasKey("Locale"))
        {
            SetLanguage(PlayerPrefs.GetInt("Locale"));
        
            // Changing dropdown
            if (localizationDropdown != null) localizationDropdown.value = PlayerPrefs.GetInt("Locale");
            
        }
        else
        {
            int _selectedLocale = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
            
            // Setting in player preferences
            PlayerPrefs.SetInt("Locale", _selectedLocale);
        
            // Changing dropdown
            if (localizationDropdown != null) localizationDropdown.value = _selectedLocale;
        }


        // Master volume
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume"); 
        audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        // Music volume
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume"); 
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        // SFX volume
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume"); 
        audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));


        // Getting resolution options
        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        int _currentResolution = 0;
        int _preferredResolution = -1;
        // Creating the list of strings to array
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height + " : " + resolutions[i].refreshRate);
        

            // Is this the current resolution?
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height &&
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
                _currentResolution = i;


            // Is this the the expected resolution?
            if (resolutions[i].width == PlayerPrefs.GetInt("Width") &&
                resolutions[i].height == PlayerPrefs.GetInt("Height") &&
                resolutions[i].refreshRate == PlayerPrefs.GetInt("RefreshRate"))
                _preferredResolution = i;
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = _currentResolution;
        
        // Set preferred resolution as current resolution
        if (_preferredResolution != -1)
        {
            resolutionsDropdown.value = _preferredResolution;
            Resolution _resolution = resolutions[_preferredResolution];
            Screen.SetResolution(_resolution.width, _resolution.height, Screen.fullScreen, _resolution.refreshRate);
        }

        resolutionsDropdown.RefreshShownValue();


        // Should be fullscreen?
        Screen.fullScreen = PlayerPrefs.GetInt("IsFullscreen") == 1 ? false : true;
        fullscreenToggle.isOn = Screen.fullScreen;
    }


    public void SetLanguage(int _localeID)
    {
        if (localizationActive)
            return;
            
        StartCoroutine(SetLocale(_localeID));
    }

    IEnumerator SetLocale(int _localeID) 
    {
        localizationActive = true;

        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];

        PlayerPrefs.SetInt("Locale", _localeID);
    
        localizationActive = false;
    }


    // Changing the volume of the audio in game, divided in three categories

    // Master volume, that controls "every" volume
    public void SetMasterVolume(float _volume)
    {
        audioMixer.SetFloat("MasterVolume", _volume);
    
        // Changing in player preferences for future
        PlayerPrefs.SetFloat("MasterVolume", _volume);
    }
    
    // Music volume, that controls just the music
    public void SetMusicVolume(float _volume)
    {
        audioMixer.SetFloat("MusicVolume", _volume);
    
        // Changing in player preferences for future
        PlayerPrefs.SetFloat("MusicVolume", _volume);
    }

    // SFX volume that controls the sound effects of the game
    public void SetSFXVolume(float _volume)
    {
        audioMixer.SetFloat("SFXVolume", _volume);
    
        // Changing in player preferences for future
        PlayerPrefs.SetFloat("SFXVolume", _volume);
    }


    // Graphics

    // Set the current resolution
    public void SetResolution(int _resolutionIndex) 
    {
        Resolution resolution = resolutions[_resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    
        // Changing player preferences for future
        PlayerPrefs.SetInt("Width", resolution.width);
        PlayerPrefs.SetInt("Height", resolution.height);
        PlayerPrefs.SetInt("RefreshRate", resolution.refreshRate);
    }

    // Change the screen from windows mode to fullscreen
    public void SetFullscreen(bool _isFullscreen) 
    {
        Screen.fullScreen = _isFullscreen;
    
        // Changing player preferences for futue
        PlayerPrefs.SetInt("IsFullscreen", _isFullscreen ? 0 : 1);
    }
}