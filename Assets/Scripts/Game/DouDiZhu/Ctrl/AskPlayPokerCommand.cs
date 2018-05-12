//===================================================
//Author      : DRB
//CreateTime  ：12/1/2017 9:51:21 PM
//Description ：
//===================================================
using System;

namespace DRB.DouDiZhu
{
    public class AskPlayPokerCommand : IGameCommand
    {
        private int m_PlayerId;

        private int m_unixTime;

        public AskPlayPokerCommand(int playerId,int unixTime)
        {
            m_PlayerId = playerId;
            m_unixTime = unixTime;
        }

        public void Execute()
        {
            LogSystem.LogWarning("执行询问出牌命令" + m_PlayerId);
            RoomProxy.Instance.AskPlayPoker(m_PlayerId, m_unixTime);
            DouDiZhuGameCtrl.Instance.CheckPlayPoker();
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}
