using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLevers : MonoBehaviour
{
    public Lever[] levers;

    public void DisableAllLevers()
    {
        foreach (Lever component in levers)
        {
            component.canMove = false;
        }
    }

    public void EnableAllLevers()
    {
        foreach (Lever component in levers)
        {
            component.canMove = true;
        }
    }

}
