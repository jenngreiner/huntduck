using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtPlayer : MonoBehaviour
{
    private Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        //transform.LookAt(player, Vector3.up); // backwards

        //transform.rotation = Quaternion.Euler(0, player.eulerAngles.y, 0);

        Vector3 lookDir = transform.position - player.position * 2;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }

    void Update()
    {
        float moveYSpeed = 1f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
    }
}
