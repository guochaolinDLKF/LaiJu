//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 8:25:55 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShiSanZhang
{
    public class ReadyCommand : IGameCommand
    {
        private int m_Pos;

        private bool m_isReady;


        public ReadyCommand(int pos)
        {
            m_Pos = pos;            
        }



        public void Execute()
        {
            RoomShiSanZhangProxy.Instance.Ready(m_Pos);
        }

        public void Revoke()
        {

        }

    }
}
