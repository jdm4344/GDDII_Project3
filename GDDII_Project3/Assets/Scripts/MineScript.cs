using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour 
{
	// ==== VARIABLES ====

	// ~ GameObj Refs
	public GameObject explosion;

	// ~ Sprites
	public Sprite active;
	public Sprite inactive;

    // ~ Timers
	private float timer;
	private float alt;
	public float detonateTime = 5;


	// ==== METHODS ====

	void Awake () 
	{
		timer = detonateTime;
		alt = 1;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		timer -= Time.deltaTime;

		// Blowup after set amount of time
		if (timer <= 0.15f) 
		{
			GetComponent<CircleCollider2D>().radius = 1.25f;
		}
		if (timer <= 0) 
		{
			GameObject.Instantiate(explosion, transform.position, transform.rotation);
			GameObject.Destroy(gameObject);
		}

		// Flashing effect
		if (timer % alt < alt && timer % alt >= alt / 2) 
		{
			GetComponent<SpriteRenderer>().sprite = active;
		}
		else 
		{
			GetComponent<SpriteRenderer>().sprite = inactive;
		}
		alt -= 0.005f;
	}

	
	// ================================
	// ========== COLLISION ===========
	// ================================

	void OnTriggerEnter2D(Collider2D col)
    {
		GameObject.Instantiate(explosion, transform.position, transform.rotation);
		GameObject.Destroy(gameObject);
    }
}
