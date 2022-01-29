using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Transform[] points;
    public float speed;
    public int nextPointIndex = 0;
    public float restingTime = 2;
    public float tolerance;
    public bool automatic = true;

    private Transform currentTarget;
    private float rest_start;

    // Start is called before the first frame update
    void Start()
    {
        if (points.Length > 0)
            if (nextPointIndex < points.Length)
                currentTarget = points[nextPointIndex];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentTarget != null && transform.position != currentTarget.position)
            MovePlatform();
        else
            UpdateTarget();
    }

    void MovePlatform()
    {
        Vector3 dir = currentTarget.position - transform.position;
        transform.position += dir.normalized * speed * Time.deltaTime;
        // snap to position
        if (dir.magnitude < tolerance)
        {
            transform.position = currentTarget.position;
            rest_start = Time.time;
        }
    }
    void UpdateTarget()
    {
        if (automatic)
        {
            if (Time.time - rest_start > restingTime)
            {
                NextTarget();
            }
        }
    }

    void NextTarget()
    {
        if (transform.position != currentTarget?.position)
            return;
        nextPointIndex++;
        if (nextPointIndex >= points.Length)
        {
            nextPointIndex = 0;
        }
        currentTarget = points[nextPointIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent.parent = transform;
            if (!automatic)
                NextTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent.parent = null;
        }
    }
}
