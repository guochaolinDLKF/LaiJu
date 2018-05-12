//===================================================
//Author      : DRB
//CreateTime  ：12/6/2017 3:51:19 PM
//Description ：结算命令
//===================================================

using System;

namespace DRB.DouDiZhu
{
    public class SettleCommand : IGameCommand
    {

        private RoomEntity m_Room;

        public SettleCommand(RoomEntity room)
        {
            m_Room = room;
        }

        public void Execute()
        {
            LogSystem.LogWarning("执行结算命令");

            RoomProxy.Instance.Settle(m_Room);

            DouDiZhuGameCtrl.Instance.OpenSettleView(m_Room);
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}