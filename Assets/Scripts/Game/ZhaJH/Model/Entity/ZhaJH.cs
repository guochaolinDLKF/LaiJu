//===================================================
//Author      : CZH
//CreateTime  ：6/14/2017 11:24:43 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.oegame.zjh.protobuf;
using zjh.proto;

namespace ZhaJh
{
    public enum RoomMode
    {
        Ordinary=1,//普通房
        Senior,//高级房
        Passion //激情房
    }
    public class RoomEntity : RoomEntityBase
    {
        public enum RoomStatus
        {
            Ready = 1,//准备
            Bgin,//开始
            Settle,//结算中
            Deal,//发牌中
            Pour,//下注
            Pocker,//看牌         
        }

        /// <summary>
        /// 加注是否到最高分
        /// </summary>
        public bool jiazhuScroc = true;



        /// <summary>
        /// 房间状态
        /// </summary>
        public RoomStatus status;
        /// <summary>
        /// 房间加注最高分
        /// </summary>
        public float roomPour;
        /// <summary>
        /// 房间配置ID
        /// </summary>
        public RoomMode roomSettingId = RoomMode.Ordinary;

        ///// <summary>
        ///// 房间号
        ///// </summary>
        //public int roomId;      
        ///// <summary>
        ///// 当前第几局
        ///// </summary>
        //public int currentLoop;
        ///// <summary>
        ///// 总局数
        ///// </summary>
        //public int maxLoop;
        /// <summary>
        /// 房间座位
        /// </summary>
        public List<SeatEntity> seatList;

        /// <summary>
        /// 账单
        /// </summary>
        public List<BillEntity> billList;
        /// <summary>
        /// 开始时间
        /// </summary>
        public long beginTime;
        /// <summary>
        /// 房间玩家
        /// </summary>
        public List<PlayerEntity> playerList;
        /// <summary>
        /// 房间状态计时
        /// </summary>
        public long sysTime;
        /// <summary>
        /// 房间的底分分数
        /// </summary>
        public int scores;
        /// <summary>
        /// 第几轮
        /// </summary>
        public int round;
        /// <summary>
        /// 总轮数
        /// </summary>
        public int totalRound;
        /// <summary>
        /// 房间下注总分数
        /// </summary>
        public float baseScore;
        /// <summary>
        /// 座位数
        /// </summary>
        public int SeatCount;

        public ENUM_ROOM_STATUS roomStatus = ENUM_ROOM_STATUS.IDLE;

        public RoomEntity() { }

    }

    //炸金花牌型
    public enum ZJHCardType
    {
        Ordinary = 0,//普通
        Sub,//对子
        ShunZi,//顺子
        WithFlowers,//同花
        WithFlowersShun,//同花顺
        Bomb,//炸弹
        Nothing//无
    }

    public class SeatEntity : SeatEntityBase
    {
       
        public enum SeatStatus
        {
            Idle,//空闲
            Ready = 1,//准备
            Wait,//等待
            Deal,//发牌
            Bet,//下注
            TheCard //比牌                    
        }
        public bool isEnabled = false;

        public int currentLoop = 0;

        /// <summary>
        /// 牌型
        /// </summary>
       public  ZJHCardType zjhCardType = ZJHCardType.Nothing;

        //低分模式
        public bool isLowScore=false;

        /// <summary>
        /// 座位第几轮
        /// </summary>
        public int seatRound = 0;

        /// <summary>
        /// 每次下注分数 用来实例化筹码
        /// </summary>
        public float betPoints = 0;
        /// <summary>
        /// 每局每个座位下的注码
        /// </summary>
        public float totalPour;
        /// <summary>
        /// 结算的时候是否显示分数上移
        /// </summary>
        public bool isScore = false;       
        /// <summary>
        /// 索引
        /// </summary>
        public int Index;
     
        ///// <summary>
        ///// 头像
        ///// </summary>
        //public string avatar;

        /// <summary>
        /// 普通房底分（初始值为0）
        /// 高级房底分（初始值为1000）
        /// </summary>
        public float gold;
        /// <summary>
        /// 收益
        /// </summary>
        public float profit = 0;
        /// <summary>
        /// 总收益
        /// </summary>
        public float totalProfit = 0;
        /// <summary>
        /// 手牌
        /// </summary>
        public List<Poker> pokerList;
        /// <summary>
        /// 座位号
        /// </summary>
        public int pos;
        /// <summary>
        /// 是否是庄
        /// </summary>
        public bool isBanker;
        public bool Ready;
        /// <summary>
        /// 是否是赢家
        /// </summary>
        public bool winner;
        /// <summary>
        /// 下注，分数
        /// </summary>
        public float pour;
        /// <summary>
        /// 是否同意解散 1.同意 2.不同意
        /// </summary>
        public int dissolve;
        /// <summary>
        /// 每局的收益
        /// </summary>
        public int earnings;
        /// <summary>
        /// 座位状态
        /// </summary>
        public ENUM_SEAT_STATUS seatStatus = ENUM_SEAT_STATUS.IDLE;
        /// <summary>
        /// 座位操作状态 （弃牌或者看牌）
        /// </summary>
        public ENUM_SEATOPERATE_STATUS seatToperateStatus = ENUM_SEATOPERATE_STATUS.SEAT_STATUS_IDLE;
        /// <summary>
        /// 座位扑克状态（看牌或者未看牌）
        /// </summary>
        public ENUM_POKER_STATUS pokerStatus = ENUM_POKER_STATUS.OPPOSITE;
        /// <summary>
        /// 时间戳
        /// </summary>
        public long systemTime=0;
        /// <summary>
        /// 玩家信息
        /// </summary>
        public List<PlayerEntity> seatPlayerList;
        /// <summary>
        /// 是不是房主
        /// </summary>
        public bool homeLorder;  
        /// <summary>
        /// 解散房间结果
        /// </summary>
        public enum RoomResult
        {
            agree,//同意
            Disagree,//不同意
            Nonrresponse // 无响应
        }
        public ENUM_ROOMRESULT roomResult = ENUM_ROOMRESULT.NONRESPONSE;
        /// <summary>
        /// 牌的长度
        /// </summary>
        public int pokerCount;

        public SeatEntity() { }


    }
    public class PlayerEntityZjh
    {
        /// <summary>
        /// 玩家ID
        /// </summary>
        public int playerId;
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string nickname;
        /// <summary>
        /// 性别
        /// </summary>
        public int gender;
        /// <summary>
        /// 头像
        /// </summary>
        public string avatar;
        /// <summary>
        /// 金币
        /// </summary>
        public int gold;
        /// <summary>
        /// 下注
        /// </summary>
        public int pour;
        /// <summary>
        /// 时间戳
        /// </summary>
        public long unixtime;
        /// <summary>
        /// 房卡数量
        /// </summary>
        public int cards;
        /// <summary>
        /// 玩家在的房间ID
        /// </summary>
        public int roomId;
        /// <summary>
        /// 玩家的准备状态
        /// </summary>
        public int status;
        /// <summary>
        /// 是不是房主
        /// </summary>
        public bool homeLorder;

        public PlayerEntityZjh() { }

    }

    public class Poker
    {

        public enum PokerStatus
        {
            Postive,//正面
            Opposite//反面
        }
        /// <summary>
        /// 索引
        /// </summary>
        public int index;
        /// <summary>
        /// 花色
        /// </summary>
        public int color;
        /// <summary>
        /// 大小
        /// </summary>
        public int size;

        public Poker() { }
        public override string ToString()
        {
            return string.Format("{0}_{1}", size, color);
        }
    }

    public class BillEntity
    {
        /// <summary>
        /// 玩家ID
        /// </summary>
        public int playerIDBill;
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string nameBill;
        /// <summary>
        /// 玩家头像
        /// </summary>
        public string avatarBill;
        /// <summary>
        /// 玩家分数
        /// </summary>
        public float pourBill;
    }
}
