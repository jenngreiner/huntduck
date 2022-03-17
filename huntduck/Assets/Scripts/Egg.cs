using UnityEngine;
using huntduck;

[RequireComponent(typeof(BNG.Damageable))]
public class Egg : MonoBehaviour
{
    public float eggSpeed = 2f;
    public float eggDamage = 34f;
    private bool eggHitPlayer = false;

    private BNG.Damageable thisDamageable;
    private Player player;

    void Start()
    {
        player = ObjectManager.instance.player;
        thisDamageable = GetComponent<BNG.Damageable>();
    }

    void Update()
    {       
        if (transform.position == player.controller.position)
        {
            if (!eggHitPlayer)
            {
                eggHitPlayer = true;
                player.health -= eggDamage;
                thisDamageable.DestroyThis();
                Debug.Log("Did " + eggDamage + " eggDamage to player");
                Debug.Log("PlayerHealth is now " + player.health);
            }
        }
        else
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, player.controller.position, eggSpeed * Time.deltaTime);
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