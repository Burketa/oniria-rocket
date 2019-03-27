using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateManager
{
    public enum gameState
    {
        AWAKE, START, STAGE1, SEPARATING, STAGE2, LANDING
    }

    public static gameState currentState = gameState.AWAKE;
}
