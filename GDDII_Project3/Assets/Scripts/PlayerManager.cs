using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Tracks player names, score and currency between scenes
 */
public class PlayerManager : MonoBehaviour {

    // Variables
    public string[] playerNames;
    public int[] playerFame;
    public int[] turnOrder;

    //public int player1fame;
    //public int player2fame;
    //public int player3fame;
    //public int player4fame;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

	// Use this for initialization
	void Start ()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

}
