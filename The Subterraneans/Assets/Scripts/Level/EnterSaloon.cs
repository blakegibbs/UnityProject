using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterSaloon : MonoBehaviour
{
    public GameObject saloonPlayer, player, UI, saloonCamera, playerCamera;
    private bool playerInTrigger = false;
    public Tutorial tutorial;

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (!saloonCamera.activeInHierarchy)
            {
                Enter();
            }
            else
            {
                Exit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && tutorial.spokenToBartender)
        {
            playerInTrigger = true;
            UI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && tutorial.spokenToBartender)
        {
            playerInTrigger = false;
            UI.SetActive(false);
        }
    }

    private void Enter()
    {
        saloonPlayer.SetActive(true);
        saloonCamera.SetActive(true);
        player.SetActive(false);
        playerCamera.SetActive(false);
        UI.SetActive(false);
    }

    private void Exit()
    {
        saloonPlayer.SetActive(false);
        saloonCamera.SetActive(false);
        player.SetActive(true);
        playerCamera.SetActive(true);
        UI.SetActive(true);
    }
}
