using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;

    public void gotoLevel()
    {
     StartCoroutine(GoToLevelAsync(nextLevel));
     Debug.Log("GTL going to: " + nextLevel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("KeyDown.G: Go to next level");
            StartCoroutine(GoToLevelAsync(nextLevel));
            // SceneManager.LoadScene(nextLevel);
        }
    }

    IEnumerator GoToLevelAsync(string nextLevel)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel);
        Debug.Log("GTLA going to: " + nextLevel);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
