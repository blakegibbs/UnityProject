using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector2 respawnPosition;
    public float respawnTime = 1.5f; // Time taken to lerp to respawn position
    public float scaleMultiplier = 1.5f; // How much bigger the player gets
    public float respawnYOffset = 2f; // Offset above the respawn point

    private PlayerMovement playerMovement;
    private Vector3 originalScale;
    bool isRespawning = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        originalScale = transform.localScale;
    }

    public void Respawn()
    {
        if (!isRespawning)
        {
            StartCoroutine(RespawnSequence());
        }
    }

    private IEnumerator RespawnSequence()
    {
        // Disable movement
        playerMovement.ToggleMovementDisabled();
        isRespawning=true;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(respawnPosition.x, respawnPosition.y + respawnYOffset, transform.position.z);
        Vector3 maxScale = originalScale * scaleMultiplier;

        float elapsedTime = 0f;

        while (elapsedTime < respawnTime)
        {
            float t = elapsedTime / respawnTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.localScale = Vector3.Lerp(originalScale, maxScale, t * 2f); // Faster scaling
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // Quickly lerp scale back to normal
        elapsedTime = 0f;
        while (elapsedTime < respawnTime * 0.3f) // Shorter time for shrinking back
        {
            transform.localScale = Vector3.Lerp(maxScale, originalScale, elapsedTime / (respawnTime * 0.3f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;

        // Ensure correct facing direction after respawning
        if (!playerMovement.isFacingRight)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = originalScale;
        }

        // Re-enable movement
        playerMovement.ToggleMovementDisabled();
        isRespawning = false;
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
        respawnPosition = newPoint.position;
        newPoint.GetComponent<Animator>().SetBool("Active", true);
    }
}
