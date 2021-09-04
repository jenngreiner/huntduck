using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginGameTrigger : MonoBehaviour
{
    public BeginGameManager beginGameManager;

    // we begin the game when the collider is entered by the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Begin the game
            beginGameManager.theGameBegins("PracticeRange");            
        }
    }


}
