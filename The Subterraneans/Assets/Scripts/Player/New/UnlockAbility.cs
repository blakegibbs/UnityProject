using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAbility : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Unlock"))
        {
            this.GetComponent<PlayerMovement>().wallJumpUnlocked = true;

        }
    }
}
