using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigTrigger : MonoBehaviour
{
    public float diggingTime = 3f;
    private Animator animator;
    bool active = false;
    public GameObject ThingToBeDug;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog"))
        {
            animator = collision.GetComponentInParent<Animator>();
            animator.SetBool("Digging", true);
            active = true;
        }
    }

    private void Update()
    {
        if (active)
        {
            diggingTime -= Time.deltaTime;
            {
                if (diggingTime < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnDestroy()
    {
        animator.SetBool("Digging", false);
        Destroy(ThingToBeDug);
    }
}
