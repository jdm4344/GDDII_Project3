using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {
    
    // ==== VARIABLES ====

    // ~ Settings
    private Renderer sceneRenderer;

    // ~ GameObj Refs
    public GameObject exhausts;
    public GameObject mine;
    public Camera cam;
    public GameManagerScript manager;


    // ~ Input
    private bool singlePressA = false;
    private bool singlePressB = false;
    private float joyStickHorizInput = 0;
    private float triggerInput = 0;
    private float inputDelayA = 0;
    private float inputDelayB = 0;

    // ~ Physics
    private Rigidbody2D rgbd;
    private Vector2 position;
    private Vector2 velocity;
    private Vector2 acceleration;
    private float rotationSpeed = 3;
    private float deltaAngle = 0;
    private float decelConst = 0.0005f;
    public float maxVel;
    public float maxAcc;

    // ~ In-game Settings
    private Rect gameBorderRect;
    private float currentShipFloat = 0.01f;
    const float defaultFloatVal = 0.010f;
    public int player;
    
    // ~ Stats & Effects
    public bool slowed;
    public bool boosting;
    public int mines;
    public int boosts;
    public int health = 3;


    // ==== PROPERTIES ====

    public Vector2 Velocity
    {
        get { return velocity; }
    }
    public float Angle
    {
        get { return deltaAngle; }
    }


    // ==== METHODS ====

    void Awake ()
    {
        sceneRenderer = GetComponentInChildren<Renderer>();
        gameBorderRect = new Rect(cam.transform.position.x - 5.75f, cam.transform.position.y - 4.6f, 11.25f, 9.25f);
        rgbd = GetComponent<Rigidbody2D>();

        maxVel = manager.defaultMaxVel;
        maxAcc = manager.defaultMaxAcc;

        position = transform.position;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        deltaAngle = transform.rotation.eulerAngles.z + 90;

        mines = 0;
        boosts = 1;
        boosting = false;
    }

    void FixedUpdate ()
    {
        // ================= Debugging ===================

        //Debug.Log(acceleration);
        //Debug.Log(velocity);
        //Debug.Log(position);

        // ================= Movement ====================

        // Turning - rotation
        if (joyStickHorizInput < -0.5f)
        {
            deltaAngle -= rotationSpeed; //Left
        }
        else if (joyStickHorizInput > 0.5f)
        {
            deltaAngle += rotationSpeed; //Right
        }

        // Gas - acceleration
        if (triggerInput > 0.1f)
        {
            acceleration = new Vector2(maxAcc * triggerInput * Time.deltaTime, maxAcc * triggerInput * Time.deltaTime);
            rotationSpeed = 3;
        }
        else
        {
            if (velocity.x > currentShipFloat)
            {
                acceleration.x -= decelConst;
            }
            else
            {
                acceleration.x = 0;
                velocity.x = currentShipFloat;
            }
            if (velocity.y > currentShipFloat)
            {
                acceleration.y -= decelConst;
            }
            else
            {
                acceleration.y = 0;
                velocity.y = currentShipFloat;
            }
            rotationSpeed = 2;
        }

        // Braking
        if (triggerInput < -0.4f)
        {
            currentShipFloat = 0;
            rotationSpeed = 0; // Shouldn't be able to turn/rotate while not moving
        }
        // Not Braking and not pressing Gas
        else
        {
            currentShipFloat = defaultFloatVal;
        }
                    
        // ============== Acc & Vel Limits ===============

        // Max Velocity and Acceleration
        if (acceleration.x > maxAcc)
        {
            acceleration.x = maxAcc;
        }
        if (acceleration.y > maxAcc)
        {
            acceleration.y = maxAcc;
        }

        if (velocity.x > maxVel)
        {
            velocity.x = maxVel;
            acceleration.x = 0;
        }
        if (velocity.y > maxVel)
        {
            velocity.y = maxVel;
            acceleration.y = 0;
        }

        // ================= Borders =====================

        GameBorder();
        
        // ========= Adjust GameObject Values ============

        // Alter rotation
        transform.rotation = Quaternion.Euler(0, 0, deltaAngle - 90);

        // Alter velocity
        velocity += acceleration;

        // Alter position
        // position += new Vector2(velocity.x * Mathf.Cos(deltaAngle * Mathf.Deg2Rad), velocity.y * Mathf.Sin(deltaAngle * Mathf.Deg2Rad));

        // Set position vector to gameObject transform position
        transform.position += new Vector3(velocity.x * Mathf.Cos(deltaAngle * Mathf.Deg2Rad), velocity.y * Mathf.Sin(deltaAngle * Mathf.Deg2Rad));
        
        // =============== Adjust Children ===============

        // Alter position
        exhausts.transform.position = transform.position;
        exhausts.transform.localPosition = exhausts.transform.localPosition + new Vector3(0.0f, -0.466f);
        exhausts.transform.rotation = Quaternion.Euler(0, 0, deltaAngle - 90);
    }

    void Update () 
    {
        // Timer
        inputDelayA += Time.deltaTime;
        inputDelayB += Time.deltaTime;

        // Input
        joyStickHorizInput = Input.GetAxis("J" + player + "Horizontal");
        
        if (boosting) 
        {
            triggerInput = 1.5f;
            GetComponentsInChildren<ParticleSystem>()[0].textureSheetAnimation.SetSprite(0, manager.nitrousParticle);
            GetComponentsInChildren<ParticleSystem>()[1].textureSheetAnimation.SetSprite(0, manager.nitrousParticle);
        }
        else
        {
            triggerInput = Input.GetAxis("J" + player + "Triggers");
            GetComponentsInChildren<ParticleSystem>()[0].textureSheetAnimation.SetSprite(0, manager.smokeParticle);
            GetComponentsInChildren<ParticleSystem>()[1].textureSheetAnimation.SetSprite(0, manager.smokeParticle);
        }
        
        if (Input.GetButton(player + "A") && boosts > 0 && !singlePressA) // Boost
        {
            singlePressA = true;
            GetComponent<PlayerCollision>().SpeedBoost();
            inputDelayA = 0;
        }
        
        if (Input.GetButton(player + "B") && boosts > 0 && !singlePressB) // Drop Mine
        {
            singlePressB = true;
            //GetComponent<PlayerCollision>().SpeedBoost();
            inputDelayB = 0;
        }

        // Delay
        if (inputDelayA > 2.0f)
        {
            singlePressA = false;
        }
        if (inputDelayB > 2.0f)
        {
            singlePressB = false;
        }

        // Move Boundaries
        gameBorderRect = new Rect(cam.transform.position.x - 5.75f, cam.transform.position.y - 4.6f, 11.25f, 9.25f);
    }

    public void LoseHealth ()
    {
        health--;
        Debug.Log("Player " + player + " Health : " + health);
    }

    void GameBorder ()
    {
        if (transform.position.x > gameBorderRect.xMax) 
        {
            transform.position = new Vector2(gameBorderRect.xMax, transform.position.y);
            velocity.x = 0;
            rgbd.AddForce(Vector2.left);
        } 
        else if (transform.position.x < gameBorderRect.xMin) 
        {
            transform.position = new Vector2(gameBorderRect.xMin, transform.position.y);
            velocity.x = 0;
            rgbd.AddForce(Vector2.right);
        }
        if (transform.position.y > gameBorderRect.yMax) 
        {
            transform.position = new Vector2(transform.position.x, gameBorderRect.yMax);
            velocity.y = 0;
            rgbd.AddForce(Vector2.down);
        }
        else if (transform.position.y < gameBorderRect.yMin) 
        {
            transform.position = new Vector2(transform.position.x, gameBorderRect.yMin);
            velocity.y = 0;
            rgbd.AddForce(Vector2.up);
        }
    }
}
