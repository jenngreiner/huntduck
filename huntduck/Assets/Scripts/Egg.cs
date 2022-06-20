using UnityEngine;
using huntduck;

[RequireComponent(typeof(BNG.Damageable))]
public class Egg : MonoBehaviour
{
    public float eggSpeed = 2f;
    public int eggDamage = 1;
    public AudioClip hitPlayerSound;

    public bool isHeatSeeking = false; 
    private bool isFrozen = false; 

    private BNG.Damageable thisDamageable;
    private PlayerData player;
    private Vector3 playerPosition;
    private Vector3 target;

    public delegate void EggHitPlayer(int damage);
    public static event EggHitPlayer onEggHitPlayer;

    void Awake()
    {
        player = ObjectManager.instance.player;
        playerPosition = player.controller.position;
        thisDamageable = GetComponent<BNG.Damageable>();
        target = playerPosition + (playerPosition - transform.position); // egg overshoots player
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
            target = player.controller.position; 
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
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagManager.SHOOTINGSTAND_TAG)
        {
            Destroy(other.gameObject);
        }
        else if (other.tag == TagManager.PLAYER_TAG)
        {
            DamagePlayer(eggDamage);
        }

        EggSplode(gameObject); // destroy egg any time it hits anything
    }

    void DamagePlayer(int damage)
    {
        onEggHitPlayer?.Invoke(damage);

        // play this "ouch" sound as long as not dead (that's a dif sound)
        if (player.maxHealth > 0)
        {
            BNG.VRUtils.Instance.PlaySpatialClipAt(hitPlayerSound, transform.position, 1f, 1f);
        }
    }
}