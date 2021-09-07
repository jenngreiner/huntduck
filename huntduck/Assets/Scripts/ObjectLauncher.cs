using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLauncher : MonoBehaviour
{
    public BNG.ProjectileLauncher objLauncher;
    List<GameObject> launchedObjects;

    // Object to launch 
    public GameObject ProjectileObject;
    public float ProjectileForce = 15f;

    /// Where the projectile will launch from
    public Transform launchTransform;
    public Transform launchRotation;

    // Max number of objects to launch from Launcher. Old objects will be destroyed to make room for new.
    public int maxLaunchedObjects = 5;

    // amount of time to wait before launching
    public float launchDelayTime = 3f;

    public AudioClip LaunchSound;

    public ParticleSystem LaunchParticles;

    private float _initialProjectileForce;


    void Start()
    {
        // Setup initial velocity for launcher so we can modify it later
        _initialProjectileForce = ProjectileForce;

        launchedObjects = new List<GameObject>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ShootLauncher();
        }
    }

    public void ObjLaunch()
    {
        StartCoroutine(Wait(launchDelayTime));
        Debug.Log("the launch begins..");
    }

    IEnumerator Wait(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log("Waiting " + delayTime + " seconds to launch a duck");
        ShootLauncher();
        Debug.Log("this " + transform.name + " has officially launched!");
    }

    public void ShootLauncher()
    {
        if (launchedObjects == null)
        {
            launchedObjects = new List<GameObject>();
        }

        // Went over max. Destroy oldest launch object
        if (launchedObjects.Count > maxLaunchedObjects)
        {
            launchedObjects.Remove(launchedObjects[0]);
            GameObject.Destroy(launchedObjects[0]);
        }

        launchedObjects.Add(objLauncher.ShootProjectile(objLauncher.ProjectileForce));
    }

    /// <summary>
    /// Returns the object that was shot
    /// </summary>
    /// <returns>The object that was shot</returns>
    public GameObject ShootProjectile(float projectileForce)
    {

        if (launchTransform && ProjectileObject)
        {
            GameObject launched = Instantiate(ProjectileObject, launchTransform.transform.position, launchTransform.transform.rotation) as GameObject;

            launched.transform.position = launchTransform.transform.position;
            //decoupling rotation from launch direction, using launchRotation
            launched.transform.rotation = launchRotation.transform.rotation;

            //original rotation
            //launched.transform.rotation = launchTransform.transform.rotation;

            launched.GetComponentInChildren<Rigidbody>().AddForce(launchTransform.forward * projectileForce, ForceMode.VelocityChange);

            BNG.VRUtils.Instance.PlaySpatialClipAt(LaunchSound, launched.transform.position, 1f);

            if (LaunchParticles)
            {
                LaunchParticles.Play();
            }

            return launched;
        }

        return null;
    }

    public void ShootProjectile()
    {
        ShootProjectile(ProjectileForce);
    }

    public void SetForce(float force)
    {
        ProjectileForce = force;
    }

    public float GetInitialProjectileForce()
    {
        return _initialProjectileForce;
    }
}
