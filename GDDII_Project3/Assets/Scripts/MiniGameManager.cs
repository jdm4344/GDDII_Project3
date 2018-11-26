using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour {

	// GameObject Refs
	public Camera cam;
	
	// ~ Movement Values
	private Vector3 offset;
	private float increment;
	public float camMovementSpd = 0.5f;
	

	void Start () 
	{
		offset = cam.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Camera Circular Movement Effect
		Vector3 pos = cam.transform.position;
		increment += camMovementSpd * Time.deltaTime;

		cam.transform.position += new Vector3(Mathf.Sin(increment) / 4, Mathf.Cos(increment) / 4, 0);
	}
}
