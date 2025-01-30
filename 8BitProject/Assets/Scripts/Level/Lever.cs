using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject animatedObject;
    private Animator anim;
    private bool moved = false;

    private void Start()
    {
        anim = animatedObject.GetComponent<Animator>();
    }

    public void FlipLever()
    {
        anim.SetTrigger("Move");
    }
}
