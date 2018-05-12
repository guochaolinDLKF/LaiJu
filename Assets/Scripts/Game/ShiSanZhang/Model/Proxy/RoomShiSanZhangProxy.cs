//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 4:16:49 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.sss;

namespace ShiSanZhang
{
    public class RoomShiSanZhangProxy : ProxyBase<RoomShiSanZhangProxy>
    {
        /// <summary>
        /// 房间信息变更
        /// </summary>
        public const string ON_ROOM_INFO_CHANGED = "OnShiSanZhangRoomInfoChanged";
        /// <summary>
        /// 座位信息变更
        /// </summary>
        public const string ON_SEAT_INFO_CHANGED = "OnShiSanZhangSeatInfoChanged";
        /// <summary>
        /// 发牌
        /// </summary>
        public const string ON_DEAL_POKER = "OnShiSanZhangDealPoker";


        public RoomEntity CurrentRoom;//当前房间 

        public SeatEntity PlayerSeat;//当前座位

        public const int ROOM_SEAT_COUNT = 4;//房间座位数量


        public void InitRoom(SSS_CREATE_ROOM proto)
        {
            //CurrentRoom = room;
            //CalculateSeatIndexOne();  //普通场计算座位 Index
            //SendRoomInfoChangeNotify();
            CurrentRoom = new RoomEntity()
            {
                roomId = proto.roomInfo.roomId,
                currentLoop = proto.roomInfo.loop,
                maxLoop = proto.roomInfo.maxLoop,
                SszRoomStatus = proto.roomInfo.roomStatus,
                BaseScore = proto.roomInfo.baseScore,//底分
                SeatCount = proto.roomInfo.seatInfoCount(),
                gameId = GameCtrl.Instance.CurrentGameId,
                groupId = 0,
                matchId = 0,
                SeatList = new List<SeatEntity>(),
            };
            Debug.Log(proto.roomInfo.seatInfoCount() + "              座位长度");
            for (int i = 0; i < proto.roomInfo.seatInfoCount(); ++i)
            {
                SEAT_INFO ssz_seat = proto.roomInfo.getSeatInfo(i);
                SeatEntity seat = new SeatEntity();
                
                seat.PlayerId = ssz_seat.playerId;//玩家ID
                if (seat.PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
                {
                    seat.IsPlayer = true;
                }
                Debug.Log(ssz_seat.nickname+"              玩家名字");           
                seat.Nickname = ssz_seat.nickname;//玩家名字           
                seat.Avatar = ssz_seat.avatar;//玩家头像
                seat.Gender = ssz_seat.gender;//玩家性别
                seat.Gold = ssz_seat.gold;//底分
                seat.Pos = ssz_seat.pos;//座位位置    
                seat.seatStatus = ssz_seat.seatStatus;//座位状态
                seat.handPokerList = new List<Poker>();
                seat.firstPokerList = new List<Poker>();
                seat.middlePokerList = new List<Poker>();
                seat.endPokerList = new List<Poker>();
                for (int j = 0; j < ssz_seat.firstPokerInfoCount(); ++j)
                {
                    FIRST_POKER_INFO firstPoker = ssz_seat.getFirstPokerInfo(j);
                    seat.firstPokerList.Add(new Poker(firstPoker.index, firstPoker.color, firstPoker.size));
                }
                for (int j = 0; j < ssz_seat.secondPokerInfoCount(); ++j)
                {
                    SECOND_POKER_INFO middlePoker = ssz_seat.getSecondPokerInfo(j);
                    seat.middlePokerList.Add(new Poker(middlePoker.index, middlePoker.color, middlePoker.size));
                }
                for (int j = 0; j < ssz_seat.thirdPokerInfoCount(); ++j)
                {
                    THIRD_POKER_INFO endPoker = ssz_seat.getThirdPokerInfo(j);
                    seat.endPokerList.Add(new Poker(endPoker.index, endPoker.color, endPoker.size));
                }
                CurrentRoom.SeatList.Add(seat);
            }
            if (proto.roomInfo.seatInfoCount() == 3) CurrentRoom.SeatList.Add(new SeatEntity());
            CalculateSeatIndexOne();
        }

        #region 计算客户端 Index
        /// <summary>
        /// 普通场计算客户端index
        /// </summary>
        private void CalculateSeatIndexOne()
        {
            PlayerSeat = null;
            if (CurrentRoom == null) return;
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                if (CurrentRoom.SeatList[i].PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
                {
                    PlayerSeat = CurrentRoom.SeatList[i];
                    for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
                    {
                        SeatEntity seat = CurrentRoom.SeatList[j];
                        int seatIndex = seat.Pos - PlayerSeat.Pos;
                        seatIndex = seatIndex < 0 ? seatIndex + ROOM_SEAT_COUNT : seatIndex;
                        seat.Index = seatIndex;
                    }
                    break;
                }
            }
            if (PlayerSeat == null)
            {
                PlayerSeat = CurrentRoom.SeatList[0];
                for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
                {
                    SeatEntity seat = CurrentRoom.SeatList[j];
                    int seatIndex = seat.Pos - PlayerSeat.Pos;
                    seatIndex = seatIndex < 0 ? seatIndex + ROOM_SEAT_COUNT : seatIndex;
                    seat.Index = seatIndex;
                }
            }
        }
        #endregion

        #region EnterRoom 进入房间
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="pbSeat"></param>
        public void EnterRoom(int playerId, int gold, string avatar, int gender, string nickname, int pos)
        {
            SeatEntity seat = GetSeatBySeatId(pos);
            if (seat == null) return;
            seat.PlayerId = playerId;
            seat.Gold = gold;
            seat.Avatar = avatar;
            seat.Gender = gender;
            seat.Nickname = nickname;
            seat.isReady = false;
            SendSeatInfoChangeNotify(seat);
            LogSystem.Log(seat.Nickname + "进入房间");
        }
        #endregion

        #region Ready 准备
        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="proto"></param>
        public void Ready(int pos)
        {
            SeatEntity seat = GetSeatBySeatId(pos);
            if (seat == null) return;
            seat.seatStatus = SEAT_STATUS.SEAT_STATUS_READY;
            SendSeatInfoChangeNotify(seat);
        }
        #endregion

        #region 开始游戏
        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="pokers"></param>
        public void Begin(List<DEAL_POKER> dealPoker)
        {
            CurrentRoom.SszRoomStatus = ROOM_STATUS.ROOM_STATUS_DEAL;
            for (int i = 0; i < dealPoker.Count; i++)
            {                            
                SeatEntity seat= GetSeatBySeatId(dealPoker[i].pos);
                for (int j = 0; j < dealPoker[i].pokerInfoCount(); j++)
                {
                    POKER_INFO pokerInfo = dealPoker[i].getPokerInfo(j);
                    seat.handPokerList.Add(new Poker(pokerInfo.index, pokerInfo.color, pokerInfo.size));
                }
            }          
            SendRoomInfoChangeNotify();
        }
        #endregion

        #region 组合牌
        /// <summary>
        /// 组合牌
        /// </summary>
        public void GroupPokerProxy()
        {
            TransferData data = new TransferData();
            data.SetValue("PokerList", PlayerSeat.handPokerList);
            SendNotification(ShiSanZhangConstant.OnShiSanZhangGroupPoker,data);           
        }
        #endregion

        #region 出牌
        public void PlayPokerProxy(int pos)
        {
            SeatEntity seat = GetSeatBySeatId(pos);
            if (seat == null)
            {
                Debug.Log("怎么可能为空呢！！！！！！！！！！！！！！！！！！！！！！！！！！！");
                return;
            }
            TransferData data = new TransferData();
            data.SetValue("Seat",seat);    
            SendNotification(ShiSanZhangConstant.OnShiSanZhangGroupPokerShow,data);
        }
        #endregion

        #region ExitRoom 离开房间
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="proto"></param>
        public void ExitRoom(int playerId)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            seat.PlayerId = 0;
            seat.Gold = 0;
            seat.Avatar = string.Empty;
            seat.Gender = 0;
            seat.Nickname = string.Empty;
            seat.isReady = false;
            seat.bet = 0;
            SendSeatInfoChangeNotify(seat);
            LogSystem.Log(seat.Nickname + "离开房间");
        }
        #endregion

        #region 移除手牌
        /// <summary>
        /// 移除手牌
        /// </summary>
        /// <param name="poker"></param>
        public void RemoveHandPokerProxy(Poker poker, LevelType levelType)
        {
            for (int i = 0; i < PlayerSeat.handPokerList.Count; i++)
            {
                if (PlayerSeat.handPokerList[i].Index==poker.Index)
                {
                    if (levelType == LevelType.TOU_DAO)
                        PlayerSeat.firstPokerList.Add(poker);
                    if(levelType == LevelType.ZHONG_DAO)
                        PlayerSeat.middlePokerList.Add(poker);
                    if(levelType == LevelType.WEI_DAO)
                        PlayerSeat.endPokerList.Add(poker);

                    PlayerSeat.handPokerList.RemoveAt(i);
                    break;
                }
            }           
        }
        #endregion

        #region 增加手牌
        /// <summary>
        /// 增加手牌
        /// </summary>
        /// <param name="poker"></param>
        public void AddHandPokerProxy(Poker poker, LevelType levelType)
        {
            if (levelType==LevelType.TOU_DAO)
            {
                for (int i = 0; i < PlayerSeat.firstPokerList.Count; i++)
                {
                    if (PlayerSeat.firstPokerList[i].Index == poker.Index)
                    {
                        PlayerSeat.firstPokerList.RemoveAt(i);
                        PlayerSeat.handPokerList.Add(poker);
                        break;
                    }
                }
            }
            else if (levelType == LevelType.ZHONG_DAO)
            {
                for (int i = 0; i < PlayerSeat.middlePokerList.Count; i++)
                {
                    if (PlayerSeat.middlePokerList[i].Index == poker.Index)
                    {
                        PlayerSeat.middlePokerList.RemoveAt(i);
                        PlayerSeat.handPokerList.Add(poker);
                        break;
                    }
                }
            }
            else if (levelType == LevelType.WEI_DAO)
            {
                for (int i = 0; i < PlayerSeat.endPokerList.Count; i++)
                {
                    if (PlayerSeat.endPokerList[i].Index == poker.Index)
                    {
                        PlayerSeat.endPokerList.RemoveAt(i);
                        PlayerSeat.handPokerList.Add(poker);
                        break;
                    }
                }
            }          
        }
        #endregion

        #region GetSeatByPlayerId 根据玩家Id获取座位
        /// <summary>
        /// 根据玩家Id获取座位
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public SeatEntity GetSeatByPlayerId(int playerId)
        {
            if (CurrentRoom == null) return null;
            if (CurrentRoom.SeatList == null) return null;
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                if (CurrentRoom.SeatList[i].PlayerId == playerId)
                {
                    return CurrentRoom.SeatList[i];
                }
            }
            return null;
        }
        #endregion

        #region GetSeatBySeatId 根据座位位置获取座位
        /// <summary>
        /// 根据座位位置获取座位
        /// </summary>
        /// <param name="seatId"></param>
        /// <returns></returns>
        public SeatEntity GetSeatBySeatId(int seatPos)
        {
            if (CurrentRoom == null) return null;
            if (CurrentRoom.SeatList == null) return null;
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                if (CurrentRoom.SeatList[i].Pos == seatPos)
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
        public void SendRoomInfoChangeNotify(bool isSendSeatInfoChangedNotify = true)
        {
            TransferData roomData = new TransferData();
            //roomData.SetValue("BaseScore", CurrentRoom.baseScore);
            roomData.SetValue("CurrentLoop", CurrentRoom.currentLoop);
            roomData.SetValue("MaxLoop", CurrentRoom.maxLoop);
            roomData.SetValue("RoomId", CurrentRoom.roomId);
            roomData.SetValue("Room",CurrentRoom);      
            SendNotification(ON_ROOM_INFO_CHANGED, roomData);

            if (isSendSeatInfoChangedNotify)
            {
                for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
                {
                    SendSeatInfoChangeNotify(CurrentRoom.SeatList[i]);
                }
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
            data.SetValue("SeatIndex", seat.Index);
            data.SetValue("SeatPos", seat.Pos);
            data.SetValue("PlayerId", seat.PlayerId);
            data.SetValue("Avatar", seat.Avatar);
            data.SetValue("Gold", seat.Gold);
            data.SetValue("IsPlayer", seat == PlayerSeat);
            data.SetValue("Nickname", seat.Nickname);
            //data.SetValue("PokerList", seat.pokerList);
            data.SetValue("IsReady", seat.isReady);
            data.SetValue("Bet", seat.bet);
            data.SetValue("RoomStatus", CurrentRoom.SszRoomStatus);
            data.SetValue("SeatEntity",seat);
            SendNotification(ON_SEAT_INFO_CHANGED, data);           
        }
        #endregion

    }
}
