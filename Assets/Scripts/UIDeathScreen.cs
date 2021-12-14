using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDeathScreen : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI titleText;
    public String titleTextString = "GAME OVER!";

    public Color backgroundColor = new Color (0.368f, 0f, 0f, 0f);
    public Image Background;


    /// <summary>
    /// Sets the information for the end screen
    /// </summary>
    public void Activate()
    {
        scoreText.text = "SCORE: " + PlayerPrefs.GetInt("PrevScore");
        titleText.text = titleTextString;
        Background.color = backgroundColor;
    }

    /// <summary>
    /// Target function for try again button
    /// </summary>
    public void Click_TryAgain()
    {
        // Reload the game scene
        FindObjectOfType<GameController>().endScreenActive = false;
        FindObjectOfType<GameController>().deathScreen.SetActive(false);
        FindObjectOfType<GameController>().gameHUD.SetActive(true);
        enabled = false;        
    }

    /// <summary>
    /// Target function for quit button
    /// </summary>
    public void Click_Quit()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
