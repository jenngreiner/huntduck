using UnityEngine;

public class BeginTargetTrigger : MonoBehaviour
{
    public static bool isTargetReady = false;
    public static bool isTargetStarted = false;

    public PracticeRangeManager practiceRangeManager;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("BeginTargetTrigger has been triggered");
        // begin the game when player touches trigger, if game hasn't already started
        if (other.tag == "Player" && isTargetReady && !isTargetStarted)
        {
            // Begin the target round
            practiceRangeManager.StartTargetRound();
            isTargetStarted = true;
            Debug.Log("Starting Target Round");
        }
    }
}