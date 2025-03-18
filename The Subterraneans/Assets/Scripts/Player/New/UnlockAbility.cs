using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unlock;

public class UnlockAbility : MonoBehaviour
{
    bool collected1 = false;
    bool collected2 = false;
    bool collected3 = false;
    bool collected4 = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Unlock"))
        {
            Unlock unlock = collision.GetComponent<Unlock>();
            if(collision.GetComponent<Animator>() != null)
            {
                collision.GetComponent<Animator>().SetTrigger("Interacted");
            }
            if (unlock != null && unlock.unlocks == Unlocks.DoubleJump && !collected1)
            {
                PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.canUnlockDoubleJump = true;
                    collected1 = true;
                }
            }
            else if (unlock != null && unlock.unlocks == Unlocks.WallJump && !collected2)
            {
                PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.canUnlockWallJump = true;
                    collected2 = true;
                }
            }
            else if (unlock != null && unlock.unlocks == Unlocks.WallClimb && !collected3)  
            {
                PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.canUnlockWallClimb = true;
                    collected3 = true;
                }
            }
            else if (unlock != null && unlock.unlocks == Unlocks.Dash && !collected4)
            {
                PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.canUnlockDash = true;
                    collected4 = true;
                }
            }
        }
    }
}
