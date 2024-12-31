using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Checkpoint[] checkpoints;

    public void Respawn()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.active)
            {
                transform.position = checkpoint.transform.position;
            }
        }
    }
}
