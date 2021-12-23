using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBumps : MonoBehaviour
{
    public delegate void OnBump();
    public static event OnBump onBump;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagManager.INFINITEDUCK_TAG)
        {
            Debug.Log(transform.name + " collided with " + other.name);

            DuckFly duckFly = other.GetComponentInParent<DuckFly>();
            duckFly?.Swerve();
            //onBump?.Invoke(other.transform.parent.transform);
        }
    }
}
