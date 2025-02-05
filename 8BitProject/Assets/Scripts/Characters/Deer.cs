using UnityEngine;

public class Deer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float targetDistance;
    public Transform target;
    public bool isWaiting = false;
    bool isMoving = false; 

    [Header("Targets")]
    public Transform target1, target2, target3;
    public float distance1, distance2, distance3;

    [Header("References")]
    private Transform player;
    bool facingRight = false;
    public GameObject deerTrap;

    [Header("Animator")]
    public float walkAnimationSpeedMultiplier = 4f;
    private Animator animator;

    [Header("GroundCheck")]
    public Collider2D groundCheckCollider;
    public LayerMask groundLayer;
    public float rayLength = 0.1f;

    private Rigidbody2D rb;

    private void Start()
    {
        isWaiting = false;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        facingRight = false;
    }

    private void Update()
    {
        if (!isWaiting)
        {
            Move();
        }
        DistanceToPlayer();
        IsGrounded();
    }

    private float DistanceToPlayer()
    {
        float distance = transform.position.x - player.position.x;
        return Mathf.Abs(distance);
    }

    private void Move()
    {
        if(target != null)
        {
            if (target == target1)
            {
                if (DistanceToPlayer() < distance1)
                {
                    animator.SetBool("isMoving", true);
                    animator.SetBool("idle", false);

                    Vector2 newPosition = new Vector2(Mathf.MoveTowards(transform.position.x, target.position.x, moveSpeed * Time.deltaTime), transform.position.y);
                    transform.position = newPosition;
                    animator.speed = walkAnimationSpeedMultiplier;
                    isMoving = true;
                }
                else
                {
                    animator.SetBool("isMoving", false);
                    animator.SetBool("idle", true);
                    animator.speed = 1;
                    isMoving = false;
                }

                if (transform.position.x == target1.position.x)
                {
                    animator.SetBool("isMoving", false);
                    animator.SetBool("idle", true);
                    animator.speed = 1;
                    isMoving = false;
                    target = target2;
                }

            }
            else if (target == target2)
            {
                if (transform.position.x == target2.position.x)
                {
                    animator.SetBool("isMoving", false);
                    animator.SetBool("idle", true);
                    animator.speed = 1;
                    isMoving = false;
                    if (DistanceToPlayer() < distance2)
                    {
                        target = target3;
                    }
                }
                else
                {
                    animator.SetBool("isMoving", true);
                    animator.SetBool("idle", false);
                    Vector2 newPosition = new Vector2(Mathf.MoveTowards(transform.position.x, target.position.x, moveSpeed * Time.deltaTime), transform.position.y);
                    transform.position = newPosition;
                    animator.speed = walkAnimationSpeedMultiplier;
                    isMoving = true;
                }
            }
            else if (target == target3)
            {
                if (DistanceToPlayer() < distance3)
                {
                    animator.SetBool("isMoving", true);
                    animator.SetBool("idle", false);
                    Vector2 newPosition = new Vector2(Mathf.MoveTowards(transform.position.x, target.position.x, moveSpeed * Time.deltaTime), transform.position.y);
                    transform.position = newPosition;
                    animator.speed = walkAnimationSpeedMultiplier;
                    isMoving = true;
                }

                if (transform.position.x == target3.position.x)
                {
                    animator.SetBool("isMoving", false);
                    animator.SetBool("idle", false);
                    animator.SetBool("drinking", true);
                    animator.speed = 1;
                    isMoving = false;
                    if(DistanceToPlayer() < 6)
                    {
                        deerTrap.SetActive(true);
                        this.gameObject.SetActive(false);
                    }
                }

            }

        }
        else
        {
            animator.SetBool("Idle", true);
        }

        if (isMoving && !facingRight)
        {
            Flip();
        }
    }

    private bool IsGrounded()
    {
        Vector2 rayStart = groundCheckCollider.bounds.center;

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, groundCheckCollider.bounds.extents.y + 0.1f, groundLayer);

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
