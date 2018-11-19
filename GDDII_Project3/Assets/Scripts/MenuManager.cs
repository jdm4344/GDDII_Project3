using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/* Authors: Jordan Machalek
 * Contains behavior for main menu functionality
 */
public class MenuManager : MonoBehaviour {

    // Variables
    public GameObject menuPanel;
    // Minigame specific variables
    [Header("Minigame Variables")]
    [Space(5)]
    public GameObject minigamePanel;    
    public List<string> minigames; // List of names of scenes
    private Stack<string> playedMinigames; // Keeps track of minigames that have been played, clears when all have been played once
    private string selectedMinigame;
    public GameObject iconContainer;
    [SerializeField]
    private List<GameObject> icons;
    public GameObject countdownPopup; // Parent Image with a child Text displaying time before minigame loads
    private float timer = 3;
    private bool loadMinigame = false;

    // Board-space specific variables
    [Header("Map Variables")]
    [Space(5)]
    public GameObject mapPanel;    
    public GameObject spaceContainer; // Gameobject whose children are spaces on the board
    [SerializeField]
    private List<GameObject> spaces;

	// Use this for initialization
	void Start () 
    {
        // Show and hide panels for redundancy's sake
        menuPanel.SetActive(true);
        minigamePanel.SetActive(false);
        mapPanel.SetActive(false);

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
	}
	
	// Update is called once per frame
	void Update () 
    {
        // Clear the stack of played minigames if they have all been played
	    if(playedMinigames.Count == minigames.Count) {playedMinigames.Clear(); }

        if(loadMinigame)
        {
            timer -= Time.deltaTime;
            countdownPopup.GetComponentInChildren<Text>().text = "Minigame begins in: " + timer.ToString("F1");
            
            // Play the selected minigame
            if (timer <= 0) { SceneManager.LoadScene(selectedMinigame); }
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

        while(true)
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
}
