//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 7:19:19 PM
//Description ：出牌命令
//===================================================
using UnityEngine;

namespace DRB.MahJong
{
    public class PlayPokerCommand : IGameCommand
    {
        private int m_PlayerId;

        private Poker m_PlayPoker;

        private bool m_isTing;

        public PlayPokerCommand(int playerId, Poker poker, bool isTing)
        {
            m_PlayerId = playerId;
            m_PlayPoker = poker;
            m_isTing = isTing;
        }

        public void Execute()
        {
            Debug.Log("===========================================执行出牌命令");
            RoomMaJiangProxy.Instance.PlayPoker(m_PlayerId, m_PlayPoker, m_isTing);
            RoomMaJiangProxy.Instance.SetCountDown(0);
        }

        public void Revoke()
        {

        }
    }
}
