﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//used as Data_struct
public abstract class PositionData : MonoBehaviour {

    //EDITOR_SET
    public uint ID;
    //EDITOR_SET
    public BallData BallOn;

    //private
    /*----------------------------------------------*/
    private BallData m_oBallData;
    private Color m_oOriginalColor;
    protected List<PositionData> m_vNeighbors;

    //Called by derived class: example PositionExagon.cs
    protected void InnerAwake()
    {
        m_oBallData = BallOn;
        m_oOriginalColor = GetComponent<SpriteRenderer>().material.color;
        m_vNeighbors = new List<PositionData>();
    }

    public List<PositionData> GetNeighbors()
    {
        return m_vNeighbors;
    }

    public abstract PositionData GetOppositeTileOf(PositionData inTileOrigin);

    public void SetBallOn(BallData inBallData)
    {
        m_oBallData = inBallData;
        BallOn = m_oBallData;
    }
    public BallData GetBallOn()
    {
        return m_oBallData;
    }

    public void OnMouseOverDraggingOK()
    {
        GetComponent<SpriteRenderer>().material.color = Color.green;
    }
    public void OnMouseOverDraggingNO()
    {
        GetComponent<SpriteRenderer>().material.color = Color.red;
    }

    public void OnMouseExit()
    {
        ResetColor();
    }


    public void ResetColor()
    {
        GetComponent<SpriteRenderer>().material.color = m_oOriginalColor;
    }
    
}
