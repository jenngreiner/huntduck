using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;

    public void goToLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
