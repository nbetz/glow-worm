using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyController : MonoBehaviour
{
    //Instance variables
    public Vector3 velocity;

    public List<Vector3> lastRotatePositionQueue = new List<Vector3>();
    public List<Vector3> lastVelocityQueue = new List<Vector3>();
    public int bodyPieceNum;

    public Material material;

    public Color defaultColor;

    public Light light;
    public GameObject cube;

    public bool coroutineRunning = false;
    public bool reset;

    // Start is called before the first frame update
    void Start()
    {
        //set fixedUpdate time to 100FPS
        Time.fixedDeltaTime = 0.01f;

        //set the velocity
        if (bodyPieceNum == 0)
            velocity = EnemyController.velocity;
        else
            velocity = GameController.enemyBodyPieces[bodyPieceNum - 1].velocity;

        //create the body color on object creation
        material = GetComponentInChildren<Renderer>().material;
        defaultColor = material.GetColor("_EmissionColor");
        material.SetColor("_EmissionColor", Color.black);
        light = GetComponentInChildren<Light>();
        light.color = defaultColor;
        cube = this.gameObject.transform.GetChild(0).gameObject;
    }

    /// <summary>
    /// starts the colorfade coroutine to fade the color to black over 1 second
    /// </summary>
    public void GlowFade()
    {
        // Make sure we stop any other nonsense running
        StopAllCoroutines();
        // Get the current color
        Color oldColor = material.GetColor("_EmissionColor");
        cube.layer = 0;
        StartCoroutine(ColorFade(oldColor, Color.black, 2f, 0f, 0, 1.0f));
    }

    /// <summary>
    /// starts the colorfade coroutine to reset the color to the starting color over .25 seconds
    /// </summary>
    public void GlowReset()
    {
        // Make sure we stop any other nonsense running
        StopAllCoroutines();
        // Get the current color
        Color oldColor = material.GetColor("_EmissionColor");
        StartCoroutine(ColorFade(oldColor, defaultColor, 0f, 2f, 6, 0.25f));
        cube.layer = 6;
    }

    /// <summary>
    /// coroutine to lerp from a start color to an end color over a set time. Changes the
    /// emission color of a body piece.
    /// </summary>  
    ///<param name="start">The starting color</param>
    ///<param name="end">The color to end on</param>
    ///<param name="duration">The time to go from the start color to the end color</param>  
    IEnumerator ColorFade(Color start, Color end, float startFloat, float endFloat, int layer, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            material.SetColor("_EmissionColor", Color.Lerp(start, end, normalizedTime));
            light.intensity = Mathf.Lerp(startFloat, endFloat, normalizedTime);
            yield return null;
        }
        cube.layer = layer;
        light.intensity = endFloat;
        material.SetColor("_EmissionColor", end); //without this, the value will end at something like 0.9992367
    }

    /// <summary>
    /// moves the body piece in whatever direction it needs to move
    /// </summary>
    public void MoveBody()
    {
        bool snapped = false;
        //if its the first piece, check if it needs to rotate based upon the player itself
        if (bodyPieceNum == 0)
        {
            //check to see if the player has queued any rotations
            if (EnemyController.lastRotatePositionQueue.Count != 0)
            {
                Vector3 lastVelocity = EnemyController.lastVelocityQueue[0];
                Vector3 lastRotatePosition = EnemyController.lastRotatePositionQueue[0];

                //if the body piece is within margin of error for the last rotate position of the player,
                //change the direction
                if ((transform.position.x <= lastRotatePosition.x + GameController.speed &&
                    transform.position.x >= lastRotatePosition.x - GameController.marginOfError) &&
                    (transform.position.z <= lastRotatePosition.z + GameController.speed &&
                    transform.position.z >= lastRotatePosition.z - GameController.marginOfError))
                {

                    //informs FixedUpdate() in gamecontroller that it needs to snap the body to the grid
                    GameController.snapToGrid = true;
                    snapped = true;

                    //set the velocity to the players then removes from player queues and adds to self queues
                    velocity = lastVelocity;
                    lastRotatePositionQueue.Add(lastRotatePosition);
                    lastVelocityQueue.Add(lastVelocity);
                    EnemyController.lastRotatePositionQueue.RemoveAt(0);
                    EnemyController.lastVelocityQueue.RemoveAt(0);
                }
            }
        }

        //if not the first piece, check if it needs to rotate based upon the piece infront of it
        else
        {
            //checks to see if there if the bodypiece infront of it has queued any rotations
            if (GameController.enemyBodyPieces[bodyPieceNum - 1].lastRotatePositionQueue.Count != 0)
            {
                Vector3 lastVelocity = GameController.enemyBodyPieces[bodyPieceNum - 1].lastVelocityQueue[0];
                Vector3 lastRotatePosition = GameController.enemyBodyPieces[bodyPieceNum - 1].lastRotatePositionQueue[0];

                //check if bodypiece is within margin of error from the grid position it should be at.
                if ((transform.position.x <= lastRotatePosition.x + GameController.speed &&
                    transform.position.x >= lastRotatePosition.x - GameController.marginOfError) &&
                    (transform.position.z <= lastRotatePosition.z + GameController.speed &&
                    transform.position.z >= lastRotatePosition.z - GameController.marginOfError))
                {
                    //informs FixedUpdate() in gamecontroller that it needs to snap the body to the grid
                    GameController.snapToGrid = true;
                    snapped = true;

                    //set the velocity to the bodyPiece in front of it, then removes from that pieces queues and adds to its own
                    velocity = lastVelocity;
                    lastRotatePositionQueue.Add(lastRotatePosition);
                    lastVelocityQueue.Add(lastVelocity);
                    GameController.enemyBodyPieces[bodyPieceNum - 1].lastRotatePositionQueue.RemoveAt(0);
                    GameController.enemyBodyPieces[bodyPieceNum - 1].lastVelocityQueue.RemoveAt(0);
                }
            }

            //check if the body piece has any children, 
            //and if it has rotations queued up but is 1 unit past the queued rotation
            if (lastRotatePositionQueue.Count > 0 && GameController.enemyBodyPieces.Count < bodyPieceNum + 2 &&
                ((transform.position.x <= lastRotatePositionQueue[0].x + 1 + GameController.speed * 2 &&
                transform.position.x >= lastRotatePositionQueue[0].x + 1 - GameController.marginOfError) ||
                (transform.position.x >= lastRotatePositionQueue[0].x - 1 - GameController.speed * 2 &&
                transform.position.x <= lastRotatePositionQueue[0].x - 1 + GameController.marginOfError) ||
                (transform.position.z <= lastRotatePositionQueue[0].z + 1 + GameController.speed * 2 &&
                transform.position.z >= lastRotatePositionQueue[0].z + 1 - GameController.marginOfError) ||
                (transform.position.z >= lastRotatePositionQueue[0].z - 1 - GameController.speed * 2 &&
                transform.position.z <= lastRotatePositionQueue[0].z - 1 + GameController.marginOfError)))
            {
                //remove the first rotation in the body pieces queue
                lastRotatePositionQueue.RemoveAt(0);
                lastVelocityQueue.RemoveAt(0);
            }
        }

        //move the body every FixedUpdate frame if snapped is false
        if (snapped == false)
            transform.Translate(velocity);
    }
}
