using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObjScript : MonoBehaviour {

	public float lifeTime = 2;
	
	void Update () 
	{
		lifeTime -= Time.deltaTime;

		if (lifeTime <= 0) 
			Destroy(gameObject);
	}
}
