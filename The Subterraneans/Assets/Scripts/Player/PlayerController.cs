using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // max speed
    public float maxSpeed = 7f;  // clamp max speed
    public float jumpForce = 12f; // force applied to jump
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public float ledgeHangDistance = 1.0f;
    public float ledgeCheckHeight = 1.0f;
    bool canMove = true;

    // control over jump behavior
    public float maxJumpHeight = 4f; // maximum jump height
    public float timeToApex = 0.4f; // time to reach apex (in seconds)

    public float fallMultiplier = 2.5f; // increase falling speed after reaching apex
    public float lowJumpMultiplier = 2f; // apply this multiplier when jumping upwards

    [Header("Acceleration and Deceleration")]
    public float groundAcceleration = 10f;
    public float groundDeceleration = 8f;
    public float friction = 0.5f; // Friction force applied when grounded

    [Header("Ground Check")]
    public CapsuleCollider2D groundCheckCollider;
    public LayerMask groundLayer;
    public float rayLength = 0.1f;

    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;
    private float attackCooldownTimer;
    public float attackRange = 1.0f;
    public float attackDamage = 40.0f;
    private bool isAttacking = false;
    public LayerMask attackMask;
    public float attackKnockback = 1f;
    public float attackKnockbackDuration = 0.1f;
    private float attackKnockbackTimer = 0.1f;
    private bool applyingKnockback = false;
    private bool appliedKnockback = false;

    [Header("Health")]
    public float maxHealth = 100f;
    [SerializeField]private float currentHealth;
    public float knockbackEffectAmount = 3f;

    [Header("Animation")]
    [HideInInspector] public Animator animator;
    public Animator animatorAttackSlash;
    public float walkAnimationSpeedMultiplier = 4f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping;
    private bool isLedgeHanging;
    private bool facingRight = true;

    [Header("Sounds")]
    public AudioClip footsteps;
    public float footstepInterval = 0.5f;
    private float footstepTimer = 0f;
    public float landingNoisePitch = 1.5f;
    public float walkingNoisePitchUpper = 1f;
    public float walkingNoisePitchLower = 0.7f;

    private float currentVelocityX = 0f;
    private bool wasGroundedLastFrame = false;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackKnockbackTimer = attackKnockbackDuration;
    }

    private void Update()
    {
        if (canMove)
        {
            if (!isLedgeHanging)
            {
                Movement();
                Jump();
                Attack();
                WalkingEffects();
                PlayLandingSound();
            }
            else
            {
                LedgeHang();
            }
            wasGroundedLastFrame = isGrounded;
        }
    }

    private void Movement()
    {
        if (!isLedgeHanging && !applyingKnockback)
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
                    // apply friction when no input is given
                    currentVelocityX = Mathf.MoveTowards(currentVelocityX, 0f, groundDeceleration * Time.deltaTime);
                }
            }
            else
            {
                // while in the air, apply some air control (less responsive)
                currentVelocityX = Mathf.MoveTowards(currentVelocityX, horizontalInput * moveSpeed, groundAcceleration * Time.deltaTime);
            }

            // apply force for horizontal movement
            rb.velocity = new Vector2(currentVelocityX, rb.velocity.y);

            // clamp the maximum speed
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }

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

        if(applyingKnockback)
        {
            attackKnockbackTimer -= Time.deltaTime;
            if(attackKnockbackTimer < 0)
            {
                applyingKnockback = false;
                attackKnockbackTimer = attackKnockbackDuration;
            }
        }
    }

    private void WalkingEffects()
    {
        if (isGrounded && Mathf.Abs(rb.velocity.x) > 0.01f) //only play footsteps when grounded and moving
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                // Play footstep sound with random pitch
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.pitch = Random.Range(walkingNoisePitchLower, walkingNoisePitchUpper); //random pitch between 0.8 and 1.2
                audioSource.PlayOneShot(footsteps);

                // Reset the timer
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0;
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

        // apply the jump velocity as a force
        rb.AddForce(new Vector2(0f, initialJumpVelocity), ForceMode2D.Impulse);
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

    private void PlayLandingSound()
    {
        if (isGrounded && !wasGroundedLastFrame)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.pitch = landingNoisePitch;
            audioSource.PlayOneShot(footsteps);
        }
    }

    private void Attack()
    {
        float direction = 1;

        if (!facingRight)
        {
            direction = -1;
        }

        if (!isAttacking && !isLedgeHanging)
        {
            if (Input.GetButtonDown("Fire1") && attackCooldownTimer <= 0.0f)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * direction, attackRange, attackMask);
                //Debug.DrawRay(transform.position, (transform.right * attackRange) * direction, Color.red, 1.0f);

                if (hit.collider != null)
                {
                    CheckDamageReciever(hit.collider.gameObject);
                }
                animatorAttackSlash.SetTrigger("Attacking");
                attackCooldownTimer = attackCooldown;
                applyingKnockback = true;
                isAttacking = true;
            }
        }

        if (isAttacking)
        {
            //apply knockback
            if(applyingKnockback)
            {
                if(!appliedKnockback)
                {
                    rb.velocity = Vector2.zero;
                    rb.AddForce((transform.right * -direction) * 100 * attackKnockback * Time.fixedDeltaTime, ForceMode2D.Impulse);
                    appliedKnockback = true;
                }
                attackKnockbackTimer -= Time.deltaTime;
                if(attackKnockbackTimer <= 0.0f)
                {
                    applyingKnockback = false;
                    appliedKnockback = false;
                    attackKnockbackTimer = attackKnockbackDuration;
                }
            }

            //reset attack cooldown
            attackCooldownTimer -= Time.deltaTime;

            if (attackCooldownTimer <= 0.0f)
            {
                isAttacking = false;
                applyingKnockback = false;
            }
        }
    }

    private void CheckDamageReciever(GameObject target)
    {
        if (target.CompareTag("Enemy"))
        {
            if(target.GetComponent<Dummy>()  != null)
            {
                target.GetComponent<Dummy>().TakeDamage(attackDamage, facingRight);
                target.GetComponent<AudioSource>().Play();
            }
            else
            {
                target.GetComponent<BasicGrounded>().TakeDamage(attackDamage, facingRight);
            }

        }
        if (target.CompareTag("Decoration"))
        {
            Debug.Log("Here");
            target.GetComponent<Animator>().SetTrigger("Smash");
            if(target.GetComponent<AudioSource>() != null)
            {
                target.GetComponent<AudioSource>().Play();
            }
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

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        rb.velocity = Vector2.zero;
        applyingKnockback = true;
        rb.AddForce(transform.up * knockbackEffectAmount * 100 * Time.fixedDeltaTime, ForceMode2D.Impulse);
        if (facingRight)
        {
            rb.AddForce(-transform.right * knockbackEffectAmount * 100 * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(transform.right * knockbackEffectAmount * 100 * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        if (currentHealth < 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Debug.Log("Dead");
    }

    private void LedgeHang()
    {
        // Implement ledge hanging logic here
    }

    private void CheckLedge()
    {
        // Implement ledge checking logic here
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