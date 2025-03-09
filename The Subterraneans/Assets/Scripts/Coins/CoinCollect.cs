using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public int coinValue = 1;
    bool collected = false;
    float timer = 1f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            other.GetComponentInParent<PlayerInventory>().AddMoney(coinValue);
            this.GetComponent<SpriteRenderer>().enabled = false;
            collected = true;
        }
    }

    private void Update()
    {
        if (collected)
        {
            timer -= Time.deltaTime;
            if(timer< 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
