using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToNpc : MonoBehaviour
{
    public GameObject UI;
    private bool playerInTrigger = false;

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            this.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
            UI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = true;
            UI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = false;
            UI.SetActive(false);
            this.SendMessage("EndInteraction", SendMessageOptions.DontRequireReceiver);
        }
    }
}
