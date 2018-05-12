//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 8:27:02 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;

namespace DRB.MahJong
{
    public class SwapPokerCommand : IGameCommand
    {
        private int m_Mode;

        private List<Poker> m_PlayerPokers;

        public SwapPokerCommand(int mode, List<Poker> playerPokers)
        {
            m_Mode = mode;
            m_PlayerPokers = playerPokers;
        }

        public void Execute()
        {
            LogSystem.Log("开始换牌，模式" + m_Mode.ToString());
            RoomMaJiangProxy.Instance.SwapPoker(m_Mode, m_PlayerPokers);

            if (MaJiangSceneCtrl.Instance != null)
            {
                MaJiangSceneCtrl.Instance.PlaySwapAnimation(m_Mode);
            }
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}
