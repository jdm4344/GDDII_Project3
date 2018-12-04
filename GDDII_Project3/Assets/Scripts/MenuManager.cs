using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
/* Authors: Jordan Machalek
 * Contains behavior for main menu functionality
 */
public class MenuManager : MonoBehaviour {

    // Variables
    public GameObject menuPanel;
    public GameObject optionsPanel;
    public GameObject playerCardTemplate; // template for player card objects
    public GameObject[] playerCards; // array of created player cards
    public GameObject optionsManager;

    #region Persistent variables
    private bool onMenu;
    [SerializeField]
    private PlayerManager playerManager;
    #endregion

    #region Minigame specific variables
    [Header("Minigame Variables")]
    [Space(5)]
    public GameObject minigamePanel; // UI Panel obj that is parent of all minigame UI objs
    public List<string> minigames; // List of names of scenes
    private Stack<string> playedMinigames; // Keeps track of minigames that have been played, clears when all have been played once
    private string selectedMinigame; // Minigame chosen in PickMinigame()
    public GameObject iconContainer; // Gameobject whose children are Image objs displaying minigame names
    [SerializeField]
    private List<GameObject> icons; // List of Image objs 
    public GameObject countdownPopup; // Parent Image obj with a child Text obj displaying time before minigame loads
    private float timer = 3;
    private bool loadMinigame = false;
    #endregion

    #region Board-space specific variables
    [Header("Map Variables")]
    [Space(5)]
    public GameObject mapPanel; // UI Panel obj that is parent of all map UI objs
    public GameObject spaceContainer; // Gameobject whose children are spaces on the board
    [SerializeField]
    private List<GameObject> spaces; // List of Image objs
    #endregion

    //void Awake()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //}

    // Use this for initialization
    void Start () 
    {
        // Show and hide panels for redundancy's sake
        menuPanel.SetActive(true);
        minigamePanel.SetActive(false);
        mapPanel.SetActive(false);
        optionsPanel.SetActive(false);


        playedMinigames = new Stack<string>();
        spaces = new List<GameObject>();

        // Add all of the spaces to the list of spaces
		foreach(Transform child in spaceContainer.transform)
        {
            spaces.Add(child.gameObject);
        }

        // Add all of the icons to the list of icons
		foreach(Transform child in iconContainer.transform)
        {
            icons.Add(child.gameObject);
        }

        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        optionsManager = GameObject.Find("OptionsManager");
        onMenu = true;

        GeneratePlayerCards();
	}
	
	// Update is called once per frame
	void Update () 
    {
        Time.timeScale = 1;

        if(loadMinigame)
        {
            timer -= Time.deltaTime;
            countdownPopup.GetComponentInChildren<Text>().text = "Minigame begins in: " + timer.ToString("F1");
            
            // Play the selected minigame
            if (timer <= 0)
            {
                onMenu = false;
                loadMinigame = false;
                SceneManager.LoadScene(selectedMinigame);
            }
        }

        //Set current player fame to display
        if(menuPanel.activeInHierarchy)
        {
            int numPlayers = playerManager.playerNames.Length;
            TextMeshProUGUI[] fameVal = new TextMeshProUGUI[numPlayers];

            for (int i = 0; i < numPlayers; i++)
            {
                fameVal[i] = playerCards[i].transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>();
                fameVal[i].text = playerManager.playerFame[i].ToString();
            }
        }

        if (optionsPanel.activeInHierarchy)
        {
            TMP_InputField checkVal = GameObject.Find("CheckpointCostValue").GetComponent<TMP_InputField>();
            TMP_InputField fameVal = GameObject.Find("AddFameValue").GetComponent<TMP_InputField>();

            checkVal.text = optionsManager.GetComponent<optionsManager>().checkpointCost.ToString();
            fameVal.text = optionsManager.GetComponent<optionsManager>().fameForAllValue.ToString();
        }
    }

    /// <summary>
    /// Randomly selects and loads a minigame from the 'minigames' list
    /// To be attached to 'ChooseGameButton' object in menu canvas
    /// </summary>
    public void PickMinigame()
    {
        // Swap UI
        ToggleMinigames();

        int gameNum;

        // Clear the stack of played minigames if they have all been played
        if (playedMinigames.Count == minigames.Count) { playedMinigames.Clear(); }

        // Pick a minigame that hasn't been played
        while (true)
        {
            gameNum = Random.Range(0, minigames.Count);
            if(!playedMinigames.Contains(minigames[gameNum])) { break;}
        }

        selectedMinigame = minigames[gameNum];
        playedMinigames.Push(selectedMinigame);
        icons[gameNum].GetComponent<RawImage>().color = Color.red;
        // Show popup and start countdown
        countdownPopup.SetActive(true);
        loadMinigame = true;
    }

    /// <summary>
    /// Randomly selects a position for a new checkpoint
    /// </summary>
    public void PickNewCheckpoint()
    {
        // Swap UI
        ToggleMap();

        int spaceNum;
        
        while(true)
        {
            // Get a random space
            spaceNum = Random.Range(0, spaces.Count);
            // Check if the space is not a shop, if so, break
            // If it's a shop, choose a new space
            if(spaces[spaceNum].tag != "Shop Space") { break;}
        }

        spaces[spaceNum].GetComponent<RawImage>().color = Color.red;
    }
    
    /// <summary>
    /// Helper Method
    /// Shows/hides main menu and map panels
    /// </summary>
    public void ToggleMap()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        mapPanel.SetActive(!mapPanel.activeSelf);

        Color spaceColor = new Color32(177, 191, 255, 255);

        // Reset colors of map spaces
        foreach(GameObject space in spaces)
        {
            if(space.tag != "Shop Space") {space.GetComponent<RawImage>().color = spaceColor;}
        }
    }

    /// <summary>
    /// Helper Method
    /// Shows/hides main menu and minigame panels
    /// </summary>
    public void ToggleMinigames()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        minigamePanel.SetActive(!minigamePanel.activeSelf);
        countdownPopup.SetActive(!countdownPopup.activeSelf);

        // Stop countdown for loading minigame
        timer = 3;
        loadMinigame = false;

        // Reset colors of game icons
        foreach(GameObject icon in icons)
        {
            icon.GetComponent<RawImage>().color = Color.white;
        }
    }

    /// <summary>
    /// Creates UI elements displaying player data and names
    /// </summary>
    public void GeneratePlayerCards()
    {
        // BASIC CODE - Working
        // Update Player names
        //player1Objects[2].GetComponent<TextMeshProUGUI>().text = playerManager.playerNames[0];
        //player2Objects[2].GetComponent<TextMeshProUGUI>().text = playerManager.playerNames[1];
        //player3Objects[2].GetComponent<TextMeshProUGUI>().text = playerManager.playerNames[2];
        //player4Objects[2].GetComponent<TextMeshProUGUI>().text = playerManager.playerNames[3];

        // DYNAMIC CODE - In Progress
        int numPlayers = playerManager.playerNames.Length;
        playerCards = new GameObject[numPlayers];

        GameObject canvas = GameObject.Find("Canvas");
        Vector3 startPos = new Vector3(0, 0, 0);

        //Initial position based on # of players
        if (numPlayers < 3)
            startPos = new Vector3(-175, -260, 0);
        else if (numPlayers == 3)
            startPos = new Vector3(-275, -260, 0);
        else if (numPlayers == 4)
            startPos = new Vector3(-375, -260, 0);

        for (int i = 0; i < playerManager.playerNames.Length; i++)
        {
            // Create card object
            GameObject temp = Instantiate(playerCardTemplate);
            temp.transform.SetParent(menuPanel.transform);

            // Set the player's name
            temp.GetComponent<TMP_InputField>().text = playerManager.playerNames[i];

            // Set position
            temp.GetComponent<RectTransform>().localPosition = startPos;
            // Template is not active, so set active - doing this may be redundant
            temp.SetActive(true);
            // Save the card
            playerCards[i] = temp;

            // Assign position (based on # of players)
            if (numPlayers < 3)
                startPos = new Vector3(startPos.x + 350, -260, 0);
            else if (numPlayers == 3)
                startPos = new Vector3(startPos.x + 275, -260, 0);
            else if (numPlayers == 4)
                startPos = new Vector3(startPos.x + 250, -260, 0);
        }
    }

    #region Temp Score Code
    /// <summary>
    /// Purchase stuff with the shop button
    /// </summary>
    public void Purchase()
    {
        string coststr = GameObject.Find("costValue").GetComponent<Text>().text;
        int cost;
        int.TryParse(coststr, out cost);

        int numPlayers = playerManager.playerNames.Length;

        bool player1Selected = GameObject.Find("Toggle1").GetComponent<Toggle>().isOn;
        bool player2Selected = GameObject.Find("Toggle2").GetComponent<Toggle>().isOn;
        bool player3Selected = GameObject.Find("Toggle3").GetComponent<Toggle>().isOn;
        bool player4Selected = GameObject.Find("Toggle4").GetComponent<Toggle>().isOn;


        if (player1Selected && numPlayers > 0)
           playerManager.playerFame[0] -= cost;
        else if (player2Selected && numPlayers > 1)
            playerManager.playerFame[1] -= cost;
        else if (player3Selected && numPlayers > 2)
            playerManager.playerFame[2] -= cost;
        else if (player4Selected && numPlayers > 3)
            playerManager.playerFame[3] -= cost;

    }

    ///// <summary>
    ///// Purchase stuff with the checkpoint button
    ///// </summary>
    public void PurchaseCheck()
    {
        int numPlayers = playerManager.playerNames.Length;

        bool player1Selected = GameObject.Find("ToggleCheck1").GetComponent<Toggle>().isOn;
        bool player2Selected = GameObject.Find("ToggleCheck2").GetComponent<Toggle>().isOn;
        bool player3Selected = GameObject.Find("ToggleCheck3").GetComponent<Toggle>().isOn;
        bool player4Selected = GameObject.Find("ToggleCheck4").GetComponent<Toggle>().isOn;


        if (player1Selected && numPlayers > 0)
            playerManager.playerFame[0] -= optionsManager.GetComponent<optionsManager>().checkpointCost;
        else if (player2Selected && numPlayers > 1)
            playerManager.playerFame[1] -= optionsManager.GetComponent<optionsManager>().checkpointCost;
        else if (player3Selected && numPlayers > 2)
            playerManager.playerFame[2] -= optionsManager.GetComponent<optionsManager>().checkpointCost;
        else if (player4Selected && numPlayers > 3)
            playerManager.playerFame[3] -= optionsManager.GetComponent<optionsManager>().checkpointCost;
    }

    ///// <summary>
    ///// Add Fame to all players
    ///// </summary>
    public void AddFame()
    {
        int numPlayers = playerManager.playerNames.Length;

        for (int i = 0; i < numPlayers; i++)
        {
            playerManager.playerFame[i] += optionsManager.GetComponent<optionsManager>().fameForAllValue;
        }
    }

    #endregion

    public void DisplayOptions()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    public void UpdateOptions()
    {
        TMP_InputField checkVal = GameObject.Find("CheckpointCostValue").GetComponent<TMP_InputField>();
        TMP_InputField fameVal = GameObject.Find("AddFameValue").GetComponent<TMP_InputField>();

        string tempStr = checkVal.text;
        int tempCheck;
        int.TryParse(tempStr, out tempCheck);

        tempStr = fameVal.text;
        int tempFame;
        int.TryParse(tempStr, out tempFame);

        optionsManager.GetComponent<optionsManager>().fameForAllValue = tempFame;
        optionsManager.GetComponent<optionsManager>().checkpointCost = tempCheck;
    }
}
