//===================================================
//Author      : DRB
//CreateTime  ：11/28/2017 2:34:04 PM
//Description ：离开房间命令
//===================================================
using System;
using UnityEngine;

namespace DRB.DouDiZhu
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
            LogSystem.LogWarning("执行离开房间命令" + m_PlayerId);
            RoomProxy.Instance.ExitRoom(m_PlayerId);
            Debug.LogWarning(m_PlayerId);
            Debug.LogWarning(AccountProxy.Instance.CurrentAccountEntity.passportId);
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
