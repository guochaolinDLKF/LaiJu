//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 7:48:43 PM
//Description ：
//===================================================

using UnityEngine;
using proto.pdk;
namespace PaoDeKuai
{
    public class NoticeSeatOperateCommand : IGameCommand
    {
        private PDK_NEXT_PLAYER m_DataEntity;

        public NoticeSeatOperateCommand(PDK_NEXT_PLAYER data)
        {
            m_DataEntity = data;
        }


        public void Execute()
        {
            //自动过
            if (m_DataEntity.pos == RoomPaoDeKuaiProxy.Instance.PlayerSeat.Pos)
            {
                if (RoomPaoDeKuaiProxy.Instance.CurrentRoom.RecentlyPlayPoker.PokersType != PokersType.None)
                {

                }
                RoomPaoDeKuaiProxy.Instance.Currhint.Reset();
                PaoDeKuaiHelper.HintPoker(RoomPaoDeKuaiProxy.Instance.Currhint, RoomPaoDeKuaiProxy.Instance.PlayerSeat.pokerList);
                //判断是否有比上家大的牌
                if (RoomPaoDeKuaiProxy.Instance.Currhint.CurrHint.PokersType == PokersType.None)
                {
                    //RoomPaoDeKuaiProxy.Instance.Currhint.Reset();
                    //过
                    PaoDeKuaiGameCtrl.Instance.ClientSendPass();
                    return;
                }
            }

           


            RoomPaoDeKuaiProxy.Instance.NoticeSeatOperate(m_DataEntity);
            if (PaoDeKuaiSceneCtrl.Instance != null)
            {
                PaoDeKuaiSceneCtrl.Instance.SeatOperateNotice(m_DataEntity.pos);
            }


        }

        public void Revoke()
        {

        }

    }
}