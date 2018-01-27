using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Experimental.UIElements;

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

    #region Hud Directions 
    public GameObject upIndicator;
    public GameObject downIndicator;
    public GameObject leftIndicator;
    public GameObject rightIndicator;
    GameObject activeIndicator = null;
    #endregion



    void Start () {
        upIndicator.SetActive(false);
        downIndicator.SetActive(false);
        leftIndicator.SetActive(false);
        rightIndicator.SetActive(false);

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
            Ping();
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
        var closestObject = gc.objectives.Where(x => x.IsActive && x.gameObject.tag == "Goal").OrderBy(y => Vector2.Distance(transform.position, y.transform.position)).FirstOrDefault();
        if (closestObject != null)
        {
            Vector2 objectDirection = GetDirectionOfObject(closestObject.gameObject);
            SetActiveHUDDirection(objectDirection);
            Invoke("DisableActiveIndicator", 3);
        }
    }

    private void SetActiveHUDDirection(Vector2 direction)
    {
        if(direction.x == -1)
        {
            leftIndicator.SetActive(true);
            activeIndicator = leftIndicator;
        }
        if(direction.x == 1)
        {
            rightIndicator.SetActive(true);
            activeIndicator = rightIndicator;
        }
        if(Math.Ceiling(direction.y) == 1)
        {
            upIndicator.SetActive(true);
            activeIndicator = upIndicator;
        }
        if(direction.y == -1)
        {
            downIndicator.SetActive(true);
            activeIndicator = downIndicator;
        }
    }

    private void DisableActiveIndicator()
    {
        activeIndicator.SetActive(false);
    }
    
    private Vector2 GetDirectionOfObject(GameObject obj)
    {
        Vector2 myPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Vector2 objPos = new Vector2(obj.transform.position.x, obj.transform.position.y);
        var heading = objPos - myPos;
        
        return heading.normalized;
    }
    
    
    
}


