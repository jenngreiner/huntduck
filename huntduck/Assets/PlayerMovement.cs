using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    // control what objects this sphere (groundCheck) should look for
    public LayerMask groundmask;

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        // creates tiny sphere with radius specified - if touches ground, will change isGrounded to true
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundmask);

        // if you hit ground, reset velocity (-2f just a little more effective than 0f)
        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }

        // assign Horizontal and Vertical inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // turn input into direction we want to move
        // use transform to move relative to the direction player is facing (changes Vector3 from global to local)
        Vector3 move = transform.right * x + transform.forward * z;

        // velocity = direction * speed (at current framerate)
        controller.Move(move * speed * Time.deltaTime);

        // add jummp
        if (Input.GetButtonDown("Jump") && isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        // set gravity
        velocity.y += gravity * Time.deltaTime;

        // apply gravity
        controller.Move(velocity * Time.deltaTime);
    }
}
