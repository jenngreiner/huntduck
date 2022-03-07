using UnityEngine;

// maintain static publically accessible instances of ducks
// this class is a singleton (globally accessible class, only one instance). consider creating master singleton if using this service pattern with other managers https://gamedevbeginner.com/singletons-in-unity-the-right-way/

public class DuckManager : MonoBehaviour
{
    public static DuckManager instance { get; private set; }

    public GameObject normDuck;
    public GameObject fastDuck;
    public GameObject angryDuck;
    public GameObject bonusGeese;
    public GameObject goldenGoose;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}