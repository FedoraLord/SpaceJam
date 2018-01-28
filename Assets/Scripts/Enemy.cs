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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        switch (enemyState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Invesetigating:
                Investigate();
                break;
            case EnemyState.Chasing:
                Chase();
                break;
        }
    }

    private void Patrol()
    {
        Vector2 destination = waypoints[currentWaypoint].position;
        if (Vector2.Distance(transform.position, destination) > minDestinationDistance)
        {
            RotateTo(destination);
            rb.velocity = transform.up * speed;
        }
        else
        {

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

    public void Investigate()
    {

    }

    public void Chase()
    {

    }
}
