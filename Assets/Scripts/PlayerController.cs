using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb;
    public float maxSpeed;
    public float acceleration;
    public float resistance;
    public float rotateSpeed;

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
    }

    private void Accelerate()
    {
        Vector2 add = transform.up * acceleration / 100;
        rb.velocity += add;
    }

    private void Deccelerate()
    {
        rb.velocity = rb.velocity * 0.9f;
    }
}
