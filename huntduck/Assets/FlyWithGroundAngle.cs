using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyWithGroundAngle : MonoBehaviour
{
    public delegate void OnGroundHit(Collider otherCollider);
    public static event OnGroundHit onGroundHit;

    // The name of the layer that will be considered a collision
    public string collisionLayerName = "Ground";

    void OnTriggerEnter(Collider otherCollider)
    {
        // Check if the collided object has a collider on the specified layer
        if (otherCollider.gameObject.layer == LayerMask.NameToLayer(collisionLayerName))
        {
            onGroundHit?.Invoke(otherCollider);
            Debug.Log("Forcefield hit the object named: " + otherCollider.transform.name);
        }
    }
}
