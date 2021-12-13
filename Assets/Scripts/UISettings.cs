using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    
    // Color variables
    public FlexibleColorPicker wormColorPicker;
    public FlexibleColorPicker foodColorPicker;
    public Material wormMaterial;
    public Material foodMaterial;

    public Color defaultWormColor;
    public Color defaultFoodColor;
    
    private Color savedWormColor;
    private Color savedFoodColor;

    public Image currentWormColor;
    public Image currentFoodColor;

    /// <summary>
    /// Unity start function - ran on first frame
    /// </summary>
    void Start()
    {
        // Resolution
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
        
        // Color - get the saved colors
        Color tempColor1;
        Color tempColor2;
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("WormColor"), out tempColor1))
            savedWormColor = tempColor1;
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("FoodColor"), out tempColor2))
            savedFoodColor = tempColor2;

        // Set the color picker colors to the saved colors
        wormColorPicker.color = savedWormColor;
        foodColorPicker.color = savedFoodColor;

        // Set the current color images to the saved colors
        currentWormColor.color = savedWormColor;
        currentFoodColor.color = savedFoodColor;
        
        // Set the worm material colors to the saved colors
        wormMaterial.color = savedWormColor;
        foodMaterial.color = savedFoodColor;
        
        
    }

    /// <summary>
    /// Unity update function - ran every frame
    /// </summary>
    void Update()
    {
        // If we change the colors
        if (wormColorPicker.color != savedWormColor)
        {
            // Update the worm material color
            wormMaterial.color = wormColorPicker.color;
            wormMaterial.SetColor("_EmissionColor", wormColorPicker.color);
            
            Debug.Log(wormColorPicker.color);
            
            // Save it to a string
            PlayerPrefs.SetString("WormColor", "#" + ColorUtility.ToHtmlStringRGB(wormColorPicker.color));
        }

        if (foodColorPicker.color != savedFoodColor)
        {
            // Update the food material color
            foodMaterial.color = foodColorPicker.color;
            foodMaterial.SetColor("_EmissionColor", foodColorPicker.color);
            
            // Save it to a string
            PlayerPrefs.SetString("FoodColor", "#" + ColorUtility.ToHtmlStringRGB(foodColorPicker.color));
        }
            
    }

    /// <summary>
    /// Function target for the "back" button
    /// </summary>
    public void ClickBack()
    {
        // Set the current color images to the saved colors
        currentWormColor.color = savedWormColor;
        currentFoodColor.color = savedFoodColor;
        
        gameObject.SetActive(false);
        mainMenuCanvas.SetActive(true);
        
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Function target for "reset" button
    /// </summary>
    public void ClickReset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.SetString("WormColor", "#" + ColorUtility.ToHtmlStringRGB(defaultWormColor));
        PlayerPrefs.SetString("FoodColor", "#" + ColorUtility.ToHtmlStringRGB(defaultFoodColor));
        SceneManager.LoadScene("MainMenu");

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
