using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BNG.Damageable))]
public class Duck : MonoBehaviour
{
    public int duckPoints = 500;
    public GameObject pointsTextObj;

    private GameObject player;
    private PlayerScore playerScoreScript;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);
        playerScoreScript = player.GetComponent<PlayerScore>();
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
            playerScoreScript.SendMessage("UpdatePlayerScore", duckPoints);
            Debug.Log("Duck died, player receives " + duckPoints + " duckpoints");

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