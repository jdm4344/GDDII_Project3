using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMasher : MonoBehaviour {

    // how many players, get this from the main manager
    public int numOfPlayers;

    // names of the players, in order from p1 through p4
    public string[] playerNames;

    // keep track of how many times the players have hit the buttons
    public int[] buttonPresses;

    // how many to win?
    public const int pressesToWin = 100;

    // is the game over
    public bool gameOver;

    // names of players and their fame earned from this minigame
    public Dictionary<string, int> results = new Dictionary<string, int>();

    // Use this for initialization
    void Start ()
    {
        //numOfPlayers = 
        buttonPresses = new int[numOfPlayers];
        playerNames = new string[numOfPlayers];
        gameOver = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!gameOver)
        {
            // check for button presses from every player in the game
            ButtonPressed();

            // check for a win
            CheckWinCondition();
        }        
	}

    // adds to the button count of the player who pressed it
    void ButtonPressed()
    {
        // loop through each player in the game
        for (int i = 0; i < numOfPlayers; i++)
        {
            // check if the A button has been pressed this frame (NOT a hold)
            if (Input.GetButtonDown(i + "A"))
                buttonPresses[i]++; // adds to the count
        }
    }

    // returns the player number of the player who won the game, 0 if nobody has won yet
    int CheckWinCondition()
    {
        // check each value
        for (int i = 0; i < numOfPlayers; i++)
        {
            if (buttonPresses[i] >= pressesToWin)
            {
                // end the game
                gameOver = true;
                // make the leaderboard with values for fame
                ConstructLeaderboard();
                // returns who won
                return i + 1;
            }
        }
        // no winner yet
        return 0;
    }

    // sets the values for the dictionary with the player names and the fame they earned
    void ConstructLeaderboard()
    { 
        // keeps the order of players recieved from main manager
        for (int i = 0; i < numOfPlayers; i++)
        {
            // add to the dictionary
            results.Add(playerNames[i - 1], CalcFameEarned(buttonPresses[i - 1]));
        }
    }

    // helper function, returns how much fame is earned
    int CalcFameEarned(int buttonPresses)
    {
        // gives a value between 0 and 10, not linear, based on score
        return (buttonPresses * buttonPresses) / (pressesToWin * pressesToWin) * 10;
    }
}
