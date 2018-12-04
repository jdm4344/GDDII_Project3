using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionsManager : MonoBehaviour {

    public int checkpointCost;
    public int fameForAllValue;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        checkpointCost = 10;
        fameForAllValue = 2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
