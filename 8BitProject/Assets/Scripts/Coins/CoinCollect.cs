using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public int coinValue = 1;
    private bool inDogsMouth = false;
    private Transform dog;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerInventory>().AddMoney(coinValue);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Dog"))
        {
            inDogsMouth = true;
            dog = other.transform;
        }
    }

    private void Update()
    {
        if(inDogsMouth)
        {
            HeldByDog();
        }
    }

    private void HeldByDog()
    {
        this.transform.position = dog.Find("Mouth").transform.position;
    }
}
