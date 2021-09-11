using System.Collections;
using UnityEngine;
using Photon.Pun;

public class NW_ObjectLauncher : MonoBehaviourPun
{
    // Object to launch 
    public GameObject projectileObject;
    private string projectileName;
    public float projectileForce = 15f;

    /// Where the projectile will launch from
    public Transform launchTransform;
    public Transform launchRotation;

    // amount of time to wait before launching
    public float launchDelayTime = 3f;

    //public AudioClip LaunchSound;

    void Awake()
    {
        projectileName = projectileObject.name;
        PhotonView photonView = PhotonView.Get(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            photonView.RPC("RPC_ShootProjectile", RpcTarget.All);
            Debug.Log("Shooting clays on the network!!!");
        }
    }

    [PunRPC]
    public void RPC_ShootProjectile()
    {
        GameObject launched = PhotonNetwork.Instantiate(projectileName, launchTransform.transform.position, launchTransform.transform.rotation) as GameObject;

        launched.transform.position = launchTransform.transform.position;
        launched.transform.rotation = launchRotation.transform.rotation;

        launched.GetComponentInChildren<Rigidbody>().AddForce(launchTransform.forward * projectileForce, ForceMode.VelocityChange);

        //BNG.VRUtils.Instance.PlaySpatialClipAt(LaunchSound, launched.transform.position, 1f);
    }

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
}