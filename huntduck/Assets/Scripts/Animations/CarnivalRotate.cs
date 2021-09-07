using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivalRotate : MonoBehaviour
{
    public Vector3 anglesToRotate;

    void Update()
    {
        skewer();
    }

    void skewer()
    {
        this.transform.Rotate(anglesToRotate * Time.deltaTime);
    }
}
