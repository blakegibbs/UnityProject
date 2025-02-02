using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RespawnObject : MonoBehaviour
{
    private Vector2 spawnPos;

    private void Start()
    {
        spawnPos = transform.position;
    }

    public void Respawn()
    {
        transform.position = spawnPos;
    }
}
