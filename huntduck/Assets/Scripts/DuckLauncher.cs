using UnityEngine;

public class DuckLauncher : MonoBehaviour
{
    //public GameObject launchObject;
    //public GameObject bonusObject;

    public float launchForce = 15f;

    /// Where the projectile will launch from
    public Transform launchTransform;
    public Transform launchRotation;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LaunchObj(DuckManager.instance.normDuck);
        }
    }

    public void LaunchObj(GameObject _projectile)
    {
        if (launchTransform && _projectile)
        {
            GameObject launched = Instantiate(_projectile, launchTransform.transform.position, launchTransform.transform.rotation);

            // reset position and rotation so ducks fly out correctly
            launched.transform.position = launchTransform.transform.position;
            launched.transform.rotation = launchRotation.transform.rotation;

            launched.GetComponentInChildren<Rigidbody>().AddForce(launchTransform.forward * launchForce, ForceMode.VelocityChange);
        }
    }
}
