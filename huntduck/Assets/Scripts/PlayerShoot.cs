// using UnityEngine.Networking
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class PlayeShoot : NetworkBehavior
public class PlayerShoot : MonoBehaviour
{
    private const string DUCK_TAG = "Duck";

    public PlayerWeapon weapon;

    // Variable stores camera so ray shoots from center of camera
    [SerializeField]
    private Camera cam;

    // mask to control what we are allowed to hit with our raycast
    [SerializeField]
    private LayerMask mask;

    public GameObject me;

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            Debug.Log("PlayerShoot: No Camera referenced");
            this.enabled = false;
        }

        // returns values associated with GameObjects that have Player tag
        me = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("I found " + me.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

    }

    void Shoot()
    {
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            switch (_hit.collider.tag)
            {
                case DUCK_TAG:
                    shotADuck(_hit.collider.name);
                    break;
                //case "Shootable":
                //    Debug.Log("Shot something shootable!");
                //    break;
                default:
                    Debug.Log("missed, stupid idiot! you hit " + _hit.collider.name);
                    break;
            }

        }
    }

    void shotADuck(string duckName)
    {
        // We hit something shootable
        Debug.Log("We hit " + duckName);
    }
}
