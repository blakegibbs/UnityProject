using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bartender : MonoBehaviour
{
    public GameObject player, speech, saloonPlayer;
    public Tutorial tutorial;
    public TMP_Text text;
    public GameObject doubleJumpUnlockCard, dashUnlockCard, wallJumpUnlockCard, wallClimbUnlockCard;
    private GameObject cardToShow;
    bool cardActive;

    [Header("Dialogue")]
    public DialogueData dialogueData; // Scriptable object reference

    private int storyDialogueIndex = 0; // Track the story dialogue progress
    private string lastDialogue = ""; // Track the last dialogue to avoid repetition

    bool resetDialogue = false;

    private void Update()
    {
        if(cardActive)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                doubleJumpUnlockCard.SetActive(false);
                dashUnlockCard.SetActive(false);
                wallJumpUnlockCard.SetActive(false);
                wallClimbUnlockCard.SetActive(false);
                PlayerMovement playerScript = saloonPlayer.GetComponent<PlayerMovement>();
                playerScript.ToggleMovementDisabled();
                cardActive = false;
            }
        }
    }

    public void Interact()
    {
        tutorial.SpokenToBartender();
        if (!resetDialogue)
        {
            storyDialogueIndex = 0;
            resetDialogue = true;
        }
        speech.SetActive(true);
        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();

        string unlockMessage = GetUnlockable(playerScript);
        if (unlockMessage != null)
        {
            text.text = unlockMessage;
        }
        else
        {
            ShowStoryDialogueOrRandom(); // Show story dialogue first or random after unlocks
        }
    }

    private string GetUnlockable(PlayerMovement playerScript)
    {
        if (playerScript.canUnlockDoubleJump)
        {
            playerScript.doubleJumpUnlocked = true;
            playerScript.canUnlockDoubleJump = false;
            cardToShow = doubleJumpUnlockCard;
            return dialogueData.unlockDoubleJump;
        }
        if (playerScript.canUnlockDash)
        {
            playerScript.dashUnlocked = true;
            playerScript.canUnlockDash = false;
            cardToShow = dashUnlockCard;
            return dialogueData.unlockDash;
        }
        if (playerScript.canUnlockWallClimb)
        {
            playerScript.wallClimbUnlocked = true;
            playerScript.canUnlockWallClimb = false;
            cardToShow = wallClimbUnlockCard;
            return dialogueData.unlockWallClimb;
        }
        if (playerScript.canUnlockWallJump)
        {
            playerScript.wallJumpUnlocked = true;
            playerScript.canUnlockWallJump = false;
            cardToShow = wallJumpUnlockCard;
            return dialogueData.unlockWallJump;
        }
        return null;
    }

    private void ShowStoryDialogueOrRandom()
    {
        if (storyDialogueIndex < dialogueData.storyDialogue.Count)
        {
            // Show the next story dialogue line
            text.text = dialogueData.storyDialogue[storyDialogueIndex];
            storyDialogueIndex++; // Move to the next story dialogue
        }
        else
        {
            ShowRandomDialogue(); // If no more story dialogue, show random
        }
    }

    private void ShowRandomDialogue()
    {
        if (dialogueData.randomDialogue.Count > 0)
        {
            string newDialogue = GetNewRandomDialogue();
            text.text = newDialogue;
            lastDialogue = newDialogue; // Save the last spoken dialogue
        }
        else
        {
            text.text = "I don't have anything to say right now...";
        }
    }

    private string GetNewRandomDialogue()
    {
        string randomDialogue;
        do
        {
            randomDialogue = dialogueData.randomDialogue[Random.Range(0, dialogueData.randomDialogue.Count)];
        }
        while (randomDialogue == lastDialogue); // Make sure it's not the same as the last dialogue

        return randomDialogue;
    }

    public void EndInteraction()
    {
        speech.SetActive(false);
        resetDialogue = false;
        if(cardToShow != null)
        {
            ShowUnlockCard(cardToShow);
        }
    }

    public void ShowUnlockCard(GameObject card)
    {
        card.SetActive(true);
        PlayerMovement playerScript = saloonPlayer.GetComponent<PlayerMovement>();
        playerScript.ToggleMovementDisabled();
        cardActive = true;
        cardToShow = null;
    }
}
