using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float timeUntilPlatformFalls = 3.0f;
    public bool keepCollider = false;
    private float timer;
    private Rigidbody2D rb;
    public bool resetTimer = false;
    private bool activated = false;
    public bool destroy;
    public bool accountForMultipleEntries = false;
    public int timesActivatedTillDrop = 1;
    public int times = 0;
    bool isinside = false;
    public float delayTime = 0.2f; // Delay time in seconds
    private float timer1 = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = timeUntilPlatformFalls;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            // Only update isinside if enough time has passed
            if (timer1 <= 0f)
            {
                if (!isinside)
                {
                    times += 1;
                    isinside = true;
                }
                if (times >= timesActivatedTillDrop)
                {
                    activated = true;
                }
                timer1 = delayTime; // Reset the timer
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            // Only update isinside if enough time has passed
            if (timer1 <= 0f)
            {
                isinside = false;
                timer1 = delayTime; // Reset the timer
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if(!accountForMultipleEntries)
            {
                activated = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (resetTimer && collision.transform.CompareTag("Player"))
        {
            activated = false;
        }
    }

    private void Update()
    {
        if (timer1> 0f)
        {
            timer1 -= Time.deltaTime;
        }

        if (activated)
        {
            if(this.GetComponent<PlatformLand>() != null)
            {
                this.GetComponent<PlatformLand>().enabled = false;
                this.GetComponent<CapsuleCollider2D>().enabled = false;
            }
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                //this.GetComponent<Collider>().enabled = keepCollider;

                rb.constraints = RigidbodyConstraints2D.None;

                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

                if (destroy)
                {
                    Destroy destroyComp = this.AddComponent<Destroy>();
                    destroyComp.timer = 1f;
                    destroy = false;
                }
            }
        }
        else
        {
            timer = timeUntilPlatformFalls;
        }
    }
}
