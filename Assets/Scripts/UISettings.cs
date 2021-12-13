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
        // Color - get the saved colors
        Color tempColor1;
        Color tempColor2;
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("WormColor"), out tempColor1))
            savedWormColor = tempColor1;
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("FoodColor"), out tempColor2))
            savedFoodColor = tempColor2;

        // Set the current color images to the saved colors
        currentWormColor.color = savedWormColor;
        currentFoodColor.color = savedFoodColor;
        
        // Set the worm material colors to the saved colors
        wormMaterial.color = savedWormColor;
        foodMaterial.color = savedFoodColor;
        
        // Set the color picker colors to the saved colors
        wormColorPicker.color = savedWormColor;
        foodColorPicker.color = savedFoodColor;

        wormColorPicker.startingColor = savedWormColor;
        foodColorPicker.startingColor = savedFoodColor;

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

}
