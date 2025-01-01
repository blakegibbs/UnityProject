using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    public Animator aAnimator;
    public Animator dAnimator;
    public Animator spaceAnimator;
    public Transform player;

    private bool pressedSpace = false;
    private bool pressedA = false;
    private bool pressedD = false;

    private float timer = 3.0f;

    public void Update()
    {
        transform.position = player.position;

        if (Input.GetKey(KeyCode.Space))
        {
            spaceAnimator.SetBool("Pressed", true);
            pressedSpace = true;
        }
        else
        {
            spaceAnimator.SetBool("Pressed", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            aAnimator.SetBool("Pressed", true);
            pressedA = true;
        }
        else
        {
            aAnimator.SetBool("Pressed", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            dAnimator.SetBool("Pressed", true);
            pressedD = true;
        }
        else
        {
            dAnimator.SetBool("Pressed", false);
        }

        if(pressedD && pressedA && pressedSpace)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
