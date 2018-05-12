//===================================================
//Author      : DRB
//CreateTime  ：9/19/2017 3:02:29 PM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;

namespace DRB.MahJong
{
    public class CheckCommand : IGameCommand
    {
        private List<Poker> m_PokerList;

        public CheckCommand(List<Poker> lst)
        {
            m_PokerList = lst;
        }

        public void Execute()
        {
            Debug.Log("===========================================执行数据检测命令");
            RoomMaJiangProxy.Instance.Check(m_PokerList);
        }

        public void Revoke()
        {
        }
    }
}
