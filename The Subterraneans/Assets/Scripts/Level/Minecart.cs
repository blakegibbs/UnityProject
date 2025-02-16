using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour
{
    public Transform mountPosition;
    public Transform destinationPosition;
    public Transform passanger;
    public float speed = 5f;
    public float maxSpeed = 10f;
    public float acceleration = 0.1f;
    bool occupied;
    bool destinationReached;

    private float currentSpeed;

    private void Start()
    {
        destinationReached = false;
        occupied = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            occupied = true;
        }
    }

    private void Update()
    {
        if (occupied && !destinationReached)
        {
            Move();
        }

        if (destinationReached)
        {
            passanger.GetComponent<PlayerController>().enabled = true;
            passanger.GetComponent<Transform>().transform.SetParent(null);
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }

    private void Move()
    {
        passanger.GetComponent<PlayerController>().enabled = false;
        passanger.GetComponent<PlayerController>().animator.speed = 1;
        passanger.GetComponent<Transform>().transform.SetParent(transform);
        passanger.transform.position = mountPosition.position;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }

        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

        if (Mathf.Abs(transform.position.x - destinationPosition.position.x) < 0.1f)
        {
            destinationReached = true;
        }
    }
}
