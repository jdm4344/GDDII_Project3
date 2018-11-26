using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatManager : MonoBehaviour {

    // ==== VARIABLES ====
    // ~ Game
    public bool testMode = true;
    public int currentPlayers = 4;
    public float gameTimer = 0;
    public string winner;

    // ~ Prefabs
    public GameObject boost;

    // ~ Players
    public float defaultMaxVel = 0.07f;
    public float defaultMaxAcc = 0.07f;
    public float invincibilityTime = 1.5f;
    public string currentWeapon = "Beam";

    // ~ "Asteroids"
    public float defaultAsteroidSpeed = 0.02f;
    public float damageFlashTime = 0.05f;
    public int asteroidScoreBonus = 10;
    public int asteroidHitPoints = 2;

    // ~ Projectiles
    public float defaultBulletSpeed = 0.5f;
    public float bulletLifeTime = 2.5f;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (testMode)
        {
            Debug.Log("Players - " + currentPlayers);

            if (Input.GetKey("r")) 
            {   
                Debug.Log("restart");
                SceneManager.LoadScene("MinigameAsteroids");
            }
            if (Input.GetKey("o"))
            {
                Instantiate(boost, new Vector3(0, 2, 0), Quaternion.Euler(0, 0, 0));
            }
        }
	}

    // Control Lives
    public void SubtractPlayer()
    {
        currentPlayers--;
    }
}
