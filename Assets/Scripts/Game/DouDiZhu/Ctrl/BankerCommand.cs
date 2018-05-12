//===================================================
//Author      : DRB
//CreateTime  ：12/6/2017 11:31:59 AM
//Description ：设置地主命令
//===================================================

using System;
using System.Collections.Generic;

namespace DRB.DouDiZhu
{
    public class BankerCommand : IGameCommand
    {
        private int m_PlayerId;

        private List<Poker> m_Pokers;

        public BankerCommand(int playerId, List<Poker> pokers)
        {
            m_PlayerId = playerId;
            m_Pokers = pokers;
        }

        public void Execute()
        {
            RoomProxy.Instance.SetBanker(m_PlayerId, m_Pokers);
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}