//===================================================
//Author      : DRB
//CreateTime  ：12/4/2017 12:00:42 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShiSanZhang
{
    public class PlayPokerCommand : IGameCommand
    {
        private int m_Pos;

        public PlayPokerCommand(int Pos)
        {
            m_Pos = Pos;
        }

        public void Execute()
        {
            RoomShiSanZhangProxy.Instance.PlayPokerProxy(m_Pos);
        }

        public void Revoke()
        {
            
        }
    }
}
