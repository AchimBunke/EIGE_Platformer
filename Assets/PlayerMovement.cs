using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public GameObject camera;

    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2.5f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    public bool isDoingAFlip;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded) isDoingAFlip = false;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1)
        {
            move /= move.magnitude;
        }
        controller.Move(move * speed * Time.deltaTime);
        
        if (Input.GetButtonDown("Jump") && velocity.y > 3)
        {
            //Perform Double Jump + Looping
            //controller.transform.Rotate(controller.transform.right, 180);
            isDoingAFlip = true;
            camera.transform.localRotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 360.0f), 0.1f);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        
    }
}
