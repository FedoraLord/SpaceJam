using Assets.Scripts;
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
    public RaycastHit2D[] hits;
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
        if (Input.GetKey(KeyCode.Space))
        {
            hits = Physics2D.RaycastAll(transform.position, transform.up, 100.0F);
            GetHitGameObjects(hits);
        }
    }

    private void Accelerate()
    {
        Vector2 acc = (transform.up * acceleration / 100);
        Vector2 vel = rb.velocity + acc;

        if (vel.magnitude >= maxSpeed)
            vel = transform.up * maxSpeed;

        rb.velocity = acc;
    }

    private void Deccelerate()
    {
        rb.velocity = rb.velocity * 0.9f;
    }

    private Dictionary<GameObject, GameObjectTypes> GetHitGameObjects(RaycastHit2D[] hit2D)
    {
        Dictionary<GameObject, GameObjectTypes> gameObjects = new Dictionary<GameObject, GameObjectTypes>(); 

        for(int i = 0; i <= hit2D.Length; i++)
        {
            RaycastHit2D hit = hit2D[i];
            gameObjects.Add(hit.transform.gameObject, (GameObjectTypes)Enum.Parse(typeof(GameObjectTypes),hit.transform.gameObject.tag));
            ObstaclesController obstacle = hit.transform.GetComponent<ObstaclesController>();
        }
        return gameObjects;
    }



}


