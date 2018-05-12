//===================================================
//Author      : WZQ
//CreateTime  ：11/13/2017 4:02:27 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaoDeKuai
{
    public class SeatEntity :SeatEntityBase
    {
        #region SeatStatus 座位状态
        public enum SeatStatus
        {
            /// <summary>
            /// 空闲
            /// </summary>
            Idle,
            /// <summary>
            /// 准备
            /// </summary>
            Ready,
            /// <summary>
            /// 操作
            /// </summary>
            Operate,
            /// <summary>
            /// 完成
            /// </summary>
            Finish,
            /// <summary>
            /// 弃权
            /// </summary>
            Waiver,
            /// <summary>
            /// 等待
            /// </summary>
            Wait,
            /// <summary>
            /// 
            /// </summary>
            Fight
        }
        #endregion

        /// <summary>
        /// 是否是房主
        /// </summary>
        public bool isLandlord;

        /// <summary>
        /// 玩家手牌
        /// </summary>
        public List<Poker> pokerList ;

       

        /// <summary>
        /// 所有打出的牌
        /// </summary>
        public List<Poker> DeskTopPoker=new List<Poker> ();

        /// <summary>
        /// 本轮出牌
        /// </summary>
        public List<Poker> RoundPoker=new List<Poker> ();

        /// <summary>
        /// 是否准备
        /// </summary>
        public bool IsReady;

        /// <summary>
        /// 座位状态
        /// </summary>
        public SeatStatus Status = SeatStatus.Idle;

        /// <summary>
        /// 是否托管
        /// </summary>
        public bool IsTrustee;

        /// <summary>
        /// 剩余手牌数量
        /// </summary>
        public int HandPockerNum;

       

        /// <summary>
        /// 本局收益
        /// </summary>
        public int Earnings;

        /// <summary>
        /// 是否Pass
        /// </summary>
        public bool IsPass;


        /// <summary>
        /// 检测是否存在黑桃3
        /// </summary>
        /// <returns></returns>
        public bool CheckThree()
        {
            for (int i = 0; i < pokerList.Count; ++i)
            {
                if (pokerList[i].size == 3 && pokerList[i].color == 4) return true;
            }
            return false;
        }

    }


    /// <summary>
    /// 玩家操作项目
    /// </summary>
    public enum OperateItem
    {
        None,
        /// <summary>
        /// 出牌项
        /// </summary>
        ChuPaiItem,
        /// <summary>
        /// 抢关项
        /// </summary>
        QiangGuan,

    }



}