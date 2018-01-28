using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Patrolling, Invesetigating, Chasing }
    public EnemyState enemyState = EnemyState.Patrolling;
    public GameObject player;
    public Rigidbody2D rb;

    public Transform[] waypoints;
    private int currentWaypoint;

    private Vector2 investigationLocation;

    public float speed;
    public float turnSpeed;
    public float minDestinationDistance;

    public float visionDistance;
    public ContactFilter2D raycastFilter;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        RaycastHit2D[] hits = new RaycastHit2D[0];
        int asdf = Physics2D.Raycast(transform.position, transform.up, raycastFilter, hits, visionDistance);
        //Debug.Log(asdf);
        //switch (enemyState)
        //{
        //    case EnemyState.Patrolling:
        //        Patrol();
        //        break;
        //    case EnemyState.Invesetigating:
        //        Investigate();
        //        break;
        //    case EnemyState.Chasing:
        //        Chase();
        //        break;
        //}
    }

    public void StartPatrolling()
    {
        enemyState = EnemyState.Patrolling;
    }

    public void StartInvestigating(Vector2 position)
    {
        enemyState = EnemyState.Invesetigating;
        investigationLocation = position;
    }

    public void StartChasing()
    {
        enemyState = EnemyState.Chasing;
    }

    private void Patrol()
    {
        Vector2 destination = waypoints[currentWaypoint].position;
        bool inRange = MoveWithinRangeOf(destination);

        if (inRange)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
                currentWaypoint = 0;
        }
    }

    private void RotateTo(Vector2 destination)
    {
        Vector2 position = transform.position;
        Vector2 direction = (destination - position).normalized;

        float tragectory = Vector2.SignedAngle(transform.up, direction);
        if (tragectory > 10)
        {
            rb.angularVelocity = turnSpeed;
        }
        else if (tragectory < -10)
        {
            rb.angularVelocity = -turnSpeed;
        }
        else
        {
            rb.angularVelocity = rb.angularVelocity * 0.9f;
        }
    }

    private bool MoveWithinRangeOf(Vector2 destination)
    {
        if (Vector2.Distance(transform.position, destination) > minDestinationDistance)
        {
            RotateTo(destination);
            rb.velocity = transform.up * speed;
            return false;
        }
        return true;
    }

    private void Investigate()
    {
        bool inRange = MoveWithinRangeOf(investigationLocation);
        if (inRange)
        {
            rb.velocity = rb.velocity * 0.9f;
            rb.angularVelocity = turnSpeed;


            //Physics2D.Raycast(transform.position, transform.up, visionDistance, raycastLayer);
        }
    }

    private void Chase()
    {

    }
}
