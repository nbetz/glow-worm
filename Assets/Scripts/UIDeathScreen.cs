using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDeathScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    
    /// <summary>
    /// Target function for try again button
    /// </summary>
    public void Click_TryAgain()
    {
        // Reload the game scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Target function for quit button
    /// </summary>
    public void Click_Quit()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Unity start function - ran first frame
    /// </summary>
    void Start()
    {
        //Set the score text to the score from game controller
        scoreText.text = "SCORE: " + GameController.score;
    }
}
