using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SINGLESCENE CLASS: Destroy Infinite Ducks so they don't fall through the map on mode change
public class DestroyTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagManager.INFINITEDUCK_TAG)
        {
            Destroy(other.gameObject);
        }
    }
}
