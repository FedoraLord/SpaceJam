﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour {

    #region Character Movement
    public Rigidbody2D rb;
    public float maxSpeed;
    public float acceleration;
    public float forwardResistance;
    public float turnSpeed;
    public float turnResistance;
    #endregion
    #region  Character Animation
    public Animator animator;
    public RuntimeAnimatorController animationController;
    public AnimatorOverrideController hurtAnimationController;
    #endregion
    public float hurtTime;
    public GameController gc;



	void Start () {
		
	}
	
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Accelerate();
        }
        else
        {
            Deccelerate();
        }

        int rotation = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rotation += 1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rotation -= 1;
        }
        Turn(rotation);

        if (Input.GetKey(KeyCode.Space))
        {
           
        }
    }

    private void Accelerate()
    {
        Vector2 acc = (transform.up * acceleration / 100);
        Vector2 vel = rb.velocity + acc;

        if (vel.magnitude >= maxSpeed)
            vel = transform.up * maxSpeed;

        rb.velocity = vel;
    }

    private void Deccelerate()
    {
        rb.velocity = rb.velocity * 0.9f;
    }

    private void Turn(int rotation)
    {
        if (rotation == 0)
            rb.angularVelocity = 0;
        else
            rb.angularVelocity = turnSpeed * rotation;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "whatever you name bullets")
        {
            HandleHurt();
        }
    }

    private void HandleHurt()
    {

    }
    private void Ping()
    {
        var closestObject = gc.objectives.Where(x => x.IsActive).OrderBy(y => Vector2.Distance(transform.position, y.transform.position)).FirstOrDefault();
        
        //Show the UI hud and the direction to point it
        
    }
    
    
}


