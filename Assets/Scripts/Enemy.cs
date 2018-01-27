using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypoint;
    private Vector2 investigationLocation;
    public enum EnemyState { Patrolling, Invesetigating, Chasing }
    public EnemyState enemyState = EnemyState.Patrolling;

    private void FixedUpdate()
    {
        //if ()
    }

    public void Investigate()
    {

    }

    public void Chase()
    {

    }
}
