using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : MonoBehaviour
{

    public float mouseSensitivity = 10f;

    public Transform playerBody;
    public GameObject controller;

    float xRotation = 0f;

    public bool locked;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100 * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100 * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(transform.localRotation.x + xRotation, transform.localRotation.y, transform.localRotation.z);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}