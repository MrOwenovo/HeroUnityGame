using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ResolutionController : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown; 
    private Resolution originalResolution;
    private List<Resolution> resolutions;

    void Start()
    {
        originalResolution = Screen.currentResolution;

        resolutions = new List<Resolution>(Screen.resolutions);
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ResetResolution()
    {
        Screen.SetResolution(originalResolution.width, originalResolution.height, Screen.fullScreen);
        resolutionDropdown.value = resolutions.FindIndex(r => r.width == originalResolution.width && r.height == originalResolution.height);
        resolutionDropdown.RefreshShownValue();
    }
}