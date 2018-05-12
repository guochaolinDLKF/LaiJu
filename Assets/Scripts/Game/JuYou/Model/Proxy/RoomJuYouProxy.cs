//===================================================
//Author      : WZQ
//CreateTime  ：8/7/2017 3:30:11 PM
//Description ：聚友数据
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.jy;
namespace JuYou
{
    #region 
    #endregion


    public class RoomJuYouProxy : ProxyBase<RoomJuYouProxy>
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
        public const int ROOM_SEAT_COUNT = 6;

        /// <summary>
        /// 庄家位置
        /// </summary>
        public SeatEntity BankerSeat = null;

        #endregion

        #region 基本设置
        #region 初始化房间
        /// <summary>
        /// 初始化房间
        /// </summary>
        /// <param name="prRoom"></param>
        public void InitRoom(JY_ROOM prRoom)
        {

            //RoomMaJiangProxy.Instance

            CurrentRoom = new RoomEntity()
            {
                currentLoop = prRoom.loop,
                maxLoop = prRoom.maxLoop,
                roomId = prRoom.roomId,
                //matchId = prRoom.matchId,
                roomStatus = prRoom.status,
                baseScore = prRoom.baseScore,
            };

            //房间配置
            CurrentRoom.Config.Clear();
            for (int i = 0; i < prRoom.settingIdCount(); ++i)
            {
                cfg_settingEntity settingEntity = cfg_settingDBModel.Instance.Get(prRoom.getSettingId(i));
                if (settingEntity != null)
                {
                    CurrentRoom.Config.Add(settingEntity);
                }
            }

            //RoomPaiJiuProxy 
            //创建座位
            CurrentRoom.SeatList = new List<SeatEntity>();
            for (int i = 0; i < prRoom.seatListCount(); i++)
            {
                JY_SEAT jySeat = prRoom.getSeatList(i);

                SeatEntity seat = new SeatEntity();

                //手牌
                for (int j = 0; j < jySeat.pokerListCount(); j++)
                {
                    seat.PokerList.Add(new Poker());
                }

                seat.SetSeat(jySeat);

                CurrentRoom.SeatList.Add(seat);

                if (seat.IsBanker) BankerSeat = seat;
            }


            //（时间戳）
            if (prRoom.hasUnixtime())
            {
                CurrentRoom.Unixtime = prRoom.unixtime;
            }

            CalculateSeatIndex();

            PeopleCounting();
        }
        #endregion

        #region CalculateSeatIndex 计算座位的客户端序号
        /// <summary>
        /// 计算座位的客户端序号
        /// </summary>
        /// <param name="room">房间信息</param>
        private void CalculateSeatIndex()
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
                        //if (CurrentRoom.SeatList.Count == 2)
                        //{
                        //    if (seat.Index != 0)
                        //    {
                        //        seat.Index = 2;
                        //    }
                        //}
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
                    //if (CurrentRoom.SeatList.Count == 2)
                    //{
                    //    if (seat.Index != 0)
                    //    {
                    //        seat.Index = 2;
                    //    }
                    //}
                }
            }
        }
        #endregion

        #region 计算房间人数  PeopleCounting
        //计算人数
        void PeopleCounting()
        {
            //设置人数
            CurrentRoom.playerNumber = 0;
            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {
                if (CurrentRoom.SeatList[i] != null && CurrentRoom.SeatList[i].PlayerId > 0)
                {
                    CurrentRoom.playerNumber++;
                }
            }
            Debug.Log("由ID计算的当前房间玩家个数：" + CurrentRoom.playerNumber);

        }
        #endregion

        #region 进入房间
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="proto"></param>
        public void EnterRoom(JY_ROOM_ENTER proto)
        {
            Debug.Log(proto.pos + "进入房间");
             SeatEntity seat = GetSeatBySeatId(proto.pos);
            if (seat == null) return;
            seat.PlayerId = proto.playerId;
            seat.Nickname = proto.nickname;
            seat.Avatar = proto.avatar;
            seat.Gender = proto.gender;
            seat.Pos = proto.pos;
            seat.Gold = proto.gold;

            seat.IsBanker = false;
            seat.seatStatus = SEAT_STATUS.IDLE;
            seat.isReady = false;
            seat.Pour = 0;
            seat.isJoinGame = false;

            PeopleCounting();
            SendSeatInfoChangeNotify(seat);
            AppDebug.Log(seat.Nickname + "进入房间,SeatIndex:" + seat.Index);


        }
        #endregion

        #region 退出房间
        /// <summary>
        /// 退出房间
        /// </summary>
        /// <param name="proto"></param>
        public void ExitRoom(JY_ROOM_LEAVE proto)
        {
            SeatEntity seat = GetSeatByPlayerId(proto.playerId);
            if (seat == null) return;
            seat.PlayerId = 0;

            PeopleCounting();
            SendSeatInfoChangeNotify(seat);

            AppDebug.Log(seat.Nickname + "离开房间");

            if (seat == PlayerSeat)
            {
                NetWorkSocket.Instance.SafeClose(GameCtrl.Instance.SocketHandle);
                SceneMgr.Instance.LoadScene(SceneType.Main);
            }
        }
        #endregion

        #region 是否在线
        /// <summary>
        /// 是否在线
        /// </summary>
        /// <param name="proto"></param>
        public void OnLine(JY_ROOM_ONLINE proto)
        {
            if (proto.hasPos() && proto.hasIsOnLine())
            {
                SeatEntity seat =GetSeatBySeatId(proto.pos);
                seat.isOnLine = proto.isOnLine;
                SendSeatInfoChangeNotify(seat);
            }

        }

        #endregion
        #endregion

        #region 游戏中
        #region 准备
        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="proto"></param>
        public void Ready(JY_ROOM_READY proto)
        {
            //if (CurrentRoom.roomStatus == ROOM_STATUS.IDLE) CurrentRoom.roomStatus = ROOM_STATUS.READY;
            SeatEntity seat = GetSeatBySeatId(proto.pos);
            if (seat == null) return;
            seat.isReady = true;
            //seat.seatStatus = SEAT_STATUS.READY;
            //seat.IsTrustee = false;
            //SendSeatInfoChangeNotify(seat);
            SendRoomInfoChangeNotify();
            AppDebug.Log(seat.Nickname + "准备");

        }
        #endregion
        #region 开局
        /// <summary>
        /// 开局
        /// </summary>
        /// <param name="proto"></param>
        public void Begin(JY_ROOM_GAMESTART proto)
        {
            Debug.Log("开局------------proto.baseScore;------------------------" + proto.baseScore);
            CurrentRoom.roomStatus = ROOM_STATUS.GAME;
            if (proto.hasBaseScore()) CurrentRoom.baseScore = proto.baseScore;
            if (proto.hasLoop()) CurrentRoom.currentLoop = proto.loop;
            List<JY_SEAT>  seatList = proto.getSeatListList();
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i] != null && seatList[i].playerId > 0)
                {

                    SeatEntity seat = GetSeatByPlayerId(seatList[i].playerId);
                    seat.SetSeat(seatList[i]);
                    Debug.Log("开局------------seat.gold;------------------------" + seat.Gold);

                }
            }

            SendRoomInfoChangeNotify();

        }
        #endregion
        #region 发牌
        /// <summary>
        /// 发牌
        /// </summary>
        /// <param name="proto"></param>
        public void SendPoker(JY_ROOM_BEGIN proto)
        {
            
            if (proto.hasSeat() && proto.seat.hasPos())
            {
                SeatEntity seat = GetSeatBySeatId(proto.seat.pos);
                for (int i = 0; i < proto.seat.pokerListCount(); i++)
                {
                    seat.PokerList.Add(new Poker());
                }

                seat.SetSeat(proto.seat);
                SendSeatInfoChangeNotify(seat);



                //Debug.Log("proto.hasUnixtime() && seat.PokerList.Count==2" + (proto.hasUnixtime() && seat.PokerList.Count == 2));
              SetCountDown( (proto.hasUnixtime() && seat.PokerList.Count==2) ? proto.unixtime:0);
            }

        }
        #endregion
       
        #region 下注
        /// <summary>
        ///下注
        /// </summary>
        /// <param name="proto"></param>
        public void Jetton(JY_ROOM_JETTON proto)
        {
            if (!proto.hasPos()) return;
                SeatEntity seat = GetSeatBySeatId(proto.pos);
            if ( !proto.hasPour())
            {
                //通知下注
                seat.seatStatus = SEAT_STATUS.POUR;

            }
            else
            {
                seat.seatStatus = SEAT_STATUS.SETTLE;
                //下注分数
                seat.Pour = proto.pour;
                //seat.Gold = proto.gold;
                //第三张牌
                if (seat.PokerList.Count < 3)
                {
                    seat.PokerList.Add(new Poker());
                }
                seat.PokerList[2].SetPoker(proto.poker);
            }

            SendSeatInfoChangeNotify(seat);

        }
        #endregion

        #region 个人结算
        /// <summary>
        /// //个人结算
        /// </summary>
        /// <param name="proto"></param>
        public void AloneSettle(JY_ROOM_SETTLE proto)
        {
            SeatEntity seat = GetSeatBySeatId(proto.pos);
            if (seat == null) return;
            seat.seatStatus = SEAT_STATUS.WAIT;
            seat.PokerList.Clear();
            seat.Pour = 0;
            seat.Gold = proto.gold;
            CurrentRoom.baseScore = proto.baseScore;
            seat.Earnings = proto.earnings;
            SendRoomInfoChangeNotify();
           
        }
        #endregion
  
        #region  弃牌
        /// <summary>
        ///  弃牌
        /// </summary>
        /// <param name="proto"></param>
        public void GiveupPoker(JY_ROOM_GIVEUPPOKER proto)
        {
            if (proto.hasPos())
            {
                SeatEntity seat = GetSeatBySeatId(proto.pos);
                if (seat != null) seat.PokerList.Clear();
                seat.seatStatus = SEAT_STATUS.WAIT;
                seat.Pour = 0;
                SendSeatInfoChangeNotify(seat);
            }
            SetCountDown(0);
        }
        #endregion

        #region 每局结算
        /// <summary>
        /// 每局结算
        /// </summary>
        /// <param name="proto"></param>
        public void OnServerResult(JY_ROOM_LOOPENDSETTLE proto)
        {
         
            CurrentRoom.SetRoom(proto.room);
            SendRoomInfoChangeNotify();
        }
        #endregion

        #region 开始下一把
        /// <summary>
        /// 开始下一把
        /// </summary>
        public void OnServerNextGame(JY_ROOM_NEXT proto)
        {
            CurrentRoom.roomStatus = ROOM_STATUS.IDLE;
            CurrentRoom.baseScore = 0;
            if (!proto.hasBankerPos()) return;
            SeatEntity seat = GetSeatBySeatId(proto.bankerPos);
            if (seat == null) return;
            //切换庄
            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {
                if (CurrentRoom.SeatList[i] != null && CurrentRoom.SeatList[i].PlayerId > 0)
                {
                    CurrentRoom.SeatList[i].seatStatus = SEAT_STATUS.IDLE;
                    CurrentRoom.SeatList[i].IsBanker = false;
                }
            }

            seat.IsBanker = true;
       

            //UI刷新
            SendRoomInfoChangeNotify();
        }
        #endregion

        #region   换庄
        /// <summary>
        /// 换庄
        /// </summary>
        /// <param name="proto"></param>
        public void OnServerChooseBanker(JY_ROOM_INFORMBANKER proto)
        {
            if (!proto.hasPos()) return;
            SeatEntity seat = GetSeatBySeatId(proto.pos);
            if (seat == null) return;
            //切换庄
            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {
                if (CurrentRoom.SeatList[i] != null && CurrentRoom.SeatList[i].PlayerId > 0)
                {
                    CurrentRoom.SeatList[i].IsBanker = false;
                }
            }

            seat.IsBanker = true;
            BankerSeat = seat;
            //UI刷新
            SendRoomInfoChangeNotify();

        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="proto"></param>
        public void OnServerResult(JY_ROOM_RESULT proto)
        {
            CurrentRoom.SetRoom(proto.room);
            //UI刷新
            SendRoomInfoChangeNotify();

            //SendNotification(ConstDefine_JuYou.ObKey_GameOver,null);

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
        public void SendRoomInfoChangeNotify()
        {
            TransferData roomData = new TransferData();
            roomData.SetValue("Room", CurrentRoom);
            SendNotification(ConstDefine_JuYou.ObKey_RoomInfoChanged, roomData);

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
            data.SetValue("CurrentRoom", CurrentRoom);
            data.SetValue("RoomStatus", CurrentRoom.roomStatus);                     // <<------------房间状态---------------------
            data.SetValue("BankerSeat", BankerSeat);
            SendNotification(ConstDefine_JuYou.ObKey_SeatInfoChanged, data);
        }
        #endregion


        /// <summary>
        /// 发送座位gold变更
        /// </summary>
        /// <param name="seat"></param>
        public void SendSeatGoldChanged(SeatEntity seat)
        {
            TransferData data = new TransferData();
            data.SetValue("Seat", seat);
            SendNotification(ConstDefine_JuYou.ObKey_SeatGoldChanged, data);
        }

        /// <summary>
        /// 发送房间底注变更
        /// </summary>
        public void SendRoomGoldChanged()
        {
            //ModelDispatcher.Instance.RemoveEventListener(ConstDefine_JuYou.ObKey_RoomGoldChanged, OnRoomGoldChanged);//房间底注
            TransferData data = new TransferData();
            data.SetValue("BaseScore", CurrentRoom.baseScore);
            SendNotification(ConstDefine_JuYou.ObKey_RoomGoldChanged, data);
        }



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
}