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
                foreach(GameObject field in playerNameFields) { GameObject.Destroy(field); }
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

        Vector3 startPos = new Vector3(0, 200, 0); // Reference position relative to Canvas
        GameObject canvas = GameObject.Find("Canvas");

        for (int i = 0; i < numPlayers; i++)
        {
            // Create Player name field
            GameObject temp = Instantiate(playerInputTemplate);
            temp.transform.SetParent(canvas.transform); // Assign to the scene's Canvas

            // Get the Text components of the field's children to update them
            Text[] comps = temp.GetComponentsInChildren<Text>();
            comps[2].text = "Player " + (i + 1); // [2] is the child TitleText of PlayerInputTemplate
            comps[0].text = "Enter Name...";  // [0] is the child Placeholder of InputField which is the child of PlayerInputTemplate

            // Move the field down
            startPos = new Vector3(startPos.x, startPos.y - 100, startPos.z);
            temp.GetComponent<RectTransform>().localPosition = startPos;
            // Template is not active, so set active - doing this may be redundant
            temp.SetActive(true);
            // Save the new field object
            playerNameFields[i] = temp;
        }

        createFields = false;
    }

    /// <summary>
    /// Updates player names in PlayerManager.cs based on input fields
    /// Runs 'On End Edit' in InputField child of PlayerInputTemplate
    ///    - Associated object for this is PlayerManager
    /// </summary>
    public void PlayerUpdate()
    {
        // Update Player names
        //playerManager.playerNames[0] = player1Objects[2].GetComponent<InputField>().text;

        // Recreate data structures in PlayerManager
        playerManager.playerNames = new string[numPlayers];
        playerManager.playerFame = new int[numPlayers];
        playerManager.turnOrder = new int[numPlayers];

        // Save player names
        for (int i = 0; i < numPlayers; i++)
        {
            playerManager.playerNames[i] = playerNameFields[i].GetComponentsInChildren<Text>()[1].text;
        }
    }

    // Increment menu state
    public void AdvanceMenu()
    {
        menuState++;
        if(menuState > 1) { SceneManager.LoadScene("MainMenu"); }
    }

    // Decrement menu state
    public void RetreatMenu()
    {
        menuState--;
        if(menuState < 0) { menuState = 0; }
    }
}
