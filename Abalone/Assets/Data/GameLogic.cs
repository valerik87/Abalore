﻿using UnityEngine;
using System.Collections;
using System;

public enum E_GameState
{
    IDLE,
    DRAG,
    PUSHING
}

public class GameLogic : SceneSingleton {

    private static E_GameState  GameState;
    private static E_Player     PlayerTurn;

    public override void InternalAwake()
    {
        GameState = E_GameState.IDLE;
        PlayerTurn = E_Player.PLAYER_ONE;
        m_oDraggedBall = null;
        m_oPushForce = null;
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
                Log.Text("DRAG STATE", E_LogContext.GAME_LOGIC);
                break;
            case E_GameState.IDLE:
                Log.Text("IDLE STATE", E_LogContext.GAME_LOGIC);
                break;
            case E_GameState.PUSHING:
                Log.Text("PUSHING STATE", E_LogContext.GAME_LOGIC);
                break;
            default:
                Log.Error("GAME STATE NOT FOUND");
                break;
        }
    }

    private static BallData                 m_oDraggedBall;
    private static Assets.Data.PushForce    m_oPushForce;

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


    //------------------------DRAG && PUSHING
    
    public static void BallDeselectedOnTile(PositionData inTile)
    {
        Log.Text(PlayerTurn.ToString() + "-> Clicked up on position : " + inTile.name, E_LogContext.GAME_LOGIC);
        //check if dragged ball still exist and have a ballData linked to a positionData that exist
        if (CanBallMoveOnTile(inTile))
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
            if(CanBallMoveOnBall(inBall))
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
        switch (GameState)
        {
            case (E_GameState.DRAG):
            case (E_GameState.PUSHING):
                if (m_oDraggedBall != null && m_oDraggedBall.GetComponent<BallDrag>())
                {
                    m_oDraggedBall.GetComponent<BallDrag>().StopDrag();

                    if(m_oPushForce != null)
                    {
                        foreach (PositionData TileInChain in m_oPushForce.GetFriendlyChain())
                        {
                            TileInChain.ResetColor();
                        }
                        if (m_oPushForce.GetEnemyChain() != null)
                        {
                            foreach (PositionData TileInChain in m_oPushForce.GetEnemyChain())
                            {
                                TileInChain.ResetColor();
                            }
                        }
                        m_oPushForce = null;
                    }

                    SetGameState(E_GameState.IDLE);


                }
                else
                {
                    Log.Text("BallDeselectedOnNothing: error on saved draggedBall", E_LogContext.GAME_LOGIC);
                }
                break;
            default:
                break;
        }
                
    }

    public static void BallDraggingOnNothing()
    {
        switch (GameState)
        {
            case (E_GameState.DRAG):
                if (m_oDraggedBall != null)
                {
                    m_oDraggedBall.GetPositionData().OnMouseOverDraggingOK();
                }
                break;
            case (E_GameState.PUSHING):
                if (m_oPushForce != null)
                {
                    foreach (PositionData TileInChain in m_oPushForce.GetFriendlyChain())
                    {
                        TileInChain.OnMouseOverDraggingOK();
                    }
                }
                break;
            default:
                break;
        }

    }

    public static void BallDraggingOverTile(PositionData inTile)
    {
        Log.Text("BallDraggingOverTile", E_LogContext.GAME_LOGIC);
        if (CanBallMoveOnTile(inTile))
        {
            switch(GameState)
            {
                case (E_GameState.DRAG):
                    inTile.OnMouseOverDraggingOK();
                    break;
                case (E_GameState.PUSHING):
                    foreach(PositionData TileInChain in m_oPushForce.GetFriendlyChain())
                    {
                        TileInChain.OnMouseOverDraggingOK();
                    }
                    if(m_oPushForce.GetEnemyChain() != null)
                    {
                        foreach (PositionData TileInChain in m_oPushForce.GetEnemyChain())
                        {
                            TileInChain.OnMouseOverDraggingOK();
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (GameState)
            {
                case (E_GameState.DRAG):
                    inTile.OnMouseOverDraggingNO();
                    break;
                case (E_GameState.PUSHING):
                    if(m_oPushForce != null)
                    {
                        foreach (PositionData TileInChain in m_oPushForce.GetFriendlyChain())
                        {
                            TileInChain.OnMouseOverDraggingNO();
                        }
                    }
                    m_oDraggedBall.GetPositionData().OnMouseOverDraggingNO();
                    inTile.OnMouseOverDraggingNO();
                    break;
                default:
                    break;
            }
        }
    }

    public static void BallDraggingOverBall(BallData inBall)
    {
        BallDraggingOverTile(inBall.GetPositionData());
    }
    #endregion


    #region Utility

    //this method will evolve checking if ball can move on a tile????
    private static bool CanBallMoveOnTile(PositionData inTile)
    {
        Log.Text("CanBallMoveOnTile", E_LogContext.GAME_LOGIC);
        if (
            m_oDraggedBall != null &&                          //ball      exist
            m_oDraggedBall.GetPositionData() != null &&        //ballData  has a position data
            inTile != null                                     //tile      exist
            )
        {
            switch (GameState)
            {
                case (E_GameState.DRAG):
                    //Tile is empty or it's itself
                    if (inTile.GetBallOn() == null || inTile.ID == m_oDraggedBall.GetPositionData().ID)
                    {
                        return true;
                    }
                    //touching a new tile with a ball over
                    return CanBallMoveOnBall(inTile.GetBallOn());

                case (E_GameState.PUSHING):
                    if (inTile.GetBallOn() != null)
                    {
                        if (inTile.ID != m_oDraggedBall.GetPositionData().ID)
                        {
                            return CanBallMoveOnBall(inTile.GetBallOn());
                        }
                        else
                        {
                            Log.Text("CanBallMoveOnTile: pushing over starting tile", E_LogContext.GAME_LOGIC);
                        }
                    }
                    else
                    {
                        Log.Text("TODO CanBallMoveOnTile: pushing over an empty tile", E_LogContext.GAME_LOGIC);
                        //TODO sono in pushing ma su una tile vuota, come mi sposto?
                    }
                    break;
                default:
                    break;
            }

            return false;
        }

        //shouldn't be here
        Log.Error("CanBallMoveOn: error on saved draggedBall or in tile");
        return false;            
    }

    //this method will evolve checking if ball can move on a ball????
    //actually the behaviour is always as false
    private static bool CanBallMoveOnBall(BallData inBall)
    {
        switch (GameState)
        {
            case E_GameState.DRAG:
                if (m_oDraggedBall != null && inBall != null)
                {
                    //was dragging but then touched another ball, so a push is started
                    //call recursively this method as is the first touch occured!
                    SetGameState(E_GameState.PUSHING);
                    Log.Text("CanBallMoveOnBall: Touched a tile with a ball on or a ball while dragging", E_LogContext.GAME_LOGIC);
                    return CanBallMoveOnBall(inBall);
                }
                else
                {
                    Log.Error("No valid dragging ball or touched ball while dragging");
                }
                
                break;
            case E_GameState.PUSHING:
                if (m_oDraggedBall != null && inBall != null)
                {
                    Log.Text("CanBallMoveOnBall: m_oDraggedBall && inBall not null", E_LogContext.GAME_LOGIC);
                    if (m_oPushForce == null)
                    {
                        //FirstTouch!
                        if(SomeTeamBall(inBall))
                        {
                            Log.Text("CanBallMoveOnBall: Pushing start", E_LogContext.GAME_LOGIC);
                            m_oPushForce = new Assets.Data.PushForce(m_oDraggedBall.GetPositionData());
                            return m_oPushForce.ManageInput(inBall);
                        }
                        else
                        {
                            Log.Text("CanBallMoveOnBall: Can't push an adj enemy without a friendly chain", E_LogContext.GAME_LOGIC);
                            //Touching a unfriendly ball as first is always invalid, instantiate m_oPushforce isn't required
                            return false;
                        }
                    }
                    else
                    {
                        if(m_oPushForce != null)
                        {
                            Log.Text("CanBallMoveOnBall: Valid input, managing it", E_LogContext.GAME_LOGIC);
                            return m_oPushForce.ManageInput(inBall);
                        }
                        else
                        {
                            Log.Error("m_oPushForce is null!");
                        }
                    }
                }
                else
                {
                    Log.Error("No valid dragging ball or touched ball while pushing");
                }

                break;
            default:
                break;
        }
        Log.Text("CanBallMoveOnBall: SafeReturn false", E_LogContext.GAME_LOGIC);
        return false;
    }

    private static bool SomeTeamBall(BallData inBall)
    {
        return inBall.Player == m_oDraggedBall.Player;
    }
    #endregion
}
