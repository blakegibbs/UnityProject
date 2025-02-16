using UnityEngine;

public class DogMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private float originalMoveSpeed;
    public float targetDistance;
    public float moveDelay;
    public Transform target;
    public float timeUntilIdle;
    public bool isWaiting = false;
    bool isFacingWall = false;

    [Header("Jump")]
    bool isJumping = false;
    public float jumpForce = 10f;  // How high the dog jumps
    public float gravityScale = 3f;  // Control gravity for a smoother jump

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
        isWaiting = false;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;  // Set custom gravity scale for smoother jumping
    }

    private void Update()
    {
        if (!isWaiting)
        {
            if(!isFacingWall)
            {
                Move();

            }
        }

        CheckSurroundings();
        IsGrounded();
    }

    private void Move()
    {
        if (moveSpeed > 0)
        {
            originalMoveSpeed = moveSpeed;
        }

        float horizontalDistanceToTarget = target.position.x - transform.position.x;
        float verticalDistanceToTarget = target.position.y - transform.position.y;

        if (target != null)
        {
            // Move towards the target horizontally whether grounded or falling
            if (Mathf.Abs(horizontalDistanceToTarget) > targetDistance && !isNearDrop)
            {
                moveDelayTimer += Time.deltaTime;
                if (moveDelayTimer >= moveDelay)
                {
                    // Move horizontally towards target
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
                // Stop horizontal movement when near the target
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

        // Flip character if necessary
        if ((horizontalDistanceToTarget > 0 && !facingRight) || (horizontalDistanceToTarget < 0 && facingRight))
        {
            Flip();
        }

        // Only apply gravity if not jumping and not grounded
        if (!IsGrounded() && !isJumping)
        {
            // Fall down with gravity, no jump applied
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, horizontalDistanceToTarget * moveSpeed/2, moveSpeed * Time.deltaTime), rb.velocity.y);
        }
        else if (IsGrounded())
        {
            // On the ground, horizontal movement is allowed again
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, horizontalDistanceToTarget * moveSpeed, moveSpeed * Time.deltaTime), rb.velocity.y);
        }
    }

    // Handle jump when the player presses the jump key or when the dog detects a wall
    public void Jump()
    {
        if (IsGrounded() && !isJumping)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // Apply initial upward force
            animator.SetTrigger("Jump");
        }
    }

    private void CheckSurroundings()
    {
        Vector2 rayPosition = Vector2.zero;
        Vector2 wallRayPosition = Vector2.zero;
        if (facingRight)
        {
            rayPosition = new Vector2(transform.position.x + 1, transform.position.y);
        }
        else
        {
            rayPosition = new Vector2(transform.position.x - 1, transform.position.y);
        }

        wallRayPosition = new Vector2(rayPosition.x, rayPosition.y - 0.15f);
        RaycastHit2D wallCheckerHit = Physics2D.Raycast(wallRayPosition, Vector2.right, 10, groundLayer);

        RaycastHit2D floorCheckerHit = Physics2D.Raycast(rayPosition, -Vector2.up, 100, groundLayer);

        if (floorCheckerHit.distance >= dropOffThreshold)
        {
            isNearDrop = true;
        }
        else
        {
            isNearDrop = false;
        }

        if (wallCheckerHit.distance < 0.5f && wallCheckerHit.transform != null)  // Adjust the threshold for wall detection
        {
            isFacingWall = true;
            // Jump before hitting the wall
            if (!isJumping && IsGrounded())
            {
                Jump();
            }
        }
        else
        {
            isFacingWall = false;
        }
    }

    private bool IsGrounded()
    {
        // Check if the dog is grounded using a raycast
        isJumping = false;

        // Calculate the ray start position at the center of the groundCheckCollider
        Vector2 rayStart = groundCheckCollider.bounds.center;

        // Cast a ray downwards to check if the dog is touching the ground
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, groundCheckCollider.bounds.extents.y + 0.1f, groundLayer);

        // Check if the ray hit something on the groundLayer
        return hit.collider != null;
    }


    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaitZone"))
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Moving", false);
            animator.speed = 1;
            isWaiting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WaitZone"))
        {
            isWaiting = false;
        }
    }
}
