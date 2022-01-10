using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Transform[] trackingPointArray;

    [SerializeField] private float speed;
    private Vector3 tmpTarget;
    private int targetIndex;


    // Start is called before the first frame update
    void Start()
    {
        //sets initial platform position to position of first tracking point
        transform.position = trackingPointArray[0].position;
        //sets next target index on the second target point
        //(target array always must have at least 2 tracking points)
        targetIndex = 1;
        //setting target Vec3 for next destination
        tmpTarget = trackingPointArray[targetIndex].position;
    }

    void Update()
    {
        //platform speed
        float amtToMove = speed * Time.deltaTime;
        //platform travel direction
        Vector3 direction = (tmpTarget - transform.position).normalized;
        //apply movement
        transform.Translate(direction * amtToMove);
        //change target once closeness threshold is met
        if (Vector3.Distance(transform.position, tmpTarget) <= 0.1f)
        {
            targetIndex = (++targetIndex) % trackingPointArray.Length;
            tmpTarget = trackingPointArray[targetIndex].position;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }



}