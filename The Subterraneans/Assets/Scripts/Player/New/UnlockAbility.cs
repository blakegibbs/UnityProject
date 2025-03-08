using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unlock;

public class UnlockAbility : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Unlock"))
        {
            Unlock unlock = collision.GetComponent<Unlock>();
            collision.GetComponent<Animator>().SetTrigger("Interacted");
            if (unlock != null && unlock.unlocks == Unlocks.DoubleJump)
            {
                PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.canUnlockDoubleJump = true;
                }
            }
            else if (unlock != null && unlock.unlocks == Unlocks.WallJump)
            {
                PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.canUnlockWallJump = true;
                }
            }
            else if (unlock != null && unlock.unlocks == Unlocks.WallClimb)
            {
                PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.canUnlockWallClimb = true;
                }
            }
            else if (unlock != null && unlock.unlocks == Unlocks.Dash)
            {
                PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.canUnlockDash = true;
                }
            }
        }
    }
}
