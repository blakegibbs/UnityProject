using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // max speed
    public float jumpForce = 12f; // force applied to jump
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public float ledgeHangDistance = 1.0f;
    public float ledgeCheckHeight = 1.0f;

    // control over jump behavior
    public float maxJumpHeight = 4f; // maximum jump height
    public float timeToApex = 0.4f; // time to reach apex (in seconds)

    public float fallMultiplier = 2.5f; // increase falling speed after reaching apex
    public float lowJumpMultiplier = 2f; // apply this multiplier when jumping upwards

    [Header("Acceleration and Deceleration")]
    public float groundAcceleration = 10f;
    public float groundDeceleration = 8f;

    [Header("Ground Check")]
    public BoxCollider2D groundCheckCollider;
    public LayerMask groundLayer;
    public float rayLength = 0.1f;

    [Header("Animation")]
    private Animator animator;
    public float walkAnimationSpeedMultiplier = 4f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping;
    private bool isLedgeHanging;
    private bool facingRight = true;

    private float currentVelocityX = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isLedgeHanging)
        {
            Movement();
            Jump();
        }
        else
        {
            LedgeHang();
        }
    }

    private void Movement()
    {
        if (!isLedgeHanging)
        {
            // horizontal movement input
            float horizontalInput = Input.GetAxis("Horizontal");

            // if grounded, apply acceleration or deceleration
            if (isGrounded)
            {
                // accelerate or decelerate
                if (Mathf.Abs(horizontalInput) > 0.01f)
                {
                    currentVelocityX = Mathf.MoveTowards(currentVelocityX, horizontalInput * moveSpeed, groundAcceleration * Time.deltaTime);
                }
                else
                {
                    currentVelocityX = Mathf.MoveTowards(currentVelocityX, 0f, groundDeceleration * Time.deltaTime);
                }
            }
            else
            {
                // while in the air, apply some air control (less responsive)
                currentVelocityX = Mathf.MoveTowards(currentVelocityX, horizontalInput * moveSpeed, groundAcceleration * Time.deltaTime);
            }

            // update Rigidbody velocity
            rb.velocity = new Vector2(currentVelocityX, rb.velocity.y);

            // update animation speed
            if (Mathf.Abs(rb.velocity.x) > 0.01f && isGrounded)
            {
                animator.speed = walkAnimationSpeedMultiplier;
            }
            else
            {
                animator.speed = 1;
            }

            // flip the character if needed
            if (horizontalInput > 0 && !facingRight)
            {
                Flip();
            }
            else if (horizontalInput < 0 && facingRight)
            {
                Flip();
            }
        }
    }

    private void Jump()
    {
        animator.SetFloat("Vertical", Mathf.Abs(rb.velocity.y));
        isGrounded = IsGrounded();
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            StartJump();
            jumpBufferCounter = 0;
        }

        if (Input.GetButtonUp("Jump") && isJumping)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // reduce upward speed for "short jump"
            }
            isJumping = false;
        }

        ApplyJumpApexGravity();
    }

    private void StartJump()
    {
        isJumping = true;

        // calculate gravity based on the desired time to apex
        float gravity = 2 * maxJumpHeight / Mathf.Pow(timeToApex, 2);
        Physics2D.gravity = new Vector2(Physics2D.gravity.x, -gravity);

        // calculate the initial jump velocity
        float initialJumpVelocity = Mathf.Abs(Physics2D.gravity.y) * timeToApex;

        // apply the jump velocity
        rb.velocity = new Vector2(rb.velocity.x, initialJumpVelocity);
    }

    private void ApplyJumpApexGravity()
    {
        // apply gravity adjustments based on jump state
        if (rb.velocity.y < 0) // falling
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // going up but releasing jump (short jump)
        {
            rb.gravityScale = lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = 1; // normal gravity when rising
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

    private void LedgeHang()
    {
        // ledge hang logic: player is hanging if the conditions are met.
        if (Input.GetButtonDown("Jump"))
        {
            isLedgeHanging = false;
            rb.velocity = new Vector2(0, jumpForce); // push the player off the ledge
        }

        // player remains at the ledge, not moving
        rb.velocity = new Vector2(0, rb.velocity.y); // prevent horizontal movement while hanging
    }

    private void CheckLedge()
    {
        // cast a ray to check if the ledge exists (below the player)
        RaycastHit2D ledgeHit = Physics2D.Raycast(transform.position, Vector2.down, ledgeHangDistance, groundLayer);

        // cast a ray upwards to ensure it's not a wall above the player
        RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, Vector2.up, ledgeCheckHeight, groundLayer);

        if (ledgeHit.collider != null && wallCheck.collider == null && !isGrounded)
        {
            isLedgeHanging = true;
            animator.SetBool("isLedgeHanging", true);
        }
        else
        {
            isLedgeHanging = false;
            animator.SetBool("isLedgeHanging", false);
        }

        Debug.DrawRay(transform.position, Vector2.down * ledgeHangDistance, Color.blue);
        Debug.DrawRay(transform.position, Vector2.up * ledgeCheckHeight, Color.red);
    }

    private void OnDrawGizmos()
    {
        if (groundCheckCollider != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(groundCheckCollider.bounds.center, groundCheckCollider.bounds.size);
        }
    }
}