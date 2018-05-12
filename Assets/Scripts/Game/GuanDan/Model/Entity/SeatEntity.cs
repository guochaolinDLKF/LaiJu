//===================================================
//Author      : WZQ
//CreateTime  ：11/6/2017 9:54:58 AM
//Description ：掼蛋座位实体
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using guandan.proto;
namespace GuanDan
{
    public class SeatEntity :SeatEntityBase
    {
        //public enum SeatStatus
        //{
        //    Idle,
        //    Ready,
        //    Operate,
        //    Finish,
        //    /// <summary>
        //    /// 弃权
        //    /// </summary>
        //    Waiver,
        //    Wait,
        //    Fight
        //}


        /// <summary>
        /// 座位状态
        /// </summary>
        public SEAT_STATUS Status;

        /// <summary>
        /// 剩余牌的数量
        /// </summary>
        public int PokerAmount;
        /// <summary>
        /// 已打出的牌
        /// </summary>
        public List<Poker> DeskTopPoker = new List<Poker>();
        /// <summary>
        /// 万能牌
        /// </summary>
        public List<Poker> UniversalList = new List<Poker>();
        /// <summary>
        /// 手牌
        /// </summary>
        public List<Poker> PokerList = new List<Poker>();
  
        /// <summary>
        /// 是否胜利
        /// </summary>
        public bool isWiner;
  

        public int ProbMulti;

        

      

        public int Direction;

        public bool isDouble;


        public int Settle;

        public int TotalHuScore;

        public bool IsTrustee;

        /// <summary>
        /// 是否弃权
        /// </summary>
        public bool IsWaiver;

        /// <summary>
        /// 出过万能牌
        /// </summary>
        public bool isPlayedUniversal;

        

        public List<Poker> DingJiangPoker = new List<Poker>();

        public bool HasUniversal
        {
            get
            {
                for (int i = 0; i < PokerList.Count; ++i)
                {
                    Poker hand = PokerList[i];
                    for (int j = 0; j < UniversalList.Count; ++j)
                    {
                        Poker universal = UniversalList[j];
                        if (hand.color == universal.color && hand.size == universal.size)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }


        public SeatEntity() { }

        public void SetSeat(SEAT_INFO op_seat)
        {


        }

        //PaiJiu.Seat









    }
}