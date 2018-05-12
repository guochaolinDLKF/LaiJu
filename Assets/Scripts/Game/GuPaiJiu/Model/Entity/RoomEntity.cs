//===================================================
//Author      : CZH
//CreateTime  ：9/1/2017 1:54:33 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.gp;

namespace GuPaiJiu
{
    public enum EnumPlay
    {
        BigPaiJiu=1,//大牌九
        SmallPaiJiu,//小牌九      
    }
    public enum EnumCutPoker
    {
        Cut=1,//切牌
        NoCut=2,//不切牌
        NotOperational,//未操作
    }

    public enum EnumGarbBaker
    {
        Qiang=1,//抢
        NoQiang=2,//不强
    }

    public enum EnumCutPan
    {
        Cut=1,//切锅
        NoCut=2,//不切锅
    }

    public enum EnumGuiZi
    {
        NO=0,//空
        GuiZi =1,//鬼子        
    }
    public enum EnumTianJiuWang
    {
        NO,//空
        TianJiuWang,//天九王
       
    }

    public enum EnumDiJiuWang
    {
        NO,//空
        DiJiuWang,//天九王       
    }



    public class RoomEntity : RoomEntityBase
    {
       public  enum RoomMode
        {
            RoundZhuang=1,//轮庄
            RobZhuang=2,//抢庄
            KongZhuang=3,//空
        }

        public enum BetModel
        {
            NationalScore=1,//固定分
            NoNational,//不固定分
        }

        public BetModel betModel = BetModel.NationalScore;
        public  RoomMode roomMode = RoomMode.KongZhuang;
        public EnumPlay roomPlay;
        /// <summary>
        /// 第一个骰子
        /// </summary>
        public int FirstDice;
        /// <summary>
        /// 第二个骰子
        /// </summary>
        public int SecondDice;
        /// <summary>
        /// 首发牌座位号
        /// </summary>
        public int FirstGivePos;
        /// <summary>
        /// 房间牌的总数量
        /// </summary>
        public int TotalPokerNum;
        /// <summary>
        /// 房间剩余牌数量
        /// </summary>
        public int RemainPokerNum;
        /// <summary>
        /// 是否加锅底
        /// </summary>
        public bool IsAddPanBase;
        /// <summary>
        /// 锅底
        /// </summary>
        public int PanBase;
        /// <summary>
        /// 每局第几次发牌
        /// </summary>
        public int dealSecond;
        /// <summary>
        /// 房间已出过的牌
        /// </summary>
        public List<Poker> roomPokerList;
        /// <summary>
        /// 鬼子
        /// </summary>
        public EnumGuiZi enumGuiZi;
        /// <summary>
        /// 地九王
        /// </summary>
        public EnumDiJiuWang enumDiJiuWang;
        /// <summary>
        /// 天九王
        /// </summary>
        public EnumTianJiuWang enumTianJiuWang;

        ///// <summary>
        ///// 是否是鬼子
        ///// </summary>
        //public bool isDevil=false;
        ///// <summary>
        ///// 是否开启天九王
        ///// </summary>
        //public bool isTJW = false;
        /// <summary>
        /// 不固定分 封顶值
        /// </summary>
        public int scoreLimit;
        /// <summary>
        /// 固定分分值
        /// </summary>
        public int guDScore;

        /// <summary>
        /// 庄家翻牌时间
        /// </summary>
       public long roomUnixtime;
        /// <summary>
        /// 抢庄的人数
        /// </summary>
        public int isGrabBankerNum;
        /// <summary>
        /// 房主名字
        /// </summary>
        public string fzName;
                                                       

        public List<SeatEntity> seatList;

        public ROOM_STATUS roomStatus=ROOM_STATUS.IDLE;
        public RoomEntity() { }
    }
}
