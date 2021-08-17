using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//think about requirecomponent here for damageable script
public class fallDucks : MonoBehaviour
{
    public GameObject fallDuckLeft;
    public GameObject fallDuckRight;
    public GameObject shotDuck;

    private Rigidbody parentDuckRB;

    // change this to dameageable destroydelay next
    public float setupDelay = 1f;
    public float flipSpeed = 0.1f;

    private void OnEnable()
    {
        Debug.Log(this.gameObject.name + " is alive!");

        parentDuckRB = this.gameObject.GetComponentInParent<Rigidbody>();
        StartCoroutine(SetupDucks(setupDelay));
    }

    private IEnumerator SetupDucks(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        shotDuck.SetActive(false);
        fallDuckLeft.SetActive(true);
        fallDuckRight.SetActive(false);

        // reset parent gameobject to no rotation, so falling ducks point down
        parentDuckRB.transform.rotation = Quaternion.identity;

        // flip ducks
        Debug.Log("Would you please flip those ducks???");
        StartCoroutine(RotateDucks(flipSpeed));

        // allow gravity to work on parent ShootableDuck by turning off Is Kinematic
        parentDuckRB.isKinematic = false;
        Debug.Log("Applying gravity to fallduck parent");
    }

    private IEnumerator RotateDucks(float waitTime)
    {
        // as long as this gameObject is enabled, run Coroutine
        while (true)
        {
            // alternate ducks every "waitTime" seconds
            yield return new WaitForSeconds(waitTime);
            fallDuckRight.SetActive(!fallDuckRight.activeInHierarchy);
            fallDuckLeft.SetActive(!fallDuckLeft.activeInHierarchy);
            Debug.Log("Those ducks got flipped yo!");
        }
    }
}
