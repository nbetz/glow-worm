using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Instance variables
    public GameObject prefabPlayer;
    public GameObject prefabEnemy;
    public GameObject prefabEnemyBodyPiece;
    public GameObject prefabBodyPiece;
    public GameObject prefabFood;
    
    public static List<GameObject> bodyPieceObjects = new List<GameObject>();
    public static List<BodyController> bodyPieces = new List<BodyController>();
    public static List<EnemyBodyController> enemyBodyPieces = new List<EnemyBodyController>();
    public static List<GameObject> enemyBodyPieceObjects = new List<GameObject>();
    public static bool moveLock = false;
    public GameObject player;
    public GameObject food;
    public GameObject gameHUD;
    public GameObject deathScreen;
    public GameObject foodParticle;
    public GameObject playerParticle;
    public GameObject enemy;
    public AudioSource ate;
    public AudioSource death;
    public AudioSource backgroundAudio;
    public AudioSource gameoverAudio;
    

    public bool endScreenActive = false;
    public static float marginOfError = 0.0005f;
    public static float speed = 0.03f;
    public static int score = 0;

    public static bool addBodyPiece = false;
    public static bool addEnemyBodyPiece = false;
    public static bool updateSpeed = false;

    public static bool respawnFood = false;

    public static bool snapToGrid = false;

    public static int currentGlowFade = 0;
    public static int enemyCurrentGlowFade = 0;
    
    /// <summary>
    /// Unity start function - ran on first frame
    /// </summary>
    void Start()
    {
        //create the player at 0,0,0
        backgroundAudio.Play ();
        
        moveLock = true;
        player = Object.Instantiate(prefabPlayer, new Vector3(0f, 0f, 0f), Quaternion.identity);
        enemy = Object.Instantiate(prefabEnemy, new Vector3(5f, 0f, 5f), Quaternion.identity);

        //create the food at a random location
        food = Object.Instantiate(prefabFood, SpawnLocation(), Quaternion.identity);
        food.GetComponentInChildren<Light>().color = food.GetComponentInChildren<Renderer>().material.GetColor("_EmissionColor");
    }

    /// <summary>
    /// Unity update function - ran every frame
    /// </summary>
    void Update()
    {
        if(!endScreenActive){
            if(player == null){
            player = Object.Instantiate(prefabPlayer, new Vector3(0f, 0f, 0f), Quaternion.identity);
            food = Object.Instantiate(prefabFood, new Vector3(4f, 0f, 3f), Quaternion.identity);
            enemy = Object.Instantiate(prefabEnemy, new Vector3(5f, 0f, 5f), Quaternion.identity);
            }
            else{
            //when the worm has no body add 2 body pieces 1 frame apart
                if(bodyPieces.Count == 0)
                    AddBodyPiece();
                else if (bodyPieces.Count == 1){
                    AddBodyPiece();
                    moveLock = false;
                    StartCoroutine(GlowReset());
                }

                if(enemyBodyPieces.Count == 0 && enemy != null){
                    AddEnemyBodyPiece();
                }
                else if (enemyBodyPieces.Count == 1 && enemy != null)
                {
                    AddEnemyBodyPiece();
                    moveLock = false;
                    StartCoroutine(EnemyGlowReset());
                }
            }
        }
        
    }

    /// <summary>
    /// Unity FixedUpdate function - ran 100 frames per second
    /// </summary>
    void FixedUpdate() {
        if(!endScreenActive){

            //move the worm
            MoveWorm();

            //add a body piece if necessary
            if(addBodyPiece == true){
                AddBodyPiece();
                addBodyPiece = false;          
            }

            //snap the body to the grid if necessary
            if(snapToGrid == true){
                AlignAll();
                snapToGrid = false;
            }

            //respawn the food if necessary
            if(respawnFood == true){
                RespawnFood();
                respawnFood = false;
            }

            if(addEnemyBodyPiece == true && enemy != null)
            {
                AddEnemyBodyPiece();
                addEnemyBodyPiece = false;
            }

            //update the difficulty if necessary
            if(updateSpeed == true){
                speed += 0.0005f;
                updateSpeed = false;
            }
        }
    }

    /// <summary>
    /// Moves the entire worm if movelock is false
    /// </summary>
    private void MoveWorm()
    {
        if(moveLock == false){
            player.GetComponent<PlayerController>().MovePlayer();
            //FindObjectOfType<PlayerController>().MovePlayer();
            for(int i = 0; i < bodyPieces.Count; i++){
                bodyPieces[i].MoveBody();
            }
            if(enemy != null)
            {
                enemy.GetComponent<EnemyController>().MoveEnemy();
            }

            //FindObjectOfType<EnemyController>().MoveEnemy();
            for (int i = 0; i < enemyBodyPieces.Count; i++)
            {
                enemyBodyPieces[i].MoveBody();
            }
        }
    }

    /// <summary>
    /// Aligns the entire worm to the grid
    /// </summary>
    public void AlignAll(){
        Vector3 temp = new Vector3(0, 0, 0);
        temp.x = Mathf.Round(player.transform.position.x);
        temp.z = Mathf.Round(player.transform.position.z);
        player.transform.position = temp;

        for(int i = 0; i < bodyPieceObjects.Count; i++){
            Vector3 temp2 = new Vector3(0, 0, 0);
            temp2.x = Mathf.Round(bodyPieceObjects[i].transform.position.x);
            temp2.z = Mathf.Round(bodyPieceObjects[i].transform.position.z);
            bodyPieceObjects[i].transform.position = temp2;            
        }

        if(enemy != null)
        {
            Vector3 temp3 = new Vector3(0, 0, 0);
            temp3.x = Mathf.Round(enemy.transform.position.x);
            temp3.z = Mathf.Round(enemy.transform.position.z);
            enemy.transform.position = temp3;

            for (int i = 0; i < enemyBodyPieceObjects.Count; i++)
            {
                Vector3 temp4 = new Vector3(0, 0, 0);
                temp4.x = Mathf.Round(enemyBodyPieceObjects[i].transform.position.x);
                temp4.z = Mathf.Round(enemyBodyPieceObjects[i].transform.position.z);
                enemyBodyPieceObjects[i].transform.position = temp4;
            }
        }

    }

    /// <summary>
    /// Adds a body piece to the worm
    /// </summary>
    public void AddBodyPiece()
    {
        //checks if the worm has any body pieces
        if (bodyPieceObjects.Count == 0)
        {
            Vector3 position = player.transform.position;
            Vector3 velocity = PlayerController.velocity;
            AddBodyPieceHelper(position, velocity);
        }
        else
        {
            Vector3 position = bodyPieces[bodyPieces.Count - 1].transform.position;
            Vector3 velocity = bodyPieces[bodyPieces.Count - 1].velocity;
            AddBodyPieceHelper(position, velocity);
        }
    }

    /// <summary>
    /// helper function to create a new bodypiece and add it to the lists
    /// </summary>
    ///<param name="position">The position of the current tail</param>
    ///<param name="velocity">The velocity of the current tail</param>
    private void AddBodyPieceHelper(Vector3 position, Vector3 velocity)
    {
        
        //set the spawn location 1 unit behind the current tail
        if (velocity.x > 0)
            position.x = position.x - 1f;
        else if (velocity.x < 0)
            position.x = position.x + 1f;
        else if (velocity.z > 0)
            position.z = position.z - 1f;
        else if (velocity.z < 0)
            position.z = position.z + 1f;
        
        //create the object, store it in the bodypiece lists, then give it a number
        bodyPieceObjects.Add(Object.Instantiate(prefabBodyPiece, position, Quaternion.identity));
        int bodyPieceNum = bodyPieceObjects.Count - 1;
        bodyPieces.Add(bodyPieceObjects[bodyPieceNum].GetComponent<BodyController>());
        bodyPieces[bodyPieceNum].bodyPieceNum = bodyPieceNum;
    }

    /// <summary>
    /// function to move the food to a random new location on the board
    /// </summary>
    public void RespawnFood()
    {
        ate.Play ();
        Vector3 spawnLocation = SpawnLocation();
        food.transform.SetPositionAndRotation(spawnLocation, Quaternion.identity);
        Instantiate(foodParticle, spawnLocation, Quaternion.identity);
        
        // Whenever we respawn the food, start the glow fade

        StopAllCoroutines();
        StartCoroutine(GlowReset());
        if(enemy != null)
        {
            StartCoroutine(EnemyGlowReset());
        }
    }

    /// <summary>
    /// helper function to determine where the food should respawn
    /// </summary>
    /// <returns>Vector3 with random X and Y within bounds</returns> 
    private Vector3 SpawnLocation()
    {
        // TODO also check enemy body pieces location

        //suspend movement until the food has a new location
        moveLock = true;

        Vector3 location = new Vector3(Random.Range(-12, 12), 0, Random.Range(-6, 6));
        Vector3 headPosition = player.transform.position;
        Vector3 enemyHeadPosition = new Vector3(-50, -50, -50);
        if(enemy != null)
            enemyHeadPosition = enemy.transform.position;

        //check if the head of the worm is where the randomly generated location is
        if((location.x == Mathf.Floor(headPosition.x) || location.x == Mathf.Ceil(headPosition.x)) &&
        (location.z == Mathf.Floor(headPosition.z) || location.z == Mathf.Ceil(headPosition.z))){
            
            //retry if worm is at location
            return SpawnLocation();
        }

        if (enemy != null && ((location.x == Mathf.Floor(enemyHeadPosition.x) || location.x == Mathf.Ceil(enemyHeadPosition.x)) &&
                (location.z == Mathf.Floor(enemyHeadPosition.z) || location.z == Mathf.Ceil(enemyHeadPosition.z))))
        {

            //retry if worm is at location
            return SpawnLocation();
        }

        //check if he body of the worm is where the randomly generated location is
        for (int i = 0; i < bodyPieces.Count; i++){
            Vector3 temp = bodyPieces[i].transform.position;
            if((location.x == Mathf.Floor(temp.x) || location.x == Mathf.Ceil(temp.x)) &&
            (location.z == Mathf.Floor(temp.z) || location.z == Mathf.Ceil(temp.z))){
                
                //retry if worm is at location
                return SpawnLocation();
            }
        }

        for (int i = 0; i < enemyBodyPieces.Count; i++)
        {
            Vector3 temp = enemyBodyPieces[i].transform.position;
            if ((location.x == Mathf.Floor(temp.x) || location.x == Mathf.Ceil(temp.x)) &&
            (location.z == Mathf.Floor(temp.z) || location.z == Mathf.Ceil(temp.z)))
            {

                //retry if worm is at location
                return SpawnLocation();
            }
        }

        moveLock = false;
        return location;
    }

    /// <summary>
    /// coroutine to fade the glow from the body of the worm
    /// </summary>
    private IEnumerator GlowFade()
    {   
        for (int i = bodyPieces.Count - 1; i >= 0; i--)
        {
            currentGlowFade = i;
            StopCoroutine(GlowReset());
            // Wait for 1 second to stop glow
            yield return new WaitForSeconds(1.0f);

            // Take the next body piece in the list and start its glow fade
            bodyPieces[i].GlowFade();
        }

    }

    /// <summary>
    /// coroutine to reset the glow for the worm
    /// </summary>    
    private IEnumerator GlowReset()
    {
        StopCoroutine(GlowFade());
        for (int i = currentGlowFade; i <= bodyPieces.Count - 1; i++)
        {
            // Wait for .1 seconds to stop glow
            yield return new WaitForSeconds(0.1f);

            // Take the next body piece in the list and start its glow fade
            bodyPieces[i].GlowReset();
        }

        // Wait for 1 second, then start the glow fade
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(GlowFade());
    }

    /// <summary>
    /// Function to kill the player and end the game
    /// </summary>
    public void Die()
    {
        // Instantiate particle on player
        Instantiate(playerParticle, player.transform.position, Quaternion.identity);

        // Check for a new high score
        if (score > PlayerPrefs.GetInt("HighScore"))
            PlayerPrefs.SetInt("HighScore", score);

        backgroundAudio.Stop();
        // backgroundAudio.volume = 0;
        gameoverAudio.Play ();
        death.Play ();
        PlayerPrefs.SetInt("PrevScore", score);
        deathScreen.GetComponent<UIDeathScreen>().titleTextString = "GAME OVER!";
        deathScreen.GetComponent<UIDeathScreen>().backgroundColor = new Color (0.368f, 0f, 0f, 0f);
        deathScreen.GetComponent<UIDeathScreen>().Activate();
        
        // Hide the game HUD and show the death screen
        endScreenActive = true;
        gameHUD.SetActive(false);
        deathScreen.SetActive(endScreenActive);
        
        // Destroy / reset game objects
        Reset();
    }

    /// <summary>
    /// Function to end the game and tell the player they've won
    /// </summary>
    public void Win()
    {
        // Check for a new high score
        if (score > PlayerPrefs.GetInt("HighScore"))
            PlayerPrefs.SetInt("HighScore", score);

        backgroundAudio.Stop();

        gameoverAudio.Play ();
        death.Play ();
        PlayerPrefs.SetInt("PrevScore", score);
        deathScreen.GetComponent<UIDeathScreen>().titleTextString = "YOU WIN!";
        deathScreen.GetComponent<UIDeathScreen>().backgroundColor = new Color (0.368f, 0.368f, 0.368f, 0f);
        deathScreen.GetComponent<UIDeathScreen>().Activate();

        // Hide the game HUD and show the death screen
        endScreenActive = true;
        gameHUD.SetActive(false);
        deathScreen.SetActive(endScreenActive);
        
        // Destroy / reset game objects
        Reset();
    }

    /// <summary>
    /// Function to reset all the variables in GameController
    /// </summary>
    public void Reset()
    {        
        // Destroy / reset game objects
        StopAllCoroutines();
        foreach(GameObject b in bodyPieceObjects) 
            Destroy(b);
        bodyPieceObjects.Clear();
        bodyPieces.Clear();

        foreach (GameObject b in enemyBodyPieceObjects)
            Destroy(b);
        enemyBodyPieceObjects.Clear();
        enemyBodyPieces.Clear();


        speed = 0.03f;

        PlayerController.lastRotatePositionQueue.Clear();
        PlayerController.lastVelocityQueue.Clear();
        PlayerController.velocity = new Vector3(speed, 0, 0);
        EnemyController.lastRotatePositionQueue.Clear();
        EnemyController.lastVelocityQueue.Clear();
        EnemyController.velocity = new Vector3(speed, 0, 0);
        Destroy(player);
        Destroy(food);
        Destroy(enemy);
        player = null;
        food = null;
        enemy = null;
        score = 0;
        moveLock = false;
        addBodyPiece = false;
        addEnemyBodyPiece = false;
        updateSpeed = false;
        respawnFood = false;
        snapToGrid = false;
        currentGlowFade = 0;
        enemyCurrentGlowFade = 0;
    }

    // TODO
    public void EnemyDie()
    {
        StopCoroutine(EnemyGlowReset());
        StopCoroutine(EnemyGlowFade());
        // Instantiate particle on player
        if (enemy != null)
        {
            Instantiate(foodParticle, enemy.transform.position, Quaternion.identity);
        }

        // Destroy / reset game objects
        foreach (GameObject b in enemyBodyPieceObjects)
            Destroy(b);
        enemyBodyPieceObjects.Clear();
        enemyBodyPieces.Clear();
        Destroy(enemy);
        enemy = null;

        backgroundAudio.Stop();

        addEnemyBodyPiece = false;
        enemyCurrentGlowFade = 0;

    }

    /// <summary>
    /// coroutine to fade the glow from the body of the worm
    /// </summary>
    private IEnumerator EnemyGlowFade()
    {
        for (int i = enemyBodyPieces.Count - 1; i >= 0; i--)
        {
            enemyCurrentGlowFade = i;
            StopCoroutine(EnemyGlowReset());
            // Wait for 1 second to stop glow
            yield return new WaitForSeconds(1.0f);

            // Take the next body piece in the list and start its glow fade
            enemyBodyPieces[i].GlowFade();
        }

    }

    /// <summary>
    /// coroutine to reset the glow for the worm
    /// </summary>    
    private IEnumerator EnemyGlowReset()
    {
        StopCoroutine(EnemyGlowFade());
        for (int i = enemyCurrentGlowFade; i <= enemyBodyPieces.Count - 1; i++)
        {
            // Wait for .1 seconds to stop glow
            yield return new WaitForSeconds(0.1f);

            // Take the next body piece in the list and start its glow fade
            enemyBodyPieces[i].GlowReset();
        }

        // Wait for 1 second, then start the glow fade
        yield return new WaitForSeconds(1.0f);
        if(enemy != null)
        {
            StartCoroutine(EnemyGlowFade());
        }
    }

    /// <summary>
    /// Adds a body piece to the worm
    /// </summary>
    public void AddEnemyBodyPiece()
    {
        //checks if the worm has any body pieces
        if (enemyBodyPieceObjects.Count == 0)
        {
            Vector3 position = enemy.transform.position;
            Vector3 velocity = EnemyController.velocity;
            AddEnemyBodyPieceHelper(position, velocity);
        }
        else
        {
            Vector3 position = enemyBodyPieces[enemyBodyPieces.Count - 1].transform.position;
            Vector3 velocity = enemyBodyPieces[enemyBodyPieces.Count - 1].velocity;
            AddEnemyBodyPieceHelper(position, velocity);
        }
    }

    /// <summary>
    /// helper function to create a new bodypiece and add it to the lists
    /// </summary>
    ///<param name="position">The position of the current tail</param>
    ///<param name="velocity">The velocity of the current tail</param>
    private void AddEnemyBodyPieceHelper(Vector3 position, Vector3 velocity)
    {

        //set the spawn location 1 unit behind the current tail
        if (velocity.x > 0)
            position.x = position.x - 1f;
        else if (velocity.x < 0)
            position.x = position.x + 1f;
        else if (velocity.z > 0)
            position.z = position.z - 1f;
        else if (velocity.z < 0)
            position.z = position.z + 1f;

        //create the object, store it in the bodypiece lists, then give it a number
        enemyBodyPieceObjects.Add(Object.Instantiate(prefabEnemyBodyPiece, position, Quaternion.identity));
        int bodyPieceNum = enemyBodyPieceObjects.Count - 1;
        enemyBodyPieces.Add(enemyBodyPieceObjects[bodyPieceNum].GetComponent<EnemyBodyController>());
        enemyBodyPieces[bodyPieceNum].bodyPieceNum = bodyPieceNum;
    }
}
