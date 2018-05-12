//===================================================
//Author      : CZH
//CreateTime  ：9/1/2017 1:58:43 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.gp;

namespace GuPaiJiu
{
    public class SeatEntity :SeatEntityBase
    {
        /// <summary>
        /// 是否同意解散房间
        /// </summary>
        public bool isDismiss;
        /// <summary>
        /// 是否是赢家
        /// </summary>
        public bool isWin;
        /// <summary>
        /// 每次收益
        /// </summary>
        public int eamings;
        /// <summary>
        /// 每局收益
        /// </summary>
        public int loopEamings;
        /// <summary>
        /// 一道
        /// </summary>
        public int firstPour;
        /// <summary>
        /// 二道
        /// </summary>
        public int secondPour;
        /// <summary>
        /// 三道
        /// </summary>
        public int threePour;

        ///// <summary>
        ///// 是否准备
        ///// </summary>
        //public bool isReady;
        /// <summary>
        /// 组合牌的时间
        /// </summary>
        public long groupTime;
        /// <summary>
        /// 手牌集合
        /// </summary>
        public List<Poker> pokerList;
        /// <summary>
        /// 是否抢庄， 1，抢，2，不抢，3，未操作
        /// </summary>
        public int isGrabBanker;
        /// <summary>
        /// 是否搓牌    1，搓，2  未操作
        /// </summary>
        public int isCuoPai;
        /// <summary>
        /// 翻牌索引
        /// </summary>
        public List<int> drawPokerList;

        public SEAT_STATUS seatStatus = SEAT_STATUS.IDLE;
       
        public SeatEntity() { }
    }
}
