//===================================================
//Author      : DRB
//CreateTime  ：3/13/2017 11:28:52 AM
//Description ：麻将房间数据实体
//===================================================
using System.Collections;
using System.Collections.Generic;
using com.oegame.mahjong.protobuf;
using DRB.MahJong;
using UnityEngine;

namespace DRB.MahJong
{
    public class RoomEntity : RoomEntityBase
    {
        /// <summary>
        /// 房间状态
        /// </summary>
        public enum RoomStatus
        {
            Ready = 0,//准备
            Begin,//开始
            Settle,//结算
            Deal,//发牌中
            Show,//初始化
            Pao,//跑
            Jiao = 7,//报叫
            Swap = 8,//交换
            LackColor = 9,//定缺
            Replay = 64,//回放
        }
        /// <summary>
        /// 房间状态
        /// </summary>
        public RoomStatus Status;
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
        /// 抓马数量
        /// </summary>
        public int ProbCount;
        /// <summary>
        /// 抓的马
        /// </summary>
        public List<Poker> Prob = new List<Poker>();
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
        /// 翻宝摇的骰子
        /// </summary>
        public Queue<DiceEntity> ObsoleteDice = new Queue<DiceEntity>();
        /// <summary>
        /// 游戏是否结束
        /// </summary>
        public bool IsOver;
        /// <summary>
        /// 当前操作的座位
        /// </summary>
        public SeatEntity CurrentOperator;

        public SeatEntity PlayerSeat;

        public RoomEntity() { }
    }
}