using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPlayer : MonoBehaviour {

    public Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        rb.angularVelocity = 25;
        rb.velocity = new Vector2(1.7f, 0.2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
