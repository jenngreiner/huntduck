using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BNG.Damageable))]
public class Duck : MonoBehaviour
{

    private bool _isDead = false;
    public bool isDead { get; protected set; }

    // override in inspector, otherwise set to 500
    public int duckPoints = 500;
    public Text duckPointsText;

    private const string PLAYER_TAG = "Player";
    private PlayerScore playerScoreScript;

    void Awake()
    {
        // set the kill points display equal to points duck is worth
        duckPointsText.text = "$" + duckPoints.ToString();
    }

    void Start()
    {
        playerScoreScript = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerScore>();
    }

    void OnEnable()
    {
        BNG.Damageable.onDuckDie += Die;
    }

    void OnDisable()
    {
        BNG.Damageable.onDuckDie -= Die;
    }

    public void Die()
    {
        isDead = true;
        Debug.Log(transform.name + " is DEAD!");

        // straighten out points, as long as not a "Carnival" duck
        if (gameObject.tag != TagManager.PRACTICEDUCK_TAG)
        {
            duckPointsText.transform.rotation = Quaternion.identity;
        }

        // MULTIPLAYER will need REFACTOR to know who killed the duck
        // since this duck was killed, add points to the score
        playerScoreScript.SendMessage("UpdatePlayerScore", duckPoints);
        Debug.Log("Duck died, player receives " + duckPoints + " duckpoints");
    }
}
