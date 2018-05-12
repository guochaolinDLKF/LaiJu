//===================================================
//Author      : DRB
//CreateTime  ：11/28/2017 2:29:16 PM
//Description ：进入房间命令
//===================================================
using System;
using UnityEngine;

namespace DRB.DouDiZhu
{
    public class EnterRoomCommand : IGameCommand
    {

        private int m_PlayerId;

        private int m_Gold;

        private string m_Avatar;

        private int m_Gender;

        private string m_Nickname;

        private int m_Pos;

        public EnterRoomCommand(int playerId, int gold, string avatar, int gender, string nickname, int pos)
        {
            m_PlayerId = playerId;
            m_Gold = gold;
            m_Avatar = avatar;
            m_Gender = gender;
            m_Nickname = nickname;
            m_Pos = pos;
        }

        public void Execute()
        {
            LogSystem.LogWarning("执行进入房间命令" + m_PlayerId);
            RoomProxy.Instance.EnterRoom(m_PlayerId, m_Gold, m_Avatar, m_Gender, m_Nickname, m_Pos);
        }

        public void Revoke()
        {

        }
    }
}
