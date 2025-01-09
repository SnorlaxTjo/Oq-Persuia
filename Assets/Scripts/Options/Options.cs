using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public static Options instance;

    [SerializeField] GameObject[] pages;

    [Header("Music Volume")]
    [SerializeField] TextMeshProUGUI musicVolumeText;
    [SerializeField] Slider musicVolumeSlider;

    [Header("SFX Volume")]
    [SerializeField] TextMeshProUGUI sfxVolumeText;
    [SerializeField] Slider sfxVolumeSlider;

    [Header("Fullscreen")]
    [SerializeField] Toggle fullscreenToggle;

    [Header("Resolution")]
    [SerializeField] TMP_Dropdown resolutionDropdown;

    [Header("Autosave")]
    [SerializeField] Toggle autosaveToggle;

    [Header("Autosave Frequency")]
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Button decreaseAutosaveButton;
    [SerializeField] Button increaseAutosaveButton;
    [SerializeField] int minAutosaveValue;
    [SerializeField] int maxAutosaveValue;

    OldOptions oldOptions;

    int currentMusicVolume;
    int currentSfxVolume;

    Resolution[] resolutions;
    List<Resolution> filteredResolutions;
    float currentRefreshRate;
    int currentResolutionIndex;
    int currentAutosaveTime = 5;

    int currentPage;

    public int CurrentMusicVolume { get { return currentMusicVolume; } }
    public int CurrentSfxVolume { get { return currentSfxVolume; } }
    public bool Autosave { get { return autosaveToggle.isOn; } }
    public int CurrentAutosaveTime { get { return currentAutosaveTime; } }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        currentMusicVolume = 100;
        currentSfxVolume = 100;

        #region Fullscreen
        fullscreenToggle.isOn = Screen.fullScreen;
        #endregion

        #region Screen Resolutions
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if ((float)resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                float sixteenbynine = 16f / 9f;
                float resolutionRatio = (float)resolutions[i].width / (float)resolutions[i].height;

                if (sixteenbynine == resolutionRatio)
                {
                    filteredResolutions.Add(resolutions[i]);
                }              
            }
        }

        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRateRatio.value + "Hz";
            resolutionOptions.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        #endregion

        LoadOptions();

        gameObject.SetActive(false);
    }

    public void SaveOldOptions()
    {
        oldOptions.musicVolume = currentMusicVolume;
        oldOptions.sfxVolume = currentSfxVolume;
        oldOptions.autosave = autosaveToggle.isOn;
        oldOptions.autosaveFrequency = currentAutosaveTime;
    }

    public void ChangePage(int direction)
    {
        int newPage = LoopPage(currentPage + direction);
        currentPage = newPage;

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }
    }

    int LoopPage(int page)
    {
        int setPage = 0;

        if (page < 0)
        {
            setPage = pages.Length - 1;
        }
        else if (page >= pages.Length)
        {
            setPage = 0;
        }
        else
        {
            setPage = page;
        }

        return setPage;
    }

    public void SetMusicVolume()
    {
        currentMusicVolume = (int)musicVolumeSlider.value;
        MusicManager musicManager = FindObjectOfType<MusicManager>();

        if (musicManager != null)
        {
            musicManager.ChangeStandardVolume(currentMusicVolume);
        }
        else
        {
            FindObjectOfType<MainMenuMusicManager>().SetVolume();
        }

        musicVolumeText.text = "Music Volume: " + currentMusicVolume;
    }

    public void SetSFXVolume()
    {
        currentSfxVolume = (int)sfxVolumeSlider.value;

        if (SFXManager.instance != null)
        {
            SFXManager.instance.SetVolume(currentSfxVolume);
        }

        sfxVolumeText.text = "Sound Effect Volume: " + currentSfxVolume;
    }

    public void Fullscreen(bool setFullscreen)
    {
        Debug.Log("Fullscreen: " + setFullscreen);
        Screen.fullScreen = setFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ChangeAutosaveTime(int direction)
    {
        currentAutosaveTime += direction;

        SetAutosaveFrequencyButtons();

        timeText.text = currentAutosaveTime + " Minutes";
    }

    void SetAutosaveFrequencyButtons()
    {
        if (currentAutosaveTime <= minAutosaveValue)
        {
            currentAutosaveTime = minAutosaveValue;
            decreaseAutosaveButton.interactable = false;
        }
        else
        {
            decreaseAutosaveButton.interactable = true;
        }

        if (currentAutosaveTime >= maxAutosaveValue)
        {
            currentAutosaveTime = maxAutosaveValue;
            increaseAutosaveButton.interactable = false;
        }
        else
        {
            increaseAutosaveButton.interactable = true;
        }
    }

    public void RevertOptions()
    {
        musicVolumeSlider.value = oldOptions.musicVolume;
        sfxVolumeSlider.value = oldOptions.sfxVolume;
        autosaveToggle.isOn = oldOptions.autosave;
        currentAutosaveTime = oldOptions.autosaveFrequency;

        SetMusicVolume();
        SetSFXVolume();
        SetAutosaveFrequencyButtons();
    }

    public void DistributeOptions()
    {
        ProgressKeeper progressKeeper = FindObjectOfType<ProgressKeeper>();

        if (progressKeeper != null)
        {
            progressKeeper.EnableAutoSave = autosaveToggle.isOn;
            progressKeeper.TimeBetweenAutoSave = currentAutosaveTime * 60;
        }

        SaveOptions();
    }

    void SaveOptions()
    {
        Debug.Log("a");

        PlayerPrefs.SetInt("musicVolume", (int)currentMusicVolume);
        PlayerPrefs.SetInt("sfxVolume", (int)currentSfxVolume);

        int isAutosaveOn;

        if (autosaveToggle.isOn)
        {
            isAutosaveOn = 1;
        }
        else
        {
            isAutosaveOn = 0;
        }

        PlayerPrefs.SetInt("Autosave", isAutosaveOn);
        PlayerPrefs.SetInt("AutosaveFrequency", currentAutosaveTime);       
    }

    void LoadOptions()
    {
        SetMusicVolume();
        SetSFXVolume();
        ChangeAutosaveTime(0);

        if (!PlayerPrefs.HasKey("musicVolume")) { return; }

        musicVolumeSlider.value = PlayerPrefs.GetInt("musicVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetInt("sfxVolume");

        int isAutosaveOn = PlayerPrefs.GetInt("Autosave");
        if (isAutosaveOn == 1)
        {
            autosaveToggle.isOn = true;
        }
        else
        {
            autosaveToggle.isOn = false;
        }
        currentAutosaveTime = PlayerPrefs.GetInt("AutosaveFrequency");       
    }

    public void QuitOptions()
    {
        FindObjectOfType<OptionsAccesser>().QuitOptions();
    }
}

[System.Serializable]
public struct OldOptions
{
    public int musicVolume;
    public int sfxVolume;
    public bool autosave;
    public int autosaveFrequency;

    public OldOptions(int musicVolume, int sfxVolume, bool autosave, int autosaveFrequency)
    {
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
        this.autosave = autosave;
        this.autosaveFrequency = autosaveFrequency;
    }
}

