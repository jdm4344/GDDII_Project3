using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour {

    public GameObject manager;

    private bool isInvulnerable = false;

    private int lives;

    private double timer;
    private double invincibilityTimer;
    //private double powerUpTimer1;
    //private double powerUpTimer2;
    //private double powerUpTimer3;

    private float invincibilityDuration;
    private float shieldDuration;

    private bool pickedUpBonus = false;
    private bool pickedUpPowerup = false;

    // Initialization
    void Start ()
    {
        timer = 0;
        invincibilityTimer = 0;
        invincibilityDuration = GetComponent<StatManager>().invincibilityTime;
        shieldDuration = 2;
        /*powerUpTimer1 = 0;
        powerUpTimer2 = 0;
        powerUpTimer3 = 0;*/
    }
	
	// Update
	void FixedUpdate () 
    {
        /* Debugging for Collision
        if (isColliding)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }*/

        lives = gameObject.GetComponent<CarController>().health;

        if (lives == 0)
        {
            manager.GetComponent<StatManager>().SubtractPlayer();
            GameObject.Destroy(gameObject);
        }

        Debug.Log(" ~ " + timer);
        timer -= Time.deltaTime;
        invincibilityTimer -= Time.deltaTime;
        /*powerUpTimer1 -= Time.deltaTime;
        powerUpTimer2 -= Time.deltaTime;
        powerUpTimer3 -= Time.deltaTime;*/

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

        /*if (powerUpTimer1 < 0 && pickedUpBonus)
        {
            Debug.Log("Powerup finished");
            // Return everything to defaults
            gameObject.GetComponent<CarController>().maxVel = manager.GetComponent<StatManager>().defaultMaxVel;
            gameObject.GetComponent<CarController>().maxAcc = manager.GetComponent<StatManager>().defaultMaxAcc;
            pickedUpBonus = false;
        }
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
            if ((col.otherCollider.gameObject.tag == "Player"))
            {
                DamageFlash();
                isInvulnerable = true;
                invincibilityTimer = invincibilityDuration;
                GetComponent<CarController>().LoseHealth();
            }
        }

        /*
        else if (col.otherCollider.gameObject.tag == "Shield")
        {
            Debug.Log("Gained Shield");
            Shield();
        }
        else if (col.otherCollider.gameObject.tag == "SpeedBoost")
        {
            pickedUpBonus = true;
            Debug.Log("Gained A Speed Boost");
            SpeedBoost();
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
        //powerUpTimer1 = manager.GetComponent<StatManager>().powerUpDuration;
        gameObject.GetComponent<CarController>().maxVel = manager.GetComponent<StatManager>().maxVelBoost;
        gameObject.GetComponent<CarController>().maxAcc = manager.GetComponent<StatManager>().maxAccBoost;
    }
}
