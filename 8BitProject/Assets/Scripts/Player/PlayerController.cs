using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;

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
    private bool facingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Movement();
        Jump();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        if(Mathf.Abs(rb.velocity.x) > 0.01f && isGrounded)
        {
            animator.speed = walkAnimationSpeedMultiplier;
        }
        else
        {
            animator.speed = 1;
        }

        if (horizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && facingRight)
        {
            Flip();
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
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            jumpBufferCounter = 0;
        }

        if (Input.GetButtonUp("Jump") && isJumping)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
            isJumping = false;
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

    private void OnDrawGizmos()
    {
        if (groundCheckCollider != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(groundCheckCollider.bounds.center, groundCheckCollider.bounds.size);
        }
    }
}
