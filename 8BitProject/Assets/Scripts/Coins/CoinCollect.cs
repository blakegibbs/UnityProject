using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public int coinValue = 1;
    private bool inDogsMouth = false;
    private Transform dog;
    bool collected = false;
    float timer = 1f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            other.GetComponentInParent<PlayerInventory>().AddMoney(coinValue);
            this.GetComponent<AudioSource>().Play();
            this.GetComponent<SpriteRenderer>().enabled = false;
            collected = true;
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
        if (collected)
        {
            timer -= Time.deltaTime;
            if(timer< 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void HeldByDog()
    {
        this.transform.position = dog.Find("Mouth").transform.position;
    }
}
