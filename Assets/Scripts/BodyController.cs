using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BodyController : MonoBehaviour
{
    public Vector3 velocity;
    public List<Vector3> lastRotatePositionQueue = new List<Vector3>();
    public List<Vector3> lastVelocityQueue = new List<Vector3>();
    public int bodyPieceNum;

    // Start is called before the first frame update
    void Start()
    {
        //set fixedUpdate time to 100FPS
        Time.fixedDeltaTime = 0.01f;

        //set the velocity
        velocity = PlayerController.velocity;
        transform.Translate(velocity);
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //fixedUpdate is called 100 FPS
    private void FixedUpdate()
    {
        //if its the first piece, check if it needs to rotate based upon the player itself
        if (bodyPieceNum == 0)
        {

            //check to see if the player has queued any rotations
            if (PlayerController.lastRotatePositionQueue.Count != 0) {
                Vector3 lastVelocity = PlayerController.lastVelocityQueue[0];
                Vector3 lastRotatePosition = PlayerController.lastRotatePositionQueue[0];

                //if ((transform.position.x <= lastRotatePosition.x + GameController.marginOfError &&
                //    transform.position.x >= lastRotatePosition.x - GameController.marginOfError / 2.0f) &&
                //    (transform.position.z <= lastRotatePosition.z + GameController.marginOfError &&
                //    transform.position.z >= lastRotatePosition.z - GameController.marginOfError / 2.0f))

                //if the body piece is within margin of error for the last rotate position of the player, change the direction
                if ((transform.position.x <= lastRotatePosition.x + GameController.speed && transform.position.x >= lastRotatePosition.x - GameController.marginOfError) &&
                    (transform.position.z <= lastRotatePosition.z + GameController.speed && transform.position.z >= lastRotatePosition.z - GameController.marginOfError))
                {
                    GameController.moveLock = true;
                    //attempt to get it aligned better by rounding the position then setting it to the rounded version
                    Vector3 temp = new Vector3(0, 0, 0);
                    temp.x = Mathf.Round(transform.position.x);
                    temp.z = Mathf.Round(transform.position.z);
                    transform.position = temp;

                    //set the velocity to the players then removes from player queues and adds to self queues
                    velocity = lastVelocity;
                    GameController.moveLock = false;

                    lastRotatePositionQueue.Add(lastRotatePosition);
                    lastVelocityQueue.Add(lastVelocity);
                    PlayerController.lastRotatePositionQueue.RemoveAt(0);
                    PlayerController.lastVelocityQueue.RemoveAt(0);
                }
            }

        }

        //if not the first piece, check if it needs to rotate based upon the piece infront of it
        else
        {
            //checks to see if there if the bodypiece infront of it has queued any rotations
            if (GameController.bodyPieces[bodyPieceNum - 1].lastRotatePositionQueue.Count != 0) {
                Vector3 lastVelocity = GameController.bodyPieces[bodyPieceNum - 1].lastVelocityQueue[0];
                Vector3 lastRotatePosition = GameController.bodyPieces[bodyPieceNum - 1].lastRotatePositionQueue[0];

                //if ((transform.position.x <= lastRotatePosition.x + GameController.marginOfError &&
                //    transform.position.x >= lastRotatePosition.x - GameController.marginOfError / 2.0f) &&
                //    (transform.position.z <= lastRotatePosition.z + GameController.marginOfError &&
                //    transform.position.z >= lastRotatePosition.z - GameController.marginOfError / 2.0f))

                //check if bodypiece is within margin of error from the grid position it should be at.
                if ((transform.position.x <= lastRotatePosition.x + GameController.speed && transform.position.x >= lastRotatePosition.x - GameController.marginOfError) &&
                    (transform.position.z <= lastRotatePosition.z + GameController.speed && transform.position.z >= lastRotatePosition.z - GameController.marginOfError))
                {
                    GameController.moveLock = true;

                    //attempt to get it aligned better by rounding the position then setting it to the rounded version
                    Vector3 temp = new Vector3(0, 0, 0);
                    temp.x = Mathf.Round(transform.position.x);
                    temp.z = Mathf.Round(transform.position.z);
                    transform.position = temp;

                    //set the velocity to the bodyPiece in front of it, then removes from that pieces queues and adds to its own
                    velocity = lastVelocity;
                    GameController.moveLock = false;

                    lastRotatePositionQueue.Add(lastRotatePosition);
                    lastVelocityQueue.Add(lastVelocity);
                    GameController.bodyPieces[bodyPieceNum - 1].lastRotatePositionQueue.RemoveAt(0);
                    GameController.bodyPieces[bodyPieceNum - 1].lastVelocityQueue.RemoveAt(0);
                }
            }
        }

        //move the body every fixedupdate frame
        if (GameController.moveLock == false) { 
            transform.Translate(velocity);
        }
    }
}
