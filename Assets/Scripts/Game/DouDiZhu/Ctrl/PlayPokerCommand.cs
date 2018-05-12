//===================================================
//Author      : DRB
//CreateTime  ：12/1/2017 4:48:50 PM
//Description ：斗地主出牌命令
//===================================================
using System;
using System.Collections.Generic;

namespace DRB.DouDiZhu
{
    public class PlayPokerCommand : IGameCommand
    {
        private int m_PlayerId;

        private Deck m_Deck;

        public PlayPokerCommand(int playerId, Deck deck)
        {
            m_PlayerId = playerId;
            m_Deck = deck;
        }

        public void Execute()
        {
            if (m_Deck == null)
            {
                throw new Exception("出的牌组合是空的");
            }
            LogSystem.LogWarning("执行出牌命令" + m_PlayerId.ToString() + "出牌类型" + m_Deck.type.ToString() + "出牌数量" + m_Deck.pokers.Count);
            for (int i = 0; i < m_Deck.pokers.Count; ++i)
            {
                LogSystem.Log(m_Deck.pokers[i].ToLog());
            }
            if (RoomProxy.Instance.CurrentRoom.PlayerSeat != null && m_PlayerId == RoomProxy.Instance.CurrentRoom.PlayerSeat.PlayerId)
            {
                DouDiZhuGameCtrl.Instance.SelectPoker.Clear();
            }
            if (DouDiZhuGameCtrl.Instance.Tip != null)
            {
                DouDiZhuGameCtrl.Instance.Tip.Clear();
                DouDiZhuGameCtrl.Instance.TipIndex = 0;
            }
            RoomProxy.Instance.PlayPoker(m_PlayerId, m_Deck);
        }

        public void Revoke()
        {

        }
    }
}
