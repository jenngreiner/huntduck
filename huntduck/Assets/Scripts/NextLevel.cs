using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(nextLevel);
        }
    }

    public void goToLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
