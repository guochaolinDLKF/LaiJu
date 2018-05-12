//===================================================
//Author      : DRB
//CreateTime  ：12/4/2017 7:24:13 PM
//Description ：设置座位状态命令
//===================================================
using System;

namespace DRB.DouDiZhu
{
    public class AskBetCommand : IGameCommand
    {
        private int m_PlayerId;

        public AskBetCommand(int playerId)
        {
            m_PlayerId = playerId;
        }


        public void Execute()
        {
            LogSystem.LogWarning("执行询问下注命令" + m_PlayerId.ToString());
            RoomProxy.Instance.AskBet(m_PlayerId);
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}
