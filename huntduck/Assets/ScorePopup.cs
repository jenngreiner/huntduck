using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    public float moveYSpeed = 0.1f;
    public float destroyTimer = 3f;

    void Start()
    {
        Destroy(gameObject, destroyTimer);
    }

    void Update()
    {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        //keep going video 10:18 to make number disappear a little
        //may have to adjust the destroy delay, make it 3 seconds, decouple from duck
    }

}
