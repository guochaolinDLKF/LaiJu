//===================================================
//Author      : WZQ
//CreateTime  ：7/4/2017 4:05:51 PM
//Description ：牌九Room
//===================================================

using System.Collections.Generic;
using proto.paigow;
using com.oegame;

namespace PaiJiu
{
    /// <summary>
    /// 牌九Room
    /// </summary>
    public class Room : RoomEntityBase
    {
        /// <summary>
        /// 房间模式   1轮庄 2抢庄
        /// </summary>
        public ROOM_MODEL roomModel = ROOM_MODEL.ROOM_MODEL_CHANGE;

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Unixtime;

        /// <summary>
        /// 牌九座位
        /// </summary>
        public List<PaiJiu.Seat> SeatList;

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
        /// 本局骰子
        /// </summary>
        public DiceEntity currentDice=new DiceEntity ();

        /// <summary>
        /// 剩余牌墙的具体信息
        /// </summary>
        public List<Poker> pokerWall=new List<Poker>();


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
        public Seat ChooseBankerSeat;

        /// <summary>
        ///  每局总牌数
        /// </summary>
        public int mahJongSum;

        /// <summary>
        /// 该局第几把
        /// </summary>
        public int dealTime = 0;

        /// <summary>
        /// 是否是切锅
        /// </summary>
        public bool isCutPan = false;

        /// <summary>
        /// 是否爆锅
        /// </summary>
        public bool isBombPan = false;

        public List<int> OperatePosList = new List<int>();

        public void SetDiceEntity(int pos, int diceA, int diceB)
        {
            currentDice.seatPos = pos;
            currentDice.diceA = diceA;
            currentDice.diceB = diceB;
        }

        public void SetRoom(PAIGOW_ROOM prRoom)
        {
       
            if (prRoom.hasRoomStatus())
            {
                roomStatus = prRoom.room_status;
            }

            if (prRoom.hasLoop())
            {
                currentLoop = prRoom.loop;
            }

            if (prRoom.hasMaxLoop())
            {
                maxLoop = prRoom.maxLoop;
            }

            if (prRoom.hasUnixtime())
            {
                Unixtime = prRoom.unixtime;
            }
            if (prRoom.hasRemainMahjong())
            {
                remainMahjong = prRoom.remainMahjong;
            }
            if (prRoom.hasLoopEnd())
            {
                loopEnd = prRoom.loopEnd;
            }

            if (prRoom.hasMahJongSum())
            {
                mahJongSum = prRoom.mahJongSum;
            }

            if (prRoom.hasDealTime())
            {
                dealTime = prRoom.dealTime;
            }

            if (prRoom.hasIsCutPan()) isCutPan = prRoom.isCutPan;
            if (prRoom.hasIsBombPan()) isBombPan = prRoom.isBombPan;

            List<PAIGOW_SEAT> prPokerList = prRoom.getPaigowSeatList();

            for (int i = 0; i < prPokerList.Count; i++)
            {


                if (prPokerList[i] != null && prPokerList[i].pos > 0 || prPokerList[i].playerId > 0)
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


    public enum RoomModel
    {

        TurnsBanker = 1,//轮庄
        robBanker = 2,//抢庄

    }



}


