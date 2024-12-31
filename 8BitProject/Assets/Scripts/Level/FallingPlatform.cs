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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = timeUntilPlatformFalls;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            activated = true;
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
        if (activated)
        {
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
