using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Tracks player names, score and currency between scenes
 */
public class PlayerManager : MonoBehaviour {

    // Variables
    public string[] playerNames;
    public int[] playerFame;
    public int[] turnOrder;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

	// Use this for initialization
	void Start ()
    {
        playerNames = new string[4];
        playerFame = new int[4];
        turnOrder = new int[4];
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
