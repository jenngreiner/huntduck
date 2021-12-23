using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBumps : MonoBehaviour
{
    //public delegate void OnBump(Transform otherDuck);
    //public static event OnBump onBump;

    private DuckFly duckFly;

    private void Start()
    {
        duckFly = transform.GetComponentInParent<DuckFly>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagManager.INFINITEDUCK_TAG)
        {
            Debug.Log(transform.name + " collided with " + other.name);
            DuckFly otherDuckFly = other.GetComponentInParent<DuckFly>();
            otherDuckFly?.Swerve(other.transform);

            duckFly.Swerve(other.transform);

            ////onBump?.Invoke();
            //onBump?.Invoke(other.transform.parent.transform);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            duckFly.Swerve(other.transform);
        }

        //if (other.tag == TagManager.INFINITEDUCK_TAG || other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        //{
        //    Debug.Log(transform.name + " collided with " + other.name);

        //    DuckFly duckFly = other.GetComponentInParent<DuckFly>();
        //    duckFly?.Swerve();
        //    //onBump?.Invoke(other.transform.parent.transform);
        //}
    }
}
