using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BodyController : MonoBehaviour
{
    //Instance variables
    public Vector3 velocity;

    public Vector3 spawnLocation;
    public Vector3 spawnLocation2;
    public Vector3 spawnVelocity;

    public List<Vector3> lastRotatePositionQueue = new List<Vector3>();
    public List<Vector3> lastVelocityQueue = new List<Vector3>();
    public int bodyPieceNum;

    public Material material;

    public Color defaultColor;

    public bool coroutineRunning = false;
    public bool reset;
    
    /// <summary>
    /// Unity start function - ran on first frame after object creation
    /// </summary>
    void Start()
    {
        //set fixedUpdate time to 100FPS
        Time.fixedDeltaTime = 0.01f;

        //set the velocity
        if(bodyPieceNum == 0)
            velocity = PlayerController.velocity;
        else
            velocity = GameController.bodyPieces[bodyPieceNum - 1].velocity;

        spawnLocation = transform.position;
        //AlignToGrid();
        //spawnLocation2 = transform.position;

        // //remove the gap between pieces by clipping into the piece in front
        // transform.Translate(velocity);  

        material = GetComponentInChildren<Renderer>().material;
        defaultColor = material.GetColor("_EmissionColor");
        //Debug.Log("Previous color " + material.color.r + " , " + material.color.g + " , " + material.color.b);
        //float randR = Random.Range(0, 255);
        //float randG = Random.Range(0, 255);
        //float randB = Random.Range(0, 255);
        //material.SetColor("_EmissionColor", new Color(randR / 100, randG / 100, randB / 100, 1.0f));
        //Debug.Log("New color " + material.color.r + " , " + material.color.g + " , " + material.color.b);
    }

    public void GlowFade()
    {
        // Make sure we stop any other nonsense running
        StopAllCoroutines();
        // Get the current color
        Color oldColor = material.GetColor("_EmissionColor");
        StartCoroutine(DoAThingOverTime(oldColor, Color.black, 1.0f));
    }

    public void GlowReset()
    {
        // Make sure we stop any other nonsense running
        StopAllCoroutines();
        // Get the current color
        Color oldColor = material.GetColor("_EmissionColor");
        StartCoroutine(DoAThingOverTime(oldColor, defaultColor, 0.5f));
    }
    
    IEnumerator DoAThingOverTime(Color start, Color end, float duration)
    {
        for (float t=0f;t<duration;t+=Time.deltaTime) {
            float normalizedTime = t/duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            material.SetColor("_EmissionColor", Color.Lerp(start, end, normalizedTime));
            yield return null;
        }
        material.SetColor("_EmissionColor", end); //without this, the value will end at something like 0.9992367
    }
    // private IEnumerator GlowResetRoutine()
    // {
    //     while (coroutineRunning)
    //         yield return null;
    //     
    //     Debug.Log("Reset coroutine");
    //     coroutineRunning = true;
    //     float setR = oldColor.r;
    //     float setG = oldColor.g;
    //     float setB = oldColor.b;
    //     while (setR <= oldColor.r || setG <= oldColor.g || setB <= oldColor.b)
    //     {
    //         material.SetColor("_EmissionColor", new Color (setR, setG, setB));
    //         setR += 0.1f;
    //         setG += 0.1f;
    //         setB += 0.1f;
    //         yield return new WaitForSeconds(0.1f);
    //         //Debug.Log("Previous color " + material.GetColor("_EmissionColor").r + " , " + material.GetColor("_EmissionColor").g + " , " + material.GetColor("_EmissionColor").b);
    //     }
    //
    //     coroutineRunning = false;
    //     yield return null;
    // }
    
    

    
    // private IEnumerator GlowFadeRoutine()
    // {
    //     // Color tempColor1;
    //     // Color savedWormColor;
    //     // if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("WormColor"), out tempColor1))
    //     //     savedWormColor = tempColor1;
    //
    //     while (coroutineRunning)
    //         yield return null;
    //
    //     Debug.Log("Fade coroutine");
    //     coroutineRunning = true;
    //     oldColor = material.GetColor("_EmissionColor");
    //     float setR = oldColor.r;
    //     float setG = oldColor.g;
    //     float setB = oldColor.b;
    //     while (setR >= 0.0f || setG >= 0.0f || setB >= 0.0f)
    //     {
    //         material.SetColor("_EmissionColor", new Color (setR, setG, setB));
    //         setR -= 0.1f;
    //         setG -= 0.1f;
    //         setB -= 0.1f;
    //         yield return new WaitForSeconds(0.1f);
    //         //Debug.Log("Previous color " + material.GetColor("_EmissionColor").r + " , " + material.GetColor("_EmissionColor").g + " , " + material.GetColor("_EmissionColor").b);
    //     }
    //
    //     coroutineRunning = false;
    //     yield return null;
    // }    

    public void MoveBody(){
        bool snapped = false;
         //if its the first piece, check if it needs to rotate based upon the player itself
        if (bodyPieceNum == 0)
        {
            //check to see if the player has queued any rotations
            if (PlayerController.lastRotatePositionQueue.Count != 0) 
            {
                Vector3 lastVelocity = PlayerController.lastVelocityQueue[0];
                Vector3 lastRotatePosition = PlayerController.lastRotatePositionQueue[0];

                //if the body piece is within margin of error for the last rotate position of the player,
                //change the direction
                if ((transform.position.x <= lastRotatePosition.x + GameController.speed &&
                    transform.position.x >= lastRotatePosition.x - GameController.marginOfError) &&
                    (transform.position.z <= lastRotatePosition.z + GameController.speed && 
                    transform.position.z >= lastRotatePosition.z - GameController.marginOfError))
                {
                    //GameController.moveLock = true;

                    //alligns the body piece to the grid
                    //FindObjectOfType<GameController>().AlignAll();
                    GameController.snapToGrid = true;
                    snapped = true;

                    //set the velocity to the players then removes from player queues and adds to self queues
                    velocity = lastVelocity;
                    lastRotatePositionQueue.Add(lastRotatePosition);
                    lastVelocityQueue.Add(lastVelocity);
                    PlayerController.lastRotatePositionQueue.RemoveAt(0);
                    PlayerController.lastVelocityQueue.RemoveAt(0);
                    //GameController.moveLock = false;
                }
            }
        }

        //if not the first piece, check if it needs to rotate based upon the piece infront of it
        else
        {
            //checks to see if there if the bodypiece infront of it has queued any rotations
            if (GameController.bodyPieces[bodyPieceNum - 1].lastRotatePositionQueue.Count != 0) 
            {
                Vector3 lastVelocity = GameController.bodyPieces[bodyPieceNum - 1].lastVelocityQueue[0];
                Vector3 lastRotatePosition = GameController.bodyPieces[bodyPieceNum - 1].lastRotatePositionQueue[0];

                //check if bodypiece is within margin of error from the grid position it should be at.
                if ((transform.position.x <= lastRotatePosition.x + GameController.speed &&
                    transform.position.x >= lastRotatePosition.x - GameController.marginOfError) &&
                    (transform.position.z <= lastRotatePosition.z + GameController.speed && 
                    transform.position.z >= lastRotatePosition.z - GameController.marginOfError))
                {
                    //GameController.moveLock = true;

                    //aligns the body piece to the grid
                    //FindObjectOfType<GameController>().AlignAll();
                    GameController.snapToGrid = true;
                    snapped = true;

                    //set the velocity to the bodyPiece in front of it, then removes from that pieces queues and adds to its own
                    velocity = lastVelocity;
                    lastRotatePositionQueue.Add(lastRotatePosition);
                    lastVelocityQueue.Add(lastVelocity);
                    GameController.bodyPieces[bodyPieceNum - 1].lastRotatePositionQueue.RemoveAt(0);
                    GameController.bodyPieces[bodyPieceNum - 1].lastVelocityQueue.RemoveAt(0);

                    //GameController.moveLock = false;
                }
            }
            
            //check if the body piece has any children, 
            //and if it has rotations queued up but is 1 unit past the queued rotation
            if (lastRotatePositionQueue.Count > 0 && GameController.bodyPieces.Count < bodyPieceNum + 2 &&
                ((transform.position.x <= lastRotatePositionQueue[0].x + 1 + GameController.speed * 2 &&
                transform.position.x >= lastRotatePositionQueue[0].x + 1 - GameController.marginOfError) || 
                (transform.position.x >= lastRotatePositionQueue[0].x - 1 - GameController.speed * 2 &&
                transform.position.x <= lastRotatePositionQueue[0].x - 1 + GameController.marginOfError) || 
                (transform.position.z <= lastRotatePositionQueue[0].z + 1 + GameController.speed * 2&&
                transform.position.z >= lastRotatePositionQueue[0].z + 1 - GameController.marginOfError) ||
                (transform.position.z >= lastRotatePositionQueue[0].z - 1 - GameController.speed * 2 &&
                transform.position.z <= lastRotatePositionQueue[0].z - 1 + GameController.marginOfError)))
            {
                
                //remove the first rotation in the body pieces queue
                lastRotatePositionQueue.RemoveAt(0);
                lastVelocityQueue.RemoveAt(0);
            }
        }

        //move the body every fixedupdate frame
        if (GameController.moveLock == false && snapped == false){
            transform.Translate(velocity);
        }
    }


    /// <summary>
    /// Helper method to align the body piece to the grid
    /// </summary>
    private void AlignToGrid()
    {
        //attempt to get it aligned better by rounding the position then setting it to the rounded version
        Vector3 temp = new Vector3(0, 0, 0);
        temp.x = Mathf.Round(transform.position.x);
        temp.z = Mathf.Round(transform.position.z);
        transform.position = temp;
    }
}
