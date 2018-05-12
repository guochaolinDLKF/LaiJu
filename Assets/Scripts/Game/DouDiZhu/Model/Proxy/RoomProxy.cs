//===================================================
//Author      : DRB
//CreateTime  ：7/25/2017 3:55:48 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace DRB.DouDiZhu
{
    public class RoomProxy : ProxyBase<RoomProxy>
    {
        /// <summary>
        /// 房间信息变更
        /// </summary>
        public const string ON_ROOM_INFO_CHANGED = "OnDouDiZhuRoomInfoChanged";
        /// <summary>
        /// 座位信息变更
        /// </summary>
        public const string ON_SEAT_INFO_CHANGED = "OnDouDiZhuSeatInfoChanged";
        /// <summary>
        /// 准备
        /// </summary>
        public const string ON_READY = "OnDouDiZhuReady";
        /// <summary>
        /// 开局
        /// </summary>
        public const string ON_BEGIN = "OnDouDiZhuBegin";
        /// <summary>
        /// 明牌
        /// </summary>
        public const string ON_SHOW_POKER = "OnDouDiZhuShowPoker";
        /// <summary>
        /// 出牌
        /// </summary>
        public const string ON_PLAY_POKER = "OnDouDiZhuPlayPoker";
        /// <summary>
        /// 过
        /// </summary>
        public const string ON_SEAT_PASS = "OnDouDiZhuPass";
        /// <summary>
        /// 询问下注
        /// </summary>
        public const string ON_ASK_BET = "OnDouDiZhuAskBet";
        /// <summary>
        /// 下注
        /// </summary>
        public const string ON_SEAT_BET = "OnDouDiZhuBet";
        /// <summary>
        /// 设置地主
        /// </summary>
        public const string ON_SET_BANKER = "OnDouDiZhuSetBanker";
        /// <summary>
        /// 询问出牌
        /// </summary>
        public const string ON_ASK_PLAY_POKER = "OnDouDiZhuAskPlayPoker";
        /// <summary>
        /// 重建房间
        /// </summary>
        public const string ON_INIT = "OnDouDiZhuInit";
        /// <summary>
        /// 结算
        /// </summary>
        public const string ON_SETTLE = "OnDouDiZhuSettle";
        /// <summary>
        /// 结算
        /// </summary>
        public const string ON_RESULT = "OnDouDiZhuResult";
        /// <summary>
        /// 翻倍
        /// </summary>
        public const string ON_PLUS_TIMES = "OnDouDiZhuPlusTimes";
        /// <summary>
        /// 托管
        /// </summary>
        public const string ON_TRUSTEE = "OnDouDiZhuTrustee";


        /// <summary>
        /// 房间数据
        /// </summary>
        public RoomEntity CurrentRoom;
        /// <summary>
        /// 房间座位数量
        /// </summary>
        public const int ROOM_SEAT_COUNT = 3;



        #region InitRoom 初始化房间
        /// <summary>
        /// 初始化房间
        /// </summary>
        /// <param name="room"></param>
        public void InitRoom(RoomEntity room)
        {
            CurrentRoom = room;
            CalculateSeatIndex();

            bool mustPlay = false;
            bool canPlay = true;
            Deck prevDeck = GetPreviourDeck();
            if (prevDeck == null)
            {
                mustPlay = true;
            }
            else
            {
                List<Deck> Tip = DouDiZhuHelper.GetStrongerDeck(prevDeck, CurrentRoom.PlayerSeat.pokerList);

                if (Tip == null || Tip.Count == 0)
                {
                    canPlay = false;
                }
            }



            TransferData data = new TransferData();
            data.SetValue("Room", CurrentRoom);
            data.SetValue("MustPlay", mustPlay);
            data.SetValue("CanPlay", canPlay);
            SendNotification(ON_INIT, data);

            SendRoomInfoChangeNotify();
        }
        #endregion

        #region CalculateSeatIndex 计算index
        /// <summary>
        /// 计算index
        /// </summary>
        private void CalculateSeatIndex()
        {
            CurrentRoom.PlayerSeat = null;
            if (CurrentRoom == null) return;
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                if (CurrentRoom.SeatList[i].IsPlayer)
                {
                    CurrentRoom.PlayerSeat = CurrentRoom.SeatList[i];
                    CurrentRoom.PlayerSeat.IsPlayer = true;
                    for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
                    {
                        SeatEntity seat = CurrentRoom.SeatList[j];
                        int seatIndex = seat.Pos - CurrentRoom.PlayerSeat.Pos;
                        seatIndex = seatIndex < 0 ? seatIndex + ROOM_SEAT_COUNT : seatIndex;
                        seat.Index = seatIndex;
                    }
                    break;
                }
            }
            if (CurrentRoom.PlayerSeat == null)
            {
                CurrentRoom.PlayerSeat = CurrentRoom.SeatList[0];
                CurrentRoom.PlayerSeat.IsPlayer = true;
                for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
                {
                    SeatEntity seat = CurrentRoom.SeatList[j];
                    int seatIndex = seat.Pos - CurrentRoom.PlayerSeat.Pos;
                    seatIndex = seatIndex < 0 ? seatIndex + ROOM_SEAT_COUNT : seatIndex;
                    seat.Index = seatIndex;
                }
            }
        }
        #endregion

        #region Reset 重置房间数据
        /// <summary>
        /// 重置房间数据
        /// </summary>
        public void Reset()
        {
            CurrentRoom = null;
            //CurrentRoom.PlayerSeat = null;
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
            seat.status = SeatEntity.SeatStatus.Idle;
            SendSeatInfoChangeNotify(seat);
            LogSystem.Log(seat.Nickname + "进入房间");
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
            seat.bet = 0;
            seat.IsBanker = false;
            seat.status = SeatEntity.SeatStatus.Idle;
            SendSeatInfoChangeNotify(seat);
            LogSystem.Log(seat.Nickname + "离开房间");
        }
        #endregion   

        #region Ready 准备
        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="proto"></param>
        public void Ready(int playerId, bool isReady)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            seat.status = isReady ? SeatEntity.SeatStatus.Ready : SeatEntity.SeatStatus.Idle;
            CurrentRoom.roomStatus = RoomEntity.RoomStatus.Idle;

            TransferData data = new TransferData();
            data.SetValue("SeatEntity", seat);
            SendNotification(ON_READY, data);
        }
        #endregion

        #region Begin 开局
        /// <summary>
        /// 开局
        /// </summary>
        /// <param name="lstSeat"></param>
        public void Begin(List<SeatEntity> lstSeat, int loop)
        {
            if (lstSeat == null) return;
            if (CurrentRoom == null) return;

            ++CurrentRoom.currentLoop;
            CurrentRoom.basePoker.Clear();
            CurrentRoom.baseScore = 0;
            CurrentRoom.roomStatus = RoomEntity.RoomStatus.Gaming;

            for (int i = 0; i < lstSeat.Count; ++i)
            {
                SeatEntity seat = GetSeatByPlayerId(lstSeat[i].PlayerId);
                if (seat == null) continue;
                seat.status = SeatEntity.SeatStatus.Wait;
                seat.IsBanker = false;
                seat.pokerList.Clear();
                seat.pokerList.AddRange(lstSeat[i].pokerList);
                seat.pokerList.Sort();
                if (seat.PreviourPoker != null)
                {
                    seat.PreviourPoker.Clear();
                }
            }
            TransferData data = new TransferData();
            data.SetValue("Room", CurrentRoom);
            data.SetValue("Loop", loop);

            SendNotification(ON_BEGIN, data);
        }
        #endregion

        #region ShowPoker 明牌
        /// <summary>
        /// 明牌
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="pokers"></param>
        public void ShowPoker(int playerId, List<Poker> pokers)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);

            AppDebug.LogWarning(pokers.Count);

            if (seat == null) return;

            seat.pokerList.Clear();
            seat.pokerList.AddRange(pokers);
            seat.pokerList.Sort();

            //if (CurrentRoom.Times == 0)
            //{
            //    CurrentRoom.Times = 1;
            //}
            //CurrentRoom.Times *= 2;

            TransferData data = new TransferData();
            data.SetValue("SeatEntity", seat);
            SendNotification(ON_SHOW_POKER, data);
        }
        #endregion

        #region AskBet 询问下注
        /// <summary>
        /// 询问下注
        /// </summary>
        public void AskBet(int playerId)
        {
            AppDebug.LogWarning("询问下注");
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            CurrentRoom.roomStatus = RoomEntity.RoomStatus.Bet;
            seat.status = SeatEntity.SeatStatus.Bet;

            TransferData data = new TransferData();
            data.SetValue("SeatEntity", seat);
            data.SetValue("BaseScore", CurrentRoom.baseScore);
            SendNotification(ON_ASK_BET, data);
        }
        #endregion

        #region Bet 下注
        /// <summary>
        /// 下注
        /// </summary>
        public void Bet(int playerId, int bet)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            CurrentRoom.roomStatus = RoomEntity.RoomStatus.Bet;
            seat.bet = bet;
            if (bet > CurrentRoom.baseScore)
            {
                CurrentRoom.baseScore = bet;
            }
            seat.status = SeatEntity.SeatStatus.Wait;

            TransferData data = new TransferData();
            data.SetValue("Room", CurrentRoom);
            data.SetValue("SeatEntity", seat);
            SendNotification(ON_SEAT_BET, data);
        }
        #endregion

        #region SetBanker 设置地主
        /// <summary>
        /// 设置地主
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="pokers"></param>
        public void SetBanker(int playerId, List<Poker> pokers)
        {
            CurrentRoom.Times = 1;
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            seat.IsBanker = true;
            seat.pokerList.AddRange(pokers);

            CurrentRoom.basePoker.Clear();
            CurrentRoom.basePoker.AddRange(pokers);

            TransferData data = new TransferData();
            data.SetValue("SeatEntity", seat);
            data.SetValue("PokerList", pokers);
            SendNotification(ON_SET_BANKER, data);
        }
        #endregion

        #region AskPlayPoker 询问出牌
        /// <summary>
        /// 询问出牌
        /// </summary>
        /// <param name="playerId"></param>
        public void AskPlayPoker(int playerId, int unixTime)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            seat.status = SeatEntity.SeatStatus.PlayPoker;
            seat.PreviourPoker = null;
            seat.unixtime = unixTime;

            bool mustPlay = false;
            bool canPlay = true;
            Deck prevDeck = GetPreviourDeck();
            if (prevDeck == null)
            {
                mustPlay = true;
            }
            else
            {
                List<Deck> Tip = DouDiZhuHelper.GetStrongerDeck(prevDeck, CurrentRoom.PlayerSeat.pokerList);

                if (Tip == null || Tip.Count == 0)
                {
                    canPlay = false;
                }
            }
            bool isDelaySendPass = false;
            if (GetPreviourDeck() != null)
            {
                if (GetPreviourDeck().type == DeckType.SS || (CurrentRoom.PlayerSeat.pokerList.Count == 1 && !canPlay))
                {
                    seat.unixtime = 2;
                    isDelaySendPass = true;
                }
            }


            TransferData data = new TransferData();
            data.SetValue("SeatEntity", seat);
            data.SetValue("PreviourDeck", prevDeck);
            data.SetValue("MustPlay", mustPlay);
            data.SetValue("CanPlay", canPlay);
            data.SetValue("IsDelaySendPass", isDelaySendPass);
            SendNotification(ON_ASK_PLAY_POKER, data);
        }
        #endregion

        #region PlayPoker 出牌
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="poker"></param>
        public void PlayPoker(int playerId, Deck deck)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;

            for (int i = 0; i < deck.pokers.Count; ++i)
            {
                for (int j = 0; j < seat.pokerList.Count; ++j)
                {
                    if (seat.pokerList[j].index == deck.pokers[i].index)
                    {
                        seat.pokerList.RemoveAt(j);
                        break;
                    }
                }
            }

            if (deck.type == DeckType.AAAA || deck.type == DeckType.SS)
            {
                PlusTimes(2);
            }

            bool isBiger = false;

            if (GetPreviourDeckNew() != null)
            {
                if (GetPreviourDeckNew().type == deck.type && GetPreviourDeckNew().mainPoker.power < deck.mainPoker.power)
                {
                    isBiger = true;
                }
            }

            seat.PreviourPoker = deck.pokers;
            seat.status = SeatEntity.SeatStatus.Wait;

            TransferData data = new TransferData();
            data.SetValue("SeatIndex", seat.Index);
            data.SetValue("Deck", deck);
            data.SetValue("IsBanker", seat.IsBanker);
            data.SetValue("isBiger", isBiger);
            SendNotification(ON_PLAY_POKER, data);
        }
        #endregion

        #region PlusTimes 增加倍数
        /// <summary>
        /// 增加倍数
        /// </summary>
        /// <param name="times"></param>
        public void PlusTimes(int times)
        {
            if (CurrentRoom == null) return;
            CurrentRoom.Times *= times;

            TransferData data = new TransferData();
            data.SetValue("Room", CurrentRoom);
            SendNotification(ON_PLUS_TIMES, data);
        }
        #endregion

        #region Pass 过
        /// <summary>
        /// 过
        /// </summary>
        /// <param name="player"></param>
        public void Pass(int playerId)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            seat.PreviourPoker = null;
            seat.status = SeatEntity.SeatStatus.Wait;

            TransferData data = new TransferData();
            data.SetValue("SeatIndex", seat.Index);
            SendNotification(ON_SEAT_PASS, data);
        }
        #endregion

        #region SetTrustee 托管
        /// <summary>
        /// 托管
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="isTrustee"></param>
        public void SetTrustee(int playerId, bool isTrustee)
        {
            SeatEntity seat = GetSeatByPlayerId(playerId);
            if (seat == null) return;
            seat.IsTrustee = isTrustee;

            TransferData data = new TransferData();
            data.SetValue("SeatEntity", seat);
            SendNotification(ON_TRUSTEE, data);
        }
        #endregion

        #region Settle 结算
        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="room"></param>
        public void Settle(RoomEntity room)
        {
            if (CurrentRoom == null) return;
            CurrentRoom.roomStatus = RoomEntity.RoomStatus.Idle;
            for (int i = 0; i < room.SeatList.Count; ++i)
            {
                SeatEntity protoSeat = room.SeatList[i];
                SeatEntity seat = GetSeatByPlayerId(protoSeat.PlayerId);
                seat.totalScore = protoSeat.totalScore;
            }
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                CurrentRoom.SeatList[i].status = SeatEntity.SeatStatus.Idle;
            }

            TransferData data = new TransferData();
            data.SetValue("Room", room);
            SendNotification(ON_SETTLE, data);
        }
        #endregion

        #region Result 总结算
        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="room"></param>
        public void Result(RoomEntity room)
        {
            //if (CurrentRoom == null) return;
            //CurrentRoom.roomStatus = RoomEntity.RoomStatus.Idle;
            //for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            //{
            //    CurrentRoom.SeatList[i].status = SeatEntity.SeatStatus.Idle;
            //}

            //TransferData data = new TransferData();
            //data.SetValue("Room", CurrentRoom);

            if (CurrentRoom == null) return;
            CurrentRoom.roomStatus = RoomEntity.RoomStatus.Idle;
            for (int i = 0; i < room.SeatList.Count; ++i)
            {
                SeatEntity protoSeat = room.SeatList[i];
                SeatEntity seat = GetSeatByPlayerId(protoSeat.PlayerId);
                seat.totalScore = protoSeat.totalScore;
            }
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                CurrentRoom.SeatList[i].status = SeatEntity.SeatStatus.Idle;
            }

            TransferData data = new TransferData();
            data.SetValue("Room", room);

            SendNotification(ON_RESULT, data);
        }
        #endregion

        #region GetPreviourDeck 获取上一次出的牌组
        /// <summary>
        /// 获取上一次出的牌组
        /// </summary>
        /// <returns></returns>
        public Deck GetPreviourDeck()
        {
            if (CurrentRoom.PlayerSeat == null) return null;

            int pos = CurrentRoom.PlayerSeat.Pos;
            List<Poker> prevPoker = null;
            while (prevPoker == null)
            {
                SeatEntity prevSeat = GetPreviourSeat(pos);
                pos = prevSeat.Pos;
                if (prevSeat == CurrentRoom.PlayerSeat)
                {
                    break;
                }
                prevPoker = prevSeat.PreviourPoker;
            }
            return DouDiZhuHelper.Check(prevPoker);
        }
        #endregion


        #region GetPreviourDeckNew 获取上一次出的牌组(包含玩家)
        /// <summary>
        /// 获取上一次出的牌组
        /// </summary>
        /// <returns></returns>
        public Deck GetPreviourDeckNew()
        {
            if (CurrentRoom.PlayerSeat == null) return null;

            int pos = CurrentRoom.PlayerSeat.Pos;
            List<Poker> prevPoker = null;
            while (prevPoker == null)
            {
                SeatEntity prevSeat = GetPreviourSeat(pos);
                pos = prevSeat.Pos;
                prevPoker = prevSeat.PreviourPoker;
                if (prevSeat == CurrentRoom.PlayerSeat)
                {
                    break;
                }
            }
            return DouDiZhuHelper.Check(prevPoker);
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

        #region GetPreviourSeat 获取上家座位
        /// <summary>
        /// 获取上家座位
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public SeatEntity GetPreviourSeat(int pos)
        {
            --pos;
            if (pos < 1)
            {
                pos = CurrentRoom.SeatList.Count;
            }
            return GetSeatBySeatId(pos);
        }
        #endregion


        #region SendRoomInfoChangeNotify 发送房间信息变更消息
        /// <summary>
        /// 发送房间信息变更消息
        /// </summary>
        public void SendRoomInfoChangeNotify()
        {
            TransferData roomData = new TransferData();
            roomData.SetValue("BaseScore", CurrentRoom.baseScore);
            roomData.SetValue("CurrentLoop", CurrentRoom.currentLoop);
            roomData.SetValue("MaxLoop", CurrentRoom.maxLoop);
            roomData.SetValue("RoomId", CurrentRoom.roomId);
            roomData.SetValue("RoomStatus", CurrentRoom.roomStatus);
            roomData.SetValue("Times", CurrentRoom.Times);
            SendNotification(ON_ROOM_INFO_CHANGED, roomData);
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
            data.SetValue("SeatEntity", seat);
            data.SetValue("SeatIndex", seat.Index);
            data.SetValue("SeatPos", seat.Pos);
            data.SetValue("PlayerId", seat.PlayerId);
            data.SetValue("Avatar", seat.Avatar);
            data.SetValue("Gold", seat.Gold);
            data.SetValue("IsPlayer", seat.IsPlayer);
            data.SetValue("Nickname", seat.Nickname);
            data.SetValue("PokerList", seat.pokerList);
            data.SetValue("SeatStatus", seat.status);
            data.SetValue("Bet", seat.bet);
            data.SetValue("RoomStatus", CurrentRoom.roomStatus);
            data.SetValue("TotalScore", seat.totalScore);
            SendNotification(ON_SEAT_INFO_CHANGED, data);
        }
        #endregion


    }
}
