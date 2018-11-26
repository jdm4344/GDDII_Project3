﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMasher : MonoBehaviour {

    // how many players, get this from the main manager
    public int numOfPlayers;

    // names of the players, in order from p1 through p4
    public string[] playerNames;

    // keep track of how many times the players have hit the buttons
    public int[] buttonPresses;

    // percentage complete
    public int[] percentages;

    // how many to win?
    public const int pressesToWin = 100;

    // is the game over
    public bool gameOver;

    // the winner
    public int winner;

    // names of players and their fame earned from this minigame
    public Dictionary<string, int> results = new Dictionary<string, int>();

    // access to the player manager script on the scene manager object
    public PlayerManager playerManager;

    void Awake()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    // Use this for initialization
    void Start ()
    {
        // how many players are playing?
        numOfPlayers = playerManager.playerNames.Length;
        // setup
        buttonPresses = new int[numOfPlayers];
        playerNames = new string[numOfPlayers];
        percentages = new int[numOfPlayers];
        gameOver = false;
        // get player names
        for (int i = 0; i < numOfPlayers; i++)
        {
            playerNames[i] = playerManager.playerNames[i];
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!gameOver)
        {
            // check for button presses from every player in the game
            ButtonPressed();

            // check for a win
            winner = CheckWinCondition() - 1;

            DisplayPercentage();
        }
        DisplayWinner();
	}

    // show the winner
    void DisplayWinner()
    {
        GUI.TextArea(new Rect(0f, 3f, 6f, 2f), "Player " + playerNames[winner] + " has won!");
    }

    // draw the percentages on screen
    void DisplayPercentage()
    {
        for (int i = 0; i < numOfPlayers; i++)
        {
            // make a new text box, shifts over for each player
            GUI.TextArea(new Rect(-6f + 4f * i, 2f, 3f, 1f), percentages[i] + "%");
        }
    }

    // adds to the button count of the player who pressed it
    void ButtonPressed()
    {
        // loop through each player in the game
        for (int i = 1; i < numOfPlayers + 1; i++)
        {
            // check if the A button has been pressed this frame (NOT a hold)
            if (Input.GetButtonDown(i + "A"))
                buttonPresses[i]++; // adds to the count
        }

        for (int i = 0; i < numOfPlayers; i++)
        {
            percentages[i] = buttonPresses[i] / pressesToWin * 100;
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
            results.Add(playerNames[i], CalcFameEarned(buttonPresses[i]));
        }
    }

    // helper function, returns how much fame is earned
    int CalcFameEarned(int buttonPresses)
    {
        // gives a value between 0 and 10, not linear, based on score
        return (buttonPresses * buttonPresses) / (pressesToWin * pressesToWin) * 10;
    }
}