using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour {

  

    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Dropdown antialiasingDropdown;
    public Dropdown vSyncDropdown;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Dropdown difficultyDropdwon;
    public Button applyButton;

    private AudioSource musicSource;

    public Resolution[] resolutions;
    public GameSettings gameSettings;


    private string settingsPath;

    private void Awake()
    {
        settingsPath = Application.persistentDataPath + "/gamesettings.json";
        musicSource = SoundManager.instance.musicSource;
        gameSettings = new GameSettings();
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        qualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSfxVolumeChange(); });
        difficultyDropdwon.onValueChanged.AddListener(delegate { OnDifficultyChange(); });
        resolutions = Screen.resolutions;
        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
        if (File.Exists(settingsPath))
        {
            LoadSettings();
        }
    }

    private void OnEnable()
    {
       
       
    }


    public void OnFullscreenToggle()
    {
        
        gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }

    public void OnTextureQualityChange()
    {
       QualitySettings.masterTextureLimit = gameSettings.textureQuality = qualityDropdown.value;
       
    }

    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = gameSettings.antialiasing = (int) Mathf.Pow(2f, antialiasingDropdown.value);
    }

    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
    }
    public void OnMusicVolumeChange()
    {
        musicSource.volume = gameSettings.musicVolume = musicVolumeSlider.value;
    }

    public void OnSfxVolumeChange()
    {
        gameSettings.sfxVolume = sfxVolumeSlider.value;
        foreach(AudioSource source in SoundManager.instance.sfxSources)
        {
            source.volume = gameSettings.sfxVolume;
        }
    }

    public void OnDifficultyChange()
    {
        gameSettings.difficulty = difficultyDropdwon.value;
    }

    public void OnApplyButtonClick()
    {
        SaveSettings();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(settingsPath, jsonData);
    }
    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(settingsPath));
        musicVolumeSlider.value = gameSettings.musicVolume;
        antialiasingDropdown.value = gameSettings.antialiasing;
        vSyncDropdown.value = gameSettings.vSync;
        qualityDropdown.value = gameSettings.textureQuality;
        resolutionDropdown.value = gameSettings.resolutionIndex;
        fullscreenToggle.isOn = gameSettings.fullscreen;
        sfxVolumeSlider.value = gameSettings.sfxVolume;
        difficultyDropdwon.value = gameSettings.difficulty;

        resolutionDropdown.RefreshShownValue();
    }

}
