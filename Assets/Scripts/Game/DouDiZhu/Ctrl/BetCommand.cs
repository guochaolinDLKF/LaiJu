//===================================================
//Author      : DRB
//CreateTime  ：12/4/2017 8:24:44 PM
//Description ：下注命令
//===================================================
using System;

namespace DRB.DouDiZhu
{
    public class BetCommand : IGameCommand
    {
        private int m_PlayerId;

        private int m_Bet;

        public BetCommand(int playerId, int bet)
        {
            m_PlayerId = playerId;
            m_Bet = bet;
        }


        public void Execute()
        {
            LogSystem.LogWarning("执行下注命令" + m_PlayerId.ToString() + "下了" + m_Bet.ToString());
            RoomProxy.Instance.Bet(m_PlayerId, m_Bet);
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}
