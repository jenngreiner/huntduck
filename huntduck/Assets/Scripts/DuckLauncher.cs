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
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("KeyDown.L: Launch Norm Duck");
            LaunchObj(ObjectManager.instance.normDuck);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("KeyDown.L: Launch Fast Duck");
            LaunchObj(ObjectManager.instance.fastDuck);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("KeyDown.L: Launch Angry Duck");
            LaunchObj(ObjectManager.instance.angryDuck);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (transform.tag == "GooseLauncher")
            {
                Debug.Log("KeyDown.C: Launch Bonus (Canadian) Goose");
                LaunchObj(ObjectManager.instance.bonusGeese);
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("KeyDown.G: Launch Golden Goose");
            LaunchObj(ObjectManager.instance.goldenGoose);
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
