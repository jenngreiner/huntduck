using UnityEngine;
using Photon.Pun;

public class NW_ObjectLauncher : MonoBehaviourPun
{
    // Object to launch 
    public GameObject projectileObject;
    public float projectileForce = 15f;

    /// Where the projectile will launch from
    public Transform launchTransform;
    public Transform launchRotation;

    // amount of time to wait before launching
    public float launchDelayTime = 3f;

    //public AudioClip LaunchSound;
    private Rigidbody rb;
    private PhotonView pv;

    void Start()
    {
        // PhotonView attached to the networked launcher
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //ShootProjectile();
            //Debug.Log("ShootProjectile() Shot a clay on the network!!!");

            photonView.RPC("RPC_ShootProjectile", RpcTarget.All);
            Debug.Log("RPC_ShootProjectile() Shot a clay on the network!!!");
        }
    }
    
    //public void ShootProjectile()
    //{
    //    GameObject launched = PhotonNetwork.Instantiate(projectileObject.name, launchTransform.transform.position, launchTransform.transform.rotation) as GameObject;
    //    Debug.Log("Launched object is " + launched.name);

    //    pv.RPC("launchProjectile", RpcTarget.All, launched);

    //    //PhotonView launchedPV = launched.GetComponentInChildren<PhotonView>();
    //    //launchedPV.RPC("launchProjectile", RpcTarget.All, launched);
    //    //rb = launched.GetComponentInChildren<Rigidbody>();
    //    //Debug.Log("Rb object is " + rb.name);
    //    //photonView.RPC("RPC_ApplyForce", RpcTarget.All, launchTransform.forward * projectileForce, ForceMode.VelocityChange);
    //    //BNG.VRUtils.Instance.PlaySpatialClipAt(LaunchSound, launched.transform.position, 1f);
    //}

    //[PunRPC]
    //void launchProjectile(GameObject launched)
    //{
    //    launched.transform.position = launchTransform.transform.position;
    //    launched.transform.rotation = launchRotation.transform.rotation;

    //    launched.GetComponentInChildren<Rigidbody>().AddForce(launchTransform.forward * projectileForce, ForceMode.VelocityChange);
    //    Debug.Log("Clay should be MOVIN");
    //}


    [PunRPC]
    public void RPC_ShootProjectile()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        GameObject launched = PhotonNetwork.Instantiate(projectileObject.name, launchTransform.transform.position, launchTransform.transform.rotation);
        Debug.Log("Launched object is " + launched.name);

        launched.transform.position = launchTransform.transform.position;
        launched.transform.rotation = launchRotation.transform.rotation;

        launched.GetComponentInChildren<Rigidbody>().AddForce(launchTransform.forward * projectileForce, ForceMode.VelocityChange);

        //rb = launched.GetComponentInChildren<Rigidbody>();
        //Debug.Log("Rb object is " + rb.name);

        //PhotonView launchedPV = launched.GetComponentInChildren<PhotonView>();
        //photonView.RPC("RPC_ApplyForce", RpcTarget.All, launchTransform.forward * projectileForce, ForceMode.VelocityChange);

        //BNG.VRUtils.Instance.PlaySpatialClipAt(LaunchSound, launched.transform.position, 1f);
    }

    [PunRPC]
    public void RPC_ApplyForce(Vector3 force, ForceMode mode)
    {
        rb.AddForce(force, mode);
    }


    //[PunRPC]
    //public void RPC_ShootProjectile()
    //{
    //    GameObject launched = PhotonNetwork.Instantiate(projectileObject.name, launchTransform.transform.position, launchTransform.transform.rotation) as GameObject;

    //    launched.transform.position = launchTransform.transform.position;
    //    launched.transform.rotation = launchRotation.transform.rotation;

    //    launched.GetComponentInChildren<Rigidbody>().AddForce(launchTransform.forward * projectileForce, ForceMode.VelocityChange);

    //    //BNG.VRUtils.Instance.PlaySpatialClipAt(LaunchSound, launched.transform.position, 1f);
    //}

    //public void DelayedLaunch()
    //{
    //    Debug.Log("launch will begin after a delay of " + launchDelayTime);
    //    StartCoroutine(Wait(launchDelayTime));
    //}

    //IEnumerator Wait(float delayTime)
    //{
    //    yield return new WaitForSeconds(delayTime);
    //    RPC_ShootProjectile();
    //}

    //public void ShootProjectile_PNI()
    //{
    //    GameObject launched = PhotonNetwork.Instantiate(projectileName, launchTransform.transform.position, launchTransform.transform.rotation) as GameObject;

    //    launched.transform.position = launchTransform.transform.position;
    //    launched.transform.rotation = launchRotation.transform.rotation;

    //    launched.GetComponentInChildren<Rigidbody>().AddForce(launchTransform.forward * projectileForce, ForceMode.VelocityChange);

    //    //BNG.VRUtils.Instance.PlaySpatialClipAt(LaunchSound, launched.transform.position, 1f);
    //}

    //[PunRPC]
    //public void RPC_ShootProjectile_PNI()
    //{
    //    GameObject launched = PhotonNetwork.Instantiate(projectileName, launchTransform.transform.position, launchTransform.transform.rotation) as GameObject;

    //    launched.transform.position = launchTransform.transform.position;
    //    launched.transform.rotation = launchRotation.transform.rotation;

    //    launched.GetComponentInChildren<Rigidbody>().AddForce(launchTransform.forward * projectileForce, ForceMode.VelocityChange);

    //    //BNG.VRUtils.Instance.PlaySpatialClipAt(LaunchSound, launched.transform.position, 1f);
    //}
}