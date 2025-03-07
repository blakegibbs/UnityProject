using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            GameObject[] tombstonesArray = GameObject.FindGameObjectsWithTag("Respawn");
            List<GameObject> tombstones = tombstonesArray.ToList();
            foreach (var respawnPos in tombstones)
            {
                respawnPos.GetComponent<Animator>().SetBool("Active", false);
            }
            ChangeRespawnPoint(collision.transform);
        }
    }

    private void ChangeRespawnPoint(Transform newPoint)
    {
        respawnPosition = newPoint.transform.position;
        newPoint.GetComponent<Animator>().SetBool("Active", true);
    }
}
