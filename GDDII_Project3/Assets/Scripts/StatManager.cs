using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour {

    // VARIABLES

    // ~ Game
    public int currentScore = 0;
    public int currentLevel = 1;
    public int scoreToLevel2 = 100;
    public int scoreToLevel3 = 250;
    // ~ Player
    public float defaultMaxVel = 0.07f;
    public float defaultMaxAcc = 0.1f;
    public float invincibilityTime = 1.5f;
    public int currentPlayers = 4;
    public string currentWeapon = "Beam";
    // ~ Asteroids
    public float defaultAsteroidSpeed = 0.02f;
    public int asteroidHitPoints = 2;
    public int asteroidScoreBonus = 10;
    public float damageFlashTime = 0.05f;
    // ~ Bullets
    public float defaultBulletSpeed = 0.5f;
    public float bulletLifeTime = 2.5f;
    // ~ Power Ups
    public float powerUpDuration = 6;
    // ~~ Speed Boost
    public float maxVelBoost = 0.15f;
    public float maxAccBoost = 0.15f;
    // ~~ Pulse Beam
    public float beamChargeTime = 1f;
    // ~~ Gattling Gun
    public float gatFireSpeed = 0.2f;


    // Use this for initialization
    void Start ()
    {
        currentLevel = 1;
        currentPlayers = 4;
        currentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Players - " + currentScore);
        // Debug.Log("Score - " + currentScore);

        if (currentScore >= scoreToLevel2)
        {
            currentLevel = 2;
        }
        if (currentScore >= scoreToLevel3)
        {
            currentLevel = 3;
        }
	}

    // Control Score
    public void AddScore(int num)
    {
        currentScore += num;
    }

    // Control Lives
    public void SubtractPlayer()
    {
        currentPlayers--;
    }
}
