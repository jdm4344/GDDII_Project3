using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour {

    // ==== VARIABLES ====
    // ~ Refs
    public GameObject gameObjects;
    public GameObject pc1;
    public GameObject pc2;
    public GameObject pc3;
    public GameObject pc4;

    // ~ Game
    private int curScreen = 0;
    private bool gameStart = false;
    private bool singlePress = false;
    private float minTime = 0;
    public bool testMode = true;
    public bool paused;
    public int currentPlayers = 4;
    public float spawnNormTimer = 0;
    public float boostSpawnTimer = 0;
    public float spawnBomberTimer = 0;
    public float spawnSwerverTimer = 0;
    public string winner;

    // ~ Prefabs
    public List<GameObject> infoScreens;
    public GameObject Vehicles;
    public GameObject boost;

    // ~ Players
    public float defaultMaxVel = 0.07f;
    public float defaultMaxAcc = 0.07f;
    public float invincibilityTime = 1.5f;
    public float damageFlashTime = 0.05f;

    // ~ Power Ups
    public float powerUpDuration = 6;
    // ~~ Speed Boost
    public float maxVelBoost = 0.1f;
    public float maxAccBoost = 0.08f;


    // ===== METHODS =====
    // Initialization
    void Start ()
    {
        currentPlayers = 4;
        curScreen = 0;
        minTime = 0;
    }

    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {
        if (gameStart)
        {
            spawnNormTimer += Time.deltaTime;
            boostSpawnTimer += Time.deltaTime;
            spawnBomberTimer += Time.deltaTime;
            spawnSwerverTimer += Time.deltaTime;
        }
        else 
        {
            minTime += Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Debugging
        if (testMode)
        {
            Debug.Log("Players - " + currentPlayers);

            if (Input.GetKey("r")) 
                SceneManager.LoadScene("MinigameAsteroids");
        }

        if (!gameStart && (Input.GetButton("1A") || Input.GetButton("2A") || Input.GetButton("3A") || Input.GetButton("4A")) && !singlePress)
        {
            singlePress = true;
            minTime = 0;
            NextScreen();
        }
        if (minTime > 0.8f)
        {
            singlePress = false;
        }

        // Display
        if (gameStart)
        {
            infoScreens[curScreen].SetActive(false);
            gameObjects.SetActive(true);

            try {
                pc1.GetComponent<Text>().text = "Health: " + GameObject.Find("P1").GetComponent<PlayerCollision>().Lives + "    Boosts:" + GameObject.Find("P1").GetComponent<CarController>().boosts + "";
            } 
            catch (System.NullReferenceException e) { 
                pc1.GetComponent<Text>().text = "WASTED";
                Debug.Log("Player1 Reference Gone: " + e);
            }
            
            try {
                pc2.GetComponent<Text>().text = "Health: " + GameObject.Find("P2").GetComponent<PlayerCollision>().Lives + "    Boosts:" + GameObject.Find("P2").GetComponent<CarController>().boosts + "";
            } 
            catch (System.NullReferenceException e) { 
                pc2.GetComponent<Text>().text = "WASTED";
                Debug.Log("Player2 Reference Gone: " + e); 
            }

            try {
                pc3.GetComponent<Text>().text = "Health: " + GameObject.Find("P3").GetComponent<PlayerCollision>().Lives + "    Boosts:" + GameObject.Find("P3").GetComponent<CarController>().boosts + "";
            } 
            catch (System.NullReferenceException e) { 
                pc3.GetComponent<Text>().text = "WASTED";
                Debug.Log("Player3 Reference Gone: " + e);
            }

            try {
                pc4.GetComponent<Text>().text = "Health: " + GameObject.Find("P4").GetComponent<PlayerCollision>().Lives + "    Boosts:" + GameObject.Find("P4").GetComponent<CarController>().boosts + "";
            } 
            catch (System.NullReferenceException e) { 
                pc4.GetComponent<Text>().text = "WASTED";
                Debug.Log("Player4 Reference Gone: " + e);
            }
        }
        
        // Win Screen
        if (currentPlayers == 1)
        {
            infoScreens[5].SetActive(true);
            infoScreens[6].GetComponent<Text>().text = "P" + GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>().player + " Wins";
            Time.timeScale = 0;

            if (Input.GetKeyDown("m")) 
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        // Spawning
        if (currentPlayers != 1 && gameStart)
        {
            // Spawn normal vehicles
            if (spawnNormTimer > 5) 
            {
                GameObject obj = Instantiate(Vehicles, new Vector3(10, Random.Range(-2.0f, 5)), Quaternion.Euler(0, 0, 180));
                obj.GetComponent<VehicleScript>().movePattern = MovePattern.Straight;
                spawnNormTimer = 0;
            }
            if (spawnSwerverTimer > 8) 
            {
                GameObject obj = Instantiate(Vehicles, new Vector3(-10, 2), Quaternion.Euler(0, 0, 45));
                obj.GetComponent<VehicleScript>().movePattern = MovePattern.Swerve;
                spawnSwerverTimer = 0;
            }
            if (spawnBomberTimer > 10) 
            {
                GameObject obj = Instantiate(Vehicles, new Vector3(Random.Range(-3.0f, 5), 7), Quaternion.Euler(0, 0, -90));
                obj.GetComponent<VehicleScript>().movePattern = MovePattern.BombDropper;
                spawnBomberTimer = 0;
            }
            // Spawn boosts
            if (boostSpawnTimer > 3)
            {
                GameObject obj = Instantiate(boost, new Vector3(Random.Range(-3.0f, 3), Random.Range(-2.0f, 4)), Quaternion.Euler(0, 0, 0));
                boostSpawnTimer = 0;
            }
        }
	}

    // Control Lives
    public void SubtractPlayer()
    {
        currentPlayers--;
    }

    
    // =================================
	// ======= BUTTON FUNCTIONS ========
	// =================================

    public void NextScreen()
    {   
        if (curScreen < 4)
        {
            curScreen++;
            infoScreens[curScreen].SetActive(true);
            infoScreens[curScreen - 1].SetActive(false);
        }
        else
            gameStart = true;
    }
    public void PrevScreen()
    {
        if (curScreen > 0)
        {
            curScreen--;
            infoScreens[curScreen].SetActive(true);
            infoScreens[curScreen + 1].SetActive(false);
            
        }
    }
}
