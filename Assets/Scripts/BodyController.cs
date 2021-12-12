using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BodyController : MonoBehaviour
{
    //Instance variables
    public Vector3 velocity;
    public List<Vector3> lastRotatePositionQueue = new List<Vector3>();
    public List<Vector3> lastVelocityQueue = new List<Vector3>();
    public int bodyPieceNum;

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

        //remove the gap between pieces by clipping into the piece in front
        transform.Translate(velocity);  
    }

    /// <summary>
    /// Unity fixed update function - runs 100 frames per second
    /// </summary>
    private void FixedUpdate()
    {
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
                    GameController.moveLock = true;

                    //alligns the body piece to the grid
                    AlignToGrid();

                    //set the velocity to the players then removes from player queues and adds to self queues
                    velocity = lastVelocity;
                    lastRotatePositionQueue.Add(lastRotatePosition);
                    lastVelocityQueue.Add(lastVelocity);
                    PlayerController.lastRotatePositionQueue.RemoveAt(0);
                    PlayerController.lastVelocityQueue.RemoveAt(0);

                    GameController.moveLock = false;
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
                    GameController.moveLock = true;

                    //aligns the body piece to the grid
                    AlignToGrid();

                    //set the velocity to the bodyPiece in front of it, then removes from that pieces queues and adds to its own
                    velocity = lastVelocity;
                    lastRotatePositionQueue.Add(lastRotatePosition);
                    lastVelocityQueue.Add(lastVelocity);
                    GameController.bodyPieces[bodyPieceNum - 1].lastRotatePositionQueue.RemoveAt(0);
                    GameController.bodyPieces[bodyPieceNum - 1].lastVelocityQueue.RemoveAt(0);

                    GameController.moveLock = false;
                }
            }
            
            //check if the body piece has any children, 
            //and if it has rotations queued up but is 1 unit past the queued rotation
            if (lastRotatePositionQueue.Count > 0 && GameController.bodyPieces.Count < bodyPieceNum + 2 &&
                ((transform.position.x <= lastRotatePositionQueue[0].x + 1 + GameController.speed &&
                transform.position.x >= lastRotatePositionQueue[0].x + 1 - GameController.marginOfError) || 
                (transform.position.x >= lastRotatePositionQueue[0].x - 1 - GameController.speed &&
                transform.position.x <= lastRotatePositionQueue[0].x - 1 + GameController.marginOfError) || 
                (transform.position.z <= lastRotatePositionQueue[0].z + 1 + GameController.speed &&
                transform.position.z >= lastRotatePositionQueue[0].z + 1 - GameController.marginOfError) ||
                (transform.position.z >= lastRotatePositionQueue[0].z - 1 - GameController.speed &&
                transform.position.z <= lastRotatePositionQueue[0].z - 1 + GameController.marginOfError)))
            {
                
                //remove the first rotation in the body pieces queue
                lastRotatePositionQueue.RemoveAt(0);
                lastVelocityQueue.RemoveAt(0);
            }
        }

        //move the body every fixedupdate frame
        if (GameController.moveLock == false)
            transform.Translate(velocity);
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
