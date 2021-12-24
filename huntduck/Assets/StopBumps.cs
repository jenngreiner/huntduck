using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBumps : MonoBehaviour
{
    private DuckFly duckFly;

    private void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("InfiniteDuck"))
        {
            duckFly = transform.GetComponentInParent<DuckFly>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(transform.name + " collided with " + other.name);
        
        if (other.tag == TagManager.INFINITEDUCK_TAG)
        {

            DuckFly otherDuckFly = other.GetComponentInParent<DuckFly>();

            if (otherDuckFly != null)
            {
                otherDuckFly.Swerve(other.transform);
                return;
            }

            return;
        }

        if (gameObject.layer == LayerMask.NameToLayer("InfiniteDuck") && other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            duckFly.Swerve(other.transform);
        }
    }
}
