using UnityEngine;
using System.Collections;
using System;

public class PositionExagon : PositionData {

    //EDITOR_SET
    public PositionData LeftDown;
    //EDITOR_SET
    public PositionData RightDown;
    //EDITOR_SET
    public PositionData Right;
    //EDITOR_SET
    public PositionData RightUp;
    //EDITOR_SET
    public PositionData LeftUp;
    //EDITOR_SET
    public PositionData Left;

    void Awake()
    {
        InnerAwake();

        if (LeftDown)
        {
            m_vNeighbors.Add(LeftDown);
        }
        if (RightDown)
        {
            m_vNeighbors.Add(RightDown);
        }
        if (Right)
        {
            m_vNeighbors.Add(Right);
        }
        if (RightUp)
        {
            m_vNeighbors.Add(RightUp);
        }
        if (LeftUp)
        {
            m_vNeighbors.Add(LeftUp);
        }
        if (Left)
        {
            m_vNeighbors.Add(Left);
        }
    }

    public override PositionData GetOppositeTileOf(PositionData inTileOrigin)
    {
        if(LeftDown && LeftDown.ID == inTileOrigin.ID)
        {
            return RightUp;
        }
        else if (RightDown && RightDown.ID == inTileOrigin.ID)
        {
            return LeftUp;
        }
        else if (Right && Right.ID == inTileOrigin.ID)
        {
            return Left;
        }
        else if (RightUp && RightUp.ID == inTileOrigin.ID)
        {
            return LeftDown;
        }
        else if (LeftUp && LeftUp.ID == inTileOrigin.ID)
        {
            return RightDown;
        }
        else if (Left && Left.ID == inTileOrigin.ID)
        {
            return Right;
        }

        Log.Text("Invalid Tile Origin", E_LogContext.TILE);
        return null;
    }
}
