using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * 
 */
public class MenuManager : MonoBehaviour {

    // Variables
    // Minigame specific variables
    public List<string> minigames; // List of names of scenes

    // Board-space specific variables
    public GameObject spaceContainer; // Gameobject whose children are spaces on the board
    [SerializeField]
    private List<GameObject> spaces;
    public GameObject menuPanel;
    public GameObject mapPanel;

	// Use this for initialization
	void Start () 
    {
        spaces = new List<GameObject>();

        // Add all of the spaces to the list of spaces
		foreach(Transform child in spaceContainer.transform)
        {
            spaces.Add(child.gameObject);
        }
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
}
