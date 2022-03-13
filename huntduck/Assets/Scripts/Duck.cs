using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BNG.Damageable))]
public class Duck : MonoBehaviour
{
    public int duckPoints = 500;
    public GameObject pointsTextObj;
    public bool dropsEggs;
    private GameObject player;

    public delegate void DuckDied(int points);
    public static event DuckDied onDuckDied;


    void Start()
    {
        player = ObjectManager.instance.player;
    }

    void OnEnable()
    {
        StopBumps.onBump += dropThaEgg;
        BNG.Damageable.onDuckDie += Die;
    }

    void OnDisable()
    {
        StopBumps.onBump -= dropThaEgg;
        BNG.Damageable.onDuckDie -= Die;
    }

    public void dropThaEgg(Transform duck)
    {
        if (duck == transform && dropsEggs)
        {
            Instantiate(ObjectManager.instance.egg, transform.position, Quaternion.identity);
        }
    }

    public void Die(GameObject deadDuck)
    {
        if (deadDuck == gameObject)
        {
            onDuckDied?.Invoke(duckPoints); // subscribe in PlayerScore.cs

            CreatePointsText(duckPoints);
        }
    }

    public void CreatePointsText(int duckPoints)
    {
        GameObject pointsObj = Instantiate(pointsTextObj, transform.position, Quaternion.identity);
        pointsObj.transform.LookAt(player.transform);
        Text pointsText = pointsObj.GetComponentInChildren<Text>();
        pointsText.text = "$" + duckPoints.ToString();
    }
}