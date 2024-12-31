using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool active = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        active = true;

        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.active && checkpoint != this)
            {
                checkpoint.DeactivateCheckpoint();
            }
        }
    }

    public void DeactivateCheckpoint()
    {
        active = false;
    }
}
