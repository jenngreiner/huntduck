using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextLevel;

public void Update(){
    if (Input.GetKeyDown(KeyCode.L)){

        SceneManager.LoadScene(nextLevel);
    }
}
    public void goToLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
