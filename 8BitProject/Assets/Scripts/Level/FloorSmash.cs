using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSmash : MonoBehaviour
{
    public float rayDistance = 10f;
    public GameObject smallLift;
    public GameObject dogWait;

    public LayerMask weightLayer; // Set in the Inspector

    public Lever[] levers;

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, rayDistance, weightLayer);

        if (hit.collider != null)
        {
            string objectName = hit.collider.gameObject.name;
            float distance = hit.distance;
            Rigidbody2D rb = hit.collider.attachedRigidbody;
            Vector2 velocity = rb ? rb.velocity : Vector2.zero;

            if (rb != null && Mathf.Abs(rb.velocity.y) >= 3 && objectName == "Weight" && distance <= 3)
            {
                this.gameObject.SetActive(false);
                smallLift.SetActive(false);
                DisableLevers();
                dogWait.SetActive(false);
                hit.collider.gameObject.layer = 3;
            }

            Debug.Log($"Hit: {objectName}, Distance: {distance}, Velocity: {velocity}");
        }

        Debug.DrawRay(transform.position, Vector2.up * rayDistance, Color.red);
    }

    private void DisableLevers()
    {
        foreach (Lever component in levers)
        {
            component.canMove = false;
            component.BigLiftDown();
        }
    }
}
