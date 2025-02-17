using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public float detectionDistance = 1f;
    public float idleTime = 2f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;  // Added for wall detection
    public float groundCheckOffset = 0.5f;
    public Animator animator;

    private enum State { Moving, Idle }
    private State currentState;
    private Rigidbody2D rb;
    private int direction = 1;
    private bool isIdle = false;
    public bool facingRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = State.Moving;
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
    }

    private void Patrol()
    {
        if (!isIdle)
        {
            rb.velocity = new Vector2(speed * direction, rb.velocity.y);

            if (ShouldTurnAround())
            {
                StartCoroutine(IdleThenTurn());
            }
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
        RaycastHit2D wallCheckerHit = Physics2D.Raycast(rayPosition, Vector2.right * direction, detectionDistance, wallLayer);
        Debug.DrawRay(rayPosition, Vector2.right * direction * detectionDistance, Color.red);  // Debug line for wall detection

        if (wallCheckerHit.collider != null)
        {
            return true;
        }

        return false;
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
