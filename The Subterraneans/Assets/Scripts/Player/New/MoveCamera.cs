using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPos;
    public Transform targetPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Here");
        if (collision.CompareTag("ZoneTransition"))
        {
            Debug.Log("Eads");
            targetPos = collision.transform;
            cameraPos.position = targetPos.position;
        }
    }
}
