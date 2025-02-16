using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    public Animator aAnimator;
    public Animator dAnimator;
    public Animator spaceAnimator;
    public Animator eAnimator;
    public Transform player;

    private bool pressedSpace = false;
    private bool pressedA = false;
    private bool pressedD = false;
    private bool EActive = false;
    private bool finishedTutorial;

    private float timer = 3.0f;

    public GameObject[] elements;

    public void Update()
    {
        transform.position = player.position;

        if(!finishedTutorial )
        {
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

            if (pressedD && pressedA && pressedSpace)
            {
                timer -= Time.deltaTime;
                if (timer <= 0.0f)
                {
                    elements[0].SetActive(false);
                    elements[1].SetActive(false);
                    elements[2].SetActive(false);
                }
            }
        }

        if(EActive)
        {
            if (Input.GetKey(KeyCode.E))
            {
                eAnimator.SetBool("Pressed", true);
            }
            else
            {
                eAnimator.SetBool("Pressed", false);
            }
        }
    }

    public void ShowE()
    {
        if(!EActive)
        {
            elements[3].SetActive(true);
        }
        EActive = true;
    }

    public void HideE()
    {
        elements[3].SetActive(false);
        EActive=false;
    }
}
