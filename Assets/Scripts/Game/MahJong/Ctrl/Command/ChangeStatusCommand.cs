//===================================================
//Author      : DRB
//CreateTime  ：9/19/2017 5:09:31 PM
//Description ：
//===================================================
using UnityEngine;

namespace DRB.MahJong
{
    public class ChangeStatusCommand : IGameCommand
    {
        private RoomEntity.RoomStatus m_Status;

        public ChangeStatusCommand(RoomEntity.RoomStatus status)
        {
            m_Status = status;
        }

        public void Execute()
        {
            Debug.Log("================================执行改变房间状态命令");
            RoomMaJiangProxy.Instance.SetStatus(m_Status);
        }

        public void Revoke()
        {
        }
    }
}
