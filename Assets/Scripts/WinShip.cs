using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinShip : MonoBehaviour {

    public Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        rb.velocity = new Vector2(1, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
