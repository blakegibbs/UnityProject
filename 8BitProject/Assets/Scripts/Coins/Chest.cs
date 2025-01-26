using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject dollarPrefab;
    public GameObject smallDollarPrefab;
    public int amountToSpawnBig = 1;
    public int amountToSpawnSmall = 5;
    public float forceMultiplier = 5f;
    public Vector2 spawnOffset = new Vector2(0.5f, 0.5f);

    public void OpenChest()
    {
        for (int i = 0; i < amountToSpawnBig; i++)
        {
            SpawnAndApplyForce(dollarPrefab);
        }

        for (int i = 0; i < amountToSpawnSmall; i++)
        {
            SpawnAndApplyForce(smallDollarPrefab);
        }
    }

    private void SpawnAndApplyForce(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float randomX = Random.Range(-spawnOffset.x, spawnOffset.x);
            float randomY = Random.Range(0, spawnOffset.y);
            Vector2 force = new Vector2(randomX, randomY) * forceMultiplier;

            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
