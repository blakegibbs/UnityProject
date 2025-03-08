using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue System/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Header("Random Dialogue")]
    public List<string> randomDialogue = new List<string>();

    [Header("Story Dialogue")]
    public List<string> storyDialogue = new List<string>();

    [Header("Unlock Messages")]
    public string unlockDoubleJump = "Here, take this brew—it’ll get you an extra jump!";
    public string unlockDash = "This will make you faster!";
    public string unlockWallClimb = "Now you can scale walls!";
    public string unlockWallJump = "Time to bounce off walls!";

}
