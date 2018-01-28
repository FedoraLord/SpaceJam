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
    public float pingAggroDistance;

    public Transform boltLocation;
    public float boltSpeed;
    public float boltCooldown;
    private bool canShoot = true;
    private List<GameObject> bolts = new List<GameObject>();

    #region Player Stats
    public GameObject playerEnergy;
    int energy = 5000;
    #endregion
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Accelerate();
            RemoveEnergy(1);
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Ping();
            RemoveEnergy(20);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Energy")
        {
            MaxEnergy();
            Destroy(collision.gameObject);
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
    
    public void TakeDamage()
    {
        if (!isHurt)
        {
            isHurt = true;
            animator.runtimeAnimatorController = hurtAnimationController;
            Invoke("StopDamage", hurtTime);
        }
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

        var enemies = gc.enemies.Where(x => Vector2.Distance(x.transform.position, transform.position) < pingAggroDistance).ToList();
        for (int i = 0; i < enemies.Count(); i++)
        {
            enemies[i].StartInvestigating(transform.position);
        }
    }

    private void RemoveEnergy(int _energy)
    {
        energy = energy - _energy;
        var scale = (40 * energy) / 5000;
        playerEnergy.transform.localScale = new Vector3(playerEnergy.transform.localScale.x, scale, playerEnergy.transform.localScale.z);
        //TODO: Learnd Da-Wae to do this part 
        //playerEnergy.transform.localPosition = new Vector3(playerEnergy.transform.localPosition.x, -(40 - scale * 3.5f), playerEnergy.transform.localPosition.z);
    }

    private void MaxEnergy()
    {
        playerEnergy.transform.localScale = new Vector3(40, 40, 0);
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
    
    private void Shoot()
    {
        if (canShoot)
        {
            gameObject.GetComponent<AudioSource>().Play();
            canShoot = false;
            GameObject bolt = Instantiate(gc.stunBolt, boltLocation.position, boltLocation.rotation);
            bolt.GetComponent<Rigidbody2D>().velocity = bolt.transform.up * boltSpeed;
            bolts.Add(bolt);
            Invoke("FinishBoltCooldown", boltCooldown);
            Invoke("DeleteBolt", 2);
        }
    }

    private void FinishBoltCooldown()
    {
        canShoot = true;
    }

    private void DeleteBolt()
    {
        Destroy(bolts[0]);
        bolts.RemoveAt(0);
    }
}


