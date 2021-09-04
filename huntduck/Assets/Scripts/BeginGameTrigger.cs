using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginGameTrigger : MonoBehaviour
{
    public BeginGameManager beginGameManager;

    private void OnTriggerEnter(Collider other)
    {
        // begin the game when player touches trigger, if game hasn't already started
        if (other.tag == "Player" && !BeginGameManager.isGameStarted)
        {
            // Begin the game
            beginGameManager.BeginGame("PracticeRange");            
        }
    }


}
