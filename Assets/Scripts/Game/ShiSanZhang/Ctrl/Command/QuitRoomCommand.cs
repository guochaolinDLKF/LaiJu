//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 8:22:00 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShiSanZhang
{
    public class QuitRoomCommand : IGameCommand    
    {
        private int m_PlayerId;

        public QuitRoomCommand(int playerId)
        {
            m_PlayerId = playerId;
        }

        public void Execute()
        {
            RoomShiSanZhangProxy.Instance.ExitRoom(m_PlayerId);
            if (m_PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
            {
                GameCtrl.Instance.ExitGame();
            }
        }

        public void Revoke()
        {

        }

    }
}
