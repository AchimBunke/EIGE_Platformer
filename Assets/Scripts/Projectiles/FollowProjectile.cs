using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FollowProjectile : Projectile
{
    public float followForce = 20;
    public bool ignoreGravity = false;

    private Transform player;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = !ignoreGravity;

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(player != null)
        {
            rb.AddRelativeForce((player.position - transform.position).normalized * followForce);
        }

    }
}
