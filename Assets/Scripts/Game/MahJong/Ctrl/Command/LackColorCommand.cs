//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 8:23:24 PM
//Description ：定缺命令
//===================================================
using System;

namespace DRB.MahJong
{
    public class LackColorCommand : IGameCommand
    {
        private int m_PlayerId;

        private int m_LackColor;

        public LackColorCommand(int playerId, int lackColor)
        {
            m_PlayerId = playerId;
            m_LackColor = lackColor;
        }

        public void Execute()
        {
            RoomMaJiangProxy.Instance.SetLackColor(m_PlayerId,m_LackColor);
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}
