using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeryGoRound : MonoBehaviour
{
    void Update()
    {
        flap();
    }

    void flap()
    {
        this.transform.RotateAround(new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f), 90f * Time.deltaTime);
    }
}
