using UnityEngine;
using System.Collections;
using System;

public class GameLogic : SceneSingleton {

    private static E_GameState  GameState;
    private static E_Player     PlayerTurn;

    public override void InternalAwake()
    {
        GameState = E_GameState.IDLE;
        PlayerTurn = E_Player.PLAYER_ONE;
    }

    public static E_GameState GetGameState()
    {
        return GameState;
    }
    public static void SetGameState( E_GameState newState)
    {
        GameState = newState;
    }

    public static void BallSelectedInIdle(BallData inBallData)
    {
        if(inBallData.Player == PlayerTurn)
        {
            Debug.Log(PlayerTurn.ToString()+"-> Clicked down on ball : " + inBallData.name);
            BallDrag drag = inBallData.GetComponent<BallDrag>();
            drag.enabled = true;
        }
    }
}
