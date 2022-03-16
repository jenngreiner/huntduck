using UnityEngine;
using huntduck;

[RequireComponent(typeof(BNG.Damageable))]
public class Egg : MonoBehaviour
{
    public float eggSpeed = 2f;
    public float eggDamage = 34f;
    private bool eggHitPlayer = false;

    private PlayerHealth playerHealth;
    private Vector3 playerPosition;
    private BNG.Damageable thisDamageable;

    void Start()
    {
        Player player = ObjectManager.instance.player;
        playerHealth = player.PlayerHealth;
        playerPosition = player.PlayerController.position;
        thisDamageable = GetComponent<BNG.Damageable>();
    }

    void Update()
    {       
        if (transform.position == playerPosition)
        {
            if (!eggHitPlayer)
            {
                eggHitPlayer = true;
                playerHealth.playerHealth -= eggDamage;
                thisDamageable.DestroyThis();
                Debug.Log("Did " + eggDamage + " eggDamage to player");
                Debug.Log("PlayerHealth is now " + playerHealth.playerHealth);
            }
        }
        else
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, playerPosition, eggSpeed * Time.deltaTime);
            transform.position = newPos;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("egg collided with " + other.name);
        if (other.tag == TagManager.SHOOTINGSTAND_TAG)
        {
            Destroy(other.gameObject);
        }
        thisDamageable.DestroyThis();
    }
}