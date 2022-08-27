using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("KeyDown.H: Go to Hunt Mode");
            StartCoroutine(GoToLevelAsync(nextLevel));

        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("KeyDown.M: Go to Multiplayer Mode");
            StartCoroutine(GoToLevelAsync(nextLevel));

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(GoToLevelAsync(nextLevel));
            // SceneManager.LoadScene(nextLevel);
            Debug.Log("KeyDown.P: Go to Practice Mode: " + nextLevel);
        }
    }

    public void gotoLevel()
    {
        StartCoroutine(GoToLevelAsync(nextLevel));
        Debug.Log("Go to next level: " + nextLevel);
    }

    IEnumerator GoToLevelAsync(string nextLevel)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel);
        Debug.Log("Async going to: " + nextLevel);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
