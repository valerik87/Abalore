using UnityEngine;
using System.Collections;
using System;

public enum E_GameState
{
    IDLE,
    DRAG
}

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

        switch(GameState)
        {
            case E_GameState.DRAG:
                Debug.Log("Dragging Ball");
                break;
            case E_GameState.IDLE:
                Debug.Log("Idle");
                break;
            default:
                break;
        }
    }

    private static BallDrag m_oDraggedBall;

    #region ChangeStates
    //------------------------IDLE

    public static void BallSelectedInIdle(BallData inBallData)
    {
        if(inBallData.Player == PlayerTurn)
        {
            Debug.Log(PlayerTurn.ToString() + "-> Clicked down on ball : " + inBallData.name);

            SetGameState(E_GameState.DRAG);
            
            BallDrag drag = inBallData.GetComponent<BallDrag>();
            m_oDraggedBall = drag;
            drag.StartDrag();
        }
    }


    //------------------------DRAG
    
    public static void BallDeselectedOnTile(PositionData inTile)
    {
        Debug.Log(PlayerTurn.ToString() + "-> Clicked up on position : " + inTile.name);
        //check if dragged ball still exist and have a ballData linked to a positionData that exist
        if (
            m_oDraggedBall != null &&                                                       
            m_oDraggedBall.GetComponent<BallData>() &&
            m_oDraggedBall.GetComponent<BallData>().GetPositionData() != null
            )
        {
            PositionData oldPositionData = m_oDraggedBall.GetComponent<BallData>().GetPositionData();
            oldPositionData.SetBallOn(null);

            inTile.SetBallOn(m_oDraggedBall.GetComponent<BallData>());
            inTile.GetBallOn().SetPositionData(inTile);

            BallDrag drag = m_oDraggedBall.GetComponent<BallDrag>();
            drag.StopDrag();

            SetGameState(E_GameState.IDLE);
        }
        else
        {
            Debug.Log("BallDeselectedOnNothing: error on saved draggedBall or in tile");
        }
    }

    public static void BallDeselectedOnNothing()
    {
        if(m_oDraggedBall != null && m_oDraggedBall.GetComponent<BallDrag>())
        {
            BallDrag drag = m_oDraggedBall.GetComponent<BallDrag>();
            drag.StopDrag();

            SetGameState(E_GameState.IDLE);
        }
        else
        {
            Debug.Log("BallDeselectedOnNothing: error on saved draggedBall");
        }
    }

    public static void MouseDraggingOverTile(PositionData inTile)
    {
        inTile.OnMouseOverDragging();
    }
    #endregion
}
