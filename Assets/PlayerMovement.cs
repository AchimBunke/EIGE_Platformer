using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public LookAround camera;

    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2.5f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool hasJumpedTwice;
    bool isGrounded;
    private float turnSpeed;

    private float sprintFactor = 1;

    Vector3 velocity;
    
    
    
    private bool firstRotationFinished = false;
    private bool secondRotationFinished = false;
    private Quaternion finalFlipRotation;

    bool respawn = false;
    int i = 0;

    void Update()
    {
        if (respawn)
        {
            i++;

            if (i > 10)
            {
                respawn = false;
                i = 0;

            }
            else
                return;
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
            camera.locked = false;
            hasJumpedTwice = false;
        }
        

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 1)
        {
            move /= move.magnitude;
        }
        
        controller.Move(move * speed * Time.deltaTime * sprintFactor);
        
        //SPRINT
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Sprint");
            controller.Move(move * speed * Time.deltaTime * sprintFactor);
        }


        //DOUBLE JUMP
        if (Input.GetButtonDown("Jump") && velocity.y > 0.2 && !hasJumpedTwice && !camera.locked)
        {
            //JUMP
            velocity.y += Mathf.Sqrt(jumpHeight / 2f * -2f * gravity);
            hasJumpedTwice = true;
            Debug.Log("Rotation wird aktiviert");
            camera.locked = true;
            finalFlipRotation = camera.gameObject.transform.localRotation;
            turnSpeed = 3.0f * Time.deltaTime;
            firstRotationFinished = false;
            secondRotationFinished = false;
        }


        if (camera.locked)
        {
            Debug.Log(firstRotationFinished);
            Debug.Log(secondRotationFinished);
            Debug.Log(camera.gameObject.transform.localRotation.x * 100);
            Debug.Log("Rotiere gerade");
            if (!firstRotationFinished)
            {
                Debug.Log("First Rotation");
                camera.gameObject.transform.localRotation =
                    Quaternion.Lerp(camera.gameObject.transform.localRotation, Quaternion.Euler(new Vector3(
                            180f * Mathf.Sign(camera.gameObject.transform.localRotation.x),
                            camera.gameObject.transform.localRotation.y,
                            camera.gameObject.transform.localRotation.z)),
                        turnSpeed * 1.7f
                    );
                if (Mathf.Abs(Mathf.Round(camera.gameObject.transform.localRotation.x * 100)) >= 96)
                {
                    firstRotationFinished = true;
                }
            }
            else if (firstRotationFinished && !secondRotationFinished)
            {
                Debug.Log("Second Rotation");
                camera.gameObject.transform.localRotation =
                    Quaternion.Lerp(camera.gameObject.transform.localRotation, Quaternion.Euler(new Vector3(
                        50f * -Mathf.Sign(camera.gameObject.transform.localRotation.x),
                            camera.gameObject.transform.localRotation.y,
                            camera.gameObject.transform.localRotation.z)),
                        turnSpeed * 1.5f
                    );
                if (Mathf.Abs(Mathf.Round(camera.gameObject.transform.localRotation.x * 100)) < 60)
                {
                    secondRotationFinished = true;
                }
            }
            else
            {
                Debug.Log("Third Rotation");
                            camera.gameObject.transform.localRotation =
                                Quaternion.Lerp(camera.gameObject.transform.localRotation, finalFlipRotation,
                                    turnSpeed
                                );
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DeathZone")
        {
            GameData.Instance.Score--;
            transform.position = GameObject.FindGameObjectWithTag("Checkpoint").transform.position;
            respawn = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (other.tag == "End")
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
                SceneManager.LoadScene(0);
        }
    }
}