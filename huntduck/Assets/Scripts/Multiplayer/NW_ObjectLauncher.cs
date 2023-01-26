using UnityEngine;
using Photon.Pun;

public class NW_ObjectLauncher : MonoBehaviourPun
{
    // Object to launch 
    public GameObject projectileObject;
    public float projectileForce = 15f;
    [SerializeField]
    private GameObject launchedObj;

    /// Where the projectile will launch from
    public Transform launchTransform;
    public Transform launchRotation;

    // amount of time to wait before launching
    public float launchDelayTime = 3f;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            photonView.RPC(nameof(RPC_ShootProjectile), RpcTarget.All);
            Debug.Log(nameof(RPC_ShootProjectile) + " function has completed");
        }
    }

    [PunRPC]
    public void RPC_ShootProjectile()
    {
        Debug.Log(nameof(RPC_ShootProjectile) + " function has started!");
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("You are not the master, bailing function locally");
            return;
        }

        GameObject launched = PhotonNetwork.Instantiate(projectileObject.name, launchTransform.transform.position, launchTransform.transform.rotation);
        Debug.Log("MC created a clay on the network!!!");


        launched.transform.position = launchTransform.transform.position;
        launched.transform.rotation = launchRotation.transform.rotation;
        launched.GetComponent<Rigidbody>().AddForce(launchTransform.forward * projectileForce, ForceMode.VelocityChange);

        Debug.Log("MC applied force to " + launched.name + " , it should be MOVIN");
    }
}