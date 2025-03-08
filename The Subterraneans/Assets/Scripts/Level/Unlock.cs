using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Unlocks
{
    DoubleJump,
    WallJump,
    WallClimb,
    Dash
}

public class Unlock : MonoBehaviour
{
    public Unlocks unlocks;
}
