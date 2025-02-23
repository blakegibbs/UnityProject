using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector2 respawnPosition;

    public void Respawn()
    {
        transform.position = respawnPosition;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            respawnPosition = collision.transform.position;
        }
    }
}
