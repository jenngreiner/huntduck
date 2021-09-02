using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flap : MonoBehaviour
{
    //public Transform wingPivot;

    private float startFlap = 0f;
    private float midFlap = 85f;
    private float fullFlap = 125f;

    public Vector3 anglesToRotate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        flap();
    }

    void flap()
    {
        // move the wing from 0 degrees to 85 degrees (mid flap)
        this.transform.Rotate(anglesToRotate * Time.deltaTime);

        // move the wing from 85 degrees to 125 degrees (full flap)

        // fo 125 to 85 (back to mid)

        // 85 back to 0 (return to start)
    }
}
