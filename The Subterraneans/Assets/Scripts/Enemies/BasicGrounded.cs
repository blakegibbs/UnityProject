using System.Collections;
using UnityEngine;

public class BasicGrounded : MonoBehaviour
{
    public float speed = 2f;
    public float detectionDistance = 1f;
    public float idleTime = 2f;
    public LayerMask groundLayer;
    public float groundCheckOffset = 0.5f;
    public Animator animator;

    private enum State { Moving, Idle }
    private State currentState;
    private Rigidbody2D rb;
    private int direction = 1;
    private bool isIdle = false;
    public bool facingRight;

    [Header("Health")]
    public float maxHealth = 100;
    float currentHealth;
    public float knockbackEffectAmount = 3f;
    bool isApplyingKnockback;
    float knockbackTimer = 0.5f;

    [Header("Attack")]
    public float attackDamage;
    public float attackCooldown;
    private float timer;
    private bool attacking = false;
    public GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = State.Moving;
        currentHealth = maxHealth;
        timer = attackCooldown;
    }

    void Update()
    {

        if (currentState == State.Moving)
        {
            Patrol();
            animator.speed = 2;
        }
        else
        {
            animator.speed = 1;
        }

        if(attacking)
        {
            currentState = State.Idle;
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = attackCooldown;
                attacking = false;
                currentState = State.Moving;
            }
        }
    }

    private void Patrol()
    {
        if (!isIdle && !isApplyingKnockback)
        {
            if (ShouldTurnAround())
            {
                StartCoroutine(IdleThenTurn());
            }
            else
            {
                rb.velocity = new Vector2(speed * direction, rb.velocity.y);
            }
        }
        if (isApplyingKnockback)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                knockbackTimer = 0.5f;
                isApplyingKnockback = false;
            }
        }
    }

    public void TakeDamage(float amount, bool right)
    {
        isApplyingKnockback = true;
        currentHealth -= amount;
        animator.SetTrigger("TakeDamage");
        rb.AddForce(transform.up * knockbackEffectAmount * 100 * Time.fixedDeltaTime, ForceMode2D.Impulse);
        if (!right)
        {
            rb.AddForce(-transform.right * knockbackEffectAmount * 100 * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(transform.right * knockbackEffectAmount * 100 * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        if (currentHealth < 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private bool ShouldTurnAround()
    {
        Vector2 rayPosition = Vector2.zero;

        if (facingRight)
        {
            rayPosition = new Vector2(transform.position.x + 1, transform.position.y);
        }
        else
        {
            rayPosition = new Vector2(transform.position.x - 1, transform.position.y);
        }

        // Raycast for ground check
        RaycastHit2D floorCheckerHit = Physics2D.Raycast(rayPosition, -Vector2.up, 100, groundLayer);
        Debug.DrawRay(rayPosition, -Vector2.up * 100, Color.green);  // Debug line for ground check

        if (floorCheckerHit.distance >= 3)
        {
            return true;
        }

        // Raycast for wall detection
        RaycastHit2D wallCheckerHit = Physics2D.Raycast(rayPosition, Vector2.right * direction, detectionDistance, groundLayer);
        Debug.DrawRay(rayPosition, Vector2.right * direction * detectionDistance, Color.red);  // Debug line for wall detection

        if (wallCheckerHit.collider != null)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if(timer > 0 && !attacking)
        {
            player.GetComponent<PlayerController>().TakeDamage(attackDamage);
            animator.SetTrigger("Attack");
            attacking = true;
        }
    }

    private IEnumerator IdleThenTurn()
    {
        isIdle = true;
        currentState = State.Idle;
        rb.velocity = Vector2.zero;  // Stop movement while idle
        yield return new WaitForSeconds(idleTime);
        isIdle = false;
        Flip();  // Flip after idle time
        currentState = State.Moving;
    }

    private void Flip()
    {
        facingRight = !facingRight;

        // Update direction based on facingRight
        direction = facingRight ? 1 : -1;

        // Flip the sprite visually
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
