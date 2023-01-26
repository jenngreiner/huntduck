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
    //private Rigidbody rb;
    //private PhotonView pv;

    void Start()
    {
        // PhotonView attached to the networked launcher
        //pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //ShootProjectile();
            //Debug.Log("ShootProjectile() Shot a clay on the network!!!");

            photonView.RPC(nameof(RPC_ShootProjectile), RpcTarget.All);
            Debug.Log(nameof(RPC_ShootProjectile) + " function has completed");
        }
    }

    //[PunRPC]
    //public void RPC_ShootProjectile()
    //{
    //    if (!PhotonNetwork.IsMasterClient)
    //        return;

    //    GameObject launched = PhotonNetwork.Instantiate(projectileObject.name, launchTransform.transform.position, launchTransform.transform.rotation);
    //    Debug.Log("MC created a clay on the network!!!");


    //    launched.transform.position = launchTransform.transform.position;
    //    launched.transform.rotation = launchRotation.transform.rotation;
    //    launched.GetComponentInChildren<Rigidbody>().AddForce(launchTransform.forward * projectileForce, ForceMode.VelocityChange);

    //    Debug.Log(launched.name + " should be MOVIN");

    //    //Rigidbody rb = launched.GetComponentInChildren<Rigidbody>();
    //    Vector3 launchSpeed = launchTransform.forward * projectileForce;
    //    //PhotonView launchedPV = launched.GetComponent<PhotonView>();
    //    //photonView.RPC(nameof(RPC_ApplyForce), RpcTarget.All, launchedPV, launchSpeed, ForceMode.VelocityChange);

    //    int launchedPVId = launched.GetComponent<PhotonView>().ViewID;

    //    photonView.RPC(nameof(RPC_ApplyForce), RpcTarget.All, launchedPVId, launchSpeed, ForceMode.VelocityChange);

    //    //Debug.Log("Fired " + nameof(RPC_ApplyForce) + ", clay should be launching across the sky for all!");


    //    //rb = launched.GetComponentInChildren<Rigidbody>();
    //    //Debug.Log("Rb object is " + rb.name);



    //    //BNG.VRUtils.Instance.PlaySpatialClipAt(LaunchSound, launched.transform.position, 1f);
    //}

    [PunRPC]
    public void RPC_ShootProjectile()
    {
        //if (!PhotonNetwork.IsMasterClient)
            //return;

        GameObject launched = PhotonNetwork.InstantiateRoomObject(projectileObject.name, launchTransform.transform.position, launchTransform.transform.rotation);
        Debug.Log("MC created a clay on the network!!!");


        //launched.transform.position = launchTransform.transform.position;
        //launched.transform.rotation = launchRotation.transform.rotation;
        //launched.GetComponentInChildren<Rigidbody>().AddForce(launchTransform.forward * projectileForce, ForceMode.VelocityChange);

        //Debug.Log(launched.name + " should be MOVIN");

        Vector3 launchSpeed = launchTransform.forward * projectileForce;
        int launchedPVId = launched.GetComponent<PhotonView>().ViewID;
        photonView.RPC(nameof(RPC_ApplyForce), RpcTarget.Others, launchedPVId, launchSpeed, ForceMode.VelocityChange);
    }

    [PunRPC]
    public void RPC_ApplyForce(int launchedPVId, Vector3 launchSpeed, ForceMode mode)
    {
        PhotonView launchedPV = PhotonView.Find(launchedPVId);
        if (launchedPV != null)
        {
            Rigidbody rb = launchedPV.GetComponent<Rigidbody>();
            if (rb != null)
            {
                launchedPV.transform.SetPositionAndRotation(launchTransform.transform.position, launchRotation.transform.rotation);
                launchedPV.RPC("ApplyForce", RpcTarget.All, launchSpeed, mode);
            }
        }
    }

    [PunRPC]
    public void ApplyForce(Vector3 launchSpeed, ForceMode mode)
    {
        GetComponentInChildren<Rigidbody>().AddForce(launchSpeed, mode);
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