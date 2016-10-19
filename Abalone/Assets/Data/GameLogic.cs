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
                Log.Text("Dragging Ball", E_LogContext.GAME_LOGIC);
                break;
            case E_GameState.IDLE:
                Log.Text("Idle", E_LogContext.GAME_LOGIC);
                break;
            default:
                break;
        }
    }

    private static BallData m_oDraggedBall;

    #region ChangeStates
    //------------------------IDLE

    public static void BallSelectedInIdle(BallData inBallData)
    {
        Log.Text(PlayerTurn.ToString() + "-> Clicked down on ball : " + inBallData.name, E_LogContext.GAME_LOGIC);
        if (inBallData.Player == PlayerTurn)
        {
            SetGameState(E_GameState.DRAG);

            if(inBallData.GetComponent<BallDrag>())
            {
                m_oDraggedBall = inBallData;
                inBallData.GetComponent<BallDrag>().StartDrag();
            }
            else
            {
                Log.Text(inBallData.name +" miss BallDrag component", E_LogContext.GAME_LOGIC);
            }
        }
    }


    //------------------------DRAG
    
    public static void BallDeselectedOnTile(PositionData inTile)
    {
        Log.Text(PlayerTurn.ToString() + "-> Clicked up on position : " + inTile.name, E_LogContext.GAME_LOGIC);
        //check if dragged ball still exist and have a ballData linked to a positionData that exist
        if (CanBallMoveOn(inTile))
        {
            Log.Text("Ball can be release up on position : " + inTile.name, E_LogContext.GAME_LOGIC);
            PositionData oldPositionData = m_oDraggedBall.GetPositionData();
            oldPositionData.SetBallOn(null);

            inTile.SetBallOn(m_oDraggedBall);
            inTile.GetBallOn().SetPositionData(inTile);

            BallDrag drag = m_oDraggedBall.GetComponent<BallDrag>();
            if (drag != null)
            {
                drag.gameObject.transform.SetParent(inTile.gameObject.transform, false);
                drag.StopDrag();
            }

            SetGameState(E_GameState.IDLE);
        }
        else
        {
            Log.Text("Ball can't be release up on position : " + inTile.name, E_LogContext.GAME_LOGIC);
            inTile.ResetColor();
            BallDeselectedOnNothing();
        }
    }

    public static void BallDeselectedOnBall(BallData inBall)
    {
        if (m_oDraggedBall != null && m_oDraggedBall.GetComponent<BallDrag>())
        {
            if(CanBallMoveOn(inBall))
            {

            }
            else
            {
                m_oDraggedBall.GetComponent<BallDrag>().StopDrag();
                SetGameState(E_GameState.IDLE);

                PositionData TileInBall = inBall.GetPositionData();
                if(TileInBall)
                {
                    TileInBall.ResetColor();
                }
            }
        }
        else
        {
            Log.Text("BallDeselectedOnNothing: error on saved draggedBall", E_LogContext.GAME_LOGIC);
        }
    }

    public static void BallDeselectedOnNothing()
    {
        if(m_oDraggedBall != null && m_oDraggedBall.GetComponent<BallDrag>())
        {
            m_oDraggedBall.GetComponent<BallDrag>().StopDrag();
            SetGameState(E_GameState.IDLE);
        }
        else
        {
            Log.Text("BallDeselectedOnNothing: error on saved draggedBall", E_LogContext.GAME_LOGIC);
        }
    }

    public static void MouseDraggingOverTile(PositionData inTile)
    {
        if(CanBallMoveOn(inTile))
        {
            inTile.OnMouseOverDraggingOK();
        }
        else
        {
            inTile.OnMouseOverDraggingNO();
        }
        
    }

    public static void MouseDraggingOverBall(BallData inBall)
    {
        if (CanBallMoveOn(inBall))
        {
            inBall.GetPositionData().OnMouseOverDraggingOK();
        }
        else
        {
            inBall.GetPositionData().OnMouseOverDraggingNO();
        }
    }
    #endregion


    #region Utility
    //this method will evolve checking if ball can move on a tile????
    private static bool CanBallMoveOn(PositionData inTile)
    {
        if (
            m_oDraggedBall != null &&                          //ball      exist
            m_oDraggedBall.GetPositionData() != null &&        //ballData  has a position data
            inTile != null                                     //tile      exist
            )
        {
            //Tile is empty
            if (inTile.GetBallOn() == null)
            {
                return true;
            }
            else
            {
                //Selecting the starting tile
                if(inTile.ID == m_oDraggedBall.GetPositionData().ID)
                {
                    return true;
                }
                return false;
            }
        }
        else
        {
            Log.Text("CanBallMoveOn: error on saved draggedBall or in tile");
        }

        return false;
    }

    //this method will evolve checking if ball can move on a ball????
    //actually the behaviour is always as false
    private static bool CanBallMoveOn(BallData inBall)
    {
        return false;
    }
    #endregion
}
