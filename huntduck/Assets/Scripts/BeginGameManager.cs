using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// The BeginGameUI is set from WaveSpawner.cs
public class BeginGameManager : MonoBehaviour
{
    public Text beginGameText;
    public WaveSpawner practiceWaves;

    public void BeginGame(string GameMode)
    {
        Debug.Log("LET THE GAMES BEGIN!!");

        if (GameMode == "PracticeRange")
        {
            StartCoroutine(PracticeRangeIntro());

            // for now we just start practice waves, but this should be kicked off by something else - gun picked up or carnival ducks killed
            practiceWaves.enabled = true;
        }
    }

    IEnumerator PracticeRangeIntro()
    {
        beginGameText.text = "Welcome to the Practice Range!";
        yield return new WaitForSeconds(3);
        beginGameText.text = "Select your weapon to begin";
    }
}
