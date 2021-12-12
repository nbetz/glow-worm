using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class NewTestScript
{
    // This tests the construction of the game controller
    // and all of its components. The test verifies that
    // there are no null prefabs / components
    // Tests: FR6, FR7, FR10, FR11, FR12, FR13
    [UnityTest]
    public IEnumerator TestGameControllerDeployment()
    {
        yield return new WaitForSeconds(0.1f);
        
        // Instantiate a new game controller and all of its dependent game objects
        GameObject gameControllerGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameController"));
        GameObject foodGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Food"));
        GameObject particleFoodGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/ParticleFoodPickup"));
        GameObject particleDeathGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/ParticleWormDeath"));
        
        GameController gameController = gameControllerGameObject.GetComponent<GameController>();
        
        // Set the prefabs of GameController
        gameController.prefabFood = foodGameObject;
        gameController.foodParticle = particleFoodGameObject;
        gameController.playerParticle = particleDeathGameObject;
        
        // Verify that it spawned correctly
        Assert.IsNotNull(gameController.prefabFood, "Missing prefab: food");
        Assert.IsNotNull(gameController.prefabBodyPiece, "Missing prefab: body piece");
        Assert.IsNotNull(gameController.prefabPlayer, "Missing prefab: player");
        Assert.IsNotNull(gameController.foodParticle, "Missing prefab: food particle");
        Assert.IsNotNull(gameController.playerParticle, "Missing prefab: player particle");


    }
    
    // Tests the configuration of the player controller
    // on deployment
    // Tests FR1, FR2, FR4
    [UnityTest]
    public IEnumerator TestPlayerControllerDeployment()
    {
        yield return new WaitForSeconds(0.1f);
        
        // Instantiate a new game controller and all of its dependent game objects
        GameObject gameControllerGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameController"));
        GameObject foodGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Food"));
        GameObject particleFoodGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/ParticleFoodPickup"));
        GameObject particleDeathGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/ParticleWormDeath"));
        
        GameController gameController = gameControllerGameObject.GetComponent<GameController>();

        GameObject playerControllerGameObject = gameController.prefabPlayer;

        PlayerController playerController = playerControllerGameObject.GetComponent<PlayerController>();
        
        // Set the prefabs of GameController
        gameController.prefabFood = foodGameObject;
        gameController.foodParticle = particleFoodGameObject;
        gameController.playerParticle = particleDeathGameObject;
        
        // Verify that the player is set up correctly
        Assert.IsNotNull(playerController, "Missing script: player");
    }
    
    // Tests the pickup of food and whether the snake
    // gets longer / score increases correctly
    // Tests: FR3, FR8
    [UnityTest]
    public IEnumerator TestFoodPickup()
    {
        // Instantiate a new game controller and all of its dependent game objects
        GameObject gameControllerGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameController"));
        //GameObject foodGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Food"));
        GameObject particleFoodGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/ParticleFoodPickup"));
        GameObject particleDeathGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/ParticleWormDeath"));
        
        GameController gameController = gameControllerGameObject.GetComponent<GameController>();

        GameObject playerControllerGameObject = gameController.prefabPlayer;

        PlayerController playerController = playerControllerGameObject.GetComponent<PlayerController>();
        
        // Set the prefabs of GameController
        gameController.prefabFood = Resources.Load<GameObject>("Prefabs/Food");
        gameController.foodParticle = particleFoodGameObject;
        gameController.playerParticle = particleDeathGameObject;
        
        // The player should move to the right by default
        // and spawns at 0,0,0
        
        // Let's spawn food at 5,0,0 and have the worm
        // run into it
        MonoBehaviour.Instantiate(Resources.Load("Prefabs/Food"), new Vector3(5.0f, 0.0f, 0.0f), Quaternion.identity);
        
        
        // Wait 2 seconds for the worm to run into it
        yield return new WaitForSeconds(2.0f);
        
        // The score should now be 10, since the worm ate a 
        // piece of food, verify
        Assert.True(GameController.score == 10, "Score not equal to 10");
        
    }

    // Test for the main menu play button 
    // Verifies that the scene loading is correct
    // Tests: FR14
    [UnityTest]
    public IEnumerator TestMainMenuPlay()
    {

        // Instantiate the main menu
        GameObject mainMenuGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainMenuCanvas"));
        UIMainMenu mainMenu = mainMenuGameObject.GetComponent<UIMainMenu>();

        // Pretend to click play
        mainMenu.ClickPlay();
        
        // Wait 2 seconds for loading
        yield return new WaitForSeconds(2.0f);
        
        // Verify that the new scene loaded, should have
        // loaded the scene named "Game" in the editor
        Assert.True(SceneManager.GetActiveScene().name == "Game", "Scene not equal to game");
    }
    
    // Test for the main menu settings button 
    // Verifies that the canvas loading is correct
    // Tests: FR15
    [UnityTest]
    public IEnumerator TestMainMenuSettings()
    {

        // Instantiate the main menu
        GameObject mainMenuGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainMenuCanvas"));
        UIMainMenu mainMenu = mainMenuGameObject.GetComponent<UIMainMenu>();
        
        // Instantiate the settings
        GameObject settingsGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/SettingsCanvas"));
        UISettings settings = mainMenuGameObject.GetComponent<UISettings>();
        
        // Set the prefabs so there is no null pointer exceptions
        mainMenu.settingsCanvas = settingsGameObject;
        
        // The settings canvas should be not active by default
        settingsGameObject.SetActive(false);
        
        // Verify that there are no null pointer exceptions
        Assert.IsNotNull(mainMenu.settingsCanvas);
        
        // Pretend to click settings
        mainMenu.ClickSettings();
        
        // Wait 0.5 seconds for loading
        yield return new WaitForSeconds(0.5f);
        
        // Verify that the settings UI is visible to the player
        Assert.True(settingsGameObject.active == true, "Settings canvas not visible to player");
    }
    
    // Test for the main menu quit button 
    // Verifies that the call for quit was called
    // Tests: FR16
    [UnityTest]
    public IEnumerator TestMainMenuQuit()
    {

        // Instantiate the main menu
        GameObject mainMenuGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainMenuCanvas"));
        UIMainMenu mainMenu = mainMenuGameObject.GetComponent<UIMainMenu>();

        // Pretend to click quit
        mainMenu.ClickQuit();
        
        // Wait 0.5 seconds for loading
        yield return new WaitForSeconds(0.5f);
        
        // Verify that the quit bool is set to true
        Assert.True(mainMenu.quit == true, "Quit function not called");
    }
    
    // Test for the main menu quit button 
    // Verifies that the call for quit was called
    // Tests: FR17
    [UnityTest]
    public IEnumerator TestMainMenuHighScore()
    {

        // Instantiate the main menu
        GameObject mainMenuGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainMenuCanvas"));
        UIMainMenu mainMenu = mainMenuGameObject.GetComponent<UIMainMenu>();

        // Wait 0.5 seconds for loading
        yield return new WaitForSeconds(0.5f);
        
        // Verify that the high scores are equal to the saved ones
        Assert.True(mainMenu.highScore == PlayerPrefs.GetInt("HighScore"), "High score not set correctly");
    }
    
}

