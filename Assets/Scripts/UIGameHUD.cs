using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGameHUD : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    /// <summary>
    /// Unity start function - ran first frame
    /// </summary>
    void Start()
    {
        //On start, set the text to 0
        scoreText.text = "SCORE: 0";
    }

    /// <summary>
    /// Unity update function - ran every frame
    /// </summary>
    void Update()
    { 
        //Update the score on the canvas with the current score
        //in the game controller
        scoreText.text = "SCORE: " + GameController.score;
    }
}
