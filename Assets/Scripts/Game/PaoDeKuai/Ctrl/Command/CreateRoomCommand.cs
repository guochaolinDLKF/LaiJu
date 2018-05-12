//===================================================
//Author      : WZQ
//CreateTime  ：11/29/2017 4:28:45 PM
//Description ：
//===================================================
using UnityEngine;

namespace PaoDeKuai {
    public class CreateRoomCommand : IGameCommand
    {
        private RoomEntity m_Room;

        public CreateRoomCommand(RoomEntity room)
        {
            m_Room = room;
        }


        public void Execute()
        {
            //RoomPaoDeKuaiProxy.Instance.InitRoom(m_Room);
        }

        public void Revoke()
        {

        }

    }
}