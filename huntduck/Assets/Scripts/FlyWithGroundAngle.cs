using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyWithGroundAngle : MonoBehaviour
{
    public delegate void OnGroundHitDuck(Collider groundCollider, Transform parent);
    public static event OnGroundHitDuck onGroundHit;

    public delegate void OnNoLongerTouchingGround();
    public static event OnNoLongerTouchingGround onNoLongerTouchingGround;

    Transform parent;

    // The name of the layer that will be considered a collision
    public const string collisionLayerName = "Ground";

    void Start()
    {
        GetComponent<Collider>();
        parent = transform.root;
    }

    void OnTriggerEnter(Collider groundCollider)
    {
        // Check if the collided object has a collider on the specified layer
        if (groundCollider.gameObject.layer == LayerMask.NameToLayer(collisionLayerName))
        {
            onGroundHit?.Invoke(groundCollider, parent);

            Debug.Log("Forcefield hit the object named: " + groundCollider.transform.name);
        }
    }

    void OnTriggerExit(Collider groundCollider)
    {
        // Check if the collided object has a collider on the specified layer
        if (groundCollider.gameObject.layer == LayerMask.NameToLayer(collisionLayerName))
        {
            onNoLongerTouchingGround?.Invoke();

            Debug.Log("Forcefield no longer touching the object named: " + groundCollider.transform.name);
        }
    }
}
