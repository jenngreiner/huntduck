using UnityEngine;
using huntduck;

[RequireComponent(typeof(BNG.Damageable))]
public class Egg : MonoBehaviour
{
    public float eggSpeed = 2f;
    public float eggDamage = 34f;
    private bool eggHitPlayer = false;

    private BNG.Damageable thisDamageable;
    private PlayerData playerData;

    void Start()
    {
        playerData = ObjectManager.instance.player;
        thisDamageable = GetComponent<BNG.Damageable>();
    }

    void Update()
    {       
        if (transform.position == playerData.controller.position)
        {
            if (!eggHitPlayer)
            {
                eggHitPlayer = true;
                playerData.health -= eggDamage;
                thisDamageable.DestroyThis();
                Debug.Log("Did " + eggDamage + " eggDamage to player");
                Debug.Log("PlayerHealth is now " + playerData.health);
            }
        }
        else
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, playerData.controller.position, eggSpeed * Time.deltaTime);
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