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
        duckPointsText.text = duckPoints.ToString();
    }

    void Start()
    {
        playerScoreScript = GameObject.FindGameObjectWithTag(PLAYER_TAG).GetComponent<PlayerScore>();
    }


    public void Die()
    {
        isDead = true;
        Debug.Log(transform.name + " is DEAD!");

        // straighten out points, as long as not a "Carnival" duck
        if (gameObject.layer != LayerMask.NameToLayer("Carnival"))
        {
            duckPointsText.transform.rotation = Quaternion.identity;
        }

        // MULTIPLAYER will need REFACTOR to know who killed the duck
        // since this duck was killed, add points to the score
        playerScoreScript.SendMessage("UpdatePlayerScore", duckPoints);
        Debug.Log("Duck died, player receives " + duckPoints + " duckpoints");
    }

    // reliese on DUCK_DURATION && ROUNDS
    //public void FlyAway()
    //{
    //    if (!isDead && Rounds.Duration <= 0)
    //    {
    //        //duck flies off screen
    //        DecrementDuck();
    //    }
    //}

    // reliese on ROUNDS
    //public void DecrementDucks()
    //{
    //    Rounds.ducksLeft--;

    //    if (Rounds.ducksLeft <= 0)
    //    {
    //        // end round
    //    };
    //}

    // OLD SCRIPT BELOW

    //[SerializeField]
    //private int maxHealth = 100;
    //private int currentHealth;



    //void Awake()
    //{
    //    ////SetDefaults();
    //}

    //public void TakeDamage(int _amount)
    //{
    //    if (isDead)
    //    {
    //        return;
    //    }

    //    currentHealth -= _amount;

    //    Debug.Log(transform.name + " now has " + currentHealth + " health.");

    //    if (currentHealth <= 0)
    //    {
    //        Die();
    //    }
    //}

    //public void Die()
    //{
    //    isDead = true;
    //    Debug.Log(transform.name + " is DEAD!");

    //}

    //public void SetDefaults()
    //{
    //    currentHealth = maxHealth;
    //}
}
