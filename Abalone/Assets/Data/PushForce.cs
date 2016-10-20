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
        private int iMAX_TOUCH = 2;
        private int m_iTouch;
        private E_Player m_Owner;
        
        public PushForce(PositionData inOrigin,BallData inBallOnOrigin)
        {
            m_iTouch = 0;
            m_oOriginTile = inOrigin;
            m_oLastTouched = inOrigin;
            m_vChain.Add(inOrigin);

            m_Owner = inBallOnOrigin.Player;
        }

        public void SetValidLastTouch(PositionData inNewTile)
        {
            //new tile isn't in chain and still there are available touch and the new tile is inside neighbor of last touched
            if(!m_vChain.Contains<PositionData>(inNewTile) && m_iTouch < iMAX_TOUCH && m_oLastTouched.GetNeighbors().Contains(inNewTile))
            {
                //for each tile inside chain
                bool bHasMoreNeighbour = false;
                for(int i = 0; i < m_vChain.Count && !bHasMoreNeighbour; ++i)
                {
                    //check if exist a second neighbour, if yes input isn't a line
                    PositionData tile = m_vChain[i];
                    if (tile.ID != m_oLastTouched.ID && !tile.GetNeighbors().Contains<PositionData>(inNewTile))
                    {
                        bHasMoreNeighbour = true;
                    }

                    //check what is on the tile
                    if(inNewTile.HasBallOn())
                    {
                        //is a friend or an enemy?
                    }
                    else
                    {
                        //empty tile, can move chain balls
                    }
                }


                if(!bHasMoreNeighbour)
                {

                }
            }
        }

    }
}
