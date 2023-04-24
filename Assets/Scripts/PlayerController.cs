using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Vector3 velocity = new Vector3(GameController.speed, 0, 0);
    public static List<Vector3> lastRotatePositionQueue = new List<Vector3>();
    public static List<Vector3> lastVelocityQueue = new List<Vector3>();

    private static bool goLeft = false;
    private static bool goRight = false;
    private static bool goUp = false;
    private static bool goDown = false;
    private static bool waiting = false;
    private static int goLeftWait = 0;
    private static int goRightWait = 0;
    private static int goUpWait = 0;
    private static int goDownWait = 0;

    public GameObject foodPickupParticle;
    public AudioSource move;

    /// <summary>
    /// Unity trigger function that is ran anytime this object collides
    /// with another collider
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter(Collider collider)
    {
        GameController gc = FindObjectOfType<GameController>();

        //ends the game if the worm touches the border
        if (collider.gameObject.tag == "Border")
        {
            if (GameController.score > PlayerPrefs.GetInt("HighScore"))
            {
                gc.Win();
            }
            else
            {
                gc.Die();
            }
        }

        //adds a body piece and moves the food if the worm touches the food    
        if(collider.gameObject.tag == "Food")
        {
            GameController.addBodyPiece = true;
            GameController.respawnFood = true;
            GameController.updateSpeed = true;
            GameController.score += 10;

            // TODO update for when map size is increased
            //win condition
            if(GameController.score >= 3210 || GameController.bodyPieces.Count > 322)
                gc.Win();
        }
        if (collider.gameObject.tag != "Body")
        {
            Debug.Log(collider.gameObject.tag);
        }

        //ends the game if the worm touches a body piece in the direction its moving
        if (collider.gameObject.tag == "Body" || collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "EnemyBody")
        {
            float colliderPositionX = collider.transform.position.x;
            float colliderPositionZ = collider.transform.position.z;

            if (collider.gameObject.tag == "EnemyBody" || collider.gameObject.tag == "Enemy")
            {
                Debug.Log(colliderPositionX);
                Debug.Log(colliderPositionZ);
                Debug.Log(transform.position.x);
                Debug.Log(transform.position.z);
                Debug.Log(velocity.x);
                Debug.Log(velocity.z);
            }

            // TODO add condition for hitting enemy
            if (velocity.x > 0 &&  colliderPositionX > transform.position.x)
            {
                if(GameController.score > PlayerPrefs.GetInt("HighScore"))
                {
                    gc.Win();
                }
                else
                {
                    gc.Die();
                }
            }
            else if(velocity.x < 0 && colliderPositionX < transform.position.x)
            {
                if (GameController.score > PlayerPrefs.GetInt("HighScore"))
                {
                    gc.Win();
                }
                else
                {
                    gc.Die();
                }
            }
            else if(velocity.z > 0 && colliderPositionZ > transform.position.z)
            {
                if (GameController.score > PlayerPrefs.GetInt("HighScore"))
                {
                    gc.Win();
                }
                else
                {
                    gc.Die();
                }
            }
            else if(velocity.z < 0 && colliderPositionZ < transform.position.z)
            {
                if (GameController.score > PlayerPrefs.GetInt("HighScore"))
                {
                    gc.Win();
                }
                else
                {
                    gc.Die();
                }
            }
        }
    }

    /// <summary>
    /// Unity start function - ran on first frame
    /// </summary>
    void Start()
    {
        //set fixedUpdate time to 100FPS
        Time.fixedDeltaTime = 0.01f;
        Light light = GetComponentInChildren<Light>();
        light.color = GetComponentInChildren<Renderer>().material.GetColor("_EmissionColor");
    }

    /// <summary>
    /// Unity update function - ran every frame
    /// handles player input and queues up a rotation if an input was given
    /// </summary>
    void Update()
    {
        //checks to see if the player is trying to go right and if it is already moving on that axis
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && velocity.x == 0 
            && (goDown != true && goLeft != true && goRight != true && goUp != true && waiting != true))
        {
            //queue going right immediately unless were on the grid then queue going right in 2 frames
            move.Play ();
            if (!(Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                && !(Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
                    goRight = true;
                else
                {
                    goRightWait = 2;
                    waiting = true;
                }
        }

        //checks to see if the player is trying to go left and if it is already moving on that axis
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && velocity.x == 0
            && (goDown != true && goLeft != true && goRight != true && goUp != true && waiting != true))
        {
            //queue going left immediately unless were on the grid then queue going left in 2 frames
            move.Play ();
            if (!(Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                && !(Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
                    goLeft = true;
            else
            {
                goLeftWait = 2;
                waiting = true;
            }
        }

        //checks to see if the player is trying to go up and if it is already moving on that axis
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && velocity.z == 0
            && (goDown != true && goLeft != true && goRight != true && goUp != true && waiting != true))
        {
            //queue going up immediately unless were on the grid then queue going up in 2 frames
            move.Play ();
            if (!(Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                && !(Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
                    goUp = true;
            else 
            {
                goUpWait = 2;
                waiting = true;
            }
        }

        //checks to see if the player is trying to go down and if it is already moving on that axis
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && velocity.z == 0
            && (goDown != true && goLeft != true && goRight != true && goUp != true && waiting != true))
        {
            //queue going down immediately unless were on the grid then queue going down in 2 frames
            move.Play ();
            if (!(Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                && !(Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
                    goDown = true;
            else
            {
                goDownWait = 2;
                waiting = true;
            }
        }
    }

    /// <summary>
    /// moves the head in whatever direction it needs to move
    /// </summary>
    public void MovePlayer(){
        bool snapped = false;

        if(goUp == true)
        {
            //check if the head is at a grid position within MoE then turn upwards
            if ((Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                && (Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
            {

                //change the velocity so the direction changes
                velocity.x = 0;
                velocity.z = GameController.speed;

                //informs FixedUpdate() in gamecontroller that it needs to snap the body to the grid
                GameController.snapToGrid = true;
                snapped = true;
                    
                //add the velocity and rounded position to the lastRotatePositionQueue
                lastRotatePositionQueue.Add(AlignToGrid());
                lastVelocityQueue.Add(velocity);

                goUp = false;
            }
        }
        else if(goDown == true)
        {
            //check if the head is at a grid position within MoE then turn downwards
            if ((Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                && (Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
            {
                //change the velocity so the direction changes
                velocity.x = 0;
                velocity.z = -GameController.speed;

                //informs FixedUpdate() in gamecontroller that it needs to snap the body to the grid
                GameController.snapToGrid = true;
                snapped = true;

                //add the velocity and rounded position to the lastRotatePositionQueue
                lastRotatePositionQueue.Add(AlignToGrid());
                lastVelocityQueue.Add(velocity);

                goDown = false;
            }
        }
        else if(goLeft == true)
        {
            //check if the head is at a grid position within MoE then turn left
            if ((Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                && (Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
            {
                //change the velocity so the direction changes
                velocity.z = 0;
                velocity.x = -GameController.speed;

                //informs FixedUpdate() in gamecontroller that it needs to snap the body to the grid
                GameController.snapToGrid = true;
                snapped = true;

                //add the velocity and rounded position to the lastRotatePositionQueue
                lastRotatePositionQueue.Add(AlignToGrid());
                lastVelocityQueue.Add(velocity);

                goLeft = false;
            }
        }
        else if(goRight == true)
        {
            //check if the head is at a grid position within MoE then turn right
            if ((Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                && (Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
            {
                //change the velocity so the direction changes
                velocity.z = 0;
                velocity.x = GameController.speed;

                //informs FixedUpdate() in gamecontroller that it needs to snap the body to the grid
                GameController.snapToGrid = true;
                snapped = true;
                    
                //add the velocity and rounded position to the lastRotatePositionQueue
                lastRotatePositionQueue.Add(AlignToGrid());
                lastVelocityQueue.Add(velocity);

                goRight = false;
            }
        }

        //move the head
        if (snapped == false)
            transform.Translate(velocity);

        //skip the number of frames needed before moving
        //the player in the direction they want on the next frame 
        if(goUpWait == 2)
            goUpWait--;
        else if (goDownWait == 2)
            goDownWait--;
        else if (goLeftWait == 2)
            goLeftWait--;
        else if (goRightWait == 2)
            goRightWait--;
        else if (goUpWait == 1)
        {
            goUpWait--;
            goUp = true;
            waiting = false;
        }
        else if (goDownWait == 1)
        {
            goDownWait--;
            goDown = true;
            waiting = false;
        }
        else if (goLeftWait == 1)
        {
            goLeftWait--;
            goLeft = true;
            waiting = false;
        }
        else if (goRightWait == 1)
        {
            goRightWait--;
            goRight = true;
            waiting = false;
        }
    }
    
    /// <summary>
    /// Helper method to align the body piece to the grid
    /// </summary>
    /// <returns>Vector3 of the players position but rounded to the nearest integer</returns> 
    private Vector3 AlignToGrid()
    {
        //attempt to get it aligned better by rounding the position then setting it to the rounded version
        Vector3 temp = new Vector3(0, 0, 0);
        temp.x = Mathf.Round(transform.position.x);
        temp.z = Mathf.Round(transform.position.z);
        return temp;
    }
}