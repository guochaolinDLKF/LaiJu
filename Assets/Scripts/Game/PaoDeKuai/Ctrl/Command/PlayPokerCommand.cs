//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 5:11:28 PM
//Description ：出牌指令
//===================================================

using UnityEngine;
using proto.pdk;
namespace PaoDeKuai
{
    public class PlayPokerCommand : IGameCommand
    {
        private PDK_OPERATE m_DataEntity;

        public PlayPokerCommand(PDK_OPERATE data)
        {
            m_DataEntity = data;
        }


        public void Execute()
        {
          PokersType pokerType = RoomPaoDeKuaiProxy.Instance.PlayPoker(m_DataEntity);
            if (PaoDeKuaiSceneCtrl.Instance != null)
            {
                SeatEntity seat = RoomPaoDeKuaiProxy.Instance.GetSeatBySeatPos(RoomPaoDeKuaiProxy.Instance.CurrentRoom.CurrAlreadyPlayPos);
              if(seat!=null)  PaoDeKuaiSceneCtrl.Instance.PlayPoker(seat, RoomPaoDeKuaiProxy.Instance.CurrentRoom.RecentlyPlayPoker.Pokers, pokerType);
            }
        }

        public void Revoke()
        {

        }

    }
}