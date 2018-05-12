//===================================================
//Author      : DRB
//CreateTime  ：11/28/2017 2:41:46 PM
//Description ：发牌命令
//===================================================
using System.Collections.Generic;

namespace DRB.DouDiZhu
{
    public class BeginCommand : IGameCommand
    {
        private List<SeatEntity> m_SeatList;
        private int m_loop;

        public BeginCommand(List<SeatEntity> lstSeat,int loop)
        {
            m_SeatList = lstSeat;
            m_loop = loop;
        }


        public void Execute()
        {
            LogSystem.LogWarning("执行发牌");
            RoomProxy.Instance.Begin(m_SeatList, m_loop);
        }

        public void Revoke()
        {

        }

    }
}
