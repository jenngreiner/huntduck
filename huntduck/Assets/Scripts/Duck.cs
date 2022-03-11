using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BNG.Damageable))]
public class Duck : MonoBehaviour
{
    public int duckPoints = 500;
    public GameObject pointsTextObj;

    private GameObject player;

    public delegate void DuckDied(int points);
    public static event DuckDied onDuckDied;

    public bool dropsEggs;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
    }

    void OnEnable()
    {
        BNG.Damageable.onDuckDie += Die;
    }

    void OnDisable()
    {
        BNG.Damageable.onDuckDie -= Die;
    }

    public void Die(GameObject deadDuck)
    {
        if (deadDuck == gameObject)
        {
            onDuckDied?.Invoke(duckPoints); // subscribe in PlayerScore.cs

            CreatePointsText(duckPoints);
        }
    }

    public static void dropThaEgg()
    {
        Instantiate(DuckManager.instance.egg);
    }

    public void CreatePointsText(int duckPoints)
    {
        GameObject pointsObj = Instantiate(pointsTextObj, transform.position, Quaternion.identity);
        pointsObj.transform.LookAt(player.transform);
        Text pointsText = pointsObj.GetComponentInChildren<Text>();
        pointsText.text = "$" + duckPoints.ToString();
    }
}