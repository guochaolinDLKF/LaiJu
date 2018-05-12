//===================================================
//Author      : DRB
//CreateTime  ：12/18/2017 11:04:46 AM
//Description ：总结算命令
//===================================================
using System;

namespace DRB.DouDiZhu
{
    public class ResultCommand : IGameCommand
    {

        private RoomEntity m_Room;

        public ResultCommand(RoomEntity room)
        {
            m_Room = room;
        }

        public void Execute()
        {
            LogSystem.LogWarning("执行结算命令");

            RoomProxy.Instance.Result(m_Room);

            DouDiZhuGameCtrl.Instance.OpenResultView(m_Room);
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}
