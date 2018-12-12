using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour {

    // ==== VARIABLES ====

    // ~ Obj & Prefab Refs
    public GameObject manager;
    public GameObject explosion;

    // ~ Status
    private bool isInvulnerable = false;
    private bool pickedUpPowerup = false;
    private bool pickedUpItem = false;
    public bool maxMov;
    private int lives;
    private double invincibilityTimer;
    private double powerUpTimer1;
    private double timer;
    private float maxVel;

    private float invincibilityDuration;
    private float shieldDuration;


    // ==== PROPERTIES ====
    public int Lives
    {
        get { return lives; }
    }    


    // ==== METHODS ====

    // Initialization
    void Start ()
    {
        maxMov = false;
        timer = 0;
        invincibilityTimer = 0;
        invincibilityDuration = manager.GetComponent<GameManagerScript>().invincibilityTime;
        shieldDuration = 2;
        powerUpTimer1 = 0;
        maxVel = manager.GetComponent<GameManagerScript>().maxVelBoost;
    }
	
	// Update
	void FixedUpdate () 
    {
        // ================= Debugging ===================

        /*
        if (isColliding)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        // */

        // ================ Check Lives ==================

        lives = gameObject.GetComponent<CarController>().health;

        if (lives == 0)
        {
            manager.GetComponent<GameManagerScript>().SubtractPlayer();
            manager.GetComponent<GameManagerScript>().players[manager.GetComponent<GameManagerScript>().players.IndexOf(gameObject)] = null;
            manager.GetComponent<GameManagerScript>().placement.Insert(0, manager.GetComponent<GameManagerScript>().playerNames[GetComponent<CarController>().player - 1]);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        // ================== Movement ===================

        if (GetComponent<CarController>().Velocity.x >= maxVel || GetComponent<CarController>().Velocity.y >= maxVel) 
        {
            maxMov = true;
        }
        else
        {
            maxMov = false;
        }

        // ================== Timers =====================

        timer -= Time.deltaTime;
        invincibilityTimer -= Time.deltaTime;
        powerUpTimer1 -= Time.deltaTime;
        
        // ================== Visuals ====================

        // For flashing color for visual effect
        if (timer < 0)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (timer < invincibilityDuration / 2)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (timer < invincibilityDuration)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        // Invincibility frames
        if (invincibilityTimer < 0)
        {
            isInvulnerable = false;
        }

        if (powerUpTimer1 < 0 && pickedUpPowerup)
        {
            Debug.Log("Powerup finished");
            // Return everything to defaults
            GetComponent<CarController>().boosting = false;
            GetComponent<CarController>().maxVel = manager.GetComponent<GameManagerScript>().defaultMaxVel;
            GetComponent<CarController>().maxAcc = manager.GetComponent<GameManagerScript>().defaultMaxAcc;
            pickedUpPowerup = false;
        }
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (!isInvulnerable) 
        {
            if (
               (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerCollision>().maxMov) ||
                col.gameObject.tag == "Enemy")
            {
                DamageFlash();
                isInvulnerable = true;
                invincibilityTimer = invincibilityDuration;
                GetComponent<CarController>().LoseHealth();
            }
            if (col.gameObject.tag == "Spikes")
            {
                DamageFlash();
                isInvulnerable = true;
                invincibilityTimer = invincibilityDuration;
                // slow down
                GetComponent<CarController>().LoseHealth();
            }
        }
    }

    void OnTriggerEnter2D (Collider2D col) 
    {
        if (col.gameObject.tag == "SpeedBoost")
        {
            //Debug.Log("Gained A Speed Boost");
            GetComponent<CarController>().boosts++;
        }
        if (col.gameObject.tag == "MineItem")
        {
            Debug.Log("Gained A Mine");
            GetComponent<CarController>().mines++;
        }
        if (col.gameObject.tag == "Mine")
        {
            DamageFlash();
            isInvulnerable = true;
            invincibilityTimer = invincibilityDuration;
            GetComponent<CarController>().LoseHealth();
        }
    }

    void DamageFlash ()
    {
        Debug.Log("Damaged");
        timer = manager.GetComponent<GameManagerScript>().damageFlashTime;
        GetComponent<SpriteRenderer>().color = Color.gray;
    }

    void Shield ()
    {
        invincibilityTimer = shieldDuration;
        isInvulnerable = true;
    }

    public void SpeedBoost ()
    {
        pickedUpPowerup = true;
        powerUpTimer1 = manager.GetComponent<GameManagerScript>().speedBoostDuration;
        GetComponent<CarController>().boosts--;
        GetComponent<CarController>().boosting = true;
        GetComponent<CarController>().maxVel = manager.GetComponent<GameManagerScript>().maxVelBoost;
        GetComponent<CarController>().maxAcc = manager.GetComponent<GameManagerScript>().maxAccBoost;
    }
}
