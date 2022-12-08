using UnityEngine;

public class StopBumps : MonoBehaviour
{
    public delegate void OnBump(Transform objectThatBumped, string thisTransformName);
    public static event OnBump onBump;

    void OnTriggerEnter(Collider objectThatBumped)
    {
        Debug.Log(transform.name + " collided with root: " + objectThatBumped.transform.root.name);

        onBump?.Invoke(objectThatBumped.transform.root, transform.name);
    }
}