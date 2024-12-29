using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private float currentHealth = 100000;
    private Animator animator;
    private Rigidbody2D rb;
    public float knockbackEffectAmount = 3f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        animator.SetTrigger("TakeDamage");
        rb.AddForce(transform.up * knockbackEffectAmount * 100 * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
}
