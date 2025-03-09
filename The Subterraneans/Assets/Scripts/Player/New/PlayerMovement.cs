using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 8f;
    private float horizontal;
    private float vertical;
    [SerializeField] private float walkingSpeed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [HideInInspector] public bool isFacingRight = true;
    private bool isMovementDisabled = false;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Double Jump")]
    private bool canDoubleJump;

    [Header("Wall Jump")]
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private float wallClimbingSpeed = 2f;
    private bool isWallSliding;
    private bool isWallClimbing;
    private bool isWallJumping;
    private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [Header("Checks")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallClimbLayer;
    private bool wasGrounded;

    [Header("Dash")]
    [SerializeField] private float dashingPower = 24f;
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    [Header("Unlocks")]
    public bool canUnlockDoubleJump;
    public bool canUnlockDash;
    public bool canUnlockWallJump;
    public bool canUnlockWallClimb;
    public bool doubleJumpUnlocked;
    public bool wallClimbUnlocked;
    public bool wallJumpUnlocked;
    public bool dashUnlocked;
    public bool lightUnlocked;

    [Header("Effects")]
    public CameraShake cameraShake;
    public float jumpCameraShakeDuration;
    public GameObject jumpingDust, landingDust, doubleJumpDust, runningDust;
    public float footstepInterval = 0.5f;
    private float footstepTimer = 0f;

    [Header("Animation")]
    public Animator animator;
    public float animationSpeedMultiplier = 4f;
    private bool isWalking;
    private bool isWallHanging;

    [Header("Platforms")]
    public TilemapCollider2D platformCollider;

    private void Start()
    {
        canDoubleJump = doubleJumpUnlocked;
    }

    private void Update()
    {
        if (isMovementDisabled)
        {
            return;
        }

        bool isCurrentlyGrounded = IsGrounded();

        if (!wasGrounded && isCurrentlyGrounded)
        {
            GameObject dust = Instantiate(landingDust, groundCheck.position, Quaternion.identity);
            Destroy destroyComp = dust.AddComponent<Destroy>();
            destroyComp.timer = 0.5f;
        }

        wasGrounded = isCurrentlyGrounded;

        UpdateAnimations();
        WalkingEffects();

        if (isDashing)
        {
            return;
        }

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            if (doubleJumpUnlocked)
            {
                canDoubleJump = true;
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && horizontal != 0f && dashUnlocked)
        {
            cameraShake.shakeDuration = jumpCameraShakeDuration;
            StartCoroutine(Dash());
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteTimeCounter > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                GameObject dust = Instantiate(jumpingDust, groundCheck.position, Quaternion.identity);
                Destroy destroyComp = dust.AddComponent<Destroy>();
                destroyComp.timer = 0.5f;
                coyoteTimeCounter = 0f;
            }
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                GameObject dust = Instantiate(doubleJumpDust, groundCheck.position, Quaternion.identity);
                Destroy destroyComp = dust.AddComponent<Destroy>();
                destroyComp.timer = 0.5f;
                canDoubleJump = false;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        if (wallClimbUnlocked)
        {
            WallClimb();
        }
        if (wallJumpUnlocked)
        {
            WallJump();
        }
        if (!isWallJumping)
        {
            Flip();
        }

        isWalking = Mathf.Abs(rb.velocity.x) >= 0.01f;
    }

    private void FixedUpdate()
    {
        if (isMovementDisabled)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        if (isDashing)
        {
            return;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void UpdateAnimations()
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetBool("IsWallSliding", isWallSliding);
        animator.SetBool("IsWallClimbing", isWallClimbing);
        if(isWallHanging)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }
    }

    public void ToggleMovementDisabled()
    {
        isMovementDisabled = !isMovementDisabled;
        Debug.Log("movement is disabled" + isMovementDisabled);
        groundCheck.gameObject.SetActive(!groundCheck.gameObject.activeInHierarchy);
        wallCheck.gameObject.SetActive(!wallCheck.gameObject.activeInHierarchy);
        rb.velocity = Vector2.zero;
        animator.Play("PlayerIdle");
        animator.SetBool("isWalking", false);
        if(animator.speed == 1)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
    }
    private bool IsWallClimbable()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallClimbLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f && !isWallClimbing)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void WallClimb()
    {
        if (IsWallClimbable() && horizontal != 0f && vertical > 0f)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            isWallClimbing = true;
            isWallHanging = false;
            rb.velocity = new Vector2(rb.velocity.x, wallClimbingSpeed);
        }
        else if (IsWallClimbable() && horizontal != 0f && vertical == 0f)
        {
            isWallClimbing = true;
            isWallHanging = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            isWallClimbing = false;
            isWallHanging = false;
        }
    }


    private void WallJump()
    {
        if (isWallSliding || isWallClimbing)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            cameraShake.shakeDuration = jumpCameraShakeDuration;
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        speed = dashingPower;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        speed = walkingSpeed;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Platforms") && vertical <= -0.1f)
        {
            platformCollider.enabled = false;
        }
        else if (collision.CompareTag("Platforms"))
        {
            platformCollider.enabled = true;
        }

        if (collision.CompareTag("ActivateLight") && lightUnlocked)
        {
            this.GetComponent<Light2D>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ActivateLight") && lightUnlocked)
        {
            this.GetComponent<Light2D>().enabled = false;
        }
    }


    private void WalkingEffects()
    {
        if (IsGrounded() && Mathf.Abs(horizontal) > 0.01f) // Only play footsteps when grounded and moving
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                SpawnRunningDust();
                footstepTimer = footstepInterval; // Reset timer after spawning
            }
        }
        else
        {
            footstepTimer = 0; // Ensure timer resets when not moving
        }
    }

    private void SpawnRunningDust()
    {
        GameObject dust = Instantiate(runningDust, groundCheck.position, Quaternion.identity);

        // Flip dust effect if player is facing left
        if (!isFacingRight)
        {
            dust.transform.localScale = new Vector3(-1, 1, 1);
        }

        Destroy(dust, 0.5f); // Destroy dust after 0.5 seconds
    }

}
