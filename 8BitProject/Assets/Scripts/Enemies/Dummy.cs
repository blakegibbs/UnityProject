using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private float currentHealth;
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
    }

    public void IdleState()
    {

    }

    public void TakeDamageState()
    {

    }
}
