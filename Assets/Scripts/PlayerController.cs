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

    void Start()
    {
        //set fixedUpdate time to 100FPS
        Time.fixedDeltaTime = 0.01f;
    }

    void Update()
    {
        //run the coroutine for the direction we want to go if it wasn't already moving on that axis
        //(ie, wont do anything if trying to "turn" forward or backward)
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && velocity.x == 0 
            && (goDown != true && goLeft != true && goRight != true && goUp != true && waiting != true))
        {
            
            if (lastRotatePositionQueue.Count != 0) {
                if (!(Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                    && (Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed)) {
                    goRight = true;
                }
                else
                {
                    goRightWait = 2;
                    waiting = true;
                }
            }
            else
            {
                goRight = true;
            }
            //StartCoroutine(GoRight());
        }
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && velocity.x == 0
            && (goDown != true && goLeft != true && goRight != true && goUp != true && waiting != true))
        {
            if (lastRotatePositionQueue.Count != 0)
            {
                if (!(Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                    && (Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
                {
                    goLeft = true;
                }
                else
                {
                    goLeftWait = 2;
                    waiting = true;
                }

            }
            else
            {
                goLeft = true;
            }
            //StartCoroutine(GoLeft());
        }
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && velocity.z == 0
            && (goDown != true && goLeft != true && goRight != true && goUp != true && waiting != true))
        {
            if (lastRotatePositionQueue.Count != 0)
            {
                if (!(Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                    && (Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
                {
                    goUp = true;
                }
                else 
                {
                    goUpWait = 2;
                    waiting = true;
                }
            }
            else
            {
                goUp = true;
            }
            //StartCoroutine(GoUp());
        }
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && velocity.z == 0
            && (goDown != true && goLeft != true && goRight != true && goUp != true && waiting != true))
        {
            if (lastRotatePositionQueue.Count != 0)
            {
                if (!(Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                    && (Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
                {
                    goDown = true;
                }
                else
                {
                    goDownWait = 2;
                    waiting = true;
                }
            }
            else
            {
                goDown = true;
            }
            //StartCoroutine(GoDown());
        }
    }
    

    private void FixedUpdate()
    {
        if (GameController.moveLock == false)
        {
            if(goUp == true)
            {
                
                if ((Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                    && (Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
                {
                    GameController.moveLock = true;

                    //change the velocity so the direction changes
                    velocity.x = 0;
                    velocity.z = GameController.speed;

                    //try to round the position to be exactly a whole number then save it in lastRotatePosition
                    Vector3 temp = new Vector3(0, 0, 0);
                    temp.x = Mathf.Round(transform.position.x);
                    temp.z = Mathf.Round(transform.position.z);
                    transform.position = temp;
                    GameController.moveLock = false;
                    lastRotatePositionQueue.Add(temp);
                    lastVelocityQueue.Add(velocity);
                    goUp = false;
                }
            }
            else if(goDown == true)
            {
                if ((Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                    && (Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
                {
                    GameController.moveLock = true;

                    //change the velocity so the direction changes
                    velocity.x = 0;
                    velocity.z = -GameController.speed;

                    //try to round the position to be exactly a whole number then save it in lastRotatePosition
                    Vector3 temp = new Vector3(0, 0, 0);
                    temp.x = Mathf.Round(transform.position.x);
                    temp.z = Mathf.Round(transform.position.z);
                    transform.position = temp;
                    GameController.moveLock = false;

                    lastRotatePositionQueue.Add(temp);
                    lastVelocityQueue.Add(velocity);
                    goDown = false;
                }
            }
            else if(goLeft == true)
            {
                if ((Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                    && (Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
                {
                    GameController.moveLock = true;
                    //change the velocity so the direction changes
                    velocity.z = 0;
                    velocity.x = -GameController.speed;

                    //try to round the position to be exactly a whole number then save it in lastRotatePosition
                    Vector3 temp = new Vector3(0, 0, 0);
                    temp.x = Mathf.Round(transform.position.x);
                    temp.z = Mathf.Round(transform.position.z);
                    transform.position = temp;
                    GameController.moveLock = false;
                    lastRotatePositionQueue.Add(temp);
                    lastVelocityQueue.Add(velocity);
                    goLeft = false;
                }
            }
            else if(goRight == true)
            {
                if ((Mathf.Abs(transform.position.x) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.x) % 1.0f >= -GameController.speed)
                    && (Mathf.Abs(transform.position.z) % 1.0f <= GameController.speed && Mathf.Abs(transform.position.z) % 1.0f >= -GameController.speed))
                {
                    GameController.moveLock = true;
                    //change the velocity so the direction changes
                    velocity.z = 0;
                    velocity.x = GameController.speed;

                    //try to round the position to be exactly a whole number then save it in lastRotatePosition
                    Vector3 temp = new Vector3(0, 0, 0);
                    temp.x = Mathf.Round(transform.position.x);
                    temp.z = Mathf.Round(transform.position.z);
                    transform.position = temp;
                    GameController.moveLock = false;
                    lastRotatePositionQueue.Add(temp);
                    lastVelocityQueue.Add(velocity);
                    goRight = false;
                }
            }

            transform.Translate(velocity);

            if(goUpWait == 2)
            {
                goUpWait--;
            }
            else if (goUpWait == 1)
            {
                goUpWait--;
                goUp = true;
                waiting = false;
            }
            else if (goDownWait == 2)
            {
                goDownWait--;
            }
            else if (goDownWait == 1)
            {
                goDownWait--;
                goDown = true;
                waiting = false;
            }
            else if (goLeftWait == 2)
            {
                goLeftWait--;
            }
            else if (goLeftWait == 1)
            {
                goLeftWait--;
                goLeft = true;
                waiting = false;
            }
            else if (goRightWait == 2)
            {
                goRightWait--;
            }
            else if (goRightWait == 1)
            {
                goRightWait--;
                goRight = true;
                waiting = false;
            }
        }
    }


    //deprecated but too scared to delete incase we need to revert
    IEnumerator GoLeft()
    {
        //wait until the position is pretty much at a whole number
        yield return new WaitUntil(() => (transform.position.x % 1.0f <= GameController.speed && transform.position.x % 1.0f >= -GameController.speed)
        && (transform.position.z % 1.0f <= GameController.speed && transform.position.z % 1.0f >= -GameController.speed));
        GameController.moveLock = true;
        //change the velocity so the direction changes
        velocity.z = 0;
        velocity.x = -GameController.speed;

        //try to round the position to be exactly a whole number then save it in lastRotatePosition
        Vector3 temp = new Vector3(0, 0, 0);
        temp.x = Mathf.Round(transform.position.x);
        temp.z = Mathf.Round(transform.position.z);
        transform.position = temp;
        GameController.moveLock = false;
        lastRotatePositionQueue.Add(temp);
        lastVelocityQueue.Add(velocity);
    }
    IEnumerator GoRight()
    {
        //wait until the position is pretty much at a whole number
        yield return new WaitUntil(() => (transform.position.x % 1.0f <= GameController.speed && transform.position.x % 1.0f >= -GameController.speed)
        && (transform.position.z % 1.0f <= GameController.speed && transform.position.z % 1.0f >= -GameController.speed));
        GameController.moveLock = true;
        //change the velocity so the direction changes
        velocity.z = 0;
        velocity.x = GameController.speed;

        //try to round the position to be exactly a whole number then save it in lastRotatePosition
        Vector3 temp = new Vector3(0, 0, 0);
        temp.x = Mathf.Round(transform.position.x);
        temp.z = Mathf.Round(transform.position.z);
        transform.position = temp;
        GameController.moveLock = false;
        lastRotatePositionQueue.Add(temp);
        lastVelocityQueue.Add(velocity);

    }
    IEnumerator GoDown()
    {
        //wait until the position is pretty much at a whole number
        yield return new WaitUntil(() => (transform.position.x % 1.0f <= GameController.speed && transform.position.x % 1.0f >= -GameController.speed)
        && (transform.position.z % 1.0f <= GameController.speed && transform.position.z % 1.0f >= -GameController.speed));
        GameController.moveLock = true;

        //change the velocity so the direction changes
        velocity.x = 0;
        velocity.z = -GameController.speed;

        //try to round the position to be exactly a whole number then save it in lastRotatePosition
        Vector3 temp = new Vector3(0, 0, 0);
        temp.x = Mathf.Round(transform.position.x);
        temp.z = Mathf.Round(transform.position.z);
        transform.position = temp;
        GameController.moveLock = false;

        lastRotatePositionQueue.Add(temp);
        lastVelocityQueue.Add(velocity);
    }
    IEnumerator GoUp()
    {
        //wait until the position is pretty much at a whole number
        yield return new WaitUntil(() => (transform.position.x % 1.0f <= GameController.speed && transform.position.x % 1.0f >= -GameController.speed)
        && (transform.position.z % 1.0f <= GameController.speed && transform.position.z % 1.0f >= -GameController.speed));
        GameController.moveLock = true;

        //change the velocity so the direction changes
        velocity.x = 0;
        velocity.z = GameController.speed;

        //try to round the position to be exactly a whole number then save it in lastRotatePosition
        Vector3 temp = new Vector3(0, 0, 0);
        temp.x = Mathf.Round(transform.position.x);
        temp.z = Mathf.Round(transform.position.z);
        transform.position = temp;
        GameController.moveLock = false;

        lastRotatePositionQueue.Add(temp);
        lastVelocityQueue.Add(velocity);
    }
}
