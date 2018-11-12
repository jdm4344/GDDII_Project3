using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 
 */
public class MenuManager : MonoBehaviour {

    // Variables
    // Minigame specific variables
    public List<string> minigames; // List of names of scenes

    // Board-space specific variables
    public int totalSpaces;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Randomly selects and loads a minigame from the 'minigames' list
    /// To be attached to 'ChooseGameButton' object in menu canvas
    /// </summary>
    public void PickMinigame()
    {
        int gameNum = Random.Range(0, minigames.Count);


    }

    /// <summary>
    /// Randomly selects a position for a new checkpoint
    /// </summary>
    public void PickNewCheckpoint()
    {

    }

}
