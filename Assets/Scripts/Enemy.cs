using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Patrolling, Investigating, Chasing, Stunned }
    public EnemyState enemyState = EnemyState.Patrolling;
    public GameObject player;
    public Rigidbody2D rb;
    public Animator animator;
    public Transform sight;

    public Transform[] waypoints;
    private int currentWaypoint;

    private Vector2 investigationPoint;
    private bool reachedInvestigationPoint;

    public float speed;
    public float turnSpeed;
    public float minDestinationDistance;
    public float minPlayerDistance;
    public float getAwayDistance;
    public float viewConeAngle;
    public float stunTime;

    public float visionDistance;
    public ContactFilter2D raycastFilter;

    private bool isAiming;
    private bool isFiring;
    public float chargeTime;
    public float fireLatency;
    public GameObject aimBeam;
    public GameObject deathBeam;
    public GameObject deathBlast;
    public GameObject deathHit;
    public float beamDistance;

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
            case EnemyState.Investigating:
                Investigate();
                break;
            case EnemyState.Chasing:
                Chase();
                break;
        }

        if (enemyState != EnemyState.Chasing && enemyState != EnemyState.Stunned && IsPlayerInSight())
            StartChasing();

        aimBeam.SetActive(isAiming);
        deathBeam.SetActive(isFiring);
        deathBlast.SetActive(isFiring);
        deathHit.SetActive(isFiring);
    }

    private bool IsPlayerInSight()
    {
        Vector2 lineToPlayer = (player.transform.position - sight.position);
        if (lineToPlayer.magnitude > visionDistance)
            return false;

        Vector2 lineOfSight = (sight.position + transform.up) - sight.position;
        float angle = Vector2.Angle(lineOfSight, lineToPlayer);
        if (angle < viewConeAngle)
        {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            int numHits = Physics2D.Raycast(sight.position, lineToPlayer, raycastFilter, hits, beamDistance);
            if (numHits > 0 && hits[0].transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    public void StartPatrolling()
    {
        enemyState = EnemyState.Patrolling;
    }

    public void StartInvestigating(Vector2 position)
    {
        if (enemyState != EnemyState.Chasing && enemyState != EnemyState.Stunned)
        {
            enemyState = EnemyState.Investigating;
            investigationPoint = position;
            reachedInvestigationPoint = false;
        }
    }

    public void StartChasing()
    {
        enemyState = EnemyState.Chasing;
        CancelInvoke("StartPatrolling");
    }

    private void Patrol()
    {
        Vector2 destination = waypoints[currentWaypoint].position;
        bool inRange = MoveWithinRangeOf(destination, minDestinationDistance);

        if (inRange)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
                currentWaypoint = 0;
        }
    }

    private void RotateTo(Vector2 destination, bool distanceBasedTurnSpeed)
    {
        Vector2 position = transform.position;
        Vector2 direction = (destination - position).normalized;

        float tragectory = Vector2.SignedAngle(transform.up, direction);
        float distance = Vector2.Distance(position, destination);
        float speed = distanceBasedTurnSpeed ? turnSpeed * distance / 10 : turnSpeed;

        if (tragectory > Math.Max(distance, 2))
        {
            rb.angularVelocity = speed;
        }
        else if (tragectory < Math.Min(-distance, -2))
        {
            rb.angularVelocity = -speed;
        }
        else
        {
            rb.angularVelocity = rb.angularVelocity * 0.9f;
        }
    }

    private bool MoveWithinRangeOf(Vector2 destination, float range)
    {
        if (Vector2.Distance(transform.position, destination) > range)
        {
            RotateTo(destination, true);
            rb.velocity = transform.up * speed;
            return false;
        }
        return true;
    }

    private void Investigate()
    {
        bool inRange = MoveWithinRangeOf(investigationPoint, minDestinationDistance);
        if (inRange)
        {
            if (!reachedInvestigationPoint)
            {
                reachedInvestigationPoint = true;
                Invoke("StartPatrolling", 3);
            }
            rb.velocity = rb.velocity * 0.9f;
            rb.angularVelocity = turnSpeed;
        }
    }

    private void Chase()
    {
        bool inRangeOrShooting = true;
        if (!isFiring && !isAiming)
        {
            inRangeOrShooting = MoveWithinRangeOf(player.transform.position, minPlayerDistance);
        }

        if (inRangeOrShooting)
        {
            rb.velocity = new Vector2();

            if (!isFiring)
            {
                if (!isAiming)
                {
                    isAiming = true;
                    Invoke("StartFiring", chargeTime);
                }
                Aim();
            }
            else
            {
                Fire();
            }
        }
        else if (Vector2.Distance(sight.position, player.transform.position) > getAwayDistance)
        {
            StartPatrolling();
        }
    }

    private void Aim()
    {
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int numHits = Physics2D.Raycast(sight.position, transform.up, raycastFilter, hits, beamDistance);

        Vector3 scale = aimBeam.transform.localScale;
        if (numHits > 0)
            scale.y = hits[0].distance;
        else
            scale.y = beamDistance;

        if (numHits > 0 && hits[0].transform.CompareTag("Player"))
            rb.angularVelocity = 0;
        else
            RotateTo(player.transform.position, false);

        aimBeam.transform.localScale = scale;
        aimBeam.transform.localPosition = new Vector2(0, scale.y / 2);
    }

    private void StartFiring()
    {
        isFiring = true;
        isAiming = false;
    }

    private void Fire()
    {
        rb.angularVelocity = 0;

        RaycastHit2D[] hits = new RaycastHit2D[1];
        int numHits = Physics2D.Raycast(sight.position, transform.up, raycastFilter, hits, beamDistance);

        Vector3 scale = deathBeam.transform.localScale;
        if (numHits > 0)
        {
            scale.y = hits[0].distance;
            deathHit.transform.position = hits[0].point;
            if (hits[0].transform.CompareTag("Player"))
            {
                player.GetComponent<Player>().TakeDamage();
            }
            //else if (hits[0].transform.CompareTag("Asteroid"))
            //{
            //    //break asteroid?
            //}
        }
        else
        {
            scale.y = beamDistance;
            deathHit.transform.position = (sight.position + transform.up * beamDistance);
        }

        deathBeam.transform.localScale = scale;
        deathBeam.transform.localPosition = new Vector2(0, scale.y / 2);
        Invoke("StopFiring", fireLatency);
    }

    private void StopFiring()
    {
        isFiring = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stun Bolt"))
        {
            Stun();
            collision.gameObject.SetActive(false);
        }
    }

    public void Stun()
    {
        CancelInvoke();
        isAiming = false;
        isFiring = false;
        enemyState = EnemyState.Stunned;
        animator.SetBool("IsShocked", true);
        Invoke("FinishStun", stunTime);
    }

    private void FinishStun()
    {
        animator.SetBool("IsShocked", false);
        StartPatrolling();
    }
}
