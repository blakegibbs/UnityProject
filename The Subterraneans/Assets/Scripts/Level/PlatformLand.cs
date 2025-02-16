using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLand : MonoBehaviour
{
    public float increaseY = 0.5f;
    bool isColliding = false;
    float timer;
    Vector2 targetPosition;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (timer <= 0)
        {
            AnimateDown();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (timer <= 0)
        {
            AnimateUp();
        }
    }

    private void AnimateUp()
    {
        targetPosition = new Vector2(transform.position.x, transform.position.y + increaseY);
        isColliding = false;
        timer = 0.1f;
    }

    private void AnimateDown()
    {
        if (!isColliding)
        {
            targetPosition = new Vector2(transform.position.x, transform.position.y - increaseY);
            timer = 0.1f;
            isColliding = true;
        }
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        // Smoothly move the platform toward the target position using Lerp
        if (timer > 0)
        {
            transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
        }
    }
}
