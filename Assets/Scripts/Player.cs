using System;
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
    public AnimatorOverrideController normalAnimationController;
    public AnimatorOverrideController hurtAnimationController;
    #endregion
    public float hurtTime;
    private bool isHurt;
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
            TakeDamage();
        }
        
    }

    private void Accelerate()
    {
        animator.SetBool("IsThrusting", true);
        Vector2 acc = (transform.up * acceleration / 100);
        Vector2 vel = rb.velocity + acc;

        if (vel.magnitude >= maxSpeed)
            vel = transform.up * maxSpeed;//this line makes it lose velocity at an angle while drifting

        rb.velocity = vel;
    }

    private void Deccelerate()
    {
        animator.SetBool("IsThrusting", false);
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
            if (!isHurt)
            {
                TakeDamage();
                Invoke("StopDamage", hurtTime);
            }
        }
    }

    private void TakeDamage()
    {
        isHurt = true;
        animator.runtimeAnimatorController = hurtAnimationController;
    }

    private void StopDamage()
    {
        isHurt = false;
        animator.runtimeAnimatorController = normalAnimationController;
    }

    private void Ping()
    {
        var closestObject = gc.objectives.Where(x => x.IsActive).OrderBy(y => Vector2.Distance(transform.position, y.transform.position)).FirstOrDefault();
        
        //Show the UI hud and the direction to point it
        
    }
    
    
}


