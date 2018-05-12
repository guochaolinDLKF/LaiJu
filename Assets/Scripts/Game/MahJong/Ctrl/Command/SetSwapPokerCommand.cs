//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 8:25:14 PM
//Description ：设置交换牌命令
//===================================================
using System;
using System.Collections.Generic;

namespace DRB.MahJong
{
    public class SetSwapPokerCommand : IGameCommand
    {
        private int m_PlayerId;

        private List<Poker> m_SwapPokers;

        public SetSwapPokerCommand(int playerId, List<Poker> swapPokers)
        {
            m_PlayerId = playerId;
            m_SwapPokers = swapPokers;
        }


        public void Execute()
        {
            RoomMaJiangProxy.Instance.SetSwapPoker(m_PlayerId, m_SwapPokers);
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}
