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
    
    //private double powerUpTimer2;
    //private double powerUpTimer3;

    private float invincibilityDuration;
    private float shieldDuration;


    // ==== METHODS ====

    // Initialization
    void Start ()
    {
        maxMov = false;
        timer = 0;
        invincibilityTimer = 0;
        invincibilityDuration = manager.GetComponent<StatManager>().invincibilityTime;
        shieldDuration = 2;
        powerUpTimer1 = 0;
        maxVel = manager.GetComponent<StatManager>().maxVelBoost;
        Debug.Log(maxVel);
        /*powerUpTimer2 = 0;
        powerUpTimer3 = 0;*/
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
            manager.GetComponent<StatManager>().SubtractPlayer();
            GameObject.Instantiate(explosion, transform.position, transform.rotation);
            GameObject.Destroy(gameObject);
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
            gameObject.GetComponent<CarController>().maxVel = manager.GetComponent<StatManager>().defaultMaxVel;
            gameObject.GetComponent<CarController>().maxAcc = manager.GetComponent<StatManager>().defaultMaxAcc;
            pickedUpPowerup = false;
        }/*
        if (powerUpTimer2 < 0 && pickedUpPowerup)
        {
            Debug.Log("Powerup finished");
            // Return everything to defaults
            manager.GetComponent<StatManager>().currentWeapon = "Beam";
            pickedUpPowerup = false;
        }*/
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("hit");
        if (!isInvulnerable) 
        {
            if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerCollision>().maxMov)
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
                // slow them down
                GetComponent<CarController>().LoseHealth();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.tag == "SpeedBoost")
        {
            pickedUpPowerup = true;
            Debug.Log("Gained A Speed Boost");
            SpeedBoost();
        }
        /*
        else if (col.otherCollider.gameObject.tag == "Shield")
        {
            Debug.Log("Gained Shield");
            Shield();
        }*/
    }

    void DamageFlash()
    {
        Debug.Log("Damaged");
        timer = manager.GetComponent<StatManager>().damageFlashTime;
        gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    void Shield()
    {
        invincibilityTimer = shieldDuration;
        isInvulnerable = true;
    }

    void SpeedBoost()
    {
        powerUpTimer1 = manager.GetComponent<StatManager>().powerUpDuration;
        gameObject.GetComponent<CarController>().maxVel = manager.GetComponent<StatManager>().maxVelBoost;
        gameObject.GetComponent<CarController>().maxAcc = manager.GetComponent<StatManager>().maxAccBoost;
    }
}
