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
    public List<string> placement;
    private int curScreen = 0;
    private bool gameStart = false;
    private bool singlePress = false;
    private bool fameAssigned;
    private float minTime = 0;

    [Header("Debug Mode")]
    public bool testMode = false;

    [Header("Game Control")]
    public List<GameObject> infoScreens;
    public List<GameObject> players;
    public List<string> playerNames;
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

        placement = new List<string>();
        playerNames = new List<string>();

        for (int i = 0; i < pManager.playerNum; i++)
        {
            playerNames.Add(pManager.playerNames[i]);
        }

        totalPlayers = pManager.playerNames.Length;
        fameAssigned = false;
        playersLeft = totalPlayers;
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

        // ==== Instructions Input ===========================================

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
            pc1.SetActive(true);
            pc2.SetActive(true);
            pc3.SetActive(true);
            pc4.SetActive(true);

            try 
            {
                if (players[0] != null)
                {
                    pc1.GetComponentsInChildren<Text>()[0].text = pManager.playerNames[0];
                    pc1.GetComponentsInChildren<Text>()[1].text = "Health: " + players[0].GetComponent<PlayerCollision>().Lives + "    Boosts:" + players[0].GetComponent<CarController>().boosts + "";
                }
                else 
                {
                    pc1.GetComponentsInChildren<Text>()[1].text = "WASTED";
                    players[0] = null;
                }

                if (players[1] != null)
                {
                    pc2.GetComponentsInChildren<Text>()[0].text = pManager.playerNames[1];
                    pc2.GetComponentsInChildren<Text>()[1].text = "Health: " + players[1].GetComponent<PlayerCollision>().Lives + "    Boosts:" + players[1].GetComponent<CarController>().boosts + "";
                }
                else 
                {
                    pc2.GetComponentsInChildren<Text>()[1].text = "WASTED";
                    players[1] = null;
                }
                // Only show playercards and cars for players 3 and 4 if there are enough players 
                if (totalPlayers >= 3) 
                {
                    if (players[2] != null)
                    {
                        pc3.GetComponentsInChildren<Text>()[0].text = pManager.playerNames[2];
                        pc3.GetComponentsInChildren<Text>()[1].text = "Health: " + players[2].GetComponent<PlayerCollision>().Lives + "    Boosts:" + players[2].GetComponent<CarController>().boosts + "";
                    }
                    else 
                    {
                        pc3.GetComponentsInChildren<Text>()[1].text = "WASTED";
                        players[2] = null;  
                    }
                }
                else
                {
                    if (GameObject.Find("P3")) 
                    {
                        GameObject.Find("P3").SetActive(false);
                    }
                    pc3.SetActive(false);
                    players[2] = null;
                }

                if (totalPlayers == 4) 
                {
                    if (players[3] != null)
                    {
                        pc4.GetComponentsInChildren<Text>()[0].text = pManager.playerNames[3];
                        pc4.GetComponentsInChildren<Text>()[1].text = "Health: " + players[3].GetComponent<PlayerCollision>().Lives + "    Boosts:" + players[3].GetComponent<CarController>().boosts + "";
                    }
                    else 
                    {
                        pc4.GetComponentsInChildren<Text>()[1].text = "WASTED";
                        players[3] = null;
                    }
                }
                else
                {
                    if (GameObject.Find("P4")) 
                    {
                        GameObject.Find("P4").SetActive(false);
                    }
                    pc4.SetActive(false);
                    players[3] = null;
                }
            } 
            catch (System.Exception e) 
            {
                Debug.Log("Error: " + e);
            }
        }
        
        // ==== Win Screen ==================================================

        if (playersLeft <= 1)
        {
            Debug.Log("Game Ended");
            
            if (!fameAssigned)
            {
                // Insert Victor
                for (int i = 0; i < totalPlayers; i++)
                {
                    if (players[i] != null)
                    {
                        placement.Insert(0, playerNames[i]);
                    }
                }
                // Give Fame
                for (int i = 0; i < totalPlayers; i++)
                {
                    switch (i) {
                        case 0:
                            pManager.playerFame[playerNames.IndexOf(placement[0])] += 5;
                            break;  
                        case 1:
                            pManager.playerFame[playerNames.IndexOf(placement[1])] += 3;
                            break;  
                        case 2:
                            pManager.playerFame[playerNames.IndexOf(placement[2])] += 1;
                            break;  
                        case 3:
                            pManager.playerFame[playerNames.IndexOf(placement[3])] += 0;
                            break;  
                        default:
                            break;
                    }
                }
                fameAssigned = true;
            }

            infoScreens[5].SetActive(true);
            infoScreens[6].GetComponent<Text>().text = placement[0] + " Wins";
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
            if (spawnNormTimer > 7) 
            {
                GameObject obj = Instantiate(Vehicles, new Vector3(10, Random.Range(-2.0f, 5)), Quaternion.Euler(0, 0, 180));
                obj.GetComponent<VehicleScript>().movePattern = MovePattern.Straight;
                spawnNormTimer = 0;
            }
            if (spawnSwerverTimer > 10) 
            {
                GameObject obj = Instantiate(Vehicles, new Vector3(-10, 2), Quaternion.Euler(0, 0, 45));
                obj.GetComponent<VehicleScript>().movePattern = MovePattern.Swerve;
                spawnSwerverTimer = 0;
            }
            if (spawnBomberTimer > 15) 
            {
                GameObject obj = Instantiate(Vehicles, new Vector3(Random.Range(-3.0f, 5), 7), Quaternion.Euler(0, 0, -90));
                obj.GetComponent<VehicleScript>().movePattern = MovePattern.BombDropper;
                spawnBomberTimer = 0;
            }
            // Spawn boosts
            if (boostSpawnTimer > 4)
            {
                GameObject obj = Instantiate(boost, new Vector3(Random.Range(-3.0f, 3), Random.Range(-2.0f, 4)), Quaternion.Euler(0, 0, 0));
                boostSpawnTimer = 0;
            }
        }
	}

    // Control Lives
    public void SubtractPlayer ()
    {
        playersLeft--;
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
