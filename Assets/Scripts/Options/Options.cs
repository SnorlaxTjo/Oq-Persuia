using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] GameObject[] pages;

    [Header("Resolution")]
    [SerializeField] TMP_Dropdown resolutionDropdown;

    [Header("Autosave Frequency")]
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Button decreaseAutosaveButton;
    [SerializeField] Button increaseAutosaveButton;
    [SerializeField] int minAutosaveValue;
    [SerializeField] int maxAutosaveValue;

    Resolution[] resolutions;
    List<Resolution> filteredResolutions;
    float currentRefreshRate;
    int currentResolutionIndex;
    int currentAutosaveTime = 5;

    int currentPage;

    private void Start()
    {
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

    public void ChangeAutosaveTime(int direction)
    {
        currentAutosaveTime += direction;

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

        timeText.text = currentAutosaveTime + " Minutes";
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void QuitOptions()
    {
        FindObjectOfType<OptionsAccesser>().QuitOptions();
    }
}
