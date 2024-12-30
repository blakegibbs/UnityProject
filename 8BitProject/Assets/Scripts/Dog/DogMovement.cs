using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float targetDistance;
    public float moveDelay;
    public Transform target;
    public float timeUntilIdle;

    [Header("Jump")]
    public float jumpForce;

    [Header("References")]
    private Transform player;
    private bool facingRight = true;

    [Header("Animator")]
    public float walkAnimationSpeedMultiplier = 4f;
    private Animator animator;

    [Header("GroundCheck")]
    public Collider2D groundCheckCollider;
    public LayerMask groundLayer;
    public float rayLength = 0.1f;
    private bool isNearDrop = false;
    public float dropOffThreshold = 2;

    private float idleTimer = 0f;
    private float moveDelayTimer = 0f;
    private bool isMoving = false;

    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        CheckSurroundings();
        IsGrounded();
    }

    private void Move()
    {
        float horizontalDistanceToTarget = target.position.x - transform.position.x;
        float verticalDistanceToTarget = target.position.y - transform.position.y;
        if (target != null)
        {
            // Horizontal Movement
            if (Mathf.Abs(horizontalDistanceToTarget) > targetDistance && !isNearDrop)
            {
                moveDelayTimer += Time.deltaTime;
                if (moveDelayTimer >= moveDelay)
                {
                    // Move horizontally
                    Vector2 newPosition = new Vector2(Mathf.MoveTowards(transform.position.x, target.position.x, moveSpeed * Time.deltaTime), transform.position.y);

                    transform.position = newPosition;
                    animator.SetBool("Idle", false);
                    animator.SetBool("Moving", true);
                    animator.speed = walkAnimationSpeedMultiplier;

   

                    idleTimer = 0f;
                    isMoving = true;
                }
            }
            else
            {
                // Stop horizontal movement
                animator.SetBool("Moving", false);
                animator.speed = 1;

                moveDelayTimer = 0f;

                // Start idle timer
                idleTimer += Time.deltaTime;

                if (idleTimer >= timeUntilIdle)
                {
                    animator.SetBool("Idle", true);
                    isMoving = false;
                }
            }
        }
        else
        {
            animator.SetBool("Idle", true);
        }

        if ((horizontalDistanceToTarget > 0 && !facingRight) || (horizontalDistanceToTarget < 0 && facingRight))
        {
            Flip();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("Jump");
    }

    private void CheckSurroundings()
    {
        Vector2 rayPosition = Vector2.zero;
        RaycastHit2D wallCheckerHit = Physics2D.Raycast(transform.position, Vector2.right, 10, groundLayer);
        if (facingRight)
        {
            rayPosition = new Vector2(transform.position.x + 1, transform.position.y);

        }
        else
        {
            rayPosition = new Vector2(transform.position.x + -1, transform.position.y);
        }
        RaycastHit2D floorCheckerHit = Physics2D.Raycast(rayPosition, -Vector2.up, 10, groundLayer);

        if(floorCheckerHit.distance >= dropOffThreshold)
        {
            isNearDrop = true;
        }
        else
        {
            isNearDrop = false;
        }
    }

    private bool IsGrounded()
    {
        Collider2D collider = Physics2D.OverlapBox(groundCheckCollider.bounds.center, groundCheckCollider.bounds.size, 0f, groundLayer);
        RaycastHit2D rayHit = Physics2D.Raycast(groundCheckCollider.bounds.center, Vector2.down, rayLength, groundLayer);
        Debug.DrawRay(groundCheckCollider.bounds.center, Vector2.down * rayLength, Color.red);
        return collider != null || rayHit.collider != null;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}