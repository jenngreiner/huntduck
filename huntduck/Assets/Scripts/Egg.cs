using UnityEngine;
using huntduck;

[RequireComponent(typeof(BNG.Damageable))]
public class Egg : MonoBehaviour
{
    public float eggSpeed = 2f;
    public float eggDamage = 34f;
    public AudioClip hitPlayerSound;

    public bool isHeatSeeking = false; 
    private bool isFrozen = false; 

    private BNG.Damageable thisDamageable;
    private PlayerData playerData;
    private Vector3 playerPosition;
    private Vector3 target;

    public delegate void EggHitPlayer();
    public static event EggHitPlayer onEggHitPlayer;

    void Awake()
    {
        playerData = ObjectManager.instance.player;
        playerPosition = playerData.controller.position;
        thisDamageable = GetComponent<BNG.Damageable>();
        target = playerPosition + (playerPosition - transform.position); // by default, target overshoots player position so egg flies past player
    }

    void OnEnable()
    {
        BNG.Damageable.onEggShot += EggSplode;
    }

    void OnDisable()
    {
        BNG.Damageable.onEggShot -= EggSplode;
    }

    void Update()
    {
        if (isHeatSeeking)
        {
            // when heat seeking, update target each frame to track player
            target = playerData.controller.position; 
        }
        if (!isFrozen)
        {
            MoveTowards(target);
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 newPos = Vector3.MoveTowards(transform.position, target, eggSpeed * Time.deltaTime);
        transform.position = newPos;
    }

    void EggSplode(GameObject thisEgg)
    {
        if (thisEgg == gameObject)
        {
            thisDamageable.DestroyThis();
            isFrozen = true; // egg has 2s destroy delay for particle effect, freeze its position once hits something so doesn't keep moving & double hit
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("egg collided with " + other.name);

        if (other.tag == TagManager.SHOOTINGSTAND_TAG)
        {
            Destroy(other.gameObject);
        }
        else if (other.tag == TagManager.PLAYER_TAG)
        {
            playerData.health -= eggDamage;
            onEggHitPlayer?.Invoke();
            if (playerData.health > 0)
            {
                BNG.VRUtils.Instance.PlaySpatialClipAt(hitPlayerSound, transform.position, 1f, 1f);
            }
            //Debug.Log("Did " + eggDamage + " eggDamage to player");
            //Debug.Log("PlayerHealth is now " + playerData.health);
        }

        EggSplode(gameObject); // destroy egg any time it hits anything
    }
}