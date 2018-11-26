using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
 * Handles functionality for PlayerMenu Scene
 */
public class PlayerMenu : MonoBehaviour {

    // Variables
    public GameObject playerInputTemplate;
    private PlayerManager playerManager;
    public int menuState = 0;
    public int numPlayers = 0;
    private bool createFields = true;

    [Header("Buttons")]
    [Space(5)]
    public GameObject backButton;
    public GameObject continueButton;
    [Header("Input Fields")]
    [Space(5)]
    public GameObject playerNumField;
    public GameObject[] playerNameFields;

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
                createFields = true;
                break;
            case 1:
                backButton.SetActive(true);
                playerNumField.SetActive(false);
                if (createFields) { GeneratePlayerFields(); }
                break;
        }
	}

    /// <summary>
    /// Creates a number of input fields for player names
    /// </summary>
    private void GeneratePlayerFields()
    {
        playerNameFields = new GameObject[numPlayers];

        for (int i = 0; i < numPlayers; i++)
        {
            GameObject temp = Instantiate(playerInputTemplate);
            temp.GetComponentInChildren<Text>().text = "Player " + (i + 1);

            Vector3 tempPos = temp.GetComponent<RectTransform>().position;
            temp.GetComponent<RectTransform>().position = new Vector3(0, 100, 0);

            temp.SetActive(true);

            playerNameFields[i] = temp;
        }

        createFields = false;
    }

    /// <summary>
    /// Updates player names in PlayerManager.cs based on input fields
    /// </summary>
    public void PlayerUpdate()
    {
        // Update Player names
        //playerManager.playerNames[0] = player1Objects[2].GetComponent<InputField>().text;
    }

    public void AdvanceMenu()
    {
        menuState++;
        if(menuState > 1) { SceneManager.LoadScene("MainMenu"); }
    }

    public void RetreatMenu()
    {
        menuState--;
        if(menuState < 0) { menuState = 0; }
    }

}
