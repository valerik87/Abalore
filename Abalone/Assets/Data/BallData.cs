using UnityEngine;
using System.Collections;

public class BallData : MonoBehaviour {

    public uint ID;
    public E_Player Player;
    //EDITOR_SET
    public PositionData Position;

    #region private
    private PositionData m_oPositionData;
    private E_Player m_oPlayer;
    #endregion


    //Awake
    void Awake()
    {
        m_oPositionData = Position;
        m_oPlayer = Player;
    }

    public PositionData GetPositionData()
    {
        return m_oPositionData;
    }

    public void SetPositionData(PositionData inPosData)
    {
        m_oPositionData = inPosData;
    }

    public E_Player GetPlayer()
    {
        return m_oPlayer;
    }

    public void SetPlayer(E_Player newPlayer)
    {
        m_oPlayer = newPlayer;
    }


}
