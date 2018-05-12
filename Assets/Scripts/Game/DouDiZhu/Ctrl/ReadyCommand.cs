//===================================================
//Author      : DRB
//CreateTime  ：11/28/2017 2:37:39 PM
//Description ：准备命令
//===================================================
using System;
using UnityEngine;

namespace DRB.DouDiZhu
{
    public class ReadyCommand : IGameCommand
    {
        private int m_PlayerId;

        private bool m_isReady;


        public ReadyCommand(int playerId,bool isReady)
        {
            m_PlayerId = playerId;
            m_isReady = isReady;
        }



        public void Execute()
        {
            LogSystem.LogWarning("执行准备命令" + m_PlayerId);
            RoomProxy.Instance.Ready(m_PlayerId, m_isReady);
        }

        public void Revoke()
        {

        }
    }
}
