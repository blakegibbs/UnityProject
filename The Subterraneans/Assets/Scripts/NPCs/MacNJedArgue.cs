using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MacNJedArgue : MonoBehaviour
{
    public string[] macTexts;
    public string[] jedTexts;
    public int macCounter = -1;
    public int jedCounter = -1;
    public TMP_Text macText;
    public TMP_Text jedText;

    public void ChangeJedText()
    {
        jedCounter++;
        jedText.text = jedTexts[jedCounter]; 
    }

    public void ChangeMacText()
    {
        macCounter++;
        macText.text = macTexts[macCounter];
        if (macText.text == "That’s it, you dig, I’m takin’ a nap! Let me know when you strike it rich, if you ever do!")
        {
            macCounter = 0;
            jedCounter = -1;
            macText.text = macTexts[macCounter];
        }
    }

}
