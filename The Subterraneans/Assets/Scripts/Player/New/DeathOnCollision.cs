using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathBox"))
        {
            this.GetComponent<PlayerRespawn>().Respawn();
            this.GetComponent<CameraShake>().shakeDuration = 0.1f;
        }
    }
}
