using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public GameObject settingsCanvas;

    public void ClickPlay()
    {
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
    }
}
