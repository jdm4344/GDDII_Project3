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
    private double powerUpTimer1;
    private double powerUpTimer2;
    private double powerUpTimer3;

    private float invincibilityDuration;
    private float shieldDuration;

    private bool pickedUpBonus = false;
    private bool pickedUpPowerup = false;

    // Initialization
    void Start ()
    {
        timer = 0;
        invincibilityTimer = manager.GetComponent<StatManager>().invincibilityTime;;
        invincibilityDuration = manager.GetComponent<StatManager>().invincibilityTime;
        shieldDuration = 2;
        powerUpTimer1 = 0;
        powerUpTimer2 = 0;
        powerUpTimer3 = 0;
    }
	
	// Update
	void FixedUpdate () {

        /* Debugging for Collision
        if (isColliding)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }*/

        lives = manager.GetComponent<ShipPhysics>().health;
        if (lives == 0)
        {
            manager.GetComponent<StatManager>().SubtractPlayer();
            GameObject.Destroy(gameObject);
        }

        timer -= Time.deltaTime;
        invincibilityTimer -= Time.deltaTime;
        powerUpTimer1 -= Time.deltaTime;
        powerUpTimer2 -= Time.deltaTime;
        powerUpTimer3 -= Time.deltaTime;

        // For flashing color for visual effect
        if (timer < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (timer < invincibilityDuration / 2)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (timer < invincibilityDuration)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        // Invincibility frames
        if (invincibilityTimer < 0)
        {
            isInvulnerable = false;
        }

        if (powerUpTimer1 < 0 && pickedUpBonus)
        {
            Debug.Log("Powerup finished");
            // Return everything to defaults
            gameObject.GetComponent<ShipPhysics>().maxVel = manager.GetComponent<StatManager>().defaultMaxVel;
            gameObject.GetComponent<ShipPhysics>().maxAcc = manager.GetComponent<StatManager>().defaultMaxAcc;
            pickedUpBonus = false;
        }
        if (powerUpTimer2 < 0 && pickedUpPowerup)
        {
            Debug.Log("Powerup finished");
            // Return everything to defaults
            manager.GetComponent<StatManager>().currentWeapon = "Beam";
            pickedUpPowerup = false;
        }
        if (powerUpTimer3 < 0 && pickedUpPowerup)
        {
            Debug.Log("Powerup finished");
            // Return everything to defaults
            isInvulnerable = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hit");
        if ((other.gameObject.tag == "Player") && !isInvulnerable)
        {
            Debug.Log("Hit");
            DamageFlash();
            isInvulnerable = true;
            invincibilityTimer = invincibilityDuration;
            GetComponent<ShipPhysics>().LoseHealth();
        }
        else if (other.gameObject.tag == "PulseBeam")
        {
            pickedUpPowerup = true;
            Debug.Log("Picked Up PulseBeam");
            ChangeWeapon("PulseBeam");
        }
        else if (other.gameObject.tag == "Shield")
        {
            Debug.Log("Gained Shield");
            Shield();
        }
        else if (other.gameObject.tag == "SpeedBoost")
        {
            pickedUpBonus = true;
            Debug.Log("Gained A Speed Boost");
            SpeedBoost();
        }
        else if (other.gameObject.tag == "GattlingGun")
        {
            pickedUpPowerup = true;
            Debug.Log("Picked Up Gattling Gun");
            ChangeWeapon("GattlingGun");
        }
    }

    void DamageFlash()
    {
        timer = manager.GetComponent<StatManager>().damageFlashTime;
        gameObject.GetComponent<SpriteRenderer>().color = Color.gray;

    }

    void Shield()
    {
        powerUpTimer3 = shieldDuration;
        isInvulnerable = true;
    }

    void SpeedBoost()
    {
        powerUpTimer1 = manager.GetComponent<StatManager>().powerUpDuration;
        gameObject.GetComponent<ShipPhysics>().maxVel = manager.GetComponent<StatManager>().maxVelBoost;
        gameObject.GetComponent<ShipPhysics>().maxAcc = manager.GetComponent<StatManager>().maxAccBoost;
    }

    void ChangeWeapon(string nameVar)
    {
        if (nameVar == "PulseBeam")
        {
            powerUpTimer2 = manager.GetComponent<StatManager>().powerUpDuration * 1.5;
        }
        else
        {
            powerUpTimer2 = manager.GetComponent<StatManager>().powerUpDuration;
        }
        manager.GetComponent<StatManager>().currentWeapon = nameVar;
    }
}
