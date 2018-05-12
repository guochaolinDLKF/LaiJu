//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 3:38:40 PM
//Description ：
//===================================================

using System.Collections.Generic;

namespace DRB.DouDiZhu
{
    public class SeatEntity : SeatEntityBase
    {
        public enum SeatStatus
        {
            Idle,
            Ready,
            Wait,
            Bet,
            PlayPoker,
        }
        /// <summary>
        /// 下注分数
        /// </summary>
        public int bet;

        public SeatStatus status;
        /// <summary>
        /// 手牌
        /// </summary>
        public List<Poker> pokerList;
        /// <summary>
        /// 上次出的牌
        /// </summary>
        public List<Poker> PreviourPoker;
        /// <summary>
        /// 是否是赢家
        /// </summary>
        public bool isWiner;
        /// <summary>
        /// 每局得分
        /// </summary>
        public int score;
        /// <summary>
        /// 总得分数
        /// </summary>
        public int totalScore;
        /// <summary>
        /// 是否是春天
        /// </summary>
        public bool isSpring;
        /// <summary>
        /// 打牌剩余时间
        /// </summary>
        public int unixtime;
    }
}
