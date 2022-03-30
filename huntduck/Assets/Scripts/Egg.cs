using UnityEngine;
using huntduck;

[RequireComponent(typeof(BNG.Damageable))]
public class Egg : MonoBehaviour
{
    public float eggSpeed = 2f;
    public float eggDamage = 34f;
    public bool isHeatSeeking = false; // not heat seaking by default, but can flip on in inspector
    public bool isFrozen = false; // just for testing right now, although interesting to think through how we use in gameplay
    private bool eggHitPlayer = false;
    private Vector3 playerPositionAtLaunch;

    private BNG.Damageable thisDamageable;
    private PlayerData playerData;

    void Start()
    {
        playerData = ObjectManager.instance.player;
        thisDamageable = GetComponent<BNG.Damageable>();
        playerPositionAtLaunch = playerData.controller.position;
    }

    void OnEnable()
    {
        BNG.Damageable.onEggHit += eggDie;
    }

    void OnDisable()
    {
        BNG.Damageable.onEggHit -= eggDie;
    }

    void Update()
    {       
        if (transform.position == playerData.controller.position)
        {
            if (!eggHitPlayer)
            {
                eggHitPlayer = true;
                eggDie();
            }
        }
        else if (isHeatSeeking)
        {
            moveTowards(playerData.controller.position);
        }
        else if (!isFrozen)
        {
            moveTowards(playerPositionAtLaunch);
        }
    }

    void moveTowards(Vector3 target)
    {
        Vector3 newPos = Vector3.MoveTowards(transform.position, target, eggSpeed * Time.deltaTime);
        transform.position = newPos;
    }

    void eggDie()
    {
        playerData.health -= eggDamage;
        thisDamageable.DestroyThis();
        Debug.Log("Did " + eggDamage + " eggDamage to player");
        Debug.Log("PlayerHealth is now " + playerData.health);
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