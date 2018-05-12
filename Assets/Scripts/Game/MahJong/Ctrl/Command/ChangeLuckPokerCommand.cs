//===================================================
//Author      : DRB
//CreateTime  ：10/16/2017 7:52:43 PM
//Description ：
//===================================================

namespace DRB.MahJong
{
    public class ChangeLuckPokerCommand : IGameCommand
    {

        private Poker m_PrevLuckPoker;

        private Poker m_CurrLuckPoker;

        public ChangeLuckPokerCommand(Poker prevLuckPoker, Poker currLuckPoker)
        {
            m_PrevLuckPoker = prevLuckPoker;
            m_CurrLuckPoker = currLuckPoker;
        }

        public void Execute()
        {
            RoomMaJiangProxy.Instance.SetLuckPoker(m_CurrLuckPoker.index, m_CurrLuckPoker.color, m_CurrLuckPoker.size);

            if (MaJiangSceneCtrl.Instance != null)
            {
                DiceEntity dice = null;
                if (RoomMaJiangProxy.Instance.CurrentRoom.ObsoleteDice.Count > 0)
                {
                    dice = RoomMaJiangProxy.Instance.CurrentRoom.ObsoleteDice.Dequeue();
                }
                MaJiangSceneCtrl.Instance.PlayChangeLuckPokerAnimation(m_PrevLuckPoker, dice);
            }
        }

        public void Revoke()
        {

        }
    }
}
