//===================================================
//Author      : DRB
//CreateTime  ：11/28/2017 1:32:12 PM
//Description ：
//===================================================

namespace DRB.DouDiZhu
{
    public class CreateRoomCommand : IGameCommand
    {

        private RoomEntity m_Room;

        public CreateRoomCommand(RoomEntity room)
        {
            m_Room = room;
        }


        public void Execute()
        {
            LogSystem.LogWarning("执行创建房间命令" + m_Room.roomId);
            RoomProxy.Instance.InitRoom(m_Room);
        }

        public void Revoke()
        {

        }
    }
}

