//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 7:32:49 PM
//Description ：结算命令
//===================================================
using System.Collections.Generic;
using proto.mahjong;
using UnityEngine;

namespace DRB.MahJong
{
    public class SettleCommand : IGameCommand
    {

        private OP_ROOM_SETTLE m_Settle;

        public SettleCommand(OP_ROOM_SETTLE settle)
        {
            m_Settle = settle;
        }

        public void Execute()
        {
            Debug.Log("执行结算命令");
            RoomMaJiangProxy.Instance.Settle(m_Settle);
            RoomMaJiangProxy.Instance.SetCountDown(0);

            if (MaJiangSceneCtrl.Instance != null)
            {
                bool isLiuJu = true;
                List<SeatEntity> lstWinerSeat = new List<SeatEntity>();
                //摊牌
                for (int i = 0; i < RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count; ++i)
                {
                    SeatEntity seat = RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i];
                    if (seat.isWiner)
                    {
                        lstWinerSeat.Add(seat);
                    }
                }

                MaJiangSceneCtrl.Instance.Settle(lstWinerSeat);
            }
        }

        public void Revoke()
        {

        }
    }
}