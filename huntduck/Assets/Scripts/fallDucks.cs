using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallDucks : MonoBehaviour
{
    public GameObject fallDuckLeft;
    public GameObject fallDuckRight;
    public GameObject shotDuck;
    public GameObject duckPoints;

    private Rigidbody parentDuckRB;
    private Rigidbody fallingDuckRB;

    public float setupDelay = 1f;
    public float flipSpeed = 0.1f;

    private void Start()
    {
        fallingDuckRB = this.gameObject.GetComponent<Rigidbody>();
        parentDuckRB = this.gameObject.GetComponentInParent<Rigidbody>();

        // don't want falling ducks to fall until they are visible (via SetupDucks)
        fallingDuckRB.useGravity = false;
    }

    private void OnEnable()
    {
        Debug.Log(this.gameObject.name + " is alive!");
        StartCoroutine(SetupDucks(setupDelay));
    }

    private IEnumerator SetupDucks(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        shotDuck.SetActive(false);
        duckPoints.SetActive(true);
        fallDuckLeft.SetActive(true);
        fallDuckRight.SetActive(false);

        // reset parent rigidbody to no rotation, so falling ducks point down
        parentDuckRB.transform.rotation = Quaternion.identity;
        // set duckpoints to no rotation, so they display correctly (won't fall)
        duckPoints.transform.rotation = Quaternion.identity;

        fallingDuckRB.isKinematic = false;
        fallingDuckRB.useGravity = true;
        Debug.Log("Applying gravity to fallduck");

        // flip ducks
        Debug.Log("Would you please flip those ducks???");
        StartCoroutine(FlipDucks(flipSpeed));
    }

    private IEnumerator FlipDucks(float waitTime)
    {
        // as long as this gameObject is enabled, run Coroutine
        while (true)
        {
            // alternate (flip) ducks every "waitTime" seconds
            yield return new WaitForSeconds(waitTime);
            fallDuckRight.SetActive(!fallDuckRight.activeInHierarchy);
            fallDuckLeft.SetActive(!fallDuckLeft.activeInHierarchy);
            Debug.Log("Those ducks got flipped yo!");
        }
    }
}
