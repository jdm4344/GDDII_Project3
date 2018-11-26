using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMasher : MonoBehaviour {

    // keep track of how many times the players have hit the buttons
    int[] buttonPresses = new int[4];

    // how many to win?
    const int pressesToWin = 100;

    // Use this for initialization
    void Start ()
    {
        // p1ButtonPresses = 0;
        // p2ButtonPresses = 0;
        // p3ButtonPresses = 0;
        // p4ButtonPresses = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // adds to the button count of the player who pressed it
    void ButtonPressed(int playerNumber)
    {

    }

    void CheckWinCondition()
    {

    }
}
