using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private float currentHealth = 100000;
    private Animator animator;
    private Rigidbody2D rb;
    public float knockbackEffectAmount = 3f;
    private bool facingRight = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void TakeDamage(float amount, bool moveRight)
    {
        currentHealth -= amount;

        if(moveRight)
        {
            if (!facingRight)
            {
                Flip();
            }
        }
        else
        {
            if(facingRight)
            {
                Flip();
            }
        }
        animator.SetTrigger("TakeDamage");
        rb.AddForce(transform.up * knockbackEffectAmount * 100 * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }


    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
