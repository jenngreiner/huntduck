using UnityEngine;

public class StopBumps : MonoBehaviour
{
    public delegate void OnBump(Transform otherTransform);
    public static event OnBump onBump;

    public string collisionLayerName = "Guard";

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(transform.name + " collided with root: " + other.transform.root.name);

        // Check if the collided object has a collider on the specified layer
        if (other.gameObject.layer == LayerMask.NameToLayer(collisionLayerName))
        {
            onBump?.Invoke(other.transform.root);
        }
    }
}