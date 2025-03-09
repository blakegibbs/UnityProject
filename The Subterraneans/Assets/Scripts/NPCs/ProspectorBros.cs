using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProspectorBros : MonoBehaviour
{
    public GameObject player, speech;
    public TMP_Text text;
    public GameObject lightUnlockCard;
    public GameObject purchaseOptions;
    private GameObject cardToShow;
    bool cardActive;
    bool unlockedLight;
    public int lightCost;
    bool offeringPurchase = false;
    public GameObject eToSkipText;
    public GameObject argumentText;

    [Header("Dialogue")]
    public DialogueData dialogueData; // Scriptable object reference

    private int storyDialogueIndex = 0; // Track the story dialogue progress
    private string lastDialogue = ""; // Track the last dialogue to avoid repetition
    public Color textcol1;
    public Color textcol2;
    private bool isCol1 = true;

    bool resetDialogue = false;

    private void Update()
    {
        if (cardActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                lightUnlockCard.SetActive(false);
                PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
                playerScript.ToggleMovementDisabled();
                cardActive = false;
            }
        }

        if(offeringPurchase)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            inventory.canUpdateMoneyCounterAlpha = false;
            inventory.SetTextAlpha(1);
            eToSkipText.SetActive(false);
            if(Input.GetKeyDown(KeyCode.Y) && inventory.currentMoney >= lightCost)
            {
                PurchaseLight();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                RefusePurchase();
            }
        }
        else
        {
            player.GetComponent< PlayerInventory>().canUpdateMoneyCounterAlpha = true;
            eToSkipText.SetActive(true);
        }
    }

    public void Interact()
    {
        argumentText.SetActive(false);
        if(!offeringPurchase)
        {
            if (!resetDialogue)
            {
                if (!unlockedLight)
                {
                    storyDialogueIndex = 0;
                }
                resetDialogue = true;
                offeringPurchase = false;
            }
            speech.SetActive(true);

            if (storyDialogueIndex == dialogueData.storyDialogue.Count-1 && !unlockedLight)//if story dialogye done ask about purchasing
            {
                text.text = dialogueData.unlockLight +"\n" + "Purchase light for : $" + lightCost  + "?";
                text.color = textcol1;
                offeringPurchase = true;
                purchaseOptions.SetActive(true);
            }
            else
            {
                ShowStoryDialogueOrRandom(); // Show story dialogue first or random after unlocks
            }
        }
    }

    private void ShowStoryDialogueOrRandom()
    {
        if(storyDialogueIndex%2 == 0)
        {
            text.color = textcol1;
        }
        else
        {
            text.color = textcol2;
        }
        if(!unlockedLight)
        {
            if (storyDialogueIndex < dialogueData.storyDialogue.Count)
            {
                // Show the next story dialogue line
                text.text = dialogueData.storyDialogue[storyDialogueIndex];
                storyDialogueIndex++; // Move to the next story dialogue
            }
        }
        else
        {
            ShowRandomDialogue(); // If no more story dialogue, show random
        }
    }

    private void ShowRandomDialogue()
    {
        if (isCol1)
        {
            text.color = textcol1;
            isCol1 = false;
        }
        else
        {
            text.color= textcol2;
            isCol1 = true;
        }
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
        argumentText.SetActive(true);

        offeringPurchase = false;
        purchaseOptions.SetActive(false);
        speech.SetActive(false);
        resetDialogue = false;
        if (cardToShow != null)
        {
            ShowUnlockCard(cardToShow);
        }
    }

    public void ShowUnlockCard(GameObject card)
    {
        card.SetActive(true);
        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
        playerScript.ToggleMovementDisabled();
        cardActive = true;
        cardToShow = null;
    }

    private void PurchaseLight()
    {
        player.GetComponent<PlayerInventory>().RemoveMoney(lightCost);
        player.GetComponent<PlayerMovement>().lightUnlocked = true;
        offeringPurchase = false;
        text.text = dialogueData.purchaseSuccessful;
        text.color = textcol1;
        unlockedLight = true;
        cardToShow = lightUnlockCard;
        purchaseOptions.SetActive(false);
    }

    private void RefusePurchase()
    {
        offeringPurchase = false;
        text.text = dialogueData.purchaseFailed;
        text.color = textcol1;
        purchaseOptions.SetActive(false);
    }
}
