using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayLevel : MonoBehaviour
{
    private string thisScene;

    private void Start()
    {
        thisScene = SceneManager.GetActiveScene().name;
    }

    public void Replay()
    {
        StartCoroutine(LoadAsyncScene(thisScene));
    }

    void Update()
    {
        // Press the space key to start coroutine
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LoadAsyncScene(thisScene));
        }
    }

    IEnumerator LoadAsyncScene(string thisScene)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(thisScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}