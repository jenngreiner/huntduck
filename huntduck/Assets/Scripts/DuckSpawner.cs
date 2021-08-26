using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckSpawner : MonoBehaviour
{
    public BNG.ProjectileLauncher DuckLauncher;
    List<GameObject> launchedObjects;

    /// Max number of objects to launch from DemoLauncher. Old objects will be destroyed to make room for new.
    public int MaxLaunchedObjects = 5;

    // amount of time to wait before launching
    public float launchDelayTime = 3f;

    //[Tooltip("Speed at which to launch ducks")]
    //public float launchSpeed;

    void Start()
    {
        launchedObjects = new List<GameObject>();
        //DuckLauncher.SetForce(DuckLauncher.GetInitialProjectileForce() * (launchSpeed / 100));
    }

    public void DuckLaunch()
    {
        StartCoroutine(Wait(launchDelayTime));
        Debug.Log("the duck launch begins..");
    }

    IEnumerator Wait(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log("Waiting " + delayTime + " seconds to launch a duck");
        ShootLauncher();
        Debug.Log("a duck has officially launched!");
    }

    public void ShootLauncher()
    {
        if (launchedObjects == null)
        {
            launchedObjects = new List<GameObject>();
        }

        // Went over max. Destroy oldest launch object
        if (launchedObjects.Count > MaxLaunchedObjects)
        {
            launchedObjects.Remove(launchedObjects[0]);
            GameObject.Destroy(launchedObjects[0]);
        }

        launchedObjects.Add(DuckLauncher.ShootProjectile(DuckLauncher.ProjectileForce));
    }
}
