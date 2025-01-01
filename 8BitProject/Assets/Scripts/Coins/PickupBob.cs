using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBob : MonoBehaviour
{
    public float increaseY = 0.5f;
    public float timeUntilMovement = 0.5f;
    private float timer = 0.5f;

    private bool goingUp = true;

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0.0f)
        {
            if(goingUp)
            {
                AnimateUp();
            }
            else
            {
                AnimateDown();
            }
        }
    }

    private void AnimateUp()
    {
        float posY = transform.position.y;
        float newPosY = posY + increaseY;
        transform.position = new Vector2 (transform.position.x, newPosY);
        timer = timeUntilMovement;
        goingUp = false;
    }

    private void AnimateDown()
    {
        float posY = transform.position.y;
        float newPosY = posY - increaseY;
        transform.position = new Vector2(transform.position.x, newPosY);
        timer = timeUntilMovement;
        goingUp = true;
    }
}
