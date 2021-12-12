using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Instance variables
    public GameObject prefabPlayer;
    public GameObject prefabBodyPiece;
    public static float marginOfError = 0.0005f;
    public static float speed = 0.03f;
    public static List<GameObject> bodyPieceObjects = new List<GameObject>();
    public static List<BodyController> bodyPieces = new List<BodyController>();
    public static bool moveLock = false;
    GameObject player;
    public GameObject gameHUD;
    public GameObject deathScreen;

    /// <summary>
    /// Unity start function - ran on first frame
    /// </summary>
    void Start()
    {
        //create the player at 0,0,0
        moveLock = true;
        player = Object.Instantiate(prefabPlayer, new Vector3(0f, 0f, 0f), Quaternion.identity);
        moveLock = false;
    }

    /// <summary>
    /// Unity update function - ran every frame
    /// </summary>
    void Update()
    {
        //add 2 body pieces for testing purposes
        if(bodyPieces.Count == 0)
        {
            addBodyPiece();
        }
        else if (bodyPieces.Count == 1)
        {
            addBodyPiece();
        }
    }

    /// <summary>
    /// Adds a body piece to the worm
    /// </summary>
    public void addBodyPiece()
    {
        if (bodyPieceObjects.Count == 0)
        {
            moveLock = true;
            Vector3 position = player.transform.position;
            Vector3 priorVel = PlayerController.velocity;
            if (PlayerController.velocity.x > 0)
            {
                position.x = position.x - 1f;
            }
            else if (PlayerController.velocity.x < 0)
            {
                position.x = position.x + 1f;
            }
            else if (PlayerController.velocity.z > 0)
            {
                position.z = position.z - 1f;
            }
            else if (PlayerController.velocity.z < 0)
            {
                position.z = position.z + 1f;
            }

            bodyPieceObjects.Add(Object.Instantiate(prefabBodyPiece, position, Quaternion.identity));
            int bodyPieceNum = bodyPieceObjects.Count - 1;
            bodyPieces.Add(bodyPieceObjects[bodyPieceNum].GetComponent<BodyController>());
            bodyPieces[bodyPieceNum].bodyPieceNum = bodyPieceNum;
            moveLock = false;
        }
        else
        {
            moveLock = true;
            Vector3 position = bodyPieces[bodyPieces.Count - 1].transform.position;
            Vector3 priorVel = bodyPieces[bodyPieces.Count - 1].velocity;
            if (bodyPieces[bodyPieces.Count - 1].velocity.x > 0)
            {
                position.x = position.x - 1f;
            }
            else if (bodyPieces[bodyPieces.Count - 1].velocity.x < 0)
            {
                position.x = position.x + 1f;
            }
            else if (bodyPieces[bodyPieces.Count - 1].velocity.z > 0)
            {
                position.z = position.z - 1f;
            }
            else if (bodyPieces[bodyPieces.Count - 1].velocity.z < 0)
            {
                position.z = position.z + 1f;
            }
            
            bodyPieceObjects.Add(Object.Instantiate(prefabBodyPiece, position, Quaternion.identity));
            int bodyPieceNum = bodyPieceObjects.Count - 1;
            bodyPieces.Add(bodyPieceObjects[bodyPieceNum].GetComponent<BodyController>());
            bodyPieces[bodyPieceNum].bodyPieceNum = bodyPieceNum;
            moveLock = false;
        }
    }

    /// <summary>
    /// Function ran when the player dies
    /// </summary>
    public void Die()
    {
        // Hide the game HUD and show the death screen
        gameHUD.SetActive(false);
        deathScreen.SetActive(true);
    }
}
