using UnityEngine;

// maintain publically accessible instances of objects to use throughout the game
// this class is a singleton (globally accessible class, only one instance). consider creating master singleton if using this service pattern with other managers https://gamedevbeginner.com/singletons-in-unity-the-right-way/

public class ObjectManager : MonoBehaviour
{
    // Singleton
    public static ObjectManager instance { get; private set; }

    [Header("Ducks")]
    public GameObject normDuck;
    public GameObject fastDuck;
    public GameObject angryDuck;
    public GameObject bonusGeese;
    public GameObject goldenGoose;

    [Header("Eggs")]
    public GameObject egg;

    [Header("Environment")]
    public GameObject playerArea;

    [Header("Player")]
    // TODO: consolidate various player properties into a single player script
    // TODO: extend player properties through Scriptable Objects
    public GameObject player;
    public GameObject playerController;
    //public PlayerScore playerScore { get; private set; }
    //public PlayerHealth playerHealth { get; private set; }

    // TODO: switch to list of players for multiplayer. one possibility below
    //public List<GameObject> players = new List<GameObject>()



    void Awake()
    {
        // if there is an instance, and its not me, delete me
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