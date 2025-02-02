using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject animatedObject;
    private Animator anim;
    public bool canMove = true;

    private void Start()
    {
        anim = animatedObject.GetComponent<Animator>();
    }

    public void FlipLever()
    {
        if(canMove)
        {
            anim.SetTrigger("Move");
        }
    }

    public void BigLiftDown()
    {
        anim.Play("Mill2");
    }
}
