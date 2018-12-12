using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour {

    // ==== VARIABLES ====
    // ~ Refs
    private PlayerManager pManager;
    [Header("MiniGame Objects References")]
    public GameObject gameObjects;

    [Header("Player Cards")]
    public GameObject pc1;
    public GameObject pc2;
    public GameObject pc3;
    public GameObject pc4;

    // ~ Game
    private List<string> placement;
    private int curScreen = 0;
    private bool gameStart = false;
    private bool singlePress = false;
    private bool fameAssigned;
    private float minTime = 0;

    [Header("Debug Mode")]
    public bool testMode = false;

    [Header("Game Control")]
    public List<GameObject> infoScreens;
    public bool paused;
    public int totalPlayers;
    private int playersLeft;

    [Header("Stat Changes")]
    public float spawnNormTimer = 0;
    public float boostSpawnTimer = 0;
    public float spawnBomberTimer = 0;
    public float spawnSwerverTimer = 0;
    public string winner;

    // ~ Players
    public float defaultMaxVel = 0.07f;
    public float defaultMaxAcc = 0.07f;
    public float invincibilityTime = 1.5f;
    public float damageFlashTime = 0.05f;

    // ~ Power Ups
    public float speedBoostDuration = 1.0f;
    // ~~ Speed Boost
    public float maxVelBoost = 0.1f;
    public float maxAccBoost = 0.08f;

    // ~ Prefabs
    [Header("Prefabs")]
    public GameObject Vehicles;
    public GameObject boost;
    public Sprite smokeParticle;
    public Sprite nitrousParticle;


    // ===== METHODS =====
    // Initialization
    void Start ()
    {
        pManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        totalPlayers = pManager.playerNames.Length;
        playersLeft = totalPlayers;
        fameAssigned = false;
        curScreen = 0;
        minTime = 0;
    }

    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate ()
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
    void Update ()
    {
        // ==== Debugging ====================================================

        if (testMode)
        {
            Debug.Log("Players - " + totalPlayers);

            if (Input.GetKey("r")) 
                SceneManager.LoadScene("MinigameAsteroids");
        }

        // ==== Ref Update ===================================================

        pManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        // ==== Instructions =================================================

        if (!gameStart && (Input.GetButton("1A") || Input.GetButton("2A") || Input.GetButton("3A") || Input.GetButton("4A")) && !singlePress)
        {
            singlePress = true;
            minTime = 0;
            NextScreen();
        }
        if (!gameStart && (Input.GetButton("1B") || Input.GetButton("2B") || Input.GetButton("3B") || Input.GetButton("4B")) && !singlePress)
        {
            singlePress = true;
            minTime = 0;
            PrevScreen();
        }
        if (minTime > 0.6f)
        {
            singlePress = false;
        }

        // ==== Display ======================================================

        if (gameStart)
        {
            infoScreens[curScreen].SetActive(false);
            gameObjects.SetActive(true);

            try {
                pc1.GetComponentsInChildren<Text>()[0].text = pManager.playerNames[0];
                pc1.GetComponentsInChildren<Text>()[1].text = "Health: " + GameObject.Find("P1").GetComponent<PlayerCollision>().Lives + "    Boosts:" + GameObject.Find("P1").GetComponent<CarController>().boosts + "";
            } 
            catch (System.NullReferenceException e) { 
                playersLeft--;
                pc1.GetComponentsInChildren<Text>()[1].text = "WASTED";
                placement.Insert(0, pManager.playerNames[0]);
                Debug.Log("Player1 Reference Gone: " + e);
            }
            
            try {
                pc2.GetComponentsInChildren<Text>()[0].text = pManager.playerNames[1];
                pc2.GetComponentsInChildren<Text>()[1].text = "Health: " + GameObject.Find("P2").GetComponent<PlayerCollision>().Lives + "    Boosts:" + GameObject.Find("P1").GetComponent<CarController>().boosts + "";
            } 
            catch (System.NullReferenceException e) { 
                playersLeft--;
                pc2.GetComponentsInChildren<Text>()[1].text = "WASTED";
                placement.Insert(0, pManager.playerNames[1]);
                Debug.Log("Player2 Reference Gone: " + e); 
            }

            // *Only show Playercards and cars if there are enough players

            try {
                if (GameObject.Find("GameManager").GetComponent<GameManagerScript>().totalPlayers >= 3)
                {
                    pc3.GetComponentsInChildren<Text>()[0].text = pManager.playerNames[2];
                    pc3.GetComponentsInChildren<Text>()[1].text = "Health: " + GameObject.Find("P3").GetComponent<PlayerCollision>().Lives + "    Boosts:" + GameObject.Find("P1").GetComponent<CarController>().boosts + "";
                }
                else
                {
                    pc3.SetActive(false);
                    GameObject.Find("P3").SetActive(false);
                }
            } 
            catch (System.NullReferenceException e) { 
                playersLeft--;
                pc3.GetComponentsInChildren<Text>()[1].text = "WASTED";
                if (pManager.playerNames.Length >= 3) 
                {
                    placement.Insert(0, pManager.playerNames[2]);
                }
                Debug.Log("Player3 Reference Gone: " + e);
                
            }

            try {
                if (GameObject.Find("GameManager").GetComponent<GameManagerScript>().totalPlayers >= 4)
                {
                    pc4.GetComponentsInChildren<Text>()[0].text = pManager.playerNames[3];
                    pc4.GetComponentsInChildren<Text>()[1].text = "Health: " + GameObject.Find("P4").GetComponent<PlayerCollision>().Lives + "    Boosts:" + GameObject.Find("P1").GetComponent<CarController>().boosts + "";
                }
                else
                {
                    pc4.SetActive(false);
                    GameObject.Find("P4").SetActive(false);
                }
            } 
            catch (System.NullReferenceException e) { 
                playersLeft--;
                pc4.GetComponentsInChildren<Text>()[1].text = "WASTED";
                if (pManager.playerNames.Length >= 4) 
                {
                    placement.Insert(0, pManager.playerNames[3]);
                }
                Debug.Log("Player4 Reference Gone: " + e);
            }
        }
        
        // ==== Win Screen ==================================================

        if (playersLeft <= 1)
        {
            // Give fame
            //pManager.playerFame[0];
            if (!fameAssigned)
            {
                for (int i = 0; i < placement.Count; i++)
                {
                    switch (i) {
                        case 1:
                            
                            break;  
                        default:
                            break;
                    }
                }
            }

            infoScreens[5].SetActive(true);
            infoScreens[6].GetComponent<Text>().text = "P" + GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>().player + " Wins";
            Time.timeScale = 0;

            if ((Input.GetButton("1B") || Input.GetButton("2B") || Input.GetButton("3B") || Input.GetButton("4B")) && !singlePress) 
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        // ==== Spawning ====================================================

        if (totalPlayers != 1 && gameStart)
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
    public void SubtractPlayer ()
    {
        totalPlayers--;
    }

    
    // =================================
	// ======= BUTTON FUNCTIONS ========
	// =================================

    public void NextScreen ()
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
    public void PrevScreen ()
    {
        if (curScreen > 0)
        {
            curScreen--;
            infoScreens[curScreen].SetActive(true);
            infoScreens[curScreen + 1].SetActive(false);
        }
    }
}
