using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Data
{
    class PushForce
    {
        private PositionData        m_oOriginTile;
        private PositionData        m_oLastTouched;
        private List<PositionData>  m_vChain;
        private List<PositionData>  m_vEnemyChain;
        private int iMAX_TOUCH = 2;
        private int m_iFriendBallCollected;
        private int m_iEnemyBallCollected;
        private E_Player m_Owner;
        
        public PushForce(PositionData inOrigin)
        {
            m_iFriendBallCollected = 1;
            m_iEnemyBallCollected = 0;
            m_oOriginTile = inOrigin;
            m_oLastTouched = inOrigin;
            m_vChain = new List<PositionData>();
            m_vChain.Add(inOrigin);
            m_vEnemyChain = new List<PositionData>();

            m_Owner = inOrigin.GetBallOn().GetPlayer();
        }

        public List<PositionData> GetFriendlyChain()
        {
            return m_vChain;
        }

        public bool ManageInput(BallData inNewBall)
        {
            return ManageInput(inNewBall.GetPositionData());
        }
        public bool ManageInput(PositionData inNewTile)
        {
            //friendly or enemy ball?
            if (IsLastSelectedFriendly(inNewTile.GetBallOn()))
            {
                Log.Text("FriendInput", E_LogContext.PUSH_FORCE);              
                if (!IsAlreadyCollected(inNewTile))
                {
                    Log.Text("Found not collect friend", E_LogContext.PUSH_FORCE);
                    if (CanCollectFriend())
                    {
                        Log.Text("Can still collect friend", E_LogContext.PUSH_FORCE);
                        if (HasValidDirection(inNewTile))
                        {
                            ++m_iFriendBallCollected;
                            m_vChain.Add(inNewTile);
                            m_oLastTouched = inNewTile;
                            Log.Text("Friendly ball collected:" + m_iFriendBallCollected, E_LogContext.PUSH_FORCE);
                            return true;
                        }
                        else
                        {
                            Log.Text("Friendly ball not collected but with wrong direction", E_LogContext.PUSH_FORCE);
                        }                        
                    }
                    else
                    {
                        Log.Text("Can't collect more friend", E_LogContext.PUSH_FORCE);
                    }
                }
                else
                {
                    Log.Text("Friendly ball already collected");
                    return true;
                }
            }
            else
            {
                Log.Text("EnemyInput", E_LogContext.PUSH_FORCE);
                if(CanPushEnemy(inNewTile))
                {
                    Log.Text("Can push enemy", E_LogContext.PUSH_FORCE);
                }
                else
                {
                    Log.Text("Can't push enemy", E_LogContext.PUSH_FORCE);
                }
            }

            return false;
        }

        private bool IsLastSelectedFriendly(BallData inNewBall)
        {
            return m_Owner == inNewBall.GetPlayer();
        }

        private bool CanCollectFriend()
        {
            return m_iFriendBallCollected < 3;
        }

        private bool CanPushEnemy(PositionData inNewTile)
        {
            //TODO can push enemy?????
            return false;
        }

        private bool HasValidDirection(PositionData inNewTile)
        {
            return IsAdjacentLastTouched(inNewTile) && IsAdjacentOnlyOne(inNewTile);
        }

        private bool IsAlreadyCollected(PositionData inNewTile)
        {
            return m_vChain.Contains<PositionData>(inNewTile);
        }

        private bool IsAdjacentLastTouched(PositionData inNewTile)
        {
            return m_oLastTouched.GetNeighbors().Contains<PositionData>(inNewTile);
        }

        private bool IsAdjacentOnlyOne(PositionData inNewTile)
        {
            int adj = 0;
            for (int i = 0; i < m_vChain.Count; ++i)
            {
                if (m_vChain[i].GetNeighbors().Contains<PositionData>(inNewTile))
                {
                    ++adj;
                }
            }

            return adj == 1;
        }
    }
}
