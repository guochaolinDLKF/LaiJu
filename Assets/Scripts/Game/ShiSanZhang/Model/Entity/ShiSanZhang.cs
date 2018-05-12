//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 3:53:35 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.sss;

namespace ShiSanZhang
{
    public class RoomEntity: RoomEntityBase
    {
        /// <summary>
        /// 房间配置ID
        /// </summary>
        public int roomSettingId;
        /// <summary>
        /// 房间座位
        /// </summary>
        public List<SeatEntity> SeatList;
        /// <summary>
        /// 开始时间
        /// </summary>
        public long beginTime;
        /// <summary>
        /// 房间玩家
        /// </summary>
        public List<PlayerEntity> playerList;
        /// <summary>
        /// 房间状态
        /// </summary>
        public ROOM_STATUS SszRoomStatus;
        /// <summary>
        /// 房间状态计时
        /// </summary>
        public long sysTime;
        /// <summary>
        /// 座位数
        /// </summary>
        public int SeatCount;

        public int BaseScore;
        /// <summary>
        /// 房间状态
        /// </summary>   
        // public ROOM_STATUS roomStatus = ROOM_STATUS.IDLE;
        public RoomEntity() { }
    }



    public class SeatEntity : SeatEntityBase
    {
        /// <summary>
        /// 是否准备
        /// </summary>
        public bool isReady;

        /// <summary>
        /// 座位状态
        /// </summary>
        public SEAT_STATUS seatStatus;
        /// <summary>
        /// 下注分数
        /// </summary>
        public int bet;

        public List<Poker> handPokerList;


        /// <summary>
        /// 首牌
        /// </summary>
        public List<Poker> firstPokerList;
        /// <summary>
        /// 中牌
        /// </summary>
        public List<Poker> middlePokerList;
        /// <summary>
        /// 尾牌
        /// </summary>
        public List<Poker> endPokerList;


        public SeatEntity() { }
    }


    public class Poker : IComparable<Poker>
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index;
        /// <summary>
        /// 花色
        /// </summary>
        public int Color;
        /// <summary>
        /// 大小
        /// </summary>
        public int Size;

        public Poker() { }

        public Poker(int Index, int Color, int Size)
        {
            this.Index = Index;
            this.Color = Color;
            this.Size = Size;
        }

        public override string ToString()
        {
            return string.Format("{0}_{1}", Size, Color);
        }

        public int CompareTo(Poker other)
        {
            if (other == null) return 1;
            return other.Size - Size;
        }
    }
}

