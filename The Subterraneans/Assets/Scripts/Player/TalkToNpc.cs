using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToNpc : MonoBehaviour
{
    bool interacted = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            if(!interacted)
            {
                collision.GetComponentInChildren<GameObject>().SetActive(true);
            }
            else
            {
                collision.GetComponentInChildren<GameObject>().SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                interacted = true;
                collision.SendMessage("Interact");
            }
        }

    }
}
