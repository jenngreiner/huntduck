using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using BNG;

#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
using Invector;
#endif

public class NetworkDamageable : MonoBehaviourPun
{
    public float Health = 5;
    private float _startingHealth;

    public GameObject SpawnOnDeath; // If specified, this GameObject will be instantiated at this transform's position on death
    public List<GameObject> ActivateGameObjectsOnDeath; // Activate these GameObjects on Death
    public List<GameObject> DeactivateGameObjectsOnDeath; // Deactivate these GameObjects on Death
    public List<Collider> DeactivateCollidersOnDeath; // Deactivate these Colliders on Death
    public bool DestroyOnDeath = true; // Destroy this object on Death? False if need to respawn.
    public bool DropOnDeath = true; //If this object is a Grabbable it can be dropped on Death
    public float DestroyDelay = 0f; // How long to wait before destroying this objects
    public bool Respawn = false; // If true the object will be reactivated according to RespawnTime
    public float RespawnTime = 10f; // If Respawn true, this gameObject will reactivate after RespawnTime. In seconds.
    public bool RemoveBulletHolesOnDeath = true; // Remove any decals that were parented to this object on death. Useful for clearing unused decals.

    [Header("Events")]
    public FloatEvent onDamaged; // Optional Event to be called when receiving damage. Takes damage amount as a float parameter.
    public UnityEvent onDestroyed; // Optional Event to be called once health is <= 0

    // HD specific delegate events
    public delegate void DestroyDelegate();
    public static event DestroyDelegate onDestroyedDelegate;
    public UnityEvent onRespawn; // Optional Event to be called once the object has been respawned, if Respawn is true and after RespawnTime

    public delegate void TargetHit(GameObject target);
    public static event TargetHit onTargetHit;

    public delegate void InfiniteDuckHit();
    public static event InfiniteDuckHit onInfiniteDuckHit;

    public delegate void BonusGooseHit();
    public static event BonusGooseHit onBonusGooseHit;

    public delegate void EggShot(GameObject thisEgg);
    public static event EggShot onEggShot;

    public delegate void DuckDie(GameObject deadDuck);
    public static event DuckDie onDuckDie;


#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
        // Invector damage integration
        [Header("Invector Integration")]
        [Tooltip("If true, damage data will be sent to Invector object using 'ApplyDamage'")]
        public bool SendDamageToInvector = true;
#endif

    bool destroyed = false;

    Rigidbody rigid;
    bool initialWasKinematic;

    private void Start()
    {
        _startingHealth = Health;
        rigid = GetComponent<Rigidbody>();
        if (rigid)
        {
            initialWasKinematic = rigid.isKinematic;
        }
    }

    void Update()
    {
        // kill key
        if (Input.GetKeyDown(KeyCode.K))
        {
            this.DealDamage(99999);
            Debug.Log("FEEL MY WRATH, K");
        }
    }

    [PunRPC]
    public virtual void DealDamage(float damageAmount)
    {
        this.DealDamage(damageAmount, transform.position);
    }

    [PunRPC]
    public virtual void RPC_DealDamage(float damageAmount, Vector3? hitPosition = null, Vector3? hitNormal = null, bool reactToHit = true, int senderViewID = default, int receiverViewID = default)
    {
        GameObject sender = PhotonView.Find(senderViewID).gameObject;
        GameObject receiver = PhotonView.Find(receiverViewID).gameObject;

        if (destroyed)
        {
            Debug.Log("We destroyed that breakable called " + transform.name);
            return;
        }

        Health -= damageAmount;

        onDamaged?.Invoke(damageAmount);

        // Invector Integration
#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
            if(SendDamageToInvector) {
                var d = new Invector.vDamage();
                d.hitReaction = reactToHit;
                d.hitPosition = (Vector3)hitPosition;
                d.receiver = receiver == null ? this.gameObject.transform : null;
                d.damageValue = (int)damageAmount;

                this.gameObject.ApplyDamage(new Invector.vDamage(d));
            }
#endif

        if (Health <= 0)
        {
            //photonView.RPC("RPC_DestroyThis", RpcTarget.All);
            //photonView.RPC("RPC_BroadcastHit", RpcTarget.All);

            DestroyThis();
            BroadcastHit(); // runs switch statement against object tag
        }
    }

    public virtual void DealDamage(float damageAmount, Vector3? hitPosition = null, Vector3? hitNormal = null, bool reactToHit = true, GameObject sender = null, GameObject receiver = null)
    {

        if (destroyed)
        {
            Debug.Log("We destroyed that breakable called " + transform.name);
            return;
        }

        Health -= damageAmount;

        onDamaged?.Invoke(damageAmount);

        // Invector Integration
#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
            if(SendDamageToInvector) {
                var d = new Invector.vDamage();
                d.hitReaction = reactToHit;
                d.hitPosition = (Vector3)hitPosition;
                d.receiver = receiver == null ? this.gameObject.transform : null;
                d.damageValue = (int)damageAmount;

                this.gameObject.ApplyDamage(new Invector.vDamage(d));
            }
#endif

        if (Health <= 0)
        {
            //photonView.RPC("RPC_DestroyThis", RpcTarget.All);
            //photonView.RPC("RPC_BroadcastHit", RpcTarget.All);

            DestroyThis();
            BroadcastHit(); // runs switch statement against object tag
        }
    }

    public virtual void DestroyThis()
    {
        Health = 0;
        destroyed = true;

        // Activate
        foreach (var go in ActivateGameObjectsOnDeath)
        {
            go.SetActive(true);
        }

        // Deactivate
        foreach (var go in DeactivateGameObjectsOnDeath)
        {
            go.SetActive(false);
        }

        // Colliders
        foreach (var col in DeactivateCollidersOnDeath)
        {
            col.enabled = false;
        }

        // Spawn object
        if (SpawnOnDeath != null)
        {
            var go = GameObject.Instantiate(SpawnOnDeath);
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;
        }

        // Force to kinematic if rigid present
        if (rigid)
        {
            rigid.isKinematic = true;
        }

        // Invoke Callback Event
        if (onDestroyed != null)
        {
            onDestroyed.Invoke();
        }

        onDestroyedDelegate?.Invoke();

        if (DestroyOnDeath)
        {
            Invoke(nameof(PhotonDelayedDestroyRoot), DestroyDelay);
            //PhotonNetwork.Destroy(transform.parent.gameObject);
            //Destroy(transform.root.GetComponent<PhotonView>());
            //Destroy(this.transform.parent.gameObject, DestroyDelay);
        }
        else if (Respawn)
        {
            StartCoroutine(RespawnRoutine(RespawnTime));
        }

        // Drop this if the player is holding it
        Grabbable grab = GetComponent<Grabbable>();
        if (DropOnDeath && grab != null && grab.BeingHeld)
        {
            grab.DropItem(false, true);
        }

        // Remove an decals that may have been parented to this object
        if (RemoveBulletHolesOnDeath)
        {
            BulletHole[] holes = GetComponentsInChildren<BulletHole>();
            foreach (var hole in holes)
            {
                Destroy(hole.gameObject);
            }

            Transform decal = transform.Find("Decal");
            if (decal)
            {
                Destroy(decal.gameObject);
            }
        }
    }

    void PhotonDelayedDestroyRoot()
    {
        PhotonNetwork.Destroy(transform.root.gameObject);
    }

    // SINGLESCENE: Reset elements during practice mode for "Play Again"
    public void InstantRespawn()
    {
        Debug.Log("Calling instant respawn on " + transform.name);

        Health = _startingHealth;
        destroyed = false;

        // Deactivate
        foreach (var go in ActivateGameObjectsOnDeath)
        {
            go.SetActive(false);
            Debug.Log("Should be turning off " + go.name + " as part of respawn");
        }

        // Re-Activate
        foreach (var go in DeactivateGameObjectsOnDeath)
        {
            go.SetActive(true);
            Debug.Log("Should be turning on " + go.name + " as part of respawn");
        }
        foreach (var col in DeactivateCollidersOnDeath)
        {
            col.enabled = true;
        }

        // Reset kinematic property if applicable
        if (rigid)
        {
            rigid.isKinematic = initialWasKinematic;
        }

        // Call events
        if (onRespawn != null)
        {
            onRespawn?.Invoke();
        }
    }

    public void RespawnObject(float seconds)
    {
        StartCoroutine(RespawnRoutine(seconds));
    }

    IEnumerator RespawnRoutine(float seconds)
    {

        yield return new WaitForSeconds(seconds);

        Health = _startingHealth;
        destroyed = false;

        // Deactivate
        foreach (var go in ActivateGameObjectsOnDeath)
        {
            go.SetActive(false);
            Debug.Log("Should be turning off " + go.name + " as part of respawn");
        }

        // Re-Activate
        foreach (var go in DeactivateGameObjectsOnDeath)
        {
            go.SetActive(true);
            Debug.Log("Should be turning on " + go.name + " as part of respawn");
        }
        foreach (var col in DeactivateCollidersOnDeath)
        {
            col.enabled = true;
        }

        // Reset kinematic property if applicable
        if (rigid)
        {
            rigid.isKinematic = initialWasKinematic;
        }

        // Call events
        if (onRespawn != null)
        {
            onRespawn?.Invoke();
        }
    }

    void BroadcastHit()
    {
        switch (gameObject.tag)
        {
            case TagManager.TARGET_TAG:
                onTargetHit?.Invoke(gameObject);
                break;
            case TagManager.INFINITEDUCK_TAG:
            case TagManager.ANGRYDUCK_TAG:
            case TagManager.GOOSE_TAG:
            case TagManager.GOLDENGOOSE_TAG:
                onInfiniteDuckHit?.Invoke();
                onDuckDie?.Invoke(gameObject);
                break;
            case TagManager.EGG_TAG:
                onEggShot?.Invoke(gameObject);
                break;
            default:
                break;
        }
    }
}
