using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bartender : MonoBehaviour
{
    public GameObject player, speech;
    public TMP_Text text;

    [Header("Dialogue")]
    public DialogueData dialogueData; // Scriptable object reference

    private int storyDialogueIndex = 0; // Track the story dialogue progress
    private string lastDialogue = ""; // Track the last dialogue to avoid repetition

    public void Interact()
    {
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
            return dialogueData.unlockDoubleJump;
        }
        if (playerScript.canUnlockDash)
        {
            playerScript.dashUnlocked = true;
            return dialogueData.unlockDash;
        }
        if (playerScript.canUnlockWallClimb)
        {
            playerScript.wallClimbUnlocked = true;
            return dialogueData.unlockWallClimb;
        }
        if (playerScript.canUnlockWallJump)
        {
            playerScript.wallJumpUnlocked = true;
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
    }
}
