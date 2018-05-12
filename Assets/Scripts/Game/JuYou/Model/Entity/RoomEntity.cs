//===================================================
//Author      : WZQ
//CreateTime  ：8/7/2017 3:01:34 PM
//Description ：聚友房间实体
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.jy;
namespace JuYou
{
    public class RoomEntity : RoomEntityBase
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public long Unixtime = 0;

        /// <summary>
        /// 聚友座位
        /// </summary>
        public List<SeatEntity> SeatList;

        /// <summary>
        /// 房间状态
        /// </summary>
        public ROOM_STATUS roomStatus;

        /// <summary>
        /// 当前房间人数（客户端自定义）
        /// </summary>
        public int playerNumber = 0;

        /// <summary>
        /// 当前同意解散人数（客户端自定义）
        /// </summary>
        public int agreeDissolveCount = 0;

        /// <summary>
        /// 底分
        /// </summary>
        public int baseScore;

        /// <summary>
        /// 剩余牌数量
        /// </summary>
        public int remainMahjong;

        /// <summary>
        /// 本局是否结束
        /// </summary>
        public bool loopEnd;

        /// <summary>
        /// 正在选庄座位
        /// </summary>
        //public Seat ChooseBankerSeat;

        /// <summary>
        ///  每局总牌数
        /// </summary>
        public int mahJongSum=120;

        /// <summary>
        /// 是否爆锅
        /// </summary>
        public bool isBaoGuo = false;


        public List<int> OperatePosList = new List<int>();



        public void SetRoom(JY_ROOM prRoom)
        {

            if (prRoom.hasStatus())
            {
                roomStatus = prRoom.status;
            }

            if (prRoom.hasLoop())
            {
                currentLoop = prRoom.loop;
            }

            if (prRoom.hasMaxLoop())
            {
                maxLoop = prRoom.maxLoop;
            }

            if (prRoom.hasBaseScore())
            {
                baseScore = prRoom.baseScore;
            }
            if (prRoom.hasIsBomb())
            {
                isBaoGuo = prRoom.isBomb;
            }


            //if (prRoom.hasUnixtime())//----------------------------------------------------时间戳
            //{
            //    Unixtime = prRoom.unixtime;
            //}
            //if (prRoom.hasRemainMahjong())
            //{
            //    remainMahjong = prRoom.remainMahjong;
            //}
            //if (prRoom.hasLoopEnd())
            //{
            //    loopEnd = prRoom.loopEnd;
            //}

            //if (prRoom.hasMahJongSum())
            //{
            //    mahJongSum = prRoom.mahJongSum;
            //}


            List<JY_SEAT> prPokerList = prRoom.getSeatListList();

            for (int i = 0; i < prPokerList.Count; i++)
            {


                if (prPokerList[i] != null && (prPokerList[i].pos > 0 || prPokerList[i].playerId > 0))
                {

                    //--------------1-----------------
                    for (int j = 0; j < SeatList.Count; j++)
                    {
                        if (prPokerList[i].pos == SeatList[j].Pos)
                        {
                            SeatList[j].SetSeat(prPokerList[i]);
                            break;
                        }
                    }


                }


            }







        }

    }
}