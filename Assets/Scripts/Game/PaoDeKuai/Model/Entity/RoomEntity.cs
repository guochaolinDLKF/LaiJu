//===================================================
//Author      : WZQ
//CreateTime  ：11/13/2017 4:00:10 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaoDeKuai {

    public class RoomEntity : RoomEntityBase
    {
        /// <summary>
        /// 房间状态
        /// </summary>
        public enum RoomStatus
        {
            Ready = 0,//准备
            Begin,//开始 ,游戏中
            Settle,//结算
            Deal,//发牌中
            Show,//初始化
            
            
            Replay = 64,//回放
        }

        /// <summary>
        /// 房主ID
        /// </summary>
        public int ownerId;



        /// <summary>
        /// 房间状态
        /// </summary>
        public RoomStatus Status;
        /// <summary>
        /// 座位
        /// </summary>
        public List<SeatEntity> SeatList;
        /// <summary>
        /// 解散房间开始时间
        /// </summary>
        public long DisbandStartTime;

        /// <summary>
        /// 房间座位长度
        /// </summary>
        public int SeatCount;

        /// <summary>
        /// 牌的总数
        /// </summary>
        public int PokerTotal;

        /// <summary>
        /// 黑桃3座位Pos
        /// </summary>
        public int SpadesThreePos;

        /// <summary>
        /// 房间底分
        /// </summary>
        public int BaseScore;



        /// <summary>
        /// 当前已经出牌位置
        /// </summary>
        public int CurrAlreadyPlayPos = 0;

        /// <summary>
        /// 最近出牌
        /// </summary>
        public CombinationPokersEntity RecentlyPlayPoker;

        /// <summary>
        /// 当前操作座位
        /// </summary>
        public SeatEntity OperateSeat;


        /// <summary>
        /// 已出的牌
        /// </summary>
        public List<Poker> HistoryPoker;

        /// <summary>
        /// 最近一局赢家座位
        /// </summary>
        public int  WinnertPos=0;



    }

}