using UnityEngine;
using System.Collections;

//used as Data_struct
public class PositionData : MonoBehaviour {

    //EDITOR_SET
    public uint ID;
    //EDITOR_SET
    public BallData BallOn;
    //EDITOR_SET
    public PositionData RightDown;
    //EDITOR_SET
    public PositionData LeftDown;
    //EDITOR_SET
    public PositionData Right;
    //EDITOR_SET
    public PositionData Left;
    //EDITOR_SET
    public PositionData RightUp;
    //EDITOR_SET
    public PositionData LeftUp;

    //private
    /*----------------------------------------------*/
    private BallData m_oBallData;
    private Color m_oOriginalColor;

    void Awake()
    {
        m_oBallData = BallOn;
        m_oOriginalColor = GetComponent<SpriteRenderer>().material.color;
    }

    public BallData GetBallOn()
    {
        return m_oBallData;
    }
    public void SetBallOn(BallData inBallData)
    {
        m_oBallData = inBallData;
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
