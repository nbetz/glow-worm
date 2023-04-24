using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
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


    void OnTriggerEnter(Collider collider)
    {
        //ends the game if the worm touches the border
        if (collider.gameObject.tag == "Border")
            FindObjectOfType<GameController>().EnemyDie();

        //adds a body piece and moves the food if the worm touches the food    
        if (collider.gameObject.tag == "Food")
        {
            GameController.score -= 5;
            GameController.addEnemyBodyPiece = true;
            GameController.respawnFood = true;
        }

        // TODO update to include players and their body in this
        //ends the game if the worm touches a body piece in the direction its moving
        if (collider.gameObject.tag == "Body" || collider.gameObject.tag == "Player" || collider.gameObject.tag == "EnemyBody")
        {
            float colliderPositionX = collider.transform.position.x;
            float colliderPositionZ = collider.transform.position.z;

            if (velocity.x > 0 && colliderPositionX > transform.position.x)
                FindObjectOfType<GameController>().EnemyDie();
            else if (velocity.x < 0 && colliderPositionX < transform.position.x)
                FindObjectOfType<GameController>().EnemyDie();
            else if (velocity.z > 0 && colliderPositionZ > transform.position.z)
                FindObjectOfType<GameController>().EnemyDie();
            else if (velocity.z < 0 && colliderPositionZ < transform.position.z)
                FindObjectOfType<GameController>().EnemyDie();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //set fixedUpdate time to 100FPS
        Time.fixedDeltaTime = 0.01f;
        Light light = GetComponentInChildren<Light>();
        light.color = GetComponentInChildren<Renderer>().material.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        AStar();
    }


    // TODO
    private void AStar()
    {
        // create a map of the board where the food position is 1, any empty space is 0, and any space with a body piece (enemy or player) is -1
        GameController gc = FindObjectOfType<GameController>();

        Vector3 temp = new Vector3(0, 0, 0);
        temp.x = Mathf.Round(gc.player.transform.position.x);
        temp.z = Mathf.Round(gc.player.transform.position.z);

        Vector3 temp2 = new Vector3(0, 0, 0);
        temp2.x = Mathf.Round(gc.food.transform.position.x);
        temp2.z = Mathf.Round(gc.food.transform.position.z);

        Vector3 temp4 = new Vector3(0, 0, 0);
        temp4.x = Mathf.Round(transform.position.x);
        temp4.z = Mathf.Round(transform.position.z);

        Vector3 currentPos = temp4;
        Vector3 playerPos = temp;
        Vector3 foodPos = temp2;
        List<Vector3> bodyPosList = new List<Vector3>();
        List<Vector3> enemyBodyPosList = new List<Vector3>();

        List<Vector3> takenPositions = new List<Vector3>();
        


        for (int i = 0; i < GameController.bodyPieces.Count; i++)
        {
            Vector3 temp3 = new Vector3(0, 0, 0);
            temp3.x = Mathf.Round(GameController.bodyPieces[i].transform.position.x);
            temp3.z = Mathf.Round(GameController.bodyPieces[i].transform.position.z);
            bodyPosList.Add(temp3);
        }

        for (int i = 0; i < GameController.enemyBodyPieces.Count; i++)
        {
            Vector3 temp3 = new Vector3(0, 0, 0);
            temp3.x = Mathf.Round(GameController.enemyBodyPieces[i].transform.position.x);
            temp3.z = Mathf.Round(GameController.enemyBodyPieces[i].transform.position.z);
            enemyBodyPosList.Add(temp3);
        }

        foreach (Vector3 vec in bodyPosList)
        {
            takenPositions.Add(vec);
        }
        foreach (Vector3 vec in enemyBodyPosList)
        {
            takenPositions.Add(vec);
        }
        takenPositions.Add(playerPos);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node startingNode = new Node(currentPos);
        Node endingNode = new Node(foodPos);

        openList.Add(startingNode);

        List<Vector3> finalPath = new List<Vector3>();

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];

            int currentIndex = 0;
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].f < currentNode.f)
                {
                    currentNode = openList[i];
                    currentIndex = i;
                }
            }
            //Debug.Log("currentNode " + currentNode.pos.ToString());
            openList.RemoveAt(currentIndex);
            closedList.Add(currentNode);
            //Debug.Log("len open " + openList.Count);
            if (currentNode.Equals(endingNode))
            {
                List<Vector3> path = new List<Vector3>();
                Node tempNode = currentNode;
                while (tempNode != null)
                {
                    path.Add(tempNode.pos);
                    tempNode = tempNode.parent;
                }
               // Debug.Log("path " + path.Count);
                path.Reverse();
                finalPath = path;
                break;
            }

            List<Vector3> directionalVectors = new List<Vector3>();
            directionalVectors.Add(new Vector3(1f, 0f, 0f));
            directionalVectors.Add(new Vector3(0f, 0f, 1f));
            directionalVectors.Add(new Vector3(-1f, 0f, 0f));
            directionalVectors.Add(new Vector3(0f, 0f, -1f));

            List<Node> children = new List<Node>();
            foreach (Vector3 direction in directionalVectors)
            {
                Vector3 nodePosition = currentNode.pos + direction;

                if (!IsValidLocation(nodePosition))
                {
                    //Debug.Log("invalid location " + nodePosition.ToString());
                    continue;
                }

                if (takenPositions.Contains(nodePosition))
                {
                    //Debug.Log("in taken list " + nodePosition.ToString());
                    continue;
                }

                children.Add(new Node(currentNode, nodePosition));
                //Debug.Log("len children " + children.Count);

            }

            foreach (Node child in children)
            {
                if (!closedList.Contains(child))
                {
                    child.g = currentNode.g + 1;
                    child.h = Mathf.Pow(child.pos.x - endingNode.pos.x, 2f) + Mathf.Pow(child.pos.z - endingNode.pos.z, 2f);
                    child.f = child.g + child.h;

                    bool openContainChild = false;
                    foreach (Node open in openList)
                    {
                        if ((child.Equals(open) && child.g < open.g))
                        {
                            openContainChild = true;
                        }
                    }
                    //Debug.Log("openContainsChild" + openContainChild.ToString());
                    if (!openContainChild)
                    {
                        openList.Add(child);
                    }
                }
            }

        }
        
        if(finalPath.Count >= 1)
        {
            // get distance heuristic to the food
            if (finalPath[1].x > currentPos.x && velocity.x == 0)
            {
                goRight = true;
            }
            else if (finalPath[1].x < currentPos.x && velocity.x == 0)
            {
                goLeft = true;          
            }
            else if (finalPath[1].z > currentPos.z && velocity.z == 0)
            {
                goUp = true;              
            }
            else if (finalPath[1].z < currentPos.z && velocity.z == 0)
            {
                goDown = true;
            }
        }  
    }

    private bool IsValidLocation(Vector3 loc)
    {
        if (loc.x > 12 || loc.x < -12 || loc.z > 6 || loc.z < -6)
        {
            return false;
        }
        return true;
    }


    public void MoveEnemy()
    {
        bool snapped = false;

        if (goUp == true)
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
        else if (goDown == true)
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
        else if (goLeft == true)
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
        else if (goRight == true)
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
        if (goUpWait == 2)
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
