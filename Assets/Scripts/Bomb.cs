using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    public float speed;
    public bool invincible;
    public float bumpSpeed;
    Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("End"))
        {
            speed *= -1;
        }
    }

    public void OnDeath()
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }
}
