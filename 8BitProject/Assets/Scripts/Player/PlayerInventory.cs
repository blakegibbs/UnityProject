using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public int currentMoney;
    public TMP_Text coinCounter;

    public void AddMoney(int value)
    {
        currentMoney += value;
        coinCounter.text = currentMoney.ToString();
    }

    public void RemoveMoney(int value)
    {
        currentMoney -= value;
    }
}
