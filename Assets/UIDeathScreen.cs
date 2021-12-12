using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDeathScreen : MonoBehaviour
{
    /// <summary>
    /// Target function for try again button
    /// </summary>
    public void Click_TryAgain()
    {
        // Reload the game scene
        SceneManager.LoadScene("Game");
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
