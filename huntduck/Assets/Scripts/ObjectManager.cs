using UnityEngine;

// maintain static publically accessible instances of ducks
// this class is a singleton (globally accessible class, only one instance). consider creating master singleton if using this service pattern with other managers https://gamedevbeginner.com/singletons-in-unity-the-right-way/

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance { get; private set; }

    [Header("Ducks")]
    public GameObject normDuck;
    public GameObject fastDuck;
    public GameObject angryDuck;
    public GameObject bonusGeese;
    public GameObject goldenGoose;

    public GameObject egg;

    [Header("Environment")]
    public GameObject playerArea;

    [Header("Player")]
    public GameObject player;
    public GameObject playerController;


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