//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 4:43:11 PM
//Description ：退出房间指令
//===================================================

using UnityEngine;
using proto.pdk;
namespace PaoDeKuai
{
    public class QuitRoomCommand : IGameCommand
    {
        private PDK_LEAVE m_DataEntity;

        public QuitRoomCommand(PDK_LEAVE data)
        {
            m_DataEntity = data;
        }


        public void Execute()
        {
            RoomPaoDeKuaiProxy.Instance.ExitRoom(m_DataEntity);

            if (RoomPaoDeKuaiProxy.Instance.PlayerSeat != null && m_DataEntity.playerId == RoomPaoDeKuaiProxy.Instance.PlayerSeat.Pos)
            {
                GameCtrl.Instance.ExitGame();
            }
        }

        public void Revoke()
        {

        }
    }
}