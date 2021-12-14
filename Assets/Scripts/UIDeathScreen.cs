using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDeathScreen : MonoBehaviour
{

    public TextMeshProUGUI scoreText;

    private void Update()
    {
        scoreText.text = "SCORE: " + PlayerPrefs.GetInt("PrevScore");
    }

    /// <summary>
    /// Target function for try again button
    /// </summary>
    public void Click_TryAgain()
    {
        // Reload the game scene
        FindObjectOfType<GameController>().deathScreenActive = false;
        FindObjectOfType<GameController>().deathScreen.SetActive(false);
        FindObjectOfType<GameController>().gameHUD.SetActive(true);
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
