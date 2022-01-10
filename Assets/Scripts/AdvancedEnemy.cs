using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedEnemy : MonoBehaviour
{
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private GameObject prey;
    private Rigidbody enemyRigidbody;
    [SerializeField] private WayPoint[] wayPoints;
    private int currentWayPoint = 0;
    [SerializeField] private float distanceThreshold;
    [SerializeField] private float chaseEvadeDistance;
    [SerializeField] private float sightDistance;


    public enum Behaviour
    {
        LineOfSight,
        Intercept,
        PatternMovement,
        ChasePatternMovement,
        Hide
    }
    public Behaviour behaviour;

    void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        switch (behaviour)
        {
            case Behaviour.LineOfSight: //Exercise 1
                ChaseLineOfSight(prey.transform.position, chaseSpeed);
                break;
            case Behaviour.Intercept: //Exercise 2
                Intercept(prey.transform.position);
                break;
            case Behaviour.PatternMovement: //Exercise 3
                PatternMovement();
                break;
            case Behaviour.ChasePatternMovement: //Exercise 4
                if (Vector3.Distance(gameObject.transform.position, prey.transform.position) < chaseEvadeDistance)
                {
                    ChaseLineOfSight(prey.transform.position, chaseSpeed);
                }
                else
                {
                    PatternMovement();
                }
                break;
            case Behaviour.Hide: //Exercise 5
                if (PlayerVisible(prey.transform.position))
                {
                    ChaseLineOfSight(prey.transform.position, chaseSpeed);
                }
                else
                {
                    PatternMovement();
                }
                break;
            default:
                break;
        }
    }

        private void ChaseLineOfSight(Vector3 targetPosition, float Speed)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.Normalize();
            enemyRigidbody.velocity = new Vector3(direction.x * Speed, enemyRigidbody.velocity.y, direction.z * Speed);
        }

    private void Intercept(Vector3 targetPosition)
    {
        Vector3 enemyPosition = gameObject.transform.position;
        Vector3 velocityRelative, distance, predictedInterceptionPoint;
        float timeToClose;
        velocityRelative = prey.GetComponent<Rigidbody>().velocity - enemyRigidbody.velocity;
        distance = targetPosition - enemyPosition;
        timeToClose = distance.magnitude / velocityRelative.magnitude;
        predictedInterceptionPoint = targetPosition + (timeToClose * prey.GetComponent<Rigidbody>().velocity);
        Vector3 direction = predictedInterceptionPoint - enemyPosition;
        direction.Normalize();
        enemyRigidbody.velocity = new Vector3(direction.x * chaseSpeed, enemyRigidbody.velocity.y, direction.z * chaseSpeed);
    }

    private void PatternMovement()
    {
        //Move towards the current waypoint.
        ChaseLineOfSight(wayPoints[currentWayPoint].transform.position, normalSpeed);
        //Check if we are close to the next waypoint and incerement to the next waypoint.
        if (Vector3.Distance(gameObject.transform.position, wayPoints[currentWayPoint].transform.position) < distanceThreshold)
        {
            currentWayPoint = (currentWayPoint + 1) % wayPoints.Length; //modulo to restart at the beginning.
        }
    }

    private bool PlayerVisible(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - gameObject.transform.position;
        directionToTarget.Normalize();
        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, directionToTarget, out hit, sightDistance);
        return hit.collider.CompareTag("Player");
    }

}
