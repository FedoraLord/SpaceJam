using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour {

    public Rigidbody2D rb;
    public Animator animator;
    public RuntimeAnimatorController animationController;
    public AnimatorOverrideController hurtAnimationController;
    public float maxSpeed;
    public float acceleration;
    public float forwardResistance;
    public float turnSpeed;
    public float turnResistance;
<<<<<<< HEAD
    public float HurtTime;
=======
    public RaycastHit2D[] hits;
    public GameController gc;
    public float smallPingDistance;
    public float mediumPingDistance;
>>>>>>> aadbda1952c5c95e9d9e0148a86ef6a8954c6913

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
            RaycastHit2D[] hits;
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

<<<<<<< HEAD
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
=======
    private void Ping()
    {
        var closestObject = gc.objectives.Where(x => x.IsActive).OrderBy(y => Vector2.Distance(transform.position, y.transform.position)).FirstOrDefault();
        var distance = Vector2.Distance(transform.position, closestObject.transform.position);
        //Show the UI hud and the direction to point it
        if (distance < smallPingDistance)
        {
        }
        else if(distance < mediumPingDistance)
        {

        }
        else
        {

        }
    }

>>>>>>> aadbda1952c5c95e9d9e0148a86ef6a8954c6913
    
}


