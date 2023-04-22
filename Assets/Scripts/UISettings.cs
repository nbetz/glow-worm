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
    public FlexibleColorPicker enemyColorPicker;
    
    public Material wormMaterial;
    public Material foodMaterial;
    public Material enemyMaterial;

    public Color defaultWormColor;
    public Color defaultFoodColor;
    public Color defaultEnemyColor;
    
    private Color savedWormColor;
    private Color savedFoodColor;
    private Color savedEnemyColor;

    public Image currentWormColor;
    public Image currentFoodColor;
    public Image currentEnemyColor;

    /// <summary>
    /// Unity start function - ran on first frame
    /// </summary>
    void Start()
    {
        // Color - get the saved colors
        Color tempColor1;
        Color tempColor2;
        Color tempColor3;
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("WormColor"), out tempColor1))
            savedWormColor = tempColor1;
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("FoodColor"), out tempColor2))
            savedFoodColor = tempColor2;
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("EnemyColor"), out tempColor3))
            savedEnemyColor = tempColor3;

        // Set the current color images to the saved colors
        currentWormColor.color = savedWormColor;
        currentFoodColor.color = savedFoodColor;
        currentEnemyColor.color = savedEnemyColor;
        
        // Set the worm material colors to the saved colors
        wormMaterial.color = savedWormColor;
        foodMaterial.color = savedFoodColor;
        enemyMaterial.color = savedEnemyColor;
        
        // Set the color picker colors to the saved colors
        wormColorPicker.color = savedWormColor;
        foodColorPicker.color = savedFoodColor;
        enemyColorPicker.color = savedEnemyColor;

        wormColorPicker.startingColor = savedWormColor;
        foodColorPicker.startingColor = savedFoodColor;
        enemyColorPicker.startingColor = savedEnemyColor;

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
        if(enemyColorPicker.color != savedEnemyColor)
        {
            enemyMaterial.color = enemyColorPicker.color;
            enemyMaterial.SetColor("_EmissionColor", enemyColorPicker.color);

            // Save it to a string
            PlayerPrefs.SetString("EnemyColor", "#" + ColorUtility.ToHtmlStringRGB(enemyColorPicker.color));
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
        currentEnemyColor.color = savedEnemyColor;
        
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
        PlayerPrefs.SetString("EnemyColor", "#" + ColorUtility.ToHtmlStringRGB(defaultEnemyColor));
        SceneManager.LoadScene("MainMenu");

    }

}
