using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBumps : MonoBehaviour
{
    public delegate void OnBump(Transform otherTransform);
    public static event OnBump onBump;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(transform.name + " collided with " + other.name);

        onBump?.Invoke(other.transform.parent.transform);
    }
}
