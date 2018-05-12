//===================================================
//Author      : WZQ
//CreateTime  ：7/4/2017 2:52:21 PM
//Description ：牌九座位
//===================================================

using System.Collections.Generic;
using proto.paigow;


namespace PaiJiu
{
    /// <summary>
    /// 牌九座位
    /// </summary>
    public class Seat {


        /// <summary>
        ///  0 不能操作  1 切牌(操作中)  2 不切    3 正在切牌(动画中)  4 未操作
        /// </summary>
        public enum CutPoker
        {
            None = 0,
            CutPoker = 1,
            NoCutPoker = 2,
            IsCut = 3,
            IsNotOperating = 4,
        }

        /// <summary>
        /// 玩家编号
        /// </summary>
        public int PlayerId;

        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string Nickname;

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar;

        /// <summary>
        /// 性别
        /// </summary>
        public int Gender;

        /// <summary>
        /// 是否庄家
        /// </summary>
        public bool IsBanker;

        /// <summary>
        /// 是否是胜利者
        /// </summary>
        public bool Winner;

        /// <summary>
        /// 是否房主
        /// </summary>
        public bool IsOwner;


     
        /// <summary>
        /// 手牌
        /// </summary>
        public List<PaiJiu.Poker> PokerList=new List<Poker> ();

        //已经出过的牌
        public List<PaiJiu.Poker> TablePokerList = new List<Poker>();


        ///// <summary>
        ///// 手牌类型
        ///// </summary>
        //public int PockeType;

        /// <summary>
        /// 位置
        /// </summary>
        public int Pos;

        /// <summary>
        /// 下注分数
        /// </summary>           
        public int Pour;

        /// <summary>
        /// 是否准备
        /// </summary>
        public SEAT_STATUS seatStatus;

        /// <summary>
        /// 本次收益
        /// </summary>
        public int Earnings;

        /// <summary>
        /// 本局收益（4次）
        /// </summary>
        public int LoopEarnings;

        /// <summary>
        /// 总收益
        /// </summary>
        public int TotalEarnings;

        public int Gold = 0;
        /// <summary>
        /// 索引 （客户端使用 客户端自定义）
        /// </summary>
        public int Index;

        
        /// <summary>
        /// 是否抢庄 0：不能操作  1:抢庄 2:不抢 3:未操作
        /// </summary>
        public int isGrabBanker = 3;

        /// <summary>
        ///  是否切牌 0：不能操作  1:切牌 2:不切 3:正在切牌 4:未操作
        /// </summary>
        public CutPoker isCutPoker = CutPoker.None;

        /// <summary>
        /// 是否切锅  0：不能操作  1:切锅 2:不切锅 3:未操作
        /// </summary>
        public int isCutGuo = 0;

        /// <summary>
        /// 是否是大赢家
        /// </summary>
        public bool isBigWinner = false;


        public Seat() { }


        public void SetSeat(PAIGOW_SEAT prSeat)
        {
            if (prSeat.hasPlayerId()) PlayerId = prSeat.playerId;//ID
            if (prSeat.hasNickname()) Nickname = prSeat.nickname;//昵称
            if (prSeat.hasAvatar()) Avatar = prSeat.avatar; //头像
            if (prSeat.hasGender()) Gender = prSeat.gender;//性别                                                        
            if (prSeat.hasIsBanker())  IsBanker = prSeat.isBanker;//庄   
            if (prSeat.hasIsWiner())  Winner = prSeat.isWiner;//是否是胜利者
            //if (prSeat.has()) IsOwner = prSeat.isOwner;//是否是房主
            if (prSeat.hasPos()) Pos = prSeat.pos;//位置（服务器）
            if (prSeat.hasPour()) Pour = prSeat.pour;//下注分
            if (prSeat.hasSeatStatus()) seatStatus = prSeat.seat_status;//座位状态（是否准备）
            if (prSeat.hasEarnings()) Earnings = prSeat.earnings;//本次收益
            if (prSeat.hasLoopEarnings()) LoopEarnings = prSeat.loopEarnings;//本局收益
            if (prSeat.hasTotalEarnings()) TotalEarnings = prSeat.totalEarnings;//总收益（已有筹码）
            if (prSeat.hasIsGrabBanker()) isGrabBanker = prSeat.isGrabBanker;//是否抢庄
            if (prSeat.hasIsCutPoker()) isCutPoker = (CutPoker)prSeat.isCutPoker;//是否切牌
            if (prSeat.hasGold()) Gold = prSeat.gold;//金币
            if (prSeat.hasIsCutPan()) isCutGuo = prSeat.isCutPan;//是否能切锅
            //具体手牌
            List<PAIGOW_MAHJONG> prPokerList = prSeat.getPaigowMahjongList();
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
                        if (PokerList[j]!=null)  PokerList[j].SetPoker(prPokerList[j]);

                    }

                }

            }

            //如果有牌
            //已经出过的牌
            if (prSeat.hasHistoryPoker())
            {

                List<PAIGOW_MAHJONG> prTablePokerList = prSeat.getHistoryPokerList();

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