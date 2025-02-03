using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string startingScene;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadSceneAsync(startingScene);
    }
}
