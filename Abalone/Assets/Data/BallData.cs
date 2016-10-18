using UnityEngine;
using System.Collections;

public class BallData : MonoBehaviour {

    public uint ID;
    public E_Player Player;
    //EDITOR_SET
    public PositionData Position;

    #region private
    private PositionData m_oPositionData;
    #endregion


    //Awake
    void Awake()
    {
        m_oPositionData = Position;
    }

    public PositionData GetPositionData()
    {
        return m_oPositionData;
    }

    public void SetPositionData(PositionData inPosData)
    {
        m_oPositionData = inPosData;
    }

}
