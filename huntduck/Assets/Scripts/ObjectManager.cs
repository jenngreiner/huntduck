using UnityEngine;
using huntduck;

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
    public Player player;


    // TODO: THINK THROUGH MULTIPLAYER PLAYERS
    // 1) switch to list of players for multiplayer:
        // public List<GameObject> players = new List<GameObject>()
    // 2) use a dictionary as second layer of abstraction



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