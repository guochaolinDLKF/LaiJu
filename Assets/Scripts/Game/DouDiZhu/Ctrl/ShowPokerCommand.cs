//===================================================
//Author      : DRB
//CreateTime  ：12/6/2017 3:58:25 PM
//Description ：明牌命令
//===================================================

using System;
using System.Collections.Generic;

namespace DRB.DouDiZhu
{
    public class ShowPokerCommand : IGameCommand
    {
        private int m_PlayerId;

        private List<Poker> m_Pokers;

        public ShowPokerCommand(int playerId, List<Poker> pokers)
        {
            m_PlayerId = playerId;
            m_Pokers = pokers;
        }

        public void Execute()
        {
            RoomProxy.Instance.ShowPoker(m_PlayerId, m_Pokers);
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}
