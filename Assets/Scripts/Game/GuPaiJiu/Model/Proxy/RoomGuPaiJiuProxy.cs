//===================================================
//Author      : CZH
//CreateTime  ：9/1/2017 3:02:53 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using proto.gp;
using UnityEngine;
using GuPaiJiu;

public class RoomGuPaiJiuProxy : ProxyBase<RoomGuPaiJiuProxy>
{
    public RoomEntity CurrentRoom;//当前房间 

    public SeatEntity PlayerSeat;//当前座位

    public const int ROOM_SEAT_COUNT = 4;//房间座位数量

    public  bool isBet;//是否是固定分

    public RoomEntity CurrentOperator { get; private set; } //目前运营商


    #region 初始化房间数据
    internal void InitRoom(GP_ROOM_CREATE proto)
    {
        CurrentRoom = new RoomEntity()
        {
            roomId = proto.room.roomId,//房间ID
            currentLoop = proto.room.loop,//局数
            maxLoop = proto.room.maxLoop,//总局数                                                      
            roomStatus = proto.room.status, //房间状态   
            FirstDice = proto.room.firstDice,//第一个骰子
            SecondDice = proto.room.secondDice,//第二个骰子
            FirstGivePos = proto.room.firstGivePos,//首发牌座位号
            TotalPokerNum = proto.room.totalPokerNum,//房间牌的总数量
            RemainPokerNum = proto.room.remainPokerNum,//房间剩余牌的数量
            PanBase = proto.room.panBase,//锅底
            IsAddPanBase = proto.room.isAddPanBase,//是否加锅底  
            dealSecond = proto.room.dealTime, //每局第几次  
            fzName=proto.room.ownerNickName,
            //PlayerID = 111,
            isGrabBankerNum = 0,//抢庄的人数                           
        };        
        CurrentRoom.roomPokerList = new List<Poker>();
        for (int i = 0; i < proto.room.historyPokerListCount(); i++)
        {
            GP_POKER protoPoker = proto.room.getHistoryPokerList(i);
            CurrentRoom.roomPokerList.Add(new Poker()
            {
                Index = protoPoker.index,//索引
                Type = protoPoker.type,//花色
                Size = protoPoker.size,//大小  
            });
        }
        CurrentRoom.Config.Clear();
        Debug.Log(proto.room.settingIdCount()+"                   Stting长度");
        for (int i = 0; i < proto.room.settingIdCount(); ++i)
        {
            cfg_settingEntity settingEntity = cfg_settingDBModel.Instance.Get(proto.room.getSettingId(i));
            if (settingEntity != null)
            {
                CurrentRoom.Config.Add(settingEntity);

                if (settingEntity.tags.Equals("isGuiZi"))
                {
                    CurrentRoom.enumGuiZi = (EnumGuiZi)settingEntity.value;
                }
                if (settingEntity.tags.Equals("isTianJiuWang"))
                {
                    CurrentRoom.enumTianJiuWang = (EnumTianJiuWang)settingEntity.value;
                }
                if (settingEntity.tags.Equals("isDiJiuWang"))
                {
                    CurrentRoom.enumDiJiuWang = (EnumDiJiuWang)settingEntity.value;
                }
                if (settingEntity.tags.Equals("banker"))
                {
                    CurrentRoom.roomMode = (RoomEntity.RoomMode)settingEntity.value;
                }
                if (settingEntity.tags.Equals("pourModel"))
                {
                    CurrentRoom.betModel = (RoomEntity.BetModel)settingEntity.value;
                    if (settingEntity.value == 1) isBet = true;
                    else isBet = false;
                }
                if (settingEntity.tags.Equals("limit"))
                {
                    CurrentRoom.scoreLimit = settingEntity.value;                  
                }
                if (settingEntity.tags.Equals("defaultScore"))
                {
                    CurrentRoom.guDScore = settingEntity.value;
                }
                if (settingEntity.tags.Equals("isBig"))
                {
                    CurrentRoom.roomPlay = (EnumPlay)settingEntity.value;
                }
            }
        }
        CurrentRoom.seatList = new List<SeatEntity>();      
        for (int i = 0; i < proto.room.seatListCount(); i++)
        {
            GP_SEAT op_seat = proto.room.getSeatList(i);
            SeatEntity seat = new SeatEntity();
            seat.PlayerId = op_seat.playerId;//玩家ID            
            seat.Nickname = op_seat.nickname;//玩家名字            
            seat.Avatar = op_seat.avatar;//玩家头像
            seat.Gender = op_seat.gender;//玩家性别            
            seat.Gold = op_seat.gold;//底分                     
            seat.Pos = op_seat.pos;//座位位置                                        
            seat.seatStatus = op_seat.status; ////座位状态             
            seat.IsBanker = op_seat.isBanker;//是否是庄家
            seat.isDismiss = op_seat.isDismiss;//是否同意解散房间
            seat.isWin = op_seat.isWin;//是否是赢家
            seat.eamings = op_seat.earnings;//本次收益
            seat.loopEamings = op_seat.loopEarnings;//每局收益  
            seat.IP = op_seat.ipAddr;//IP
            seat.Longitude = op_seat.longitude;//经度
            seat.Latitude = op_seat.latitude;//维度
            seat.IsFocus = !op_seat.isAfk;//是否在线          
            seat.firstPour = op_seat.getPourList(0);//一道
            seat.secondPour = op_seat.getPourList(1);//二道  
            seat.threePour = op_seat.getPourList(2);//三道
            seat.pokerList = new List<Poker>();                        
            for (int j = 0; j < op_seat.pokerListCount(); ++j)
            {
                GP_POKER protoPoker = op_seat.getPokerList(i);
                seat.pokerList.Add(new Poker()
                {
                    Index = protoPoker.index,//索引
                    Type = protoPoker.type,//花色
                    Size = protoPoker.size,//大小                                     
                });
            }
            CurrentRoom.seatList.Add(seat);
        }
        CalculateSeatIndexOne();  //普通场计算座位 Index    
       
    }
    #endregion

    #region 断线重连
    internal void InitRoom(GP_ROOM_RECREATE proto)
    {
        CurrentRoom = new RoomEntity()
        {
            roomId = proto.room.roomId,//房间ID
            currentLoop = proto.room.loop,//局数
            maxLoop = proto.room.maxLoop,//总局数                                                      
            roomStatus = proto.room.status, //房间状态   
            FirstDice = proto.room.firstDice,//第一个骰子
            SecondDice = proto.room.secondDice,//第二个骰子
            FirstGivePos = proto.room.firstGivePos,//首发牌座位号
            TotalPokerNum = proto.room.totalPokerNum,//房间牌的总数量
            RemainPokerNum = proto.room.remainPokerNum,//房间剩余牌的数量
            PanBase = proto.room.panBase,//锅底
            IsAddPanBase = proto.room.isAddPanBase,//是否加锅底  
            dealSecond = proto.room.dealTime,
            roomUnixtime = proto.room.unixtime,//庄家翻牌时间
            fzName = proto.room.ownerNickName,
            // PlayerID = 111,
            isGrabBankerNum = 0,//抢庄的人数
        };
        Debug.Log(proto.room.status+"                      断线重连房间状态");
        CurrentRoom.roomPokerList = new List<Poker>();
        Debug.Log(proto.room.historyPokerListCount()+"                              房间牌长度");      
        for (int i = 0; i < proto.room.historyPokerListCount(); i++)
        {
            GP_POKER protoPoker = proto.room.getHistoryPokerList(i);
            CurrentRoom.roomPokerList.Add(new Poker()
            {
                Index = protoPoker.index,//索引
                Type = protoPoker.type,//花色
                Size = protoPoker.size,//大小  
            });
        }
        CurrentRoom.Config.Clear();
        for (int i = 0; i < proto.room.settingIdCount(); ++i)
        {
            cfg_settingEntity settingEntity = cfg_settingDBModel.Instance.Get(proto.room.getSettingId(i));
            if (settingEntity != null)
            {
                CurrentRoom.Config.Add(settingEntity);

                if (settingEntity.tags.Equals("isGuiZi"))
                {
                    CurrentRoom.enumGuiZi = (EnumGuiZi)settingEntity.value;
                }
                if (settingEntity.tags.Equals("isTianJiuWang"))
                {
                    CurrentRoom.enumTianJiuWang = (EnumTianJiuWang)settingEntity.value;
                }
                if (settingEntity.tags.Equals("isDiJiuWang"))
                {
                    CurrentRoom.enumDiJiuWang = (EnumDiJiuWang)settingEntity.value;
                }
                if (settingEntity.tags.Equals("banker"))
                {
                    CurrentRoom.roomMode = (RoomEntity.RoomMode)settingEntity.value;
                    Debug.Log((RoomEntity.RoomMode)settingEntity.value+"                              是否是抢庄模式");                    
                }
                if (settingEntity.tags.Equals("pourModel"))
                {
                    CurrentRoom.betModel = (RoomEntity.BetModel)settingEntity.value;
                    if (settingEntity.value == 1) isBet = true;
                    else isBet = false;
                }
                if (settingEntity.tags.Equals("limit"))
                {
                    CurrentRoom.scoreLimit = settingEntity.value;                  
                }
                if (settingEntity.tags.Equals("defaultScore"))
                {
                    CurrentRoom.guDScore = settingEntity.value;
                }
                if (settingEntity.tags.Equals("isBig"))
                {
                    CurrentRoom.roomPlay = (EnumPlay)settingEntity.value;
                }
            }
        }
        CurrentRoom.seatList = new List<SeatEntity>();     
        for (int i = 0; i < proto.room.seatListCount(); i++)
        {
            GP_SEAT op_seat = proto.room.getSeatList(i);
            SeatEntity seat = new SeatEntity();
            seat.PlayerId = op_seat.playerId;//玩家ID            
            seat.Nickname = op_seat.nickname;//玩家名字            
            seat.Avatar = op_seat.avatar;//玩家头像
            seat.Gender = op_seat.gender;//玩家性别            
            seat.Gold = op_seat.gold;//底分                     
            seat.Pos = op_seat.pos;//座位位置                                        
            seat.seatStatus = op_seat.status; ////座位状态             
            seat.IsBanker = op_seat.isBanker;//是否是庄家
            seat.isDismiss = op_seat.isDismiss;//是否同意解散房间
            seat.isWin = op_seat.isWin;//是否是赢家
            seat.eamings = op_seat.earnings;//本次收益
            seat.loopEamings = op_seat.loopEarnings;//每局收益  
            seat.groupTime = proto.room.groupPokerTime;//组合牌时间
            seat.IP = op_seat.ipAddr;//IP
            seat.Longitude = op_seat.longitude;//经度
            seat.Latitude = op_seat.latitude;//维度
            seat.IsFocus = !op_seat.isAfk;//是否在线 (true 表示不在线，false 表示在线)     
            seat.isGrabBanker = op_seat.isGrabBanker;//标记座位是否抢庄  
            if (seat.isGrabBanker == 1&&CurrentRoom.roomMode==RoomEntity.RoomMode.RobZhuang) CurrentRoom.isGrabBankerNum++;
            seat.isCuoPai = op_seat.isCuoPai;  
            seat.firstPour = op_seat.getPourList(0);//一道
            seat.secondPour = op_seat.getPourList(1);//二道 
            seat.threePour = op_seat.getPourList(2);//三道                                                         
            seat.pokerList = new List<Poker>();
            for (int j = 0; j < op_seat.pokerListCount(); ++j)
            {
                GP_POKER protoPoker = op_seat.getPokerList(j);
                seat.pokerList.Add(new Poker()
                {
                    Index = protoPoker.index,//索引
                    Type = protoPoker.type,//花色                    
                    Size = protoPoker.size,//大小                                     
                });                    
            }
            seat.drawPokerList = new List<int>();
            if (op_seat.drawPokerListCount()!=0)
            {               
                for (int k = 0; k < op_seat.drawPokerListCount(); k++)
                {
                    seat.drawPokerList.Add(op_seat.getDrawPokerList(k));
                }
            }
            CurrentRoom.seatList.Add(seat);
        }       
        CalculateSeatIndexOne();  //普通场计算座位 Index        
    }
    #endregion

    #region 计算客户端 Index
    private void CalculateSeatIndexOne()
    {
        PlayerSeat = null;
        if (CurrentRoom == null) return;
        for (int i = 0; i < CurrentRoom.seatList.Count; ++i)
        {
            if (CurrentRoom.seatList[i].PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
            {
                PlayerSeat = CurrentRoom.seatList[i];
                for (int j = 0; j < CurrentRoom.seatList.Count; ++j)
                {
                    SeatEntity seat = CurrentRoom.seatList[j];
                    int seatIndex = seat.Pos - PlayerSeat.Pos;
                    seatIndex = seatIndex < 0 ? seatIndex + ROOM_SEAT_COUNT : seatIndex;
                    seat.Index = seatIndex;
                }
                break;
            }
        }
        if (PlayerSeat == null)
        {
            PlayerSeat = CurrentRoom.seatList[0];
            for (int j = 0; j < CurrentRoom.seatList.Count; ++j)
            {
                SeatEntity seat = CurrentRoom.seatList[j];
                int seatIndex = seat.Pos - PlayerSeat.Pos;
                seatIndex = seatIndex < 0 ? seatIndex + ROOM_SEAT_COUNT : seatIndex;
                seat.Index = seatIndex;
            }
        }
    }
    #endregion

    #region 进入房间
    internal void EnterRoom(GP_ROOM_ENTER proto)
    {
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        if (seat == null) return;
        seat.PlayerId = proto.playerId;        
        seat.Gold = proto.gold;
        seat.Avatar = proto.avatar;
        seat.Gender = proto.gender;
        seat.Nickname = proto.nickname;
        seat.IP = proto.ipAddr;//IP      
        seat.Longitude = proto.longitude;//经度
        seat.Latitude = proto.latitude;//维度       
        Debug.Log(proto.ipAddr + "                            IP地址");
        Debug.Log(proto.longitude + "                            经度");
        Debug.Log(proto.latitude + "                            维度");
        SendGameSeatInfoChangeNotify(seat);
        AppDebug.Log(seat.Nickname + "进入房间");
    }
    #endregion

    #region 玩家准备
    /// <summary>
    /// 准备
    /// </summary>
    /// <param name="proto"></param>
    internal void ReadyProxy(GP_ROOM_READY proto)
    {
        CurrentRoom.roomStatus = ROOM_STATUS.READY;
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        seat.seatStatus = SEAT_STATUS.READY;
        SendGameSeatInfoChangeNotify(seat);
    }
    #endregion

    #region 游戏开始
    /// <summary>
    /// 游戏开始
    /// </summary>
    /// <param name="proto"></param>
    internal void GameStartProxy(GP_ROOM_GAMESTART proto)
    {
        CurrentRoom.currentLoop++;
        if (proto.isAddPanBase)
        {
            CurrentRoom.IsAddPanBase = proto.isAddPanBase;
            CurrentRoom.PanBase = proto.panBase;          
        }
        //把庄家的状态改为等待状态
        for (int i = 0; i < CurrentRoom.seatList.Count; i++)
        {           
            CurrentRoom.seatList[i].seatStatus = SEAT_STATUS.WAITDEAL;
            if (CurrentRoom.roomMode == RoomEntity.RoomMode.RobZhuang)
            {
                CurrentRoom.seatList[i].IsBanker = false;
            }            
        }             
        CurrentRoom.roomStatus = ROOM_STATUS.GRABBANKER;
        SendGameRoomInfoChangeNotify();
    }
    #endregion

    /// <summary>
    /// 通知抢庄
    /// </summary>
    internal void TallGrabBankerProxy(GP_ROOM_GRABBANKER proto)
    {
        CurrentRoom.roomStatus = ROOM_STATUS.GRABBANKER;
        CurrentRoom.roomUnixtime = proto.unixtime;
        TransferData data = new TransferData();
        data.SetValue("RoomStatus", CurrentRoom.roomStatus);
        data.SetValue("isGrabBaker", 3);
        data.SetValue("Unixtime", CurrentRoom.roomUnixtime);
        SendNotification(ConstantGuPaiJiu.GarbBankerSceneView,data);
    }

    /// <summary>
    /// 广播座位是否抢庄
    /// </summary>
    internal void GrabBankerAndNoBaker(GP_ROOM_GRABBANKER proto)
    {
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        seat.isGrabBanker = proto.isGrabBanker;
        if (seat.isGrabBanker == 1) CurrentRoom.isGrabBankerNum++;
        TransferData data = new TransferData();
        data.SetValue("Seat", seat);
        data.SetValue("isGrabBaker", seat.isGrabBanker);
        data.SetValue("RoomStatus", CurrentRoom.roomStatus);
        if (seat== PlayerSeat) SendNotification(ConstantGuPaiJiu.GarbBankerSceneView, data);//关闭抢庄按钮      
        SendNotification(ConstantGuPaiJiu.ISImageGarbBanker,data);
    }

    /// <summary>
    /// 抢庄结束，通知谁是庄
    /// </summary>
    internal void GrabBankerProxy(GP_ROOM_GRABBANKER proto)
    {
        CurrentRoom.roomStatus = ROOM_STATUS.GRABBANKERDONE;
        CurrentRoom.FirstDice = proto.diceFirst;//第一个筛子
        CurrentRoom.SecondDice = proto.diceSecond;//第二个筛子
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        seat.IsBanker = true;       
        TransferData data = new TransferData();
        data.SetValue("Room",CurrentRoom);
        data.SetValue("Seat", seat);
        data.SetValue("RoomStatus", CurrentRoom.roomStatus);
        data.SetValue("isGrabBankerNum",CurrentRoom.isGrabBankerNum);
        SendNotification(ConstantGuPaiJiu.CloseTime,data);                   
        SendNotification(ConstantGuPaiJiu.RollDice, data);       
    }

    /// <summary>
    /// 生成牌墙
    /// </summary>
    internal void BeginProxy()
    {
        CurrentRoom.roomStatus = ROOM_STATUS.BUILDPOKERWALL;
        if (CurrentRoom.roomPokerList.Count != 0) CurrentRoom.roomPokerList.Clear();
        CurrentRoom.dealSecond = 0;
        CurrentRoom.TotalPokerNum = 32;
        TransferData data = new TransferData();
        data.SetValue("Room", CurrentRoom);
        SendNotification(ConstantGuPaiJiu.ShuffleAnimation, data);//播放洗牌动画       
    }

    /// <summary>
    /// 服务器通知玩家切牌
    /// </summary>
    internal void TellCutPokerProxy(GP_ROOM_CUTPOKER proto)
    {
        CurrentRoom.roomStatus = ROOM_STATUS.CUTPOKER;
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        Debug.Log(seat.Pos+"                     通知切牌座位号·······");
        Debug.Log(proto.unixtime+"                      切牌时间");
        long unixtime = proto.unixtime;
        TransferData data = new TransferData();
        data.SetValue("Seat",seat);
        data.SetValue("isCut", true);
        data.SetValue("isWait",true);
        data.SetValue("IsPlayer", seat == PlayerSeat);
        data.SetValue("Unixtime",unixtime);
        SendNotification(ConstantGuPaiJiu.TellCutPoker, data);
    }

    /// <summary>
    /// 切牌或者不切
    /// </summary>
    internal void CutPokerProxy(GP_ROOM_CUTPOKER proto)
    {
        SeatEntity seat = GetSeatBySeatId(proto.pos);    
        TransferData data = new TransferData();     
        data.SetValue("isCut", false);
        data.SetValue("isWait", true);
        data.SetValue("IsPlayer", seat == PlayerSeat);       
        switch ((EnumCutPoker)proto.isCutPoker)
        {
            case EnumCutPoker.Cut:               
                data.SetValue("Room", CurrentRoom);         
                SendNotification(ConstantGuPaiJiu.StartCutPoker, data);
                break;
            case EnumCutPoker.NoCut:
                SendNotification(ConstantGuPaiJiu.NoCutPoker, data);
                break;
            case EnumCutPoker.NotOperational:
                break;
            default:
                break;
        }
        //SendNotification(ConstantGuPaiJiu.TellCutPoker, data);    
    }

    /// <summary>
    /// 切牌动画
    /// </summary>
    internal void CutPokerAniProxy(GP_ROOM_CUTPOKER proto)
    {
        int dun = proto.cutPokerIndex;
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        TransferData data = new TransferData();
        data.SetValue("Seat",seat);
        data.SetValue("Dun",dun);
        SendNotification(ConstantGuPaiJiu.CutPokerAni,data);        
    }

    internal void CutPokerEndProxy()
    {
        SendNotification(ConstantGuPaiJiu.CutPokerEnd, null);
    }

    #region 通知下注
    /// <summary>
    /// 通知下注
    /// </summary>
    internal void BetProxy(GP_ROOM_INFORMJETTON proto)
    {       
        CurrentRoom.roomStatus = ROOM_STATUS.POUR;              
        //把下注玩家的状态改为下注状态
        for (int i = 0; i < proto.posCount(); i++)
        {           
            SeatEntity seat = GetSeatBySeatId(proto.getPos(i));            
            seat.seatStatus =  SEAT_STATUS.POUR;
            if (seat==PlayerSeat)
            {
                TransferData data = new TransferData();
                data.SetValue("Seat", seat);
                //data.SetValue("IsPlayer", seat == PlayerSeat);
                data.SetValue("RoomStatus", CurrentRoom.roomStatus);
                SendNotification(ConstantGuPaiJiu.OnGuPaiTellBetInfoChanged, data);
            }           
        }
        SendNotification(ConstantGuPaiJiu.CloseCutPokerImage, null);//关闭切牌等待提示
    }
    #endregion

    #region 下注
    internal void BottomPourProxy(GP_ROOM_BOTTOMPOUR proto)
    {
        CurrentRoom.roomStatus = ROOM_STATUS.POUR;
        TransferData data = new TransferData();       
        SeatEntity seat = GetSeatBySeatId(proto.pos);     
        seat.firstPour = proto.firstPour;       
        seat.secondPour = proto.secondPour;
        seat.threePour = proto.thirdPour;
        seat.seatStatus = SEAT_STATUS.WAITDEAL;       
        data.SetValue("IsPlayer", seat == PlayerSeat);
        data.SetValue("Seat",seat);
        data.SetValue("RoomStatus",CurrentRoom.roomStatus);    
        SendNotification(ConstantGuPaiJiu.OnGuPaiSetBetPour, data);
        
    }
    #endregion

   


    #region 开始发空牌
    /// <summary>
    /// 开始发空牌
    /// </summary>
    /// <param name="proto"></param>
    internal void BeginProxy(GP_ROOM_BEGIN proto)
    {
        CurrentRoom.roomStatus = ROOM_STATUS.DEAL;
        CurrentRoom.dealSecond++;
        CurrentRoom.FirstGivePos = proto.room.firstGivePos;//首发牌座位号
        CurrentRoom.FirstDice = proto.room.firstDice;//第一个筛子
        CurrentRoom.SecondDice = proto.room.secondDice;//第二个筛子  
        for (int i = 0; i < proto.room.seatListCount(); i++)
        {
            SeatEntity seat = GetSeatBySeatId(proto.room.getSeatList(i).pos);
            seat.seatStatus = SEAT_STATUS.EMPTYPOKER;
            if (seat.pokerList.Count != 0) seat.pokerList.Clear();
            GP_SEAT op_seat = proto.room.getSeatList(i);
            for (int j = 0; j < op_seat.pokerListCount(); ++j)
            {
                GP_POKER protoPoker = op_seat.getPokerList(j);
                seat.pokerList.Add(new Poker()
                {
                    Index = protoPoker.index,//索引
                    Type = protoPoker.type,//花色
                    Size = protoPoker.size,//大小                                     
                });
            }
        }
        TransferData data = new TransferData();
        data.SetValue("Room",CurrentRoom);
        SendNotification("DealRollDice", data);//播放发牌摇筛子，发牌      
    }

   
    #endregion

    /// <summary>
    /// 清空桌面
    /// </summary>
    internal void CleardesktopProxy()
    {
        if (CurrentRoom.roomPokerList.Count != 0) CurrentRoom.roomPokerList.Clear();
        if (CurrentRoom.roomMode == RoomEntity.RoomMode.RobZhuang)
        {            
            // ModelDispatcher.Instance.Dispatch(ConstantGuPaiJiu.CloseRoomPokerTran);//清空房间挂载点
            for (int i = 0; i < CurrentRoom.seatList.Count; i++)
            {
                TransferData data = new TransferData();
                data.SetValue("Seat", CurrentRoom.seatList[i]);
                data.SetValue("IsPlayer", CurrentRoom.seatList[i] == PlayerSeat);
                data.SetValue("RoomStatus", CurrentRoom.roomStatus);
                data.SetValue("Room", CurrentRoom);
                SendNotification("LoopCloseDeal", data);
            }
        }
        else if (CurrentRoom.roomMode == RoomEntity.RoomMode.RoundZhuang)
        {            
            CurrentRoom.dealSecond = 0;
            if (CurrentRoom.roomPokerList.Count != 0) CurrentRoom.roomPokerList.Clear();
            for (int i = 0; i < CurrentRoom.seatList.Count; i++)
            {
                if (CurrentRoom.seatList[i].PlayerId == 0) continue;                            
                CurrentRoom.seatList[i].isGrabBanker = 0;//标记座位是否抢庄                 
                TransferData data = new TransferData();
                data.SetValue("Seat", CurrentRoom.seatList[i]);
                data.SetValue("IsPlayer", CurrentRoom.seatList[i] == PlayerSeat);
                data.SetValue("RoomStatus",CurrentRoom.roomStatus);
                data.SetValue("Room",CurrentRoom);
                SendGameSeatInfoChangeNotify(CurrentRoom.seatList[i]);
                SendNotification("LoopCloseDeal", data);
            }                
        }        
    }


    /// <summary>
    /// 发实牌
    /// </summary>
    internal void ValidDealProxy(GP_ROOM_VALIDDEAL proto)
    {
        if(CurrentRoom.roomPlay==EnumPlay.BigPaiJiu)CurrentRoom.roomStatus = ROOM_STATUS.GROUPPOKER;
        if (CurrentRoom.roomPlay == EnumPlay.SmallPaiJiu) CurrentRoom.roomStatus = ROOM_STATUS.CUOPAI;
        for (int i = 0; i < proto.seatListCount(); i++)
        {
            SeatEntity seat = GetSeatBySeatId(proto.getSeatList(i).pos);
            seat.seatStatus = SEAT_STATUS.GROUP;          
            if (seat == PlayerSeat)
            {
                seat.groupTime = proto.unixtime;
                if (seat.pokerList.Count != 0) seat.pokerList.Clear();             
                for (int j = 0; j < proto.getSeatList(i).pokerListCount(); ++j)
                {
                    GP_POKER protoPoker = proto.getSeatList(i).getPokerList(j);                    
                    seat.pokerList.Add(new Poker()
                    {
                        Index = protoPoker.index,//索引
                        Type = protoPoker.type,//花色
                        Size = protoPoker.size,//大小                                     
                    });
                }
                TransferData data = new TransferData();
                data.SetValue("Seat", seat);
                data.SetValue("Room", CurrentRoom);
                SendNotification(ConstantGuPaiJiu.GroupValidPoker, data);
            }
        }       
    }

    /// <summary>
    /// 组合完成
    /// </summary>
    internal void GroupPokerProxy(GP_ROOM_GROUPPOKER proto)
    {       
        SeatEntity seat = GetSeatBySeatId(proto.pos);        
        seat.seatStatus = SEAT_STATUS.GROUPDONE;
        if (seat.pokerList.Count != 0) seat.pokerList.Clear();
        for (int i = 0; i < proto.pokerIndexListCount(); i++)
        {
            seat.pokerList.Add(new Poker()
            {
                Index = proto.getPokerIndexList(i),//索引
                Type = proto.getPokerIndexList(i),//花色
                Size = proto.getPokerIndexList(i),//大小                                     
            });         
        }
        TransferData data = new TransferData();
        data.SetValue("Seat", seat);
        data.SetValue("RoomStatus", CurrentRoom.roomStatus);
        SendNotification(ConstantGuPaiJiu.EndIamge, data);//组合完成显示完成
        if (seat != PlayerSeat) return;        
        SendNotification(ConstantGuPaiJiu.GroupEnd, data);
    }

    /// <summary>
    /// 小牌九搓牌结束
    /// </summary>
    /// <param name="proto"></param>
    internal void CuoPaiProxy(GP_ROOM_CUOPAI proto)
    {
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        seat.seatStatus = SEAT_STATUS.GROUPDONE;
        seat.isCuoPai = 1;
        TransferData data = new TransferData();
        data.SetValue("Seat", seat);
        data.SetValue("RoomStatus", CurrentRoom.roomStatus);
        SendNotification(ConstantGuPaiJiu.EndIamge, data);//组合完成显示完成
        if (seat != PlayerSeat) return;
        SendNotification(ConstantGuPaiJiu.GroupEnd, data);
    }

    /// <summary>
    /// 提示
    /// </summary>
    internal void PromptPokerProxy(GP_ROOM_GROUPMAXPOKERPHINT proto)
    {
        List<int> IndexList = proto.getPokerIndexListList();
        TransferData data = new TransferData();
        data.SetValue("IndexList", IndexList);
        SendNotification(ConstantGuPaiJiu.OnGuPaiJiuPromptPoker, data);
    }

    /// <summary>
    /// 通知庄家翻牌
    /// </summary>
    /// <param name="proto"></param>
    internal void IsBankeDrawProxy(GP_ROOM_INFORMBANKERDRAW proto)
    {
        CurrentRoom.roomStatus = ROOM_STATUS.CHECK;
        CurrentRoom.roomUnixtime = proto.unixtime;
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        TransferData data = new TransferData();
        data.SetValue("Time", CurrentRoom.roomUnixtime);
        data.SetValue("Seat",seat);
        data.SetValue("IsPlayer", seat == PlayerSeat);
        data.SetValue("RoomStatus", CurrentRoom.roomStatus);
        SendNotification(ConstantGuPaiJiu.TellIsBankeDraw,data);        
    }

    /// <summary>
    /// 翻牌
    /// </summary>
    /// <param name="proto"></param>
    internal void DrawPokerProxy(GP_ROOM_DRAWPOKER proto)
    {
        List<Poker> DrawPokerList = new List<Poker>();
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        int dun = proto.index;
        for (int i = 0; i < proto.drawPokerListCount(); i++)
        {
            DrawPokerList.Add(new Poker()
            {
                Index = proto.getDrawPokerList(i).index,//索引
                Type = proto.getDrawPokerList(i).type,//花色
                Size = proto.getDrawPokerList(i).size,//大小                                     
            });
        }
        TransferData data = new TransferData();
        data.SetValue("DrawPokerList", DrawPokerList);
        data.SetValue("Seat",seat);
        data.SetValue("Index",dun);
        SendNotification(ConstantGuPaiJiu.DrawPoker,data);
    }


    /// <summary>
    /// 每次结算
    /// </summary>
    internal void SettleProxy(GP_ROOM_SETTLE proto)
    {
        CurrentRoom.roomStatus = ROOM_STATUS.SETTLE;
        //if (CurrentRoom.roomPokerList.Count != 0) CurrentRoom.roomPokerList.Clear();
        List<SeatEntity> seatList = new List<SeatEntity>();
        List<SeatEntity> seatList1 = new List<SeatEntity>();
        TransferData data = new TransferData();
        for (int i = 0; i < proto.room.seatListCount(); i++)
        {
            GP_SEAT op_seat = proto.room.getSeatList(i);
            SeatEntity seat = GetSeatBySeatId(proto.room.getSeatList(i).pos);
            if (seat.pokerList.Count != 0) seat.pokerList.Clear();
            seat.seatStatus = SEAT_STATUS.SETTLE;
            if (seat.drawPokerList != null)
            {
                seat.drawPokerList.Clear();//清空是否翻牌集合
            }
            seat.isCuoPai = 0;
            seat.firstPour = 0;
            seat.secondPour = 0;
            seat.threePour = 0;
            seat.Gold += proto.room.getSeatList(i).earnings;
            seat.eamings = proto.room.getSeatList(i).earnings;
            if (seat.IsBanker)
            {
                seat.firstPour = op_seat.getPourList(0) ;               
            }                       
            for (int j = 0; j < op_seat.pokerListCount(); ++j)
            {
                GP_POKER protoPoker = op_seat.getPokerList(j);
                seat.pokerList.Add(new Poker()
                {
                    Index = protoPoker.index,//索引
                    Type = protoPoker.type,//花色                    
                    Size = protoPoker.size,//大小                                     
                });
                CurrentRoom.roomPokerList.Add(new Poker()
                {
                    Index = protoPoker.index,//索引
                    Type = protoPoker.type,//花色
                    Size = protoPoker.size,//大小   
                });
            }
            seatList.Add(seat);
            data.SetValue("Room", CurrentRoom);
            data.SetValue("RoomStatus", CurrentRoom.roomStatus);
            data.SetValue("Seat", seat);
            data.SetValue("PlayerSeat", PlayerSeat);
            data.SetValue("IsPlayer", seat == PlayerSeat);
#if IS_CHUANTONGPAIJIU
            if(seat != PlayerSeat) SendNotification(ConstantGuPaiJiu.GroupEnd, data);//实例化其他人的牌 
            SendNotification(ConstantGuPaiJiu.OnGuPaiSetBetPour, data); //重新设置庄家下注分数
            SendNotification(ConstantGuPaiJiu.SetSeatGold, data);//设置玩家的金币          
#endif

            SendNotification(ConstantGuPaiJiu.EndIamge, data);//隐藏组合完成的图片
        }
        data.SetValue("SeatList",seatList);
#if IS_BAODINGQIPAI
        SendNotification(ConstantGuPaiJiu.GroupEndJieSuan,data);
#endif
        SendNotification(ConstantGuPaiJiu.CloseDrawPoker,data);//关闭翻牌倒计时和全开按钮
#if IS_CHUANTONGPAIJIU
        SendNotification(ConstantGuPaiJiu.LoadSmallResult, data);//小结算界面
        SendNotification(ConstantGuPaiJiu.PlayMusic,data); //播放音乐
#endif
    }

    /// <summary>
    /// 结算完设置分数
    /// </summary>
    /// <param name="data"></param>
    public void SetBetUI(List<SeatEntity> seatList)
    {        
        TransferData data = new TransferData();
        for (int i = 0; i < seatList.Count; i++)
        {
            data.SetValue("Room", CurrentRoom);
            data.SetValue("RoomStatus", CurrentRoom.roomStatus);
            data.SetValue("Seat", seatList[i]);
            data.SetValue("PlayerSeat", PlayerSeat);
            data.SetValue("IsPlayer", seatList[i] == PlayerSeat);
            SendNotification(ConstantGuPaiJiu.OnGuPaiSetBetPour, data); //重新设置庄家下注分数
            SendNotification(ConstantGuPaiJiu.SetSeatGold, data);//设置玩家的金币   
        }         
    }


    /// <summary>
    /// 每局结算
    /// </summary>
    /// <param name="proto"></param>
    internal void LoopSettleProxy(GP_ROOM_LOOPSETTLE proto)
    {
        CurrentRoom.roomStatus = ROOM_STATUS.IDLE;
        CurrentRoom.dealSecond = 0;
        if (CurrentRoom.roomPokerList.Count != 0) CurrentRoom.roomPokerList.Clear();
        for (int i = 0; i < proto.room.seatListCount(); i++)
        {
            SeatEntity seat = GetSeatBySeatId(proto.room.getSeatList(i).pos);
            seat.seatStatus = SEAT_STATUS.READY;
            //seat.Gold += proto.room.getSeatList(i).earnings;
            //seat.loopEamings = proto.room.getSeatList(i).loopEarnings;
            if (CurrentRoom.roomMode == RoomEntity.RoomMode.RoundZhuang)
            {
                if (seat.IsBanker) seat.IsBanker = false;
            }
            seat.firstPour = 0;
            seat.secondPour = 0;
            TransferData data = new TransferData();
            data.SetValue("Seat", seat);
            data.SetValue("IsPlayer", seat == PlayerSeat);
            SendGameSeatInfoChangeNotify(seat);
        }
    }

    /// <summary>
    /// 切锅
    /// </summary>
    internal void RoomCutpanProxy(GP_ROOM_CUTPAN proto)
    {      
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        long CutPanTime = proto.unixtime;       
        TransferData data = new TransferData();
        data.SetValue("Time",CutPanTime);
        data.SetValue("IsPlayer", seat == PlayerSeat);
        if (!proto.hasIsCutPan()) SendNotification(ConstantGuPaiJiu.TellCutPan, data);
        if (proto.hasIsCutPan())
        {           
            //if (proto.isCutPan==1)
            //{
            //    CurrentRoom.roomStatus = ROOM_STATUS.READY;
            //    for (int i = 0; i < CurrentRoom.seatList.Count; i++)
            //    {
            //        CurrentRoom.seatList[i].IsBanker = false;
            //        CurrentRoom.seatList[i].seatStatus = SEAT_STATUS.READY;
            //    }               
            //}
            SendNotification(ConstantGuPaiJiu.CutPanResult, data);
        }           
    }



    /// <summary>
    /// 通知坐庄
    /// </summary>
    internal void InfoRebankerProxy(GP_ROOM_INFORMBANKER proto)
    {
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        if (seat == null) return;
        CurrentRoom.roomStatus = ROOM_STATUS.READY;
        for (int i = 0; i < CurrentRoom.seatList.Count; i++)
        {            
            if (seat.Pos == CurrentRoom.seatList[i].Pos) CurrentRoom.seatList[i].IsBanker = true;
            else CurrentRoom.seatList[i].IsBanker = false;
            CurrentRoom.seatList[i].seatStatus = SEAT_STATUS.READY;
            SendGameSeatInfoChangeNotify(CurrentRoom.seatList[i]);
        }
    }

    /// <summary>
    /// 玩家离开
    /// </summary>
    internal void LeaveProxy(GP_ROOM_LEAVE proto)
    {
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        if (seat == null) return;
        seat.PlayerId = 0;
        seat.Latitude = 0f;
        seat.Longitude = 0f;
        seat.Gold = 0;
        seat.Avatar = string.Empty;
        seat.seatStatus = SEAT_STATUS.IDLE;
        SendGameSeatInfoChangeNotify(seat);
        AppDebug.Log(seat.Nickname + "离开房间");
        if (seat == PlayerSeat)
        {
            NetWorkSocket.Instance.SafeClose(GameCtrl.Instance.SocketHandle);
            SceneMgr.Instance.LoadScene(SceneType.Main);
        }
    }

    /// <summary>
    /// 关闭自动配牌的消息
    /// </summary>
    /// <param name="isSend"></param>
    internal void isSendProxy(bool isSend)
    {       
        TransferData data = new TransferData();
        data.SetValue("isSend", isSend);
        SendNotification(ConstantGuPaiJiu.isSend,data);
    }


    /// <summary>
    /// 断线重连更改座位的牌
    /// </summary>
    public void ChangeSeatPoker(SeatEntity seat)
    {
        if (CurrentRoom.roomStatus != ROOM_STATUS.IDLE && CurrentRoom.roomStatus != ROOM_STATUS.READY && CurrentRoom.roomStatus != ROOM_STATUS.GRABBANKER)
        {
            if (CurrentRoom.roomStatus == ROOM_STATUS.CHECK)
            {
                if (seat.drawPokerList.Count == 0)
                {
                    if (seat!= PlayerSeat)                   
                      SeatCtrlBreak(3, seat);
                }
                else if (seat.drawPokerList.Count == 1)
                {
                    for (int j = 0; j < seat.drawPokerList.Count; j++)
                    {
                        switch (seat.drawPokerList[j])
                        {
                            case 0:
                                SeatCtrlBreak(0, seat);
                                break;
                            case 1:
                                SeatCtrlBreak(1, seat);
                                break;
                        }
                    }
                }              
            }
            if (CurrentRoom.roomStatus==ROOM_STATUS.DEAL)
            {
                SeatCtrlBreak(3, seat);
            }
            if (CurrentRoom.roomStatus==ROOM_STATUS.GROUPPOKER|| CurrentRoom.roomStatus == ROOM_STATUS.CUOPAI)
            {
                if (seat != PlayerSeat)
                    SeatCtrlBreak(3, seat);
            }
            
        }    
    }

    /// <summary>
    /// 断线重连设置座位牌
    /// </summary>
    /// <param name="index"></param>
    /// <param name="seat"></param>
    /// <returns></returns>
    private SeatEntity SeatCtrlBreak(int index, SeatEntity seat)
    {
        for (int i = 0; i < seat.pokerList.Count; i++)
        {
            if (index == 0)
            {
                if (i >= 2)
                {
                    seat.pokerList[i].Type = 0;
                }
            }
            else if (index == 1)
            {
                if (i < 2)
                {
                    seat.pokerList[i].Type = 0;
                }
            }
            else if (index == 3)
            {
                seat.pokerList[i].Type = 0;
            }
        }
        return seat;
    }


    /// <summary>
    /// 结算的时候断线重连
    /// </summary>
    private void RoomPokerList()
    {
        if (CurrentRoom.roomStatus == ROOM_STATUS.SETTLE)
        {
            for (int i = 0; i < CurrentRoom.seatList.Count; i++)
            {
                if (CurrentRoom.seatList[i].pokerList.Count != 0)
                {
                    for (int j = 0; j < CurrentRoom.seatList[i].pokerList.Count; j++)
                    {
                        CurrentRoom.roomPokerList.Add(CurrentRoom.seatList[i].pokerList[j]);
                    }
                }
                CurrentRoom.seatList[i].pokerList.Clear();
            }
        }
    }







#region SetFocus 设置焦点状态
    /// <summary>
    /// 设置焦点状态
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="isFocus"></param>
    public void SetFocus(int playerId, bool isFocus)
    {
        SeatEntity seat = GetSeatByPlayerId(playerId);
        if (seat == null) return;
        Debug.Log(seat.Nickname + (isFocus ? "切回来" : "切出去"));
        seat.IsFocus = isFocus;
        TransferData data = new TransferData();
        data.SetValue("Seat", seat);       
        SendNotification(ConstantGuPaiJiu.SetLeave, data);
    }
#endregion




#region GetSeat 根据玩家 PlayerId，Pos，Index 来获取座位
    /// <summary>
    /// 根据玩家Id获取座位
    /// </summary>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public SeatEntity GetSeatByPlayerId(int playerId)
    {
        if (CurrentRoom == null) return null;
        for (int i = 0; i < CurrentRoom.seatList.Count; ++i)
        {
            if (CurrentRoom.seatList[i].PlayerId == playerId)
            {
                return CurrentRoom.seatList[i];
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
        for (int i = 0; i < CurrentRoom.seatList.Count; ++i)
        {
            if (CurrentRoom.seatList[i].Pos == seatId)
            {
                return CurrentRoom.seatList[i];
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
        for (int i = 0; i < CurrentRoom.seatList.Count; ++i)
        {
            if (CurrentRoom.seatList[i].Index == seatIndex)
            {
                return CurrentRoom.seatList[i];
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
        LookupPokerType.Instance.room = CurrentRoom;//判断牌型用  
        TransferData roomData = new TransferData();
        RoomPokerList();
        roomData.SetValue("Room", CurrentRoom);
        SendNotification(ConstantGuPaiJiu.OnGuPaiRoomInfoChanged, roomData);      
        for (int i = 0; i < CurrentRoom.seatList.Count; ++i)
        {           
            SendSeatInfoChangeNotify(CurrentRoom.seatList[i]);
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
        ChangeSeatPoker(seat);
        TransferData data = new TransferData();
        data.SetValue("Seat", seat);
        data.SetValue("IsPlayer", seat == PlayerSeat);
        data.SetValue("RoomStatus", CurrentRoom.roomStatus);
        data.SetValue("Room", CurrentRoom);
        data.SetValue("PanBase",CurrentRoom.PanBase);
        data.SetValue("IsAddPanBase",CurrentRoom.IsAddPanBase);
        SendNotification("OnSeatInfoChanged", data);
    }
#endregion




#region SendGameRoomInfoChangeNotify 正常游戏中发送房间信息变更消息
    /// <summary>
    /// 发送房间信息变更消息
    /// </summary>
    public void SendGameRoomInfoChangeNotify()
    {
        TransferData roomData = new TransferData();
        roomData.SetValue("Room", CurrentRoom);
        SendNotification("OnRoomGameInfoChanged", roomData);
        for (int i = 0; i < CurrentRoom.seatList.Count; ++i)
        {
            SendGameSeatInfoChangeNotify(CurrentRoom.seatList[i]);
        }
    }
#endregion

#region SendSeatInfoChangeNotify 正常游戏中发送座位信息变更消息
    /// <summary>
    /// 发送座位信息变更消息
    /// </summary>
    /// <param name="seat"></param>
    private void SendGameSeatInfoChangeNotify(SeatEntity seat)
    {
        if (seat == null) return;     
        TransferData data = new TransferData();
        data.SetValue("Seat", seat);
        data.SetValue("IsPlayer", seat == PlayerSeat);
        data.SetValue("RoomStatus", CurrentRoom.roomStatus);
        data.SetValue("Room", CurrentRoom);
        data.SetValue("PanBase", CurrentRoom.PanBase);
        data.SetValue("IsAddPanBase", CurrentRoom.IsAddPanBase);
        SendNotification("OnSeatGameInfoChanged", data);
    }
#endregion


}
