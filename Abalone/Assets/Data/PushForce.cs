using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Data
{
    class PushForce
    {
        private PositionData        m_oLastTouched;
        private List<PositionData>  m_vChain;
        private List<PositionData>  m_vEnemyChain;
        private int         m_iFriendBallCollected;
        private int         m_iEnemyBallCollected;
        private bool        m_bCanPushEnemyChain;
        private bool        m_bCanPushFriends;
        private E_Player    m_Owner;
        
        public PushForce(PositionData inOrigin)
        {
            m_iFriendBallCollected = 1;
            m_oLastTouched = inOrigin;
            m_vChain = new List<PositionData>();
            m_vChain.Add(inOrigin);

            m_Owner = inOrigin.GetBallOn().GetPlayer();

            m_iEnemyBallCollected = 0;
            m_bCanPushEnemyChain = false;
            m_bCanPushFriends = false;
        }
        public List<PositionData> GetFriendlyChain()
        {
            return m_vChain;
        }
        public List<PositionData> GetEnemyChain()
        {
            return m_vEnemyChain;
        }
        public bool IPushForceValid()
        {
            return m_bCanPushEnemyChain || m_bCanPushFriends;
        }
        public void ResetTilesInChains()
        {
            if (m_vChain != null)
            {
                foreach (PositionData TileInChain in m_vChain)
                {
                    TileInChain.ResetColor();
                }
                if (m_vEnemyChain != null)
                {
                    foreach (PositionData TileInChain in m_vEnemyChain)
                    {
                        TileInChain.ResetColor();
                    }
                }
            }
        }

        public bool ManageInput(BallData inNewBall)
        {
            Log.Text("ManageInput:Ball " + inNewBall.name, E_LogContext.PUSH_FORCE);
            return ManageInput(inNewBall.GetPositionData());
        }
        public bool ManageInput(PositionData inNewTile)
        {
            Log.Text("ManageInput:Tile "+inNewTile.name, E_LogContext.PUSH_FORCE);
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
                            m_bCanPushFriends = true;
                            return m_bCanPushFriends;
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
                    Log.Text("Friendly ball already collected", E_LogContext.PUSH_FORCE);
                    return m_bCanPushFriends;
                }
            }
            else
            {
                Log.Text("EnemyInput", E_LogContext.PUSH_FORCE);
                if (m_vEnemyChain != null && m_vEnemyChain.Contains<PositionData>(inNewTile))
                {
                    //Is an enemy already collected,
                    return m_bCanPushEnemyChain;
                }
                else
                {
                   
                    return CanPushEnemy(inNewTile);
                }
            }

            return false;
        }

        public void ResolvePushing()
        {
            Log.Text("RESOLVE PUSHING",E_LogContext.LOG);
            PrintChains();
            List<PositionData> copyWork = new List<PositionData>();
            if (m_vChain != null && m_vChain.Count > 0)
            {
                foreach (PositionData friend in m_vChain)
                {
                    copyWork.Add(friend);
                }
            }
            if (m_vEnemyChain != null && m_vEnemyChain.Count > 0)
            {
                foreach (PositionData enemy in m_vEnemyChain)
                {
                    copyWork.Add(enemy);
                }
            }
            
            PositionData last = copyWork.Last();
            copyWork.Remove(last);
            if (last.GetBallOn())
            {
                last.GetBallOn().gameObject.SetActive(false);
                last.SetBallOn(null);       
            }
            while (copyWork.Count > 0)
            {
                PositionData prevTile = copyWork.Last();
                copyWork.Remove(prevTile);
                if (prevTile != null)
                {
                    MoveBallFromTo(prevTile, last);
                }
                last = prevTile;
            }
        }

        private bool IsLastSelectedFriendly(BallData inNewBall)
        {
            if(inNewBall == null)
            {
                return true;
            }
            Log.Text("IsLastSelectedFriendly: m_Owner " + (m_Owner), E_LogContext.PUSH_FORCE);
            Log.Text("IsLastSelectedFriendly: inNewBall " + inNewBall.name, E_LogContext.PUSH_FORCE);
            Log.Text("IsLastSelectedFriendly: inNewBall.GetPlayer() " + inNewBall.GetPlayer(), E_LogContext.PUSH_FORCE);
            Log.Text("IsLastSelectedFriendly: "+(m_Owner.Equals(inNewBall.GetPlayer())), E_LogContext.PUSH_FORCE);
            return m_Owner.Equals(inNewBall.GetPlayer());
        }

        private bool CanCollectFriend()
        {
            return m_iFriendBallCollected < 3;
        }

        private bool CanPushEnemy(PositionData inNewTile)
        {
            Log.Text("CanPushEnemy", E_LogContext.PUSH_FORCE);
            if (HasValidDirection(inNewTile))
            {
                //Instantiate enemy chain with the first one
                if(m_vEnemyChain == null)
                {
                    Log.Text("Found first enemy",E_LogContext.PUSH_FORCE);
                    m_vEnemyChain = new List<PositionData>();
                    m_vEnemyChain.Add(inNewTile);
                    ++m_iEnemyBallCollected;
                    m_oLastTouched = inNewTile;
                    m_bCanPushEnemyChain = SearchEnemyInChain(inNewTile, m_oLastTouched);
                }
                else
                {
                    if(!m_vEnemyChain.Contains<PositionData>(inNewTile))
                    {
                        Log.Text("Found another enemy", E_LogContext.PUSH_FORCE);
                        m_vEnemyChain.Add(inNewTile);
                        ++m_iEnemyBallCollected;
                        m_oLastTouched = inNewTile;
                        m_bCanPushEnemyChain = SearchEnemyInChain(inNewTile, m_oLastTouched);
                    }
                }
               
            }
            Log.Text("CanPushEnemy: return "+ m_bCanPushEnemyChain, E_LogContext.PUSH_FORCE);
            return m_bCanPushEnemyChain;
        }

        private bool SearchEnemyInChain(PositionData inNewTile, PositionData InPrevDirection)
        {
            Log.Text("SearchEnemyInChain: inNewTile " + inNewTile.name+ " , InPrevDirection " + InPrevDirection.name, E_LogContext.PUSH_FORCE);
            //how many enemy ball has been collected?
            PositionData oppositeTile = inNewTile.GetOppositeTileOf(InPrevDirection);
            if (oppositeTile == null)
            {
                Log.Text("SearchEnemyInChain: inNewTile oppositeTile == null" , E_LogContext.PUSH_FORCE);
                //Border reached
                return FriendMoreEnemy();
            }
            else
            {
                Log.Text("SearchEnemyInChain: oppositeTile found, is :"+ oppositeTile.name, E_LogContext.PUSH_FORCE);
                //Is an empty tile?
                if (oppositeTile.GetBallOn() == null)
                {
                    Log.Text("SearchEnemyInChain: inNewTile hasn't ball on", E_LogContext.PUSH_FORCE);
                    return FriendMoreEnemy();
                }
                else
                {
                    //there is a ball over, is friendly?
                    if (IsLastSelectedFriendly(oppositeTile.GetBallOn()))
                    {
                        Log.Text("SearchEnemyInChain return false:  IsLastSelectedFriendly", E_LogContext.PUSH_FORCE);
                    }
                    else
                    {
                        if (m_iEnemyBallCollected < 3)
                        {
                            if(!m_vEnemyChain.Contains(inNewTile))
                            {
                                m_vEnemyChain.Add(inNewTile);
                                ++m_iEnemyBallCollected;
                                Log.Text("m_iEnemyBallCollected = " + m_iEnemyBallCollected +
                                        " \nSearchEnemyInChain valid enemy found, searching next", E_LogContext.PUSH_FORCE);
                                return SearchEnemyInChain(oppositeTile, inNewTile);
                            }
                        }
                        else
                        {
                            Log.Text("SearchEnemyInChain return false:  m_iEnemyBallCollected < 3", E_LogContext.PUSH_FORCE);
                        }
                    }
                }
            }
           
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
            Log.Text("IsAdjacentLastTouched: " + m_oLastTouched.GetNeighbors().Contains<PositionData>(inNewTile), E_LogContext.PUSH_FORCE);
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

        private bool FriendMoreEnemy()
        {
            Log.Text("FriendMoreEnemy: "+(m_iFriendBallCollected > m_iEnemyBallCollected), E_LogContext.PUSH_FORCE);
            return m_iFriendBallCollected > m_iEnemyBallCollected;
        }

        private void MoveBallFromTo(PositionData prevTile,PositionData last)
        {
            last.SetBallOn(prevTile.GetBallOn());
            last.GetBallOn().SetPositionData(last);
            last.GetBallOn().gameObject.transform.SetParent(last.transform);
            last.GetBallOn().gameObject.transform.localPosition = new UnityEngine.Vector3(0,0,0);
            prevTile.SetBallOn(null);
        }

        private void PrintChains()
        {
            string tiles = "FriendTiles: \n";
            foreach (PositionData friendTile in m_vChain)
            {
                tiles += ("FriendTile: " + friendTile.name + "\n");
            }

            tiles += ("EnemyTiles: \n");

            if(m_vEnemyChain != null && m_vEnemyChain.Count > 0)
            {
                foreach (PositionData enemyTile in m_vEnemyChain)
                {
                    tiles += ("EnemyTiles: " + enemyTile.name + "\n");
                }
                Log.Text(tiles + "\n", E_LogContext.LOG);
            }
        }
    }
}
