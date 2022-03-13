using UnityEngine;

[RequireComponent(typeof(BNG.Damageable))]
public class Egg : MonoBehaviour
{
    public float eggSpeed = 2f;
    public float eggDamage = 34f;
    private bool eggHitPlayer = false;

    private PlayerHealth playerHealth;
    private GameObject playerController;
    private BNG.Damageable damageable;

    void Start()
    {
        playerHealth = ObjectManager.instance.player.GetComponent<PlayerHealth>();
        playerController = ObjectManager.instance.playerController; // child of player. this is the transform that moves when the player moves.
        damageable = transform.GetComponent<BNG.Damageable>();
    }

    void Update()
    {       
        if (transform.position == playerController.transform.position)
        {
            if (!eggHitPlayer)
            {
                eggHitPlayer = true;
                playerHealth.playerHealth -= eggDamage;
                damageable.DestroyThis();
                Debug.Log("Did " + eggDamage + " eggDamage to player");
                Debug.Log("PlayerHealth is now " + playerHealth.playerHealth);
            }
        }
        else
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, playerController.transform.position, eggSpeed * Time.deltaTime);
            transform.position = newPos;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("egg collided with " + other.name);
        damageable.DestroyThis();
    }
}