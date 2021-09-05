using UnityEngine;

public class BeginPracticeTrigger : MonoBehaviour
{
    public static bool isGameStarted = false;

    public PracticeRangeManager practiceRangeManager;

    void OnTriggerEnter(Collider other)
    {
        // begin the game when player touches trigger, if game hasn't already started
        if (other.tag == "Player" && !isGameStarted)
        {
            // Begin the game
            practiceRangeManager.BeginGame("PracticeRange");
            isGameStarted = true;
        }
    }


}
