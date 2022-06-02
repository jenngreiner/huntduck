using UnityEngine;

public class StopBumps : MonoBehaviour
{
    public delegate void OnBump(Transform otherTransform);
    public static event OnBump onBump;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(transform.name + " collided with root: " + other.transform.root.name);

        onBump?.Invoke(other.transform.root);
    }
}
