//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 7:27:19 PM
//Description ：摸宝命令
//===================================================
using UnityEngine;

namespace DRB.MahJong
{
    public class LuckPokerCommand : IGameCommand
    {

        private Poker m_Poker;


        public LuckPokerCommand(Poker poker)
        {
            m_Poker = poker;
        }



        public void Execute()
        {
            RoomMaJiangProxy.Instance.SetLuckPoker(m_Poker.index, m_Poker.color, m_Poker.size);
            if (MaJiangSceneCtrl.Instance != null)
            {
                DiceEntity dice = null;
                if (RoomMaJiangProxy.Instance.CurrentRoom.ObsoleteDice.Count > 0)
                {
                    dice = RoomMaJiangProxy.Instance.CurrentRoom.ObsoleteDice.Dequeue();
                }
                MaJiangSceneCtrl.Instance.PlayLuckPokerAnimation(true, dice);
            }
        }

        public void Revoke()
        {

        }
    }
}
