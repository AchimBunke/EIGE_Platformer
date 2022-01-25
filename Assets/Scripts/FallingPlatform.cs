using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallSpeed;
    public float fallDistance;
    public float activationTime;

    public bool respawn;
    public float respawnDelay;

    private float timeWaited = 0;
    private bool activated = false;
    private bool falling = false;
    private float fallenDistance = 0;

    private Vector3 startPosition;
    private float respawnTimer = 0;
    private bool respawning = false;

    void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (falling)
        {
            var dist = Time.deltaTime * fallSpeed;
            transform.position += Vector3.down * dist;
            if((fallenDistance += dist) > fallDistance)
            {
                falling = false;
                if (respawn)
                {
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    respawning = true;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (activated)
        {
            timeWaited += Time.deltaTime;
            if (timeWaited >= activationTime)
            {
                activated = false;
                falling = true;
            }
        }else if (respawning)
        {
            if((respawnTimer += Time.deltaTime) > respawnDelay)
            {
                transform.position = startPosition;
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                respawning = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activated = true;
        }
    }
}
