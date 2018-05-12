//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 7:22:17 PM
//Description ：摸牌命令
//===================================================
using UnityEngine;

namespace DRB.MahJong
{
    public class DrawPokerCommand : IGameCommand
    {
        private int m_PlayerId;

        private Poker m_DrawPoker;

        private bool m_isLast;

        private bool m_isBuHua;

        private long m_CountDown;

        public DrawPokerCommand(int playerId, Poker poker, bool isLast, bool isBuHua, long countDown)
        {
            m_PlayerId = playerId;
            m_DrawPoker = poker;
            m_isLast = isLast;
            m_isBuHua = isBuHua;
            m_CountDown = countDown;
        }

        public void Execute()
        {
            Debug.Log("===========================================执行摸牌命令");
            RoomMaJiangProxy.Instance.DrawPoker(m_PlayerId, m_DrawPoker, m_isLast, m_isBuHua);
            RoomMaJiangProxy.Instance.SetCountDown(m_CountDown, m_PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId);
        }

        public void Revoke()
        {

        }
    }
}
