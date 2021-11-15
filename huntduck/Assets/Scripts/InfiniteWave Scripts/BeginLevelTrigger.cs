using UnityEngine;

public class BeginLevelTrigger : MonoBehaviour
{
    public static bool isGameStarted = false;

    public InfiniteLevelManager infiniteLevelManager;

    void OnTriggerEnter(Collider other)
    {
        // begin the game when player touches trigger, if game hasn't already started
        if (other.tag == "Player" && !isGameStarted)
        {
            // Begin the game
            //infiniteLevelManager.StartIntro();
            isGameStarted = true;
            Debug.Log("Started the game");
        }
    }
}
