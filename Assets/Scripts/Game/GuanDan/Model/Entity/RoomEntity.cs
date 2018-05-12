//===================================================
//Author      : WZQ
//CreateTime  ：11/6/2017 9:55:20 AM
//Description ：掼蛋房间实体
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using guandan.proto;
namespace GuanDan
{
    public class RoomEntity :RoomEntityBase
    {
        /// <summary>
        /// 房间状态
        /// </summary>
        public ROOM_STATUS Status;

        /// <summary>
        /// 座位
        /// </summary>
        public List<SeatEntity> SeatList;
        /// <summary>
        /// 牌的总数
        /// </summary>
        public int PokerTotal;
        /// <summary>
        /// 剩余牌的数量
        /// </summary>
        public int PokerAmount;
        /// <summary>
        /// 每人手牌数量
        /// </summary>
        public int PokerTotalPerPlayer = 14;
        /// <summary>
        /// 底分
        /// </summary>
        public int BaseScore;
        /// <summary>
        /// 第一次摇的骰子
        /// </summary>
        public DiceEntity FirstDice;
        /// <summary>
        /// 第二次摇的骰子
        /// </summary>
        public DiceEntity SecondDice;
        /// <summary>
        /// 翻的牌
        /// </summary>
        public Poker LuckPoker;


        /// <summary>
        /// 庄家座位号
        /// </summary>
        public int BankerPos
        {
            get
            {
                for (int i = 0; i < SeatList.Count; ++i)
                {
                    if (SeatList[i].IsBanker)
                    {
                        return SeatList[i].Pos;
                    }
                }
                return 1;
            }
        }
  
        /// <summary>
        /// 游戏是否结束
        /// </summary>
        public bool IsOver;

        public RoomEntity() { }


        //PaiJiu.Room

        public void SetRoom()
        {
            //for (int i = 0; i < prPokerList.Count; i++)
            //{


            //    if (prPokerList[i] != null && prPokerList[i].pos > 0 || prPokerList[i].playerId > 0)
            //    {

            //        //--------------1-----------------
            //        for (int j = 0; j < SeatList.Count; j++)
            //        {
            //            if (prPokerList[i].pos == SeatList[j].Pos)
            //            {
            //                SeatList[j].SetSeat(prPokerList[i]);
            //                break;
            //            }
            //        }


            //    }


            //}


        }





    }
}