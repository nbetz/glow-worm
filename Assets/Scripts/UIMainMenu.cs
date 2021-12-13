using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{

    public GameObject settingsCanvas;

    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI titleText;

    public TMP_FontAsset gameFont;
    
    public Material wormMaterial;
    public Material foodMaterial;
    private Color savedWormColor;
    private Color savedFoodColor;

    public Animation animation;

    // Unit Testing Variables
    public bool quit = false;
    public int highScore;
    private void Start()
    {
        // Load data
        highScoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore");
        highScore = PlayerPrefs.GetInt("HighScore");
        
        Color tempColor1;
        Color tempColor2;
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("WormColor"), out tempColor1))
            savedWormColor = tempColor1;
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("FoodColor"), out tempColor2))
            savedFoodColor = tempColor2;

        titleText.color = savedWormColor;

        gameFont.material = wormMaterial;

        // Set the material colors to the saved colors
        wormMaterial.color = savedWormColor;
        wormMaterial.SetColor("_EmissionColor", savedWormColor);
        
        foodMaterial.color = savedFoodColor;
        foodMaterial.SetColor("_EmissionColor", savedFoodColor);
    }

    public void ClickPlay()
    {
        animation.Play("MainMenuClose");
        //SceneManager.LoadScene("Game");
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Game");
    }
    public void ClickSettings()
    {
        gameObject.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void ClickQuit()
    {
        Application.Quit();
        quit = true;
    }
}
