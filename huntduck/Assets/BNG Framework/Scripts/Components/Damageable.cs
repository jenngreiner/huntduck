using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
using Invector;
#endif

namespace BNG {
    /// <summary>
    /// A basic damage implementation. Call a function on death. Allow for respawning.
    /// </summary>
    public class Damageable : MonoBehaviour {

        public float Health = 5;
        private float _startingHealth;

        public delegate void TargetHit(GameObject target);
        public static event TargetHit onTargetHit;

        public delegate void ClayHit();
        public static event ClayHit onClayHit;

        public delegate void CarniDuckHit(GameObject carniDuck);
        public static event CarniDuckHit onCarniDuckHit;

        public delegate void InfiniteDuckHit();
        public static event InfiniteDuckHit onInfiniteDuckHit;

        public delegate void DuckDie();
        public static event DuckDie onDuckDie;

        [Tooltip("If specified, this GameObject will be instantiated at this transform's position on death.")]
        public GameObject SpawnOnDeath;

        [Tooltip("Activate these GameObjects on Death")]
        public List<GameObject> ActivateGameObjectsOnDeath;

        [Tooltip("Deactivate these GameObjects on Death")]
        public List<GameObject> DeactivateGameObjectsOnDeath;

        [Tooltip("Deactivate these Colliders on Death")]
        public List<Collider> DeactivateCollidersOnDeath;

        /// <summary>
        /// Destroy this object on Death? False if need to respawn.
        /// </summary>
        [Tooltip("Destroy this object on Death? False if need to respawn.")]
        public bool DestroyOnDeath = true;

        [Tooltip("If this object is a Grabbable it can be dropped on Death")]
        public bool DropOnDeath = true;

        /// <summary>
        /// How long to wait before destroying this objects
        /// </summary>
        [Tooltip("How long to wait before destroying this objects")]
        public float DestroyDelay = 0f;

        /// <summary>
        /// If true the object will be reactivated according to RespawnTime
        /// </summary>
        [Tooltip("If true the object will be reactivated according to RespawnTime")]
        public bool Respawn = false;

        /// <summary>
        /// If Respawn true, this gameObject will reactivate after RespawnTime. In seconds.
        /// </summary>
        [Tooltip("If Respawn true, this gameObject will reactivate after RespawnTime. In seconds.")]
        public float RespawnTime = 10f;

        /// <summary>
        /// Remove any decals that were parented to this object on death. Useful for clearing unused decals.
        /// </summary>
        [Tooltip("Remove any decals that were parented to this object on death. Useful for clearing unused decals.")]
        public bool RemoveBulletHolesOnDeath = true;

        [Header("Events")]
        [Tooltip("Optional Event to be called when receiving damage. Takes damage amount as a float parameter.")]
        public FloatEvent onDamaged;

        [Tooltip("Optional Event to be called once health is <= 0")]
        public UnityEvent onDestroyed;

        public delegate void DestroyDelegate();
        public static event DestroyDelegate onDestroyedDelegate;

        [Tooltip("Optional Event to be called once the object has been respawned, if Respawn is true and after RespawnTime")]
        public UnityEvent onRespawn;

#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
        // Invector damage integration
        [Header("Invector Integration")]
        [Tooltip("If true, damage data will be sent to Invector object using 'ApplyDamage'")]
        public bool SendDamageToInvector = true;
#endif

        bool destroyed = false;

        Rigidbody rigid;
        bool initialWasKinematic;

        private void Start() {
            _startingHealth = Health;
            rigid = GetComponent<Rigidbody>();
            if (rigid) {
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

        public virtual void DealDamage(float damageAmount) {
            DealDamage(damageAmount, transform.position);
        }

        public virtual void DealDamage(float damageAmount, Vector3? hitPosition = null, Vector3? hitNormal = null, bool reactToHit = true, GameObject sender = null, GameObject receiver = null) {

            if (destroyed) {
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

            if (Health <= 0) {
                DestroyThis();

                // runs switch statement against object tag
                broadcastHit();
            }
        }


        public virtual void DestroyThis() {
            Health = 0;
            destroyed = true;

            // Activate
            foreach (var go in ActivateGameObjectsOnDeath) {
                go.SetActive(true);
            }

            // Deactivate
            foreach (var go in DeactivateGameObjectsOnDeath) {
                go.SetActive(false);
            }

            // Colliders
            foreach (var col in DeactivateCollidersOnDeath) {
                col.enabled = false;
            }

            // Spawn object
            if (SpawnOnDeath != null) {
                var go = GameObject.Instantiate(SpawnOnDeath);
                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;
            }

            // Force to kinematic if rigid present
            if (rigid) {
                rigid.isKinematic = true;
            }

            // Invoke Callback Event
            if (onDestroyed != null) {
                onDestroyed.Invoke();
            }

            onDestroyedDelegate?.Invoke();

            if (DestroyOnDeath)
            {
                Destroy(this.gameObject, DestroyDelay);
                //Destroy(this.transform.parent.gameObject, DestroyDelay);
            }
            else if (Respawn)
            {
                StartCoroutine(RespawnRoutine(RespawnTime));
            }

            // Drop this if the player is holding it
            Grabbable grab = GetComponent<Grabbable>();
            if (DropOnDeath && grab != null && grab.BeingHeld) {
                grab.DropItem(false, true);
            }

            // Remove an decals that may have been parented to this object
            if (RemoveBulletHolesOnDeath) {
                BulletHole[] holes = GetComponentsInChildren<BulletHole>();
                foreach (var hole in holes) {
                    GameObject.Destroy(hole.gameObject);
                }

                Transform decal = transform.Find("Decal");
                if (decal) {
                    GameObject.Destroy(decal.gameObject);
                }
            }
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
                onRespawn.Invoke();
            }
        }

        public void RespawnObject(float seconds)
        {
            StartCoroutine(RespawnRoutine(seconds));
        }

        IEnumerator RespawnRoutine(float seconds) {

            yield return new WaitForSeconds(seconds);

            Health = _startingHealth;
            destroyed = false;

            // Deactivate
            foreach (var go in ActivateGameObjectsOnDeath) {
                go.SetActive(false);
                Debug.Log("Should be turning off " + go.name + " as part of respawn");
            }

            // Re-Activate
            foreach (var go in DeactivateGameObjectsOnDeath) {
                go.SetActive(true);
                Debug.Log("Should be turning on " + go.name + " as part of respawn");
            }
            foreach (var col in DeactivateCollidersOnDeath) {
                col.enabled = true;
            }

            // Reset kinematic property if applicable
            if (rigid) {
                rigid.isKinematic = initialWasKinematic;
            }

            // Call events
            if (onRespawn != null) {
                onRespawn.Invoke();
            }
        }

        void broadcastHit()
        {
            switch (gameObject.tag)
            {
                case TagManager.TARGET_TAG:
                    onTargetHit?.Invoke(gameObject);
                    break;
                case TagManager.PRACTICECLAY_TAG:
                    onClayHit?.Invoke();
                    break;
                case TagManager.PRACTICEDUCK_TAG:
                    onCarniDuckHit?.Invoke(transform.parent.gameObject);
                    onDuckDie?.Invoke();
                    break;
                case TagManager.INFINITEDUCK_TAG:
                    onInfiniteDuckHit?.Invoke();
                    onDuckDie?.Invoke();
                    break;
                case TagManager.DUCK_TAG:
                    onDuckDie?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
}