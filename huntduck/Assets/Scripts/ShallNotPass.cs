using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShallNotPass : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        Debug.Log("YOU SHALL NOT PASS " + other.name);
    }
}
