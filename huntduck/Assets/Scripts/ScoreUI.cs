using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    // single player implementation
    private PlayerScore playerScript;
    private const string PLAYER_NAME = "Player";


    private void OnEnable()
    {
        // report player name and score on startup
        playerScript = GameObject.Find(PLAYER_NAME).GetComponent<PlayerScore>();
        Debug.Log(this.GetType().Name + " says that Player's name is " + playerScript.transform.name + " with a score of " + playerScript.playerScore);

        // set up points text element equal to the relevant data

    }
}
