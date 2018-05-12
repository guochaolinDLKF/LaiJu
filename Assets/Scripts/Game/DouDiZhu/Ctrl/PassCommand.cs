//===================================================
//Author      : DRB
//CreateTime  ：12/1/2017 7:21:40 PM
//Description ：
//===================================================
using System;
using UnityEngine;

namespace DRB.DouDiZhu
{
    public class PassCommand : IGameCommand
    {
        private int m_PlayerId;

        public PassCommand(int playerId)
        {
            m_PlayerId = playerId;
        }

        public void Execute()
        {
            LogSystem.LogWarning("执行过命令" + m_PlayerId);
            if (RoomProxy.Instance.CurrentRoom.PlayerSeat != null && m_PlayerId == RoomProxy.Instance.CurrentRoom.PlayerSeat.PlayerId)
            {
                DouDiZhuGameCtrl.Instance.SelectPoker.Clear();
                if (DouDiZhuGameCtrl.Instance.Tip != null)
                {
                    DouDiZhuGameCtrl.Instance.Tip.Clear();
                    DouDiZhuGameCtrl.Instance.TipIndex = 0;
                }
            }
            RoomProxy.Instance.Pass(m_PlayerId);
        }

        public void Revoke()
        {

        }
    }
}
