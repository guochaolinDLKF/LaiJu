//===================================================
//Author      : WZQ
//CreateTime  ：4/27/2017 1:53:29 PM
//Description ：
//===================================================
using System.Collections.Generic;
//using com.oegame.mahjong.protobuf;
//using com.oegame.niuniu.protobuf;
//using DRB.MahJong;
//using NiuNiu;
using niuniu.proto;
using UnityEngine;

namespace NiuNiu
{
    public class RoomNiuNiuProxy : ProxyBase<RoomNiuNiuProxy>
    {
        
        public NiuNiu.Room CurrentRoom; //当前房间
        public NiuNiu.Seat PlayerSeat;  //玩家自身座位
        public NiuNiu.Seat BankerSeat = null;  //庄座位

        
        public int playerNumber = 0;   //当前房间人数
        public int playerSeatIndex;    //玩家自己位置  在服务器的索引（1--6）
        public int agreeDissolveCount = 0;//当前同意解散人数
        public const int ROOM_SEAT_COUNT = 6;//房间座位数量
        public const int ROOM_POKERLIST_COUNT = 5;




        #region CalculateSeatIndex 计算座位的客户端序号
        /// <summary>
        /// 计算座位的客户端序号
        /// </summary>
        /// <param name="room">房间信息</param>
        private void CalculateSeatIndex(NiuNiu.Room room)
        {
            PlayerSeat = null;
            if (room == null) return;
            for (int i = 0; i < room.SeatList.Count; ++i)
            {
                if (room.SeatList[i].PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
                {
                    PlayerSeat = room.SeatList[i];
                    for (int j = 0; j < room.SeatCount; ++j)
                    {
                        NiuNiu.Seat seat = room.SeatList[j];
                        int seatIndex = seat.Pos - PlayerSeat.Pos;
                        seatIndex = seatIndex < 0 ? seatIndex + ROOM_SEAT_COUNT : seatIndex;
                        seat.Index = seatIndex;
                    }
                    break;
                }
            }
            if (PlayerSeat == null)
            {
                PlayerSeat = room.SeatList[0];
                for (int j = 0; j < room.SeatCount; ++j)
                {
                    NiuNiu.Seat seat = room.SeatList[j];
                    int seatIndex = seat.Pos - PlayerSeat.Pos;
                    seatIndex = seatIndex < 0 ? seatIndex + ROOM_SEAT_COUNT : seatIndex;
                    seat.Index = seatIndex;
                }
            }
        }
        #endregion



        #region EnterRoom  新玩家进入房间
        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="pbSeat"></param>
        public void EnterRoom(NN_ROOM_ENTER proto)
        {
            NiuNiu.Seat seat = GetSeatBySeatPos(proto.pos);
            if (seat == null) Debug.Log("新玩家进入房间,找不到对应座位");
            if (seat == null) return;

           
            seat.Avatar = proto.avatar;
            seat.PlayerId = proto.playerId;
            seat.IsReady = CurrentRoom.currentLoop == 0 ? false : true;
            seat.Nickname = proto.nickname;
            seat.Gender = proto.gender;
            seat.Gold = proto.gold;
            seat.IsBanker = proto.isBanker;
           if(proto.hasIsHomeowners()) seat.IsHomeowners = proto.IsHomeowners;
            seat.Pos = proto.pos;

            //纬度
            if (proto.hasLatitude())
            {
               seat.Latitude = proto.latitude;
            }
            //经度
            if (proto.hasLongitude())
            {
               seat.Longitude = proto.longitude;
            }

            Debug.Log(string.Format("新加入的玩家名字：{0} 服务器发送{1} ", seat.Nickname, proto.nickname));

            AppDebug.Log(seat.Nickname + "进入房间");

            PeopleCounting();//计算人数

            //显示UI
            //TransferData data = new TransferData();
            //data.SetValue("Seat", seat);
            //data.SetValue("RoomStatus", CurrentRoom.roomStatus);
            //SendNotification("AddJoinItemUI", data);

            //    SetInteractionOnOff();//设置交互显影

            SendRoomInfoChangeNotify();


        }
        #endregion





        #region 计算人数  PeopleCounting
        //计算人数
        void PeopleCounting()
        {
            //设置人数
            playerNumber = 0;
            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {
                if (CurrentRoom.SeatList[i] != null && CurrentRoom.SeatList[i].PlayerId > 0)
                {
                    //Debug.Log("6个座位ID分别为" + CurrentRoom.SeatList[i].PlayerId);
                    playerNumber++;

                }
            }
            Debug.Log("由ID计算的当前房间玩家个数：" + playerNumber);

        }
        #endregion


        #region ExitRoom 离开房间
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="pbSeat"></param>
        public void ExitRoom(NN_ROOM_LEAVE proto)
        {
            NiuNiu.Seat seat = GetSeatByPlayerId(proto.playerId);

            if (seat == null) return;

            //PBSeat.Builder pbSeatNew = PBSeat.CreateBuilder();
            //pbSeatNew.PlayerId = 0;
            //seat.SetSeat(pbSeatNew.Build());

            seat.PlayerId = 0;


            PeopleCounting();//计算人数
            Debug.Log(string.Format("{0}号位 离开房间", seat.Pos));
          

            if (seat == PlayerSeat)
            {
                NetWorkSocket.Instance.SafeClose(GameCtrl.Instance.SocketHandle);
                SceneMgr.Instance.LoadScene(SceneType.Main);
                return;
            }


            ////显示UI
            //TransferData data = new TransferData();
            //data.SetValue("Seat", seat);
            //data.SetValue("RoomStatus", CurrentRoom.roomStatus);
            //SendNotification("SetLeaveItemUI", data);

            //SetInteractionOnOff();
            SendRoomInfoChangeNotify();
            //SendSeatInfoChangeNotify(seat);

        }
        #endregion

        /// <summary>
        /// 初始化房间信息  构建房间
        /// </summary>
        /// <param name="proto"></param>
        public void InitCurrentRoom(NN_ROOM_CREATE proto)
        {
            BankerSeat = null;

            CurrentRoom = new NiuNiu.Room();

            //收到数据存到模型基类                                 <------------------------------<保存配置>----------------------------------------
            CurrentRoom.Config.Clear();
           
            for (int i = 0; i < proto.nn_room.settingIdCount(); ++i)
            {
                cfg_settingEntity settingEntity = cfg_settingDBModel.Instance.Get(proto.nn_room.getSettingId(i));
                
                if (settingEntity != null)
                {
                    CurrentRoom.Config.Add(settingEntity);
                }
            }

            //获得当前游戏模式
            for (int i = 0; i < CurrentRoom.Config.Count; i++)
            {
                if (CurrentRoom.Config[i].tags.Equals("mode"))
                {
                    CurrentRoom.roomModel = (Room.RoomModel)CurrentRoom.Config[i].value;
                    Debug.Log("服务器发送的房间 庄模式为为：" + CurrentRoom.Config[i].value);
//#if IS_GUGENG
//                    if (CurrentRoom.roomModel == Room.RoomModel.EveryTime || CurrentRoom.roomModel == Room.RoomModel.WinnerByBanker)
//                        CurrentRoom.roomModel = Room.RoomModel.AutoBanker;
//#endif
                }
            }
            for (int i = 0; i < CurrentRoom.Config.Count; i++)
            {
                if (CurrentRoom.Config[i].tags.Equals("roomMode"))
                {
                    CurrentRoom.superModel = (Room.SuperModel)CurrentRoom.Config[i].value;
                    Debug.Log("服务器发送的房间 普通高级场模式为为：" + CurrentRoom.Config[i].value);
                }
            }



            Debug.Log("房间ID为：" + proto.nn_room.roomId);
            Debug.Log("服务器发送的房间座位长度为：" + proto.nn_room.getNnSeatList().Count);
            CurrentRoom.roomId = proto.nn_room.roomId;
           
            CurrentRoom.roomStatus = proto.nn_room.nn_room_status;
            CurrentRoom.currentLoop = proto.nn_room.loop;
            CurrentRoom.maxLoop = proto.nn_room.maxLoop;


            
            CurrentRoom.SeatCount = proto.nn_room.getNnSeatList().Count;

            //CurrentRoom.SeatCount = proto.nn_room.nnSeatCount;//----------------------------------《座位长度《-----------------------------------------
            CurrentRoom.SeatList = new List<NiuNiu.Seat>();

            for (int i = 0; i < CurrentRoom.SeatCount/*proto.nn_room.getNnSeatList().Count*/; ++i)
            {
                NN_SEAT nn_seat = proto.nn_room.getNnSeat(i);
                NiuNiu.Seat seat = new NiuNiu.Seat();
                //NiuNiu.Seat seat = new NiuNiu.Seat(proto.nn_room.seatList[i]);


                //房主
                seat.IsHomeowners = nn_seat.IsHomeowners;//----------------------------------《房主《-----------------------------------------

                //是否同意解散
                if (nn_seat.hasDissolve())
                {
                    seat.Dissolve = nn_seat.dissolve;
                }

                //庄
                seat.IsBanker = nn_seat.isBanker;
                //玩家ID
                seat.PlayerId = nn_seat.playerId;
                //pos
                seat.Pos = nn_seat.pos;
                //昵称
                seat.Nickname = nn_seat.nickname;
                //头像
                seat.Avatar = nn_seat.avatar;
                //性别
                seat.Gender = nn_seat.gender;
                //是否准备
                seat.IsReady = nn_seat.ready;

                //已有金币
                seat.Gold = nn_seat.gold;
                //本局收益
                seat.Earnings = nn_seat.nn_earnings;
                //是否是胜利者
                seat.Winner = nn_seat.isWiner;
                //下注
                seat.Pour = nn_seat.pour;

                //纬度
                if (nn_seat.hasLatitude())  seat.Latitude = nn_seat.latitude;
                
                //经度
                if (nn_seat.hasLongitude()) seat.Longitude = nn_seat.longitude;
                

                //手牌类型
                if (nn_seat.hasNnPokerType())
                {

                    if (nn_seat.nn_pokerType != 0)
                    {
                        seat.PockeType = (int)nn_seat.nn_pokerType;
                    }
                }

                seat.PokerList = new List<Poker>();
                //具体手牌
                //if (nn_seat.getNnPokerList() != null && nn_seat.getNnPokerList().Count > 0)
                //{
                for (int j = 0; j < 5; j++)//nn_seat.nnPokerCount()
                {

                    NN_POKER protoPoker = null;
                    if (nn_seat.nnPokerCount() > j)
                    {
                        protoPoker= nn_seat.getNnPoker(j);
                    }
                  
                    NiuNiu.Poker poker = new NiuNiu.Poker();
                    if (protoPoker != null && protoPoker.hasIndex() && protoPoker.index != 0)
                    {
                        //seat.PokerList[j].SetPoker(nn_seat.PokerList[j]);

                        poker.color = protoPoker.color;
                        poker.index = protoPoker.index;
                        poker.size = protoPoker.size;
                        poker.status = protoPoker.pokerStatus;
                    }
                    seat.PokerList.Add(poker);

                }

                //}

                if (seat.IsBanker) BankerSeat = seat;//庄座位
                if (proto.nn_room.pos== seat.Pos) CurrentRoom.RobBankerSeat = seat; //获得当前抢庄座位

                CurrentRoom.SeatList.Add(seat);

            }

            
            


            if (proto.nn_room.hasUnixtime())
            {
                CurrentRoom.serverTime = proto.nn_room.unixtime;//（时间戳）
            }


            CalculateSeatIndex(CurrentRoom);

            playerSeatIndex = PlayerSeat.Pos;
            PeopleCounting();

            if (CurrentRoom.roomModel == Room.RoomModel.robBanker && BankerSeat != null) PlayerSeat.isAlreadyHOG = -1;
           
            if (CurrentRoom.roomStatus == NN_ENUM_ROOM_STATUS.DISSOLVE)
            {
                //计算当前同意解散人数
                ADHEnterRoom(proto.nn_room.getNnSeatList());

            }

        }






#region Ready 准备
        /// <summary>
        /// 准备
        /// </summary>
        /// <param name="pbSeat"></param>
        public void Ready(NN_SEAT_READY proto)
        {
            NiuNiu.Seat seat = GetSeatBySeatPos(proto.nn_seat.pos);
            if (seat == null) return;
            seat.IsReady = true;

             //--------------设置开始按钮遮罩---------------
            bool isEnable = false;
            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {
                NiuNiu.Seat readySeat = GetSeatBySeatIndex(i);

                if (readySeat != null && readySeat.PlayerId > 0)
                {
                    Debug.Log(string.Format("座位{0} 玩家是否准备{1}", readySeat.Pos, readySeat.IsReady));
                    if (readySeat.IsReady == false)
                    {
                        isEnable = true;
                        break;
                    }
                }

            }
            Debug.Log("bool isEnable:" + isEnable);
            Debug.Log("bool isEnable设置开始按钮遮罩");
            TransferData dataMask = new TransferData();
            dataMask.SetValue<bool>("OnOff", isEnable);
            ModelDispatcher.Instance.Dispatch(ConstDefine_NiuNiu.ObKey_EnableAllowStartBtn, dataMask);//设置开始游戏按钮遮罩

            //刷新座位UI
            SendRoomInfoChangeNotify();
        }
#endregion

#region StartRobBanker 服务器广播抢庄
        /// <summary>
        /// 服务器广播抢庄
        /// </summary>
        /// <param name="obj"></param>
        public void StartRobBanker(NN_ROOM_HOG proto)
        {
            //通知抢庄
            if (proto.hasUnixtime())
            {
                if (CurrentRoom.roomModel == Room.RoomModel.robBanker)
                {
                    for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
                    {
                        CurrentRoom.SeatList[i].IsBanker = false;
                    }
                }


                for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
                {
                    CurrentRoom.SeatList[i].isAlreadyHOG = 0;
                }

                CurrentRoom.roomStatus = NN_ENUM_ROOM_STATUS.HOG;
                CurrentRoom.RobBankerSeat = null;
                CurrentRoom.serverTime = proto.unixtime;
                SetCountDown();


                


            }
            else
            {
              Seat seat=  GetSeatBySeatPos(proto.pos);
                if (seat != null)
                {
                  if(proto.hasRobZhuang())  seat.isAlreadyHOG = proto.rob_zhuang;
                    CurrentRoom.RobBankerSeat = seat;
                }

            }


            SendRoomInfoChangeNotify();


        }
#endregion
         



#region StartRobBanker 服务器广播庄归属

        public void RobBankerWin(NN_ROOM_HOG_SUCCEED proto, bool isDelay = false)
        {

            if (proto.hasPos())
            {

                if (CurrentRoom.roomModel == Room.RoomModel.robBanker && CurrentRoom.roomStatus == NN_ENUM_ROOM_STATUS.HOG)
                {
                   
                    //设置抢庄动画
                    TransferData data = new TransferData();
                    data.SetValue("CurrentRoom", CurrentRoom);
                    data.SetValue("isOnOff", true);
                    SendNotification(ConstDefine_NiuNiu.ObKey_SetRobBankerAni, data);
                }


               

            
                if (CurrentRoom.roomModel == Room.RoomModel.AbdicateBanker)
                {

                     CurrentRoom.roomStatus = NN_ENUM_ROOM_STATUS.IDLE;
                }

                for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
                {
                    CurrentRoom.SeatList[i].IsBanker = false;
                    if(CurrentRoom.roomModel != Room.RoomModel.robBanker) CurrentRoom.SeatList[i].isAlreadyHOG = -1;
                }

                NiuNiu.Seat seat = GetSeatBySeatPos(proto.pos);
                if (seat == null)
                {
                    for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
                    {
                        if (CurrentRoom.SeatList[i].PlayerId > 0) seat = CurrentRoom.SeatList[i];

                    }
                }
                seat.IsBanker = true;
                seat.IsReady = true;
                BankerSeat = seat;
                CurrentRoom.RobBankerSeat = null;
                SendRoomInfoChangeNotify();
              
            }

        }

#endregion


#region  处理开始下注数据 
        /// <summary>
        ///   服务器广播所有玩家开始下注
        /// </summary>
        /// <param name="obj"></param>
        public void StartPour(NN_ROOM_JETTON proto)
        {

          if(CurrentRoom.superModel == Room.SuperModel.CommonRoom)   CurrentRoom.currentLoop++;

            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {
                CurrentRoom.SeatList[i].isAlreadyHOG = 0;
            }

            if (CurrentRoom.roomModel == Room.RoomModel.robBanker)
            {
                TransferData data = new TransferData();
                data.SetValue("CurrentRoom", CurrentRoom);
                data.SetValue("isOnOff", false);
                SendNotification(ConstDefine_NiuNiu.ObKey_SetRobBankerAni, data);
            }

            CurrentRoom.roomStatus = NN_ENUM_ROOM_STATUS.POUR;

            Debug.Log("通知开始下注，服务器发送时间：" + proto.unixtime);
            if (proto.hasUnixtime())
            {
                CurrentRoom.serverTime = proto.unixtime;
                SetCountDown();
            }

            SendRoomInfoChangeNotify();
        }
#endregion


#region   处理开始某玩家下注某分数据  
        /// <summary>
        ///   服务器广播  单个玩家下注分数
        /// </summary>
        /// <param name="obj"></param>
        public void OneBetScore(NN_ROOM_SB_JETTON proto)
        {

            if (proto.hasPour())
            {
                Debug.Log("应该显示的下注分" + proto.pour);
                NiuNiu.Seat seat = GetSeatBySeatPos(proto.pos);
                if (seat != null) seat.Pour = proto.pour;

                if (CurrentRoom.superModel == Room.SuperModel.PassionRoom)
                {
                    bool isAllPour = true;//是否全部下完注

                    for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
                    {
                        if (CurrentRoom.SeatList[i].PlayerId > 0 && !CurrentRoom.SeatList[i].IsBanker && CurrentRoom.SeatList[i].Pour <= 0)
                        {
                            isAllPour = false;
                        }
                    }

                    if (isAllPour)
                    {
                        CurrentRoom.roomStatus = NN_ENUM_ROOM_STATUS.LOOKPOCKER;
                        SendSeatInfoChangeNotify(PlayerSeat);
                       
                    }
                }

                ////交互显影设置
                //SetInteractionOnOff();
                SendSeatInfoChangeNotify(seat);

            }


        }
#endregion


#region  发牌数据处理
        public void Deal(NN_ROOM_BEGIN proto)
        {

            if (CurrentRoom.superModel == Room.SuperModel.PassionRoom) CurrentRoom.currentLoop++;

            CurrentRoom.roomStatus = CurrentRoom.superModel == Room.SuperModel.CommonRoom ? NN_ENUM_ROOM_STATUS.LOOKPOCKER : NN_ENUM_ROOM_STATUS.DEAL;

            if (proto.hasUnixtime())
            {
                CurrentRoom.serverTime = proto.unixtime;
                SetCountDown();
            }


            List<NN_SEAT> ptSeatLiat = proto.getNnSeatList();


            Debug.Log("拍数据");
            for (int i = 0; i < ptSeatLiat.Count; i++)
            {
                NiuNiu.Seat seat = GetSeatBySeatPos(ptSeatLiat[i].pos);

                if (seat != null)
                {
                //牌数据
                    for (int j = 0; j < ptSeatLiat[i].getNnPokerList().Count; j++)
                    {
                        Debug.Log("index" + ptSeatLiat[i].getNnPoker(j).index + "size" + ptSeatLiat[i].getNnPoker(j).size + "color" + ptSeatLiat[i].getNnPoker(j).color + "pokerStatus" + ptSeatLiat[i].getNnPoker(j).pokerStatus);
                      
                        seat.PokerList[j].SetPoker(ptSeatLiat[i].getNnPokerList()[j]);

                    }

                    //下注分
                    if (ptSeatLiat[i].hasPour()) seat.Pour = ptSeatLiat[i].pour;

                }


            }

            TransferData data = new TransferData();
            data.SetValue<NiuNiu.Room>("Room", CurrentRoom);
            SendNotification(ConstDefine_NiuNiu.ObKey_SetDeal, data);//发牌显示


           //房间变更
            SendRoomInfoChangeNotify();
   
        }


#endregion



#region //某玩家开某张牌
        public void OpenAPoker(NN_ROOM_DRAW proto)
        {
            if (proto.hasPos())
            {

                NiuNiu.Seat seat = GetSeatBySeatPos(proto.pos);
                if (seat == null) return;

                List<NN_POKER> prPokerList = proto.getNnPokerList();
               

                //便利应该开的牌
                for (int i = 0; i < prPokerList.Count; i++)
                {

                    //找到对应牌
                    for (int k = 0; k < seat.PokerList.Count; k++)
                    {
                        if (prPokerList[i].index > 0 && prPokerList[i].index == seat.PokerList[k].index)
                        {
                           
                            seat.PokerList[k].SetPoker(prPokerList[i]);
                            seat.PokerList[k].status = NN_ENUM_POKER_STATUS.POKER_STATUS_UPWARD;

                         }
                    }

                }
                SendSeatInfoChangeNotify(seat);

                //UI翻牌
                //显示这些牌
                TransferData data = new TransferData();
                data.SetValue<NiuNiu.Seat>("Seat", seat);
                SendNotification(ConstDefine_NiuNiu.ObKey_SetShowPokersUI , data);//设置某玩家手牌                 
  
            }

        }

#endregion



#region RubPoker 玩家搓牌
        public void RubPoker(NN_ROOM_ROB_POKER proto)
        {
            
            if (proto.hasNnPoker())
            {
                for (int i = 0; i < PlayerSeat.PokerList.Count; i++)
                {
                    if (PlayerSeat.PokerList[i].index == proto.nn_poker.index)
                    {
                        PlayerSeat.PokerList[i].SetPoker(proto.nn_poker);
                    }
                }
                Debug.Log(string.Format("------------------------------------- 玩家搓牌  {0}   {1}    {2}", proto.nn_poker.index, proto.nn_poker.color, proto.nn_poker.size));
            }

        }
#endregion











#region 处理开牌结算数据
        //处理开牌结算数据
        public void RoomOpenPokerSettle(NN_ROOM_CHECK proto)
        {

            //同步数据
            CurrentRoom.SetRoom(proto.nn_room);
            CurrentRoom.roomStatus = NN_ENUM_ROOM_STATUS.RSETTLE;


            //Debug.Log(string.Format("-----------------开牌结算------------------------------------------------ "));
            //for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            //{
            //    if (CurrentRoom.SeatList[i].PlayerId > 0)
            //    {
            //        if (CurrentRoom.SeatList[i].PokerList[0].index > 0)
            //        {
            //            for (int j = 0; j < CurrentRoom.SeatList[i].PokerList.Count; j++)
            //            {
            //                Poker poker = CurrentRoom.SeatList[i].PokerList[j];
            //                Debug.Log(string.Format("------------------------------------- 玩家手牌  {0}   {1}    {2}", poker.index, poker.color, poker.size));
            //            }

            //        }

            //    }
            //}


            //SetCountDown();

            //处理小结算SceneView
            TransferData data = new TransferData();
            data.SetValue<NiuNiu.Room>("CurrentRoom", CurrentRoom);
            SendNotification(ConstDefine_NiuNiu.ObKey_RoomOpenPokerSettle, data);

            SendRoomInfoChangeNotify();
        }

#endregion


#region 处理开始下一局数据
        /// <summary>
        /// 服务器广播开始下一局
        /// </summary>
        /// <param name="obj"></param>
        public void NextGame(NN_ROOM_NEXT_GAME proto)
        {
            //清空手牌 牌型  下注 
            CurrentRoom.roomStatus = NN_ENUM_ROOM_STATUS.IDLE;

            Poker poker = new NiuNiu.Poker();
            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {
                CurrentRoom.SeatList[i].PockeType = 0;
                CurrentRoom.SeatList[i].Pour = 0;


                for (int j = 0; j < CurrentRoom.SeatList[i].PokerList.Count; j++)
                {
                    CurrentRoom.SeatList[i].PokerList[j].SetPoker(poker);
                    
                }

            }

            SendRoomInfoChangeNotify();
            if (proto.hasBeginTime())
            {
                CurrentRoom.serverTime = proto.beginTime;
                SetCountDown();
            }
            //处理Scene
            TransferData data = new TransferData();
            data.SetValue<NiuNiu.Room>("CurrentRoom", CurrentRoom);
            SendNotification(ConstDefine_NiuNiu.ObKey_SetNextGameUISceneView, data);
        }
#endregion

#region 处理牌局全结束
        /// <summary>
        /// 处理牌局全结束
        /// </summary>
        public void GameOver()
        {
        
            TransferData data = new TransferData();
            data.SetValue<NiuNiu.Room>("CurrentRoom", CurrentRoom);
            SendNotification(ConstDefine_NiuNiu.ObKey_SetGameOverUISceneView, data);

        }
#endregion





#region 解散相关数据处理
        //处理申请解散消息内容
        public void SetApplicationDissolution(NN_ROOM_ASK_DISMISS proto)
        {

            Debug.Log("有人申请解散房间 服务器发送：是否有倒计时" + proto.hasUnixtime());
            if (proto.hasUnixtime())
            {
                CurrentRoom.serverTime = proto.unixtime;

            }
            if (agreeDissolveCount == 0)
            {
                agreeDissolveCount = 1;
            }
            CurrentRoom.roomStatus = NN_ENUM_ROOM_STATUS.DISSOLVE;


        }




        //处理 服务器回复当前同意解散房间人数
        public void AgreeDissolveCount(NN_ROOM_ANSWER_TO_DISMISS proto)
        {

            //------------------------------------SEAT列表-----------------------------------------------------------------

            agreeDissolveCount = proto.getNnSeatList().Count;

            TransferData data = new TransferData();
            data.SetValue<int>("SetADHWindowSum", agreeDissolveCount);
            SendNotification(ConstDefine_NiuNiu.ObKey_SetADHWindowSum, data);//刷新当前同意人数

        }

        //初始计算当前同意解散房间人数
        private void ADHEnterRoom(List<NN_SEAT> nn_seatList)
        {

            agreeDissolveCount = 0;

            int sum = 0;
            for (int i = 0; i < nn_seatList.Count; i++)
            {
                if (nn_seatList[i].hasDissolve())
                {
                    if ((int)nn_seatList[i].dissolve == 1)
                    {
                        sum++;
                    }

                }

            }
            agreeDissolveCount = sum;

        }


        /// <summary>
        /// 服务器广播解散结果
        /// </summary>
        public void DissolveResults()
        {
            agreeDissolveCount = 0;
            CurrentRoom.roomStatus = NN_ENUM_ROOM_STATUS.IDLE;

            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {

                CurrentRoom.SeatList[i].Dissolve = 0;
            }
            SendRoomInfoChangeNotify();
            //SetInteractionOnOff();
        }




#endregion




#region 获取座位

        /// <summary>
        /// 根据玩家ID获取座位
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public NiuNiu.Seat GetSeatByPlayerId(int playerId)
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
        /// 根据Pos获取座位
        /// </summary>
        /// <param name="seatPos"></param>
        /// <returns></returns>
        public NiuNiu.Seat GetSeatBySeatPos(int seatPos)
        {
            if (CurrentRoom == null) return null;
            for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
            {
                if (CurrentRoom.SeatList[i].Pos == seatPos)
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
        public NiuNiu.Seat GetSeatBySeatIndex(int seatIndex)
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

        /// <summary>
        /// 根据当前游戏状态设置 交互显影（计时器 选分按钮 庄家开始按钮）
        /// </summary>
        void SetInteractionOnOff()
        {
           
            TransferData data = new TransferData();
            data.SetValue("Room", CurrentRoom);
            data.SetValue("PlayerSeat", PlayerSeat);
            data.SetValue("playerNumber", playerNumber);

            SendNotification(ConstDefine_NiuNiu.ObKey_SetRoomInteraction, data);//设置交互显影

        }

        //RoomMaJiangProxy
#region SendRoomInfoChangeNotify 发送房间信息变更消息
    /// <summary>
    /// 发送房间信息变更消息
    /// </summary>
    public void SendRoomInfoChangeNotify()
        {
            TransferData roomData = new TransferData();
            roomData.SetValue("Room", CurrentRoom);
            SendNotification(ConstDefine_NiuNiu.ObKey_RoomInfoChanged, roomData);

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
        private void SendSeatInfoChangeNotify(NiuNiu.Seat seat)
        {
            if (seat == null) return;

            TransferData data = new TransferData();
            data.SetValue("Seat", seat);
            data.SetValue("IsPlayer", seat == PlayerSeat);
            data.SetValue("CurrentRoom", CurrentRoom);
            data.SetValue("RoomStatus", CurrentRoom.roomStatus);
            data.SetValue("BankerSeat", BankerSeat);
            data.SetValue("playerNumber", playerNumber);
            SendNotification(ConstDefine_NiuNiu.ObKey_SeatInfoChanged, data);
        }
#endregion

       
#region SetCountDown 设置倒计时
        /// <summary> 
        /// 设置倒计时
        /// </summary>
        /// <param name="countDown"></param>
        public void SetCountDown(/*long countDown*/)
        {
            TransferData data = new TransferData();
            //data.SetValue("ServerTime", countDown);
            data.SetValue("CurrentRoom", CurrentRoom);
            SendNotification(ConstDefine_NiuNiu.ObKey_OnCountDownUpdate, data);
        }
#endregion

    }
}