//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 4:43:00 PM
//Description ：进入房间指令
//===================================================
using UnityEngine;
using proto.pdk;
namespace PaoDeKuai
{
    public class EnterRoomCommand : IGameCommand
    {
        private PDK_ENTER_ROOM m_DataEntity;

        public EnterRoomCommand(PDK_ENTER_ROOM data)
        {
            m_DataEntity = data;
        }


        public void Execute()
        {
            RoomPaoDeKuaiProxy.Instance.EnterRoom(m_DataEntity);
        }

        public void Revoke()
        {

        }

    }
}