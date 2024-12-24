using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int currentMoney;

    public void AddMoney(int value)
    {
        currentMoney += value;
    }

    public void RemoveMoney(int value)
    {
        currentMoney -= value;
    }
}
