using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Handles functionality for PlayerMenu Scene
 */
public class PlayerMenu : MonoBehaviour {

    // Variables
    private PlayerManager playerManager;
    public int menuState = 0;
    public int numPlayers = 0;

    [Header("Buttons")]
    [Space(5)]
    public GameObject backButton;
    public GameObject continueButton;
    [Header("Input Fields")]
    [Space(5)]
    public GameObject playerNumField;

	// Use this for initialization
	void Start ()
    {
        playerManager = GetComponent<PlayerManager>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
		switch(menuState)
        {
            case 0:
                backButton.SetActive(false);
                playerNumField.SetActive(true);
                // Get numer of players from input field
                int.TryParse(playerNumField.GetComponent<InputField>().text, out numPlayers);
                // Value check numPlayers
                if(numPlayers < 2) { numPlayers = 2; }
                if(numPlayers > 4) { numPlayers = 4; }
                break;
            case 1:
                backButton.SetActive(true);
                playerNumField.SetActive(false);
                break;
        }
	}

    
    public void AdvanceMenu()
    {
        menuState++;
    }

    public void RetreatMenu()
    {
        menuState--;
        if(menuState < 0) { menuState = 0; }
    }
}
