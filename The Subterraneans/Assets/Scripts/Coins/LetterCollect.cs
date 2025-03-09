using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCollect : MonoBehaviour
{
    public string contentsOfLetter;

    public ControlsUI controls;

    private bool inDogsMouth = false;
    private Transform dog;

    private float destroyTimer = 0f;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                //other.GetComponentInParent<PlayerInventory>().AddLetter(contentsOfLetter, 1);
                this.GetComponent<Animator>().SetTrigger("Open");
                destroyTimer = 1.5f;
            }
        }
        else if (other.CompareTag("Dog"))
        {
            inDogsMouth = true;
            dog = other.transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            controls.ShowE();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            controls.HideE();
        }
    }

    private void Update()
    {
        if (inDogsMouth)
        {
            HeldByDog();
        }

        if(destroyTimer > 0f)
        {
            destroyTimer -= Time.deltaTime;
            if(destroyTimer < 1f)
            {
                controls.HideE();
                Destroy(this.GetComponent<Collider2D>());
            }

            if(destroyTimer < 0f)
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
