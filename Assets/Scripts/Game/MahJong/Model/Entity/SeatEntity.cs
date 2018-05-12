//===================================================
//Author      : DRB
//CreateTime  ：4/25/2017 8:05:02 PM
//Description ：座位数据实体
//===================================================
using System.Collections.Generic;
namespace DRB.MahJong
{
    public class SeatEntity : SeatEntityBase
    {
        public enum SeatStatus
        {
            Idle,
            Ready,
            Operate,
            Finish,
            /// <summary>
            /// 弃权
            /// </summary>
            Waiver,
            Wait,
            Fight
        }


        /// <summary>
        /// 座位状态
        /// </summary>
        public SeatStatus Status;
        /// <summary>
        /// 摸的牌
        /// </summary>
        public Poker HitPoker;
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
        /// 吃碰杠的牌
        /// </summary>
        public List<PokerCombinationEntity> UsedPokerList = new List<PokerCombinationEntity>();

        /// <summary>
        /// 是否胜利
        /// </summary>
        public bool isWiner;
        /// <summary>
        /// 是否被点炮的
        /// </summary>
        public bool isLoser;

        public int ProbMulti;

        public bool IsTing;

        public bool isLockTing;

        public int Direction;

        public bool isDouble;
        /// <summary>
        /// 是否胡牌
        /// </summary>
        public bool isHu;

        /// <summary>
        /// 结算信息(台详情)
        /// </summary>
        public List<IncomeDetailEntity> SettleInfo = new List<IncomeDetailEntity>();

        /// <summary>
        /// 胡分详情
        /// </summary>
        public List<IncomeDetailEntity> HuScoreDetail = new List<IncomeDetailEntity>();
        //public List<PBPokerGroup> AskPokerGroup;

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
        /// <summary>
        /// 缺门
        /// </summary>
        public int LackColor;

        /// <summary>
        /// 支对列表
        /// </summary>
        public List<List<Poker>> HoldPoker = new List<List<Poker>>();

        public List<Poker> DingJiangPoker = new List<Poker>();

        /// <summary>
        /// 交换的牌
        /// </summary>
        public List<Poker> SwapPoker = new List<Poker>();
        /// <summary>
        /// 明提的牌
        /// </summary>
        public List<Poker> MingTiPoker = new List<Poker>();

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
    }
}
