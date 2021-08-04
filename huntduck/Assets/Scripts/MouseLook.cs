using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 250f;
    public Transform playerBody;
    float xRotation = 0f;

    void Start()
    {
        // lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // *Set Up Mouse Axes*
        // Time.deltaTime is amount of time that has gone by since last time update function called
        // ensures we stay in sync with our framerate
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // flip rotation of cam - why? because if you don't its upside down :-P
        xRotation -= mouseY;
        // clamp the rotation so your face doesn't swing off your head backwards
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
