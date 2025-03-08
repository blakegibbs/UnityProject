using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OutlineFx;

public class Tutorial : MonoBehaviour
{
    public GameObject bartender, bar;
    public bool spokenToBartender = false;

    public void SpokenToBartender()
    {
        bartender.GetComponent<OutlineFx.OutlineFx>().enabled = false;
        bar.GetComponent<OutlineFx.OutlineFx>().enabled = false;
        spokenToBartender=true;
    }

}
