//===================================================
//Author      : DRB
//CreateTime  ：10/16/2017 7:48:03 PM
//Description ：
//===================================================

namespace DRB.MahJong
{
    public class RollDiceCommand : IGameCommand
    {

        private int m_PlayerId;

        private int m_Dice;

        public RollDiceCommand(int playerId, int dice)
        {
            m_PlayerId = playerId;
            m_Dice = dice;
        }



        public void Execute()
        {
            RoomMaJiangProxy.Instance.RollDice(m_PlayerId, m_Dice);
        }

        public void Revoke()
        {

        }
    }
}
