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
    public AnimatorOverrideController normalAnimationController;
    public AnimatorOverrideController hurtAnimationController;
    #endregion

    public float hurtTime;
    private bool isHurt;
    public GameController gc;

    #region Hud Directions 
    public GameObject upIndicator;
    public GameObject downIndicator;
    public GameObject leftIndicator;
    public GameObject rightIndicator;
    GameObject activeIndicator = null;
    #endregion

    public Transform pulseLocation;
    public float pulseInterval;
    private int numPulses;
    public float pingTime;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ping();
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
        StartPulsing();

        var closestObject = gc.objectives.Where(x => x.IsActive && x.gameObject.tag == "Goal").OrderBy(y => Vector2.Distance(transform.position, y.transform.position)).FirstOrDefault();
        if (closestObject != null)
        {
            Vector2 objectDirection = GetDirectionOfObject(closestObject.gameObject);
            SetActiveHUDDirection(objectDirection);
        }
    }

    private void StartPulsing()
    {
        if (!IsInvoking("SendPulse"))
        {
            numPulses = 0;
            InvokeRepeating("SendPulse", 0, pulseInterval);
        }
    }

    private void SendPulse()
    {
        Instantiate(gc.pulse, pulseLocation.position, pulseLocation.rotation);
        numPulses++;
        if (numPulses >= 3)
            CancelInvoke("SendPulse");
    }

    private void SetActiveHUDDirection(Vector2 direction)
    {
        if (Math.Abs(direction.x) > Math.Abs(direction.y))
        {
            if (direction.x > 0)
                activeIndicator = rightIndicator;
            else
                activeIndicator = leftIndicator;
        }
        else
        {
            if (direction.y > 0)
                activeIndicator = upIndicator;
            else
                activeIndicator = downIndicator;
        }

        activeIndicator.SetActive(true);
        Invoke("DisableActiveIndicator", pingTime);
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


