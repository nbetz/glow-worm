using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettings : MonoBehaviour
{
    public GameObject mainMenuCanvas;

    public void ClickBack()
    {
        gameObject.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

}
