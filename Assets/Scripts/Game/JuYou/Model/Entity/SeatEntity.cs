//===================================================
//Author      : WZQ
//CreateTime  ：8/7/2017 2:59:59 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.jy;

namespace JuYou
{
    public class SeatEntity : SeatEntityBase 
    {
        /// <summary>
        /// 下注分数
        /// </summary>           
        public int Pour;
        /// <summary>
        /// 收益分数
        /// </summary>           
        public int Earnings;
        /// <summary>
        /// 本局收益
        /// </summary>
        public int loopEarnings;

        /// <summary>
        /// 是否准备
        /// </summary>
        public bool isReady = false;

        /// <summary>
        /// 座位状态
        /// </summary>           
        public SEAT_STATUS seatStatus;

        /// <summary>
        /// 是否同意解散
        /// </summary>
        public bool isDismiss = false;

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool isOnLine = true;


        /// <summary>
        /// 是否参加过游戏
        /// </summary>
        public bool isJoinGame = false;

        /// <summary>
        /// 手牌
        /// </summary>
        public List<Poker> PokerList = new List<Poker>();

       //PaiJiu.Seat
        /// <summary>
        /// 已经出过的牌
        /// </summary>
        public List<Poker> TablePokerList = new List<Poker>();

        public void SetSeat(JY_SEAT prSeat)
        {
            if (prSeat.hasPlayerId()) PlayerId = prSeat.playerId;//ID
            if (prSeat.hasNickname()) Nickname = prSeat.nickname;//昵称
            if (prSeat.hasAvatar()) Avatar = prSeat.avatar; //头像
            if (prSeat.hasGender()) Gender = prSeat.gender;//性别
            if (prSeat.hasIsBanker()) IsBanker = prSeat.isBanker;//庄   
            //if (prSeat.hasIsWiner()) Winner = prSeat.isWiner;//是否是胜利者
            //if (prSeat.has()) IsOwner = prSeat.isOwner;//是否是房主
            if (prSeat.hasPos()) Pos = prSeat.pos;//位置（服务器）
            if (prSeat.hasGold()) Gold = prSeat.gold;//金币
            if (prSeat.hasPour()) Pour = prSeat.pour;//下注分
            if (prSeat.hasStatus()) seatStatus = prSeat.status;//座位状态
            if (prSeat.hasEarnings()) Earnings = prSeat.earnings;//本次收益
            if (prSeat.hasLoopEarnings()) loopEarnings = prSeat.loopEarnings;//本局收益
            //if (prSeat.hasLoopEarnings()) LoopEarnings = prSeat.loopEarnings;//-------------------------------------------本局收益
            //if (prSeat.hasTotalEarnings()) TotalEarnings = prSeat.totalEarnings;//总收益（已有筹码）
            if (prSeat.hasIsDismiss()) isDismiss = prSeat.isDismiss;//是否同意解散
            //if (prSeat.has()) isOnLine = prSeat.;//是否在线
            if (prSeat.hasIsReady()) isReady = prSeat.isReady;//是否同意解散
            if (prSeat.hasIsJoinGame()) isJoinGame = prSeat.isJoinGame;//是否参加过游戏

            //具体手牌
            List<JY_POKER> prPokerList = prSeat.getPokerListList();
            if (prPokerList != null && prPokerList.Count > 0)
            {
                for (int j = 0; j < prPokerList.Count; j++)
                {
                    if (PokerList.Count <= j)
                    {
                        break;
                    }

                    if (prPokerList[j] != null && prPokerList[j].hasIndex() && prPokerList[j].index != 0)
                    {
                        //手牌是否有这张牌
                        if (PokerList[j] != null) PokerList[j].SetPoker(prPokerList[j]);

                    }

                }

            }


            //已经出过的牌
            if (prSeat.hasHistoryPokerList())
            {

                List<JY_POKER> prTablePokerList = prSeat.getHistoryPokerListList();

                //int countGap = prTablePokerList.Count - TablePokerList.Count;
                //if (countGap > 0)
                //{
                //    for (int i = 0; i < countGap; i++)
                //    {
                //        TablePokerList.Add(new Poker());
                //    }

                //}
                //for (int j = 0; j < prTablePokerList.Count; j++)
                //{
                //    //打过的是否有这张牌
                //    if (TablePokerList[j] != null) TablePokerList[j].SetPoker(prTablePokerList[j]);
                //}

                if (prTablePokerList != null && prTablePokerList.Count > 0)
                {
                    for (int j = 0; j < prTablePokerList.Count; j++)
                    {

                        if (prTablePokerList[j] != null)
                        {
                            if (TablePokerList.Count < (j + 1))
                            {
                                AppDebug.Log(string.Format("添加已出过的牌：index{0} ,size{1},type{2}", prTablePokerList[j].index, prTablePokerList[j].size, prTablePokerList[j].type));
                                TablePokerList.Add(new Poker());

                            }

                            //打过的是否有这张牌
                            if (TablePokerList[j] != null) TablePokerList[j].SetPoker(prTablePokerList[j]);

                        }

                    }

                }

            }
        }

        




    }
}