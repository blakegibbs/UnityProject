using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float targetDistance;
    public float moveDelay;
    public Transform target; // The player transform
    public float timeUntilIdle;

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

    private float idleTimer = 0f;
    private float moveDelayTimer = 0f;
    private bool isMoving = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Move();
        CheckSurroundings();
        IsGrounded();
    }

    private void Move()
    {
        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (distanceToTarget > targetDistance)
            {
                moveDelayTimer += Time.deltaTime;
                if (moveDelayTimer >= moveDelay)
                {
                    Vector2 direction = (target.position - transform.position).normalized;
                    Vector2 newPosition = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

                    transform.position = newPosition;
                    animator.SetBool("Idle", false);
                    animator.speed = walkAnimationSpeedMultiplier;

                    if ((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
                    {
                        Flip();
                    }

                    idleTimer = 0f;
                    isMoving = true;
                }
            }
            else
            {
                moveDelayTimer = 0f;

                idleTimer += Time.deltaTime;

                if (idleTimer >= timeUntilIdle)
                {
                    animator.speed = 1;
                    animator.SetBool("Idle", true);
                    isMoving = false;
                }
            }
        }
    }

    private void CheckSurroundings()
    {
        RaycastHit2D wallCheckerHit = Physics2D.Raycast(transform.position, Vector2.right, 10, groundLayer);
        Vector2 rayPosition = new Vector2(transform.position.x + 1, transform.position.y);
        RaycastHit2D floorCheckerHit = Physics2D.Raycast(rayPosition, Vector2.up, 10, groundLayer);
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
