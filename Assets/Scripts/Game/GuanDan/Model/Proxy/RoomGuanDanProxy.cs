//===================================================
//Author      : WZQ
//CreateTime  ：11/6/2017 11:00:20 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuanDan;
using guandan.proto;
public class RoomGuanDanProxy : ProxyBase<RoomGuanDanProxy>
{
    #region Variable
    /// <summary>
    /// 当前房间数据实体
    /// </summary>
    public RoomEntity CurrentRoom;
    /// <summary>
    /// 玩家座位数据实体
    /// </summary>
    public SeatEntity PlayerSeat;
    /// <summary>
    /// 房间座位数量
    /// </summary>
    public const int ROOM_SEAT_COUNT = 4;
    /// <summary>
    /// 当前操作的座位
    /// </summary>
    public SeatEntity CurrentOperator { get; private set; }

    ///// <summary>
    ///// 当前游戏状态
    ///// </summary>
    //public MahjongGameState CurrentState;


    ///// <summary>
    ///// 游戏规则
    ///// </summary>
    //public MahjongRule Rule;


    #endregion


    #region 初始化房间信息  构建房间
    /// <summary>
    /// 初始化房间信息  构建房间
    /// </summary>
    /// <param name="proto"></param>
    public void InitRoom(ROOM_INFO proto)
    {
       
       
        CurrentRoom = new RoomEntity()
        {
            currentLoop = proto.loop,

            roomId = proto.roomId,
            
            Status = proto.room_status,
            maxLoop = proto.maxLoop,

            //matchId = protoRoom.matchId,//比赛场

        };


        CurrentRoom.Config.Clear();
        //for (int i = 0; i < proto.settingIdCount(); ++i)
        //{
        //    cfg_settingEntity settingEntity = cfg_settingDBModel.Instance.Get(proto.getSettingId(i));
        //    if (settingEntity != null)
        //    {
        //        CurrentRoom.Config.Add(settingEntity);
        //    }
        //}
        CurrentRoom.SeatList = new List<SeatEntity>();
        for (int i = 0; i < proto.seatinfoCount(); ++i)
        {
            SEAT_INFO op_seat = proto.getSeatinfo(i);

            SeatEntity seat = new SeatEntity();
            seat.Pos = op_seat.pos;

            seat.PlayerId = op_seat.playerId;
            seat.Gender = op_seat.gender;
            seat.Gold = op_seat.gold;
            seat.Nickname = op_seat.nickname;
            seat.Avatar = op_seat.avatar;
            //seat.IP = op_seat.ipaddr;
            seat.Status = op_seat.seat_status;
            Debug.Log(seat.Status);

            for (int j = 0; j < op_seat.pockerInfoCount(); ++j)
            {

                POCKER_INFO protoPoker = op_seat.getPockerInfo (j);
                seat.PokerList.Add(new Poker(protoPoker.index, protoPoker.color, protoPoker.size));
            }

            seat.SetSeat(op_seat);
            CurrentRoom.SeatList.Add(seat);
        
            //            for (int j = 0; j < op_seat.desktopCount(); ++j)
            //            {

            //                OP_POKER protoPoker = op_seat.getDesktop(j);
            //                seat.DeskTopPoker.Add(new Poker(protoPoker.index, protoPoker.color, protoPoker.size, protoPoker.pos));
            //            }
            //            for (int j = 0; j < op_seat.usePokerGroupCount(); ++j)
            //            {
            //                OP_POKER_GROUP protoPoker = op_seat.getUsePokerGroup(j);
            //                List<Poker> lst = new List<Poker>();
            //                for (int k = 0; k < protoPoker.pokerCount(); ++k)
            //                {
            //                    OP_POKER op_poker = protoPoker.getPoker(k);
            //                    Poker poker = new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos);
            //                    lst.Add(poker);
            //                }
            //                if (protoPoker.typeId != ENUM_POKER_TYPE.POKER_TYPE_GANG)
            //                {
            //                    MahJongHelper.SimpleSort(lst);
            //                }
            //#if IS_DAZHONG || IS_WUANJUN
            //                if ((OperatorType)protoPoker.typeId == OperatorType.Chi)//吃的牌放中间
            //                {
            //                    for (int k = 0; k < lst.Count; ++k)
            //                    {
            //                        if (lst[k].pos != seat.Pos)
            //                        {
            //                            Poker poker = lst[k];
            //                            lst.Remove(poker);
            //                            lst.Insert(1, poker);
            //                        }
            //                    }
            //                }

            //                if ((OperatorType)protoPoker.typeId == OperatorType.Peng || (OperatorType)protoPoker.typeId == OperatorType.Gang)
            //                {
            //                    for (int k = 0; k < lst.Count; ++k)
            //                    {
            //                        if (lst[k].pos != op_seat.pos)
            //                        {
            //                            Poker poker = lst[k];
            //                            lst.Remove(poker);
            //                            if (Mathf.Abs(poker.pos - op_seat.pos) == 2)
            //                            {
            //                                lst.Insert(1, poker);
            //                                break;
            //                            }
            //                            if (poker.pos - op_seat.pos == 1 || poker.pos - op_seat.pos == -3)
            //                            {
            //                                lst.Insert(2, poker);
            //                                break;
            //                            }
            //                            if (poker.pos - op_seat.pos == -1 || poker.pos - op_seat.pos == 3)
            //                            {
            //                                lst.Insert(0, poker);
            //                                break;
            //                            }
            //                        }
            //                    }
            //                }
            //#endif
            //                PokerCombinationEntity combination = new PokerCombinationEntity((OperatorType)protoPoker.typeId, (int)protoPoker.subTypeId, lst);

            //                seat.UsedPokerList.Add(combination);
        }
    //            for (int j = 0; j < op_seat.universalCount(); ++j)
    //            {
    //                OP_POKER op_poker = op_seat.getUniversal(j);
    //                seat.UniversalList.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
    //            }
    //            seat.PokerAmount = op_seat.pokerAmount;
    //            seat.IsBanker = op_seat.isBanker;
    //            seat.IsTrustee = op_seat.isTrustee;
    //            seat.IsWaiver = op_seat.isWaiver;
    //            for (int j = 0; j < op_seat.keepPokerGroupCount(); ++j)
    //            {
    //                OP_POKER_GROUP op_poker_group = op_seat.getKeepPokerGroup(j);
    //                List<Poker> lstPoker = new List<Poker>();
    //                for (int k = 0; k < op_poker_group.pokerCount(); ++k)
    //                {
    //                    OP_POKER op_poker = op_poker_group.getPoker(k);
    //                    lstPoker.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
    //                }
    //                seat.HoldPoker.Add(lstPoker);
    //            }
    //            for (int j = 0; j < op_seat.dingPokerGroupCount(); ++j)
    //            {
    //                OP_POKER_GROUP op_poker_group = op_seat.getDingPokerGroup(j);
    //                List<Poker> lstPoker = new List<Poker>();
    //                for (int k = 0; k < op_poker_group.pokerCount(); ++k)
    //                {
    //                    OP_POKER op_poker = op_poker_group.getPoker(k);
    //                    lstPoker.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
    //                }
    //                seat.DingJiangPoker.AddRange(lstPoker);
    //            }
    //            seat.Direction = op_seat.wind;
    //            if (proto.seatCount() == 2 && seat.Direction == 2)
    //            {
    //                seat.Direction = 3;
    //            }
    //            CurrentRoom.SeatList.Add(seat);
    //        }

    //        if (CurrentRoom.SeatList.Count == 2)
    //        {
    //            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
    //            {
    //                for (int j = 0; j < CurrentRoom.SeatList[i].UsedPokerList.Count; ++j)
    //                {
    //                    for (int k = 0; k < CurrentRoom.SeatList[i].UsedPokerList[j].PokerCount; ++k)
    //                    {
    //                        if (CurrentRoom.SeatList[i].UsedPokerList[j].PokerList[k].pos == 2)
    //                        {
    //                            CurrentRoom.SeatList[i].UsedPokerList[j].PokerList[k].pos = 3;
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        CurrentRoom.FirstDice = new DiceEntity()
    //        {
    //            diceA = proto.diceFirstA,
    //            diceB = proto.diceFirstB,
    //            seatPos = proto.diceFirst,
    //        };
    //        CurrentRoom.SecondDice = new DiceEntity()
    //        {
    //            diceA = proto.diceSecondA,
    //            diceB = proto.diceSecondB,
    //            seatPos = proto.diceSecond,
    //        };

    //        int askLeng = proto.askPokerGroupCount();
    //        if (askLeng > 0)
    //        {
    //            AskPokerGroup = new List<OP_POKER_GROUP>();
    //            for (int i = 0; i < askLeng; ++i)
    //            {
    //                AskPokerGroup.Add(proto.getAskPokerGroup(i));
    //            }
    //        }
    //        else
    //        {
    //            AskPokerGroup = null;
    //        }
    //        cfg_settingDBModel.Instance.SetSetting(new List<int>(proto.getSettingIdList()));

    //        CalculateSeatIndex();

    //        InitConfig();

    //        if (currentOperatorPos == 0)
    //        {
    //            currentOperatorPos = CurrentRoom.BankerPos;
    //        }

    //        for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
    //        {
    //            SeatEntity seat = CurrentRoom.SeatList[i];
    //            for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
    //            {
    //                for (int k = 0; k < CurrentRoom.SeatList[j].UsedPokerList.Count; ++k)
    //                {
    //                    for (int l = 0; l < CurrentRoom.SeatList[j].UsedPokerList[k].PokerList.Count; ++l)
    //                    {
    //                        if (CurrentRoom.SeatList[j].UsedPokerList[k].PokerList[l].pos == seat.Pos && MahJongHelper.HasPoker(CurrentRoom.SeatList[j].UsedPokerList[k].PokerList[l], seat.UniversalList))
    //                        {
    //                            seat.isPlayedUniversal = true;
    //                            break;
    //                        }
    //                    }
    //                    if (seat.isPlayedUniversal) break;
    //                }
    //                if (seat.isPlayedUniversal) break;
    //                for (int k = 0; k < CurrentRoom.SeatList[j].DeskTopPoker.Count; ++k)
    //                {
    //                    if (CurrentRoom.SeatList[j].DeskTopPoker[k].pos == seat.Pos && MahJongHelper.HasPoker(CurrentRoom.SeatList[j].DeskTopPoker[k], seat.UniversalList))
    //                    {
    //                        seat.isPlayedUniversal = true;
    //                        break;
    //                    }
    //                }
    //                if (seat.isPlayedUniversal) break;
    //                for (int k = 0; k < CurrentRoom.SeatList[j].PokerList.Count; ++k)
    //                {
    //                    if (CurrentRoom.SeatList[j].PokerList[k].pos == seat.Pos && MahJongHelper.HasPoker(CurrentRoom.SeatList[j].PokerList[k], seat.UniversalList))
    //                    {
    //                        seat.isPlayedUniversal = true;
    //                        break;
    //                    }
    //                }
    //                if (seat.isPlayedUniversal) break;
    //            }
    //        }

    //        for (int i = 0; i < CurrentRoom.Config.Count; ++i)
    //        {
    //            if (CurrentRoom.Config[i].tags.Equals("handTotal"))
    //            {
    //                CurrentRoom.PokerTotalPerPlayer = CurrentRoom.Config[i].value;
    //                break;
    //            }
    //        }

    //        SetCurrentOperator(currentOperatorPos, true);



}
    #endregion








    #region GetSeat 获取座位实体
    /// <summary>
    /// 根据玩家Id获取座位
    /// </summary>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public SeatEntity GetSeatByPlayerId(int playerId)
    {
        if (CurrentRoom == null) return null;
        for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
        {
            if (CurrentRoom.SeatList[i].PlayerId == playerId)
            {
                return CurrentRoom.SeatList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 根据座位号获取座位
    /// </summary>
    /// <param name="seatId"></param>
    /// <returns></returns>
    public SeatEntity GetSeatBySeatId(int seatId)
    {
        if (CurrentRoom == null) return null;
        for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
        {
            if (CurrentRoom.SeatList[i].Pos == seatId)
            {
                return CurrentRoom.SeatList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 根据索引获取座位
    /// </summary>
    /// <param name="seatIndex"></param>
    /// <returns></returns>
    public SeatEntity GetSeatBySeatIndex(int seatIndex)
    {
        if (CurrentRoom == null) return null;
        for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
        {
            if (CurrentRoom.SeatList[i].Index == seatIndex)
            {
                return CurrentRoom.SeatList[i];
            }
        }
        return null;
    }
    #endregion

    #region SendRoomInfoChangeNotify 发送房间信息变更消息
    /// <summary>
    /// 发送房间信息变更消息
    /// </summary>
    public void SendRoomInfoChangeNotify(bool containSeat = true)
    {
        TransferData roomData = new TransferData();
        roomData.SetValue("Room", CurrentRoom);
        roomData.SetValue("CurrentOperator", CurrentOperator);
        roomData.SetValue("PlayerSeatPos", PlayerSeat.Pos);
        SendNotification("", roomData);

        if (!containSeat) return;
        for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
        {
            SendSeatInfoChangeNotify(CurrentRoom.SeatList[i]);
        }
    }
    #endregion

    #region SendSeatInfoChangeNotify 发送座位信息变更消息
    /// <summary>
    /// 发送座位信息变更消息
    /// </summary>
    /// <param name="seat"></param>
    private void SendSeatInfoChangeNotify(SeatEntity seat)
    {
        if (seat == null) return;

        TransferData data = new TransferData();
        data.SetValue("Seat", seat);
        data.SetValue("IsPlayer", seat == PlayerSeat);
        //data.SetValue("RoomStatus", CurrentRoom.Status);
        data.SetValue("PlayerStatus", PlayerSeat.Status);
        SendNotification("", data);
    }
    #endregion

    #region SetCountDown 设置倒计时
    /// <summary> 
    /// 设置倒计时
    /// </summary>
    /// <param name="countDown"></param>
    public void SetCountDown(long countDown)
    {
        TransferData data = new TransferData();
        data.SetValue("ServerTime", countDown);
        SendNotification("OnCountDownUpdate", data);
    }
    #endregion

}
