using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateManager
{
    public enum gameState
    {
        AWAKE, START, STAGE1, SEPARATING1, STAGE2, SEPARATING2, PARACHUTE, LANDING, FINISH
    }

    public static gameState currentState = gameState.AWAKE;

    public static void SetState(gameState state)
    {
        currentState = state;
    }

    public static gameState GetState()
    {
        return currentState;
    }
    public static void NextState(gameState state)
    {
        switch (state)
        {
            case gameState.AWAKE:
                SetState(gameState.START);
                break;

            case gameState.START:
                SetState(gameState.STAGE1);
                break;

            case gameState.STAGE1:
                SetState(gameState.SEPARATING1);
                break;

            case gameState.SEPARATING1:
                SetState(gameState.STAGE2);
                break;

            case gameState.STAGE2:
                SetState(gameState.SEPARATING2);
                break;

            case gameState.SEPARATING2:
                SetState(gameState.PARACHUTE);
                break;

            case gameState.PARACHUTE:
                SetState(gameState.LANDING);
                break;

            case gameState.LANDING:
                SetState(gameState.FINISH);
                break;

            case gameState.FINISH:
                SetState(gameState.AWAKE);
                break;
        }
    }

    public static void NextState()
    {
        switch (currentState)
        {
            case gameState.AWAKE:
                SetState(gameState.START);
                break;

            case gameState.START:
                SetState(gameState.STAGE1);
                break;

            case gameState.STAGE1:
                SetState(gameState.SEPARATING1);
                break;

            case gameState.SEPARATING1:
                SetState(gameState.STAGE2);
                break;

            case gameState.STAGE2:
                SetState(gameState.SEPARATING2);
                break;

            case gameState.SEPARATING2:
                SetState(gameState.PARACHUTE);
                break;

            case gameState.PARACHUTE:
                SetState(gameState.LANDING);
                break;

            case gameState.LANDING:
                SetState(gameState.FINISH);
                break;

            case gameState.FINISH:
                SetState(gameState.AWAKE);
                break;
        }
    }
}
