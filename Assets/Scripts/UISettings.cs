using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISettings : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    /// <summary>
    /// Unity start function - ran on first frame
    /// </summary>
    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Function target for the "back" button
    /// </summary>
    public void ClickBack()
    {
        gameObject.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    /// <summary>
    /// Function to switch fullscreen modes 
    /// </summary>
    /// <param name="isFullscreen"></param>
    public void SetFullscreen(bool isFullscreen)
    {
        //Screen.fullScreen = isFullscreen;
    }

    /// <summary>
    /// Function to switch the resolution to the selected one
    /// on the dropdown
    /// </summary>
    /// <param name="resolutionIndex"></param>
    public void SetResolution(int resolutionIndex)
    {
        //Resolution resolution = resolutions[resolutionIndex];
        //Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


}
