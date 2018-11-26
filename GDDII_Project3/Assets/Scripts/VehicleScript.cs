using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleScript : MonoBehaviour 
{
	// ==== VARIABLES ====

	// ~ GameObj Refs
    public StatManager manager;
    public Camera cam;
    public GameObject exhausts;
    public GameObject explosion;

	// ~ Physics
    private Vector2 position;
    private Vector2 velocity;
    private Vector2 acceleration;
    private float rotationSpeed = 0.025f;
    private float deltaAngle = 0;
    private float decelConst = 0.0005f;
    public float maxVel;
    public float maxAcc;

    // ~ In-game Settings
    private float currentShipFloat = 0.01f;
    const float defaultFloatVal = 0.010f;
    
    // ~ Stats & Effects
	public MovePattern movePattern = MovePattern.Straight;
	public float moveSpd = 0.5f; // Acts as input
	public float swerveTimer;
	public float swerveAmount = 1.0f;
    public int bombs = 0;
    public bool slowed;

	// ==== ENUMS ====
	
	public enum MovePattern
	{
		Straight = 0,
		Swerve = 1,
		Kamikaze = 2,
		AreaBomber = 3
	}


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
		manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<StatManager>();
		cam = Camera.FindObjectOfType<Camera>();

        maxVel = manager.GetComponent<StatManager>().defaultMaxVel;
        maxAcc = manager.GetComponent<StatManager>().defaultMaxAcc;

        position = transform.position;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        deltaAngle = transform.rotation.eulerAngles.z;

		swerveTimer = 0;
	}
	
	void FixedUpdate () 
	{
		// ================= Movement ====================

		// Determines specific physics calculations based on movement pattern
        switch (movePattern)
		{
			case MovePattern.Straight:
				moveSpd = 0.65f;
				break;

			case MovePattern.Swerve:
				// Swerve back and forth
				swerveTimer += Time.deltaTime / 2;
				float mod = swerveTimer % swerveAmount;
				if (mod > 0 && mod < swerveAmount / 2) 
					deltaAngle -= rotationSpeed;
				else if (mod > swerveAmount / 2 && mod < swerveAmount)
					deltaAngle += rotationSpeed; 
				// Move forward as well
				moveSpd = 0.4f;
				break;

			case MovePattern.Kamikaze:
				break;

			case MovePattern.AreaBomber:
				break;

			default:
				Debug.Log("VehicleScript ~ Couldn't specify a movement pattern.");
				break;
		}

        // Gas - acceleration
        if (moveSpd > 0.1f)
        {
            acceleration = new Vector2(maxAcc * moveSpd * Time.deltaTime, maxAcc * moveSpd * Time.deltaTime);
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
        if (moveSpd < -0.4f)
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
        
        // ========= Adjust GameObject Values ============

        // Alter rotation
        transform.rotation = Quaternion.Euler(0, 0, deltaAngle);

        // Alter velocity
        velocity += acceleration;

        // Alter position
        // position += new Vector2(velocity.x * Mathf.Cos(deltaAngle * Mathf.Deg2Rad), velocity.y * Mathf.Sin(deltaAngle * Mathf.Deg2Rad));

        // Set position vector to gameObject transform position
        transform.position += new Vector3(velocity.x * Mathf.Cos(deltaAngle * Mathf.Deg2Rad), velocity.y * Mathf.Sin(deltaAngle * Mathf.Deg2Rad));
        
        // =============== Adjust Children ===============

        // Alter position
        exhausts.transform.position = transform.position;
        exhausts.transform.localPosition = exhausts.transform.localPosition + new Vector3(-0.8f, 0.0f);
        exhausts.transform.rotation = Quaternion.Euler(0, 0, deltaAngle - 90);

	}


	// ================================
	// ========== COLLISION ===========
	// ================================

	void OnCollisionEnter2D(Collision2D col)
    {
		GameObject.Instantiate(explosion, transform.position, transform.rotation);
		GameObject.Destroy(gameObject);
    }
}
