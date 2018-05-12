//===================================================
//Author      : CZH
//CreateTime  ：6/14/2017 11:22:11 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhaJh;
using com.oegame.zjh.protobuf;
using zjh.proto;
using System;

public class RoomZhaJHProxy : ProxyBase<RoomZhaJHProxy>
{
    public RoomEntity CurrentRoom;//当前房间 

    public SeatEntity PlayerSeat;//当前座位

    public const int ROOM_SEAT_COUNT = 5;//房间座位数量
    public RoomEntity CurrentOperator { get; private set; } //目前运营商

    public RoomEntity.RoomStatus CurrentState;//当前状态

    public List<Poker> currentPokerList;

    public int theCardPos;

    public int clientDepositBranch=0;//客户端存分

    public bool btnGDD=false;//是否跟到底

    public int indexLightPoker=0;//亮牌的索引

    public int SureWheelNumber = 1;//闷的轮数。默认为1

    public int ShufflingPoker = 0;//是否是搓牌模式，0表示 不是，1表示是；
    //public int isSendMoney = 0;//是否派彩
    public int isShopping = 0;//是否血拼

    public bool isGameOver = true;

    private bool isShoppingIamge = false;//是否飘血拼的图片

    private  float fen = 0;




    #region 初始化房间数据
    /// <summary>
    /// 初始化房间数据
    /// </summary>
    /// <param name="pbRoom"></param>

    public void InitRoom(ZJH_ROOM_CREATE proto)
    {
        ShufflingPoker = 0;
        isShopping = 0;
        fen = 0;
        isShoppingIamge = false;
        isGameOver = true;
        SureWheelNumber = 1;
        btnGDD = false;
        CurrentRoom = new RoomEntity()
        {
            roomId = proto.zjh_room.roomId,//房间ID
            currentLoop = proto.zjh_room.loop,//局数
            maxLoop = proto.zjh_room.maxLoop,//总局数                        
            scores = proto.zjh_room.scores,//分数                       
            SeatCount = proto.zjh_room.seatCount(),//座位数 
            roomStatus = proto.zjh_room.roomstatus,
            round = proto.zjh_room.round,//房间第几轮
            totalRound = proto.zjh_room.totalRound,//房间总轮数
            roomPour = proto.zjh_room.RoomPour,//房间最高下注分数
            baseScore = proto.zjh_room.baseScore,
            matchId = 0,          
        };       
        CurrentRoom.Config.Clear();
        for (int i = 0; i < proto.zjh_room.settingIdCount(); ++i)
        {
            cfg_settingEntity settingEntity = cfg_settingDBModel.Instance.Get(proto.zjh_room.getSettingId(i));
            if (settingEntity.tags.Equals("mode"))
            {
                CurrentRoom.roomSettingId = (RoomMode)settingEntity.value;  //房间模式                            
            }
            if (settingEntity.tags.Equals("mode"))
            {
                CurrentRoom.roomSettingId = (RoomMode)settingEntity.value;  //房间模式                                              
            }
            if (settingEntity.tags.Equals("maxRing"))
            {
                CurrentRoom.totalRound = settingEntity.value;    //总轮数                          
            }
            if (settingEntity.tags.Equals("barkPoker"))          
            {
                SureWheelNumber = settingEntity.value;          //比牌轮数
            }
            if (settingEntity.tags.Equals("isRubPoker"))
            {
                ShufflingPoker = settingEntity.value;          //是否搓牌                               
            }         
            if (settingEntity.tags.Equals("isShopping"))
            {
                isShopping= settingEntity.value;//是否血拼                
            }          
            if (settingEntity != null)
            {
                CurrentRoom.Config.Add(settingEntity);
            }            
            
        }
        Debug.Log(CurrentRoom.roomStatus+"                              房间状态");  
        CurrentRoom.seatList = new List<SeatEntity>();        
        for (int i = 0; i < proto.zjh_room.seatCount() ; i++)
        {
            SEAT op_seat = proto.zjh_room.getSeat(i);
            SeatEntity seat = new SeatEntity();
            seat.PlayerId = op_seat.playerId;//玩家ID                        
            seat.Nickname = op_seat.nickname;//玩家名字                       
            seat.Avatar = op_seat.avatar;//玩家头像
            seat.IP = op_seat.ipaddr;//IP地址
            seat.Latitude = op_seat.latitude;//经度
            seat.Longitude = op_seat.longitude;//维度
            seat.Gender = op_seat.gender;//玩家性别            
            seat.gold = op_seat.gold;//底分              
            seat.pos = op_seat.pos;//座位位置           
            seat.pour = op_seat.pour;//下注分  
            seat.seatRound = proto.zjh_room.round;
            seat.totalPour = op_seat.totalPour;//座位下注总分                                            
            seat.roomResult = op_seat.zjh_enum_roomresult;//房间解散结果
            seat.seatStatus = op_seat.status;                          ////座位状态                                     
            seat.homeLorder = op_seat.homeLorder;          ////是否是庄                 
            seat.pokerStatus = op_seat.pokerstatus;     //扑克状态  
            seat.systemTime = op_seat.unixtime;
            seat.seatToperateStatus = op_seat.opreateStatus;
            seat.isLowScore = op_seat.ISLOWSCORE;//是否是低分模式   
            Debug.Log(seat.seatStatus+"                              座位状态");
            Debug.Log(seat.pokerStatus+"                             扑克状态");
            seat.pokerList = new List<Poker>();
            for (int j = 0; j < op_seat.zjhPokerCount(); ++j)
            {
                POKER protoPoker = op_seat.getZjhPoker(j);
                seat.pokerList.Add(new Poker()
                {
                    index = protoPoker.index,//索引
                    color = protoPoker.color,//花色
                    size = protoPoker.size,//大小                                     
                });
            }
            CurrentRoom.seatList.Add(seat);
        }
        
         CurrentRoom.billList = new List<BillEntity>();
        if (proto.zjh_room.seatBillCount() != 0)
        {
            for (int i = 0; i < proto.zjh_room.seatBillCount(); i++)
            {
                SEAT_BILL seat_Bill = proto.zjh_room.getSeatBill(i);               
                BillEntity bill = new BillEntity();
                bill.playerIDBill = seat_Bill.playerId;
                bill.nameBill = seat_Bill.nickname;
                bill.pourBill = seat_Bill.pour;
                bill.avatarBill = seat_Bill.avatar;
                CurrentRoom.billList.Add(bill);
            }
        }
        if (CurrentRoom.roomSettingId == 0) return;

        if (CurrentRoom.roomSettingId == RoomMode.Senior)
        {
            CalculateSeatIndexOneTwo();//高级场计算座位 Index
        }
        else
        {
            CalculateSeatIndexOne();  //普通场计算座位 Index
        }
        fen = CurrentRoom.scores;       
    }
    #endregion


    /// <summary>
    /// 重建房间
    /// </summary>
    /// <param name="pbRoom"></param>
    public void Rebuild()
    {
        CurrentRoom = new RoomEntity();
        CalculateSeatIndexOne();        
        // InitBankerPos();
    }
    #region 5秒自动准备
    /// <summary>
    /// 5秒自动准备
    /// </summary>
    public void AoutReady()
    {      
        if (CurrentRoom.roomSettingId == RoomMode.Senior)
        {
            if (PlayerSeat.seatStatus == ENUM_SEAT_STATUS.IDLE && PlayerSeat.pos != 7 && CurrentRoom.roomStatus == ENUM_ROOM_STATUS.IDLE)
            {
                SendNotification(ZhaJHMethodname.OnZJHAutomaticReady, null);//5秒自动准备
            }           
        }
        else 
        {
            if (PlayerSeat.seatStatus == ENUM_SEAT_STATUS.IDLE && CurrentRoom.roomStatus == ENUM_ROOM_STATUS.IDLE)
            {
                SendNotification("AutomaticReady", null);//5秒自动准备           
            }
        }
    }
    #endregion

    #region 普通房计算客户端 Index
    /// <summary>
    /// 普通场计算客户端index
    /// </summary>
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
                    int seatIndex = seat.pos - PlayerSeat.pos;
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
                int seatIndex = seat.pos - PlayerSeat.pos;
                seatIndex = seatIndex < 0 ? seatIndex + ROOM_SEAT_COUNT : seatIndex;
                seat.Index = seatIndex;               
            }
        }       
    }
    #endregion

    #region 高级房计算客户端 Index
    /// <summary>
    /// 高级场计算客户端 Index
    /// </summary>
    private void CalculateSeatIndexOneTwo()
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
                    switch (seat.pos - PlayerSeat.pos)
                    {
                        case 1:
                        case -6:
                            seat.Index = 6;
                            break;
                        case 2:
                        case -5:
                            seat.Index = 1;
                            break;
                        case 3:
                        case -4:
                            seat.Index = 2;
                            break;
                        case 4:
                        case -3:
                            seat.Index = 3;
                            break;
                        case 5:
                        case -2:
                            seat.Index = 4;
                            break;
                        case 6:
                        case -1:
                            seat.Index = 5;
                            break;
                        case 0:
                            seat.Index = 0;
                            break;
                    }
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
                switch (seat.pos - PlayerSeat.pos)
                {
                    case 1:
                    case -6:
                        seat.Index = 6;
                        break;
                    case 2:
                    case -5:
                        seat.Index = 1;
                        break;
                    case 3:
                    case -4:
                        seat.Index = 2;
                        break;
                    case 4:
                    case -3:
                        seat.Index = 3;
                        break;
                    case 5:
                    case -2:
                        seat.Index = 4;
                        break;
                    case 6:
                    case -1:
                        seat.Index = 5;
                        break;
                    case 0:
                        seat.Index = 0;
                        break;
                }
            }
        }
    }
    #endregion




    #region 进入房间
    /// <summary>
    /// 进入房间
    /// </summary>
    /// <param name="pbSeat"></param>
    public void EnterRoom(ZJH_ROOM_ENTER proto)
    {        
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        if (seat == null) return;
        seat.PlayerId = proto.playerId;
        seat.gold = proto.gold;           
        seat.Avatar = proto.avatar;
        seat.Gender = proto.gender;       
        seat.Nickname = proto.nickname;
        seat.seatStatus = ENUM_SEAT_STATUS.IDLE;        
        seat.pos = proto.pos;
        seat.profit = 0;
        seat.isScore = false;
        SendSeatInfoChangeNotify(seat);
        AppDebug.Log(seat.Nickname + "进入房间");       
    }
    #endregion

    #region 当玩家处于下注状态断线重连，显示或隐藏下注
    /// <summary>
    /// 当玩家处于下注状态的时候，显示或者隐藏下注，看牌等按钮
    /// </summary>
    public void SendRoomSeatOperation()
    {
        TransferData data = new TransferData();
        data.SetValue("RoomStatus", CurrentRoom.roomStatus);
        data.SetValue("totalRound", CurrentRoom.totalRound);
        data.SetValue("Room", CurrentRoom);
        for (int i = 0; i < CurrentRoom.seatList.Count; ++i)
        {
            data.SetValue("Seat", CurrentRoom.seatList[i]);
            data.SetValue("IsPlayer", CurrentRoom.seatList[i] == PlayerSeat);

            SendNotification(ZhaJHMethodname.OnZJHSetSeatInfoOperation, data);
        }
        SendNotification("OnRoomZJHInfoChanged", data);
    }
    #endregion

    #region 房间信息变更消息
    /// <summary>
    /// 发送房间信息变更消息
    /// </summary>
    public void SendRoomInfoChangeNotify()
    {
        TransferData roomData = new TransferData();
        if (CurrentRoom.roomSettingId!= RoomMode.Senior)
        {
            roomData.SetValue("Room", CurrentRoom);
            SendNotification(ZhaJHMethodname.OnZJHRoomInfoChanged, roomData);
        }
        else if (CurrentRoom.roomSettingId == RoomMode.Senior)
        {
            CurrentRoom.maxLoop = 0;
            roomData.SetValue("Room", CurrentRoom);
            SendNotification(ZhaJHMethodname.OnZJHRoomInfoChanged, roomData);
        }      
        for (int i = 0; i < CurrentRoom.seatList.Count; ++i)
        {            
            SendSeatInfoChangeNotify(CurrentRoom.seatList[i]);            
        }
    }
    #endregion

    #region 座位信息变更消息
    /// <summary>
    /// 发送座位信息变更消息
    /// </summary>
    /// <param name="seat"></param>
    private void SendSeatInfoChangeNotify(SeatEntity seat)
    {       
        if (seat == null) return;
       // ReMoveSeat(seat);
        TransferData data = new TransferData();        
        data.SetValue("Seat", seat);
        data.SetValue("IsPlayer", seat == PlayerSeat);
        data.SetValue("RoomStatus", CurrentRoom.roomStatus);
        data.SetValue("Room", CurrentRoom);
        SendNotification(ZhaJHMethodname.OnZJHSeatInfoChanged, data);
    }
    #endregion

    private void ReMoveSeat(SeatEntity seat)
    {
        seat.pokerList.Clear();
        seat.Gold = 0;
        seat.pour = 0;//下注分  
        seat.seatRound = 0;
        seat.totalPour = 0;//座位下注总分                                                                                           
        seat.seatToperateStatus = ENUM_SEATOPERATE_STATUS.SEAT_STATUS_IDLE;
    }


    #region 玩家准备
    /// <summary>
    /// 玩家准备
    /// </summary>   
    public void Ready(ZJH_ROOM_READY proto)
    {        
        if (proto.pos== PlayerSeat.pos) SendNotification(ZhaJHMethodname.OnZJHCloseIEtor, null);
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        seat.seatStatus = ENUM_SEAT_STATUS.READY;
        seat.seatToperateStatus = ENUM_SEATOPERATE_STATUS.SEAT_STATUS_IDLE;       
        SendSeatInfoChangeNotify(seat);
        if (CurrentRoom.roomSettingId== RoomMode.Senior)
        {
            for (int i = 0; i < CurrentRoom.seatList.Count; i++)
            {
                SendSeatInfoChangeNotify(seat);
            }
        }
        //if (seat== PlayerSeat)
        //{
        //    NexeGameProxy();
        //}
       
        //CurrentRoom.roomStatus = ENUM_ROOM_STATUS.READY;
    }
    #endregion

    #region 发牌
    /// <summary>
    /// 发牌
    /// </summary>
    public void HariPokerProxy(ZJH_ROOM_DEAL proto)
    {
        NexeGameProxy();
        fen = CurrentRoom.scores;
        List<SeatEntity> seatList = new List<SeatEntity>();
        AudioEffectManager.Instance.Play("zjh_fapai", Vector3.zero);//播放音效        
        CurrentRoom.currentLoop += 1;       
        for (int i = 0; i < proto.zjhSeatCount(); i++)
        {
            SeatEntity seat = GetSeatBySeatId(proto.getZjhSeat(i).pos);
            seat.betPoints = 0;
            seat.totalPour = 0;
            seat.seatStatus = ENUM_SEAT_STATUS.DEAL;           
            //seat.gold -= 1* CurrentRoom.scores;            
            seat.betPoints = 1* CurrentRoom.scores;
            seat.totalPour+= 1 * CurrentRoom.scores;           
            UIItemZhaJHRoomInfo.Instance.SetBaseScoreUI(CurrentRoom.baseScore += 1* CurrentRoom.scores);
            //DicEvent("Seat", CurrentRoom.seatList[i], "OnSeatInfoGold");
            DicEvent("Seat", seat, ZhaJHMethodname.OnZJHSeatInfoGold);//刷新分数
            seatList.Add(seat);           
        }

        DicEvent("SeatList", seatList, ZhaJHMethodname.OnZJHHairPokerAni);//牌动画
       
        //seatList.Clear();

        for (int i = 0; i < CurrentRoom.seatList.Count; i++)
        {          
            CurrentRoom.seatList[i].homeLorder = false;         
        }
        CurrentRoom.roomStatus = ENUM_ROOM_STATUS.READY;
        SendRoomInfoChangeNotify();
        CurrentRoom.roomStatus = ENUM_ROOM_STATUS.DEAL;

    }
    #endregion

    #region 通知玩家下注
    /// <summary>
    /// 通知玩家下注下注
    /// </summary>
    /// <param name="pbSeat"></param>
    public void NoticeBet(ZJH_ROOM_NEXT_POUR proto)
    {
        Debug.Log("通知              "+proto.pos+"                 下注");
        Debug.Log("第几轮            "+ proto.round+"                 轮数");    
                 
        ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHCloseCountTime); //关闭时间
        for (int i = 0; i < CurrentRoom.seatList.Count; i++)
        {
            TransferData data1 = new TransferData();
            if (CurrentRoom.seatList[i].pos== PlayerSeat.pos)
            {
                data1.SetValue("Seat", CurrentRoom.seatList[i]);
                SendNotification(ZhaJHMethodname.OnZJHBtnGDD, data1);
            }           
        }
        CurrentRoom.roomStatus = ENUM_ROOM_STATUS.GAME;
        CurrentRoom.round = proto.round;//房间轮数        
        if (CurrentRoom.totalRound-CurrentRoom.round==1)
        {
            if (!isShoppingIamge)
            {
                ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHShoppingImageTweenMethod);
                isShoppingIamge = true;
            }                
        }
        UIItemZhaJHRoomInfo.Instance.SetTotalRound(CurrentRoom.round,CurrentRoom.totalRound);
        //倒计时
        SeatEntity seat = GetSeatBySeatId(proto.pos);        
        seat.seatStatus = ENUM_SEAT_STATUS.BET;
        seat.systemTime = proto.unixtime;               
        seat.seatRound = proto.round;//座位轮数        
        DicEvent("Seat",seat, ZhaJHMethodname.OnZJHSeatInfoBet);
        //if (seat.pos== PlayerSeat.pos)
        //{
            if (btnGDD)
            {
                ZhaJHGameCtrl.Instance.WithNotes(null);
                return;
            }
            TransferData data = new TransferData();      
            data.SetValue("totalRound",CurrentRoom.totalRound);            
            data.SetValue("Seat",seat);
            data.SetValue("Fen",fen);
            Debug.Log(fen + "                                 ----------------下注分数");
            SendNotification(ZhaJHMethodname.OnZJHBtnShow, data);                                     
            SendNotification(ZhaJHMethodname.OnZJHHidFen, data);                             
       // }
        //else if (seat.pos != PlayerSeat.pos)
        //{            
        //    if (PlayerSeat.seatToperateStatus != ENUM_SEATOPERATE_STATUS.Discard&& PlayerSeat.seatStatus!=ENUM_SEAT_STATUS.IDLE)
        //    {
        //        SendNotification("BtnGZ", null);              
        //    }
        //    else
        //    {
        //        SendNotification("BtnQP", null);                
        //    }
        //    if (PlayerSeat.pos == 7)
        //    {
        //        SendNotification("BtnQP", null);
        //    }
        //}       
    }
    #endregion

    #region 加载等待房间解散的人数界面
    /// <summary>
    /// 加载等待房间解散同意人数的界面
    /// </summary>
    /// <param name="count"></param>
    internal void AgreeDissolveCount(int count)
    {
        int agreeDissolveCount = count;
        TransferData data = new TransferData();
        data.SetValue("SetADHWindowSum", agreeDissolveCount);
        SendNotification(ZhaJHMethodname.OnZJHApplyForDissolution, data);//刷新当前同意人数
    }
    #endregion

    #region 加注方法
    /// <summary>
    /// 返回加注信息
    /// 座位号（pos） 加注分数（pour）
    /// </summary>
    /// <param name="pbSeat"></param>
    
    public void AddPour(ZJH_ROOM_ADD_POUR proto)
    {
        if (proto.pos == PlayerSeat.pos)
        {            
            SendNotification(ZhaJHMethodname.OnZJHBtnQP, null);
        }
        SeatEntity seat = GetSeatBySeatId(proto.pos);       
        if (seat.Gender==0)
        {
            AudioEffectManager.Instance.Play("zjh_nv_jiazhu", Vector3.zero);
        }
        else
        {
            AudioEffectManager.Instance.Play("zjh_nan_jiazhu", Vector3.zero);
        }
        //if (seat.pos == PlayerSeat.pos)
        //{
        //    SendNotification("BtnGZ",null);
        //}              
        if (proto.pokerstatus == ENUM_POKER_STATUS.POSITIVE)
        {
            //seat.gold -=  proto.pour * 2;
            seat.totalPour+= proto.pour * 2;
            //for (int i = 0; i < 2; i++)
            //{
                seat.betPoints = proto.pour * 2;               
                DicEvent("Seat", seat, ZhaJHMethodname.OnZJHSeatInfoGold);
           // }           
            UIItemZhaJHRoomInfo.Instance.SetBaseScoreUI(CurrentRoom.baseScore+= proto.pour * 2);           
        }
        else
        {
            //seat.gold -=  proto.pour;
            seat.betPoints =  proto.pour;
            seat.totalPour+= proto.pour;          
            UIItemZhaJHRoomInfo.Instance.SetBaseScoreUI(CurrentRoom.baseScore +=  proto.pour);
            DicEvent("Seat", seat, ZhaJHMethodname.OnZJHSeatInfoGold);
        }
        fen = proto.pour;             
        seat.seatStatus = ENUM_SEAT_STATUS.WAIT;
    }
    #endregion

    #region 跟注方法
    /// <summary>
    /// 跟注
    /// </summary>
    /// <param name="proto"></param>
    public void WithPour(ZJH_ROOM_FOLLOW_POUR proto)
    {
        if (proto.pos == PlayerSeat.pos)
        {
            SendNotification(ZhaJHMethodname.OnZJHBtnQP, null);
        }
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        if (seat.Gender == 0)
        {
            AudioEffectManager.Instance.Play("zjh_nv_genzhu", Vector3.zero);
        }
        else
        {
            AudioEffectManager.Instance.Play("zjh_nan_genzhu", Vector3.zero);
        }
        //if (seat.pos== PlayerSeat.pos)
        //{
        //    SendNotification("BtnGZ", null);          
        //}
        if (proto.pokerstatus == ENUM_POKER_STATUS.POSITIVE)
        {            
           // seat.gold -= proto.pour * 2 ;
            seat.totalPour += proto.pour * 2;
            seat.betPoints = proto.pour *2;           
            UIItemZhaJHRoomInfo.Instance.SetBaseScoreUI(CurrentRoom.baseScore += proto.pour * 2 );//跟新总分数           
            DicEvent("Seat",seat, ZhaJHMethodname.OnZJHSeatInfoGold);//更新分数，方法在玩家 Item 上
        }
        else
        {            
            //seat.gold -=  proto.pour ;
            seat.betPoints = proto.pour ;
            seat.totalPour += proto.pour;          
            UIItemZhaJHRoomInfo.Instance.SetBaseScoreUI(CurrentRoom.baseScore += proto.pour);
            DicEvent("Seat", seat, ZhaJHMethodname.OnZJHSeatInfoGold);
        }
        fen = proto.pour;
        seat.seatStatus = ENUM_SEAT_STATUS.WAIT;
    }
    #endregion

    #region 看牌方法
    /// <summary>
    /// 看牌
    /// </summary>
    /// <param name="proto"></param>
    public void LookPokerProxy(ZJH_ROOM_OPEN_LOOK_POCKER proto)
    {        
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        seat.zjhCardType = (ZJHCardType)proto.type;

        Debug.Log(seat.zjhCardType+"                               牌型是什么。。。。。。");
        if (CurrentRoom.roomStatus != ENUM_ROOM_STATUS.IDLE)
        {
            if (seat.Gender == 0)
            {
                AudioEffectManager.Instance.Play("zjh_nv_kanpai", Vector3.zero);
            }
            else
            {
                AudioEffectManager.Instance.Play("zjh_nan_kanpai", Vector3.zero);
            }
        }
        seat.zjhCardType = (ZJHCardType)proto.type;
        seat.pokerList.Clear();
        seat.pokerStatus = ENUM_POKER_STATUS.POSITIVE;
        int index = seat.Index;        
        for (int i = 0; i < proto.zjhPokerCount(); i++)
        {
            POKER poker = proto.getZjhPoker(i);
            seat.pokerList.Add(new Poker()
            {
                index = poker.index,//索引
                color = poker.color,//花色
                size = poker.size,//大小                    
            });
        }
       // TransformationPoker(seat,seat.pokerList);//判断牌型
        TransferData data = new TransferData();
        data.SetValue("totalRound", CurrentRoom.totalRound);
        data.SetValue("Seat", seat);
        data.SetValue("Fen", fen);
        SendNotification(ZhaJHMethodname.OnZJHHidKP, null);//隐藏看牌按钮
        SendNotification(ZhaJHMethodname.OnZJHSeatLookPoker, data);//看牌后实例化牌      
    }
    #endregion

    #region 判断牌型的方法
    /// <summary>
    /// 看牌的时候牌型是什么
    /// </summary>
    /// <param name="pokerList"></param>
    private void TransformationPoker(SeatEntity seat,List<Poker> pokerList)
    {
        bool isSize = false;
        for (int i = 0; i < pokerList.Count; i++)
        {
            if (pokerList[i].size == 14)
            {
                // pokerList[i].size = 1;
                isSize = true;               
            }
        }
        if (isSize)
        {
            if (seat.zjhCardType != ZJHCardType.ShunZi || seat.zjhCardType != ZJHCardType.WithFlowersShun)
            {
                pokerList.Sort((Poker poker1, Poker poker2) =>
                {
                    if (poker1.size < poker2.size) return -1;
                    return 1;
                });
            }
        }
        else
        {
            pokerList.Sort((Poker poker1, Poker poker2) =>
            {
                if (poker1.size < poker2.size) return -1;
                return 1;
            });
        }
    }
    #endregion

    #region 比牌
    /// <summary>
    /// 比牌
    /// </summary>
    public List<int> posList;
    public void ThenPokerProxy(ZJH_ROOM_COMPARE_POKER proto)
    {
        SendNotification(ZhaJHMethodname.OnZJHBtnQP, null);
        DicEvent("isbool", false, ZhaJHMethodname.OnZJHHidBP);
        if (PlayerSeat.pos != 7)
        {
            SendNotification("BtnGZ", null);
        }
        posList = new List<int>();
        CurrentRoom.roomStatus = ENUM_ROOM_STATUS.THECARD;

        SeatEntity discardSeat = GetSeatBySeatId(proto.pos);
        Debug.Log(proto.pos + "                                           比牌结果谁输了");
        for (int i = 0; i < proto.zjhCommonCount(); i++)
        {
            SEAT_COMMON protoSeat = proto.getZjhCommon(i);
            //if (protoSeat.pour == 0) continue;                  
            posList.Add(protoSeat.pos);
            SeatEntity seat = GetSeatBySeatId(protoSeat.pos);
            if (seat.pos == proto.getZjhCommon(0).pos)
            {
                if (seat.Gender == 0)
                {
                    AudioEffectManager.Instance.Play("zjh_nv_bipai", Vector3.zero);
                }
                else
                {
                    AudioEffectManager.Instance.Play("zjh_nan_bipai", Vector3.zero);
                }
            }
            seat.seatStatus = ENUM_SEAT_STATUS.THECARD;
            TransferData data = new TransferData();
            data.SetValue("DiscardSeat", discardSeat);
            data.SetValue("Seat", seat);
            SendNotification(ZhaJHMethodname.OnZJHTheCardAnimation, data);
            //DicEvent("Seat",seat, "TheCardAnimation");//比牌动画

            if (i==0)
            {                
                if (seat.pokerStatus == ENUM_POKER_STATUS.POSITIVE)
                {
                    // seat.gold -= protoSeat.pour * 2;                    
                    seat.betPoints = protoSeat.pour * 2;
                    seat.totalPour += protoSeat.pour * 2;                   
                    //SendSeatInfoChangeNotify(seat);
                    UIItemZhaJHRoomInfo.Instance.SetBaseScoreUI(CurrentRoom.baseScore += protoSeat.pour * 2);
                    DicEvent("Seat", seat, ZhaJHMethodname.OnZJHSeatInfoGold);
                }
                else
                {
                    // seat.gold -= protoSeat.pour;
                    seat.betPoints = protoSeat.pour;                  
                    seat.totalPour += protoSeat.pour;
                    //SendSeatInfoChangeNotify(seat);
                    UIItemZhaJHRoomInfo.Instance.SetBaseScoreUI(CurrentRoom.baseScore += protoSeat.pour);
                    DicEvent("Seat", seat, ZhaJHMethodname.OnZJHSeatInfoGold);
                }
            }
           


        }
        //DicEvent("isbool", false, ZhaJHMethodname.HidBP);
        //SendNotification("BtnGZ",null);
    }

    /// <summary>
    /// 派彩
    /// </summary>
    /// <param name="proto"></param>
    internal void SendMoneyProxy(ZJH_ROOM_SEND_MONEY proto)
    {
        return;
    }
    #endregion

    #region 弃牌
    /// <summary>
    /// 弃牌
    /// </summary>
    /// 
    public void LosePokerProxy(int pos)
    {
        Debug.Log(pos+"                               --------------------------------------------------------------弃牌座位号");
        ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHCloseCountTime);//关闭时间   
        ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHHidImage);//如果筹码点开 没有操作，自动弃牌的时候关闭筹码    
        DicEvent("isbool", false, ZhaJHMethodname.OnZJHHidBP);//弃牌的时候关闭比牌            
        SeatEntity seat = GetSeatBySeatId(pos);
        if (seat.seatStatus == ENUM_SEAT_STATUS.THECARD && seat == PlayerSeat)
        {
            AudioEffectManager.Instance.Play("zjh_bipai_bai", Vector3.zero);
        }
        else
        {
            if (seat.Gender == 0)
            {
                AudioEffectManager.Instance.Play("zjh_nv_buwan", Vector3.zero);
            }
            else
            {
                AudioEffectManager.Instance.Play("zjh_nan_buwan", Vector3.zero);
            }
        }
        // seat.seatStatus = ENUM_SEAT_STATUS.THECARD;
        seat.seatToperateStatus = ENUM_SEATOPERATE_STATUS.Discard;
        //SendSeatInfoChangeNotify(seat);
        TransferData data = new TransferData();
        data.SetValue("Seat",seat);
        SendNotification(ZhaJHMethodname.OnZJHDiscardMethod, data);
        if (pos == PlayerSeat.pos)
        {
            PlayerSeat.seatToperateStatus = ENUM_SEATOPERATE_STATUS.Discard;
            SendNotification(ZhaJHMethodname.OnZJHBtnQP, null);
        }
    }


    //public void LosePokerProxy(ZJH_ROOM_LOSE_POKER proto)
    //{        
    //    ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.CloseCountTime);//关闭时间   
    //    ModelDispatcher.Instance.Dispatch("HidImage");//如果筹码点开 没有操作，自动弃牌的时候关闭筹码                
    //    SeatEntity seat = GetSeatBySeatId(proto.pos);
    //    if (seat.seatStatus== ENUM_SEAT_STATUS.THECARD&&seat== PlayerSeat)
    //    {
            
    //        AudioEffectManager.Instance.Play("zjh_bipai_bai", Vector3.zero);
    //    }
    //    else
    //    {
    //        if (seat.Gender == 0)
    //        {
    //            AudioEffectManager.Instance.Play("zjh_nv_buwan", Vector3.zero);
    //        }
    //        else
    //        {
    //            AudioEffectManager.Instance.Play("zjh_nan_buwan", Vector3.zero);
    //        }
    //    }       
    //   // seat.seatStatus = ENUM_SEAT_STATUS.THECARD;
    //    seat.seatToperateStatus = ENUM_SEATOPERATE_STATUS.Discard;
    //    SendSeatInfoChangeNotify(seat);        
    //    if (proto.pos == PlayerSeat.pos)
    //    {
    //        PlayerSeat.seatToperateStatus= ENUM_SEATOPERATE_STATUS.Discard;
    //        SendNotification("BtnQP", null);          
    //    }     
    //}
    #endregion


    public void DicEvent<T>(string ValueStr,T isbool,string name)
    {
        TransferData data = new TransferData();
        data.SetValue(ValueStr, isbool);
        data.SetValue("RoomStatus", CurrentRoom.roomStatus);        
        SendNotification(name, data);        
    }

    #region 亮牌的方法
    /// <summary>
    /// 亮牌的方法
    /// </summary>
    /// <param name="proto"></param>
    public void LightLookProxy(ZJH_ROOM_LIGHTLOOK proto)
    {
        if (proto.pos==PlayerSeat.pos)
        {
            DicEvent("isLightPoker", false, ZhaJHMethodname.OnZJHLicensingShow);//亮牌后隐藏郎牌按钮
        }        
        int index = CurrentRoom.seatList[proto.pos - 1].Index;
        List<Poker> pokerList = new List<Poker>();
        pokerList.Clear();       
        for (int i = 0; i < proto.zjhPokerCount(); i++)
        {            
            POKER poker = proto.getZjhPoker(i);
            pokerList.Add(new Poker()
            {
                index = poker.index,//索引
                color = poker.color,//花色
                size = poker.size,//大小                    
            });
        }               
        SeatEntity seat = GetSeatBySeatId(proto.pos);
        TransformationPoker(seat, pokerList);
        TransferData data = new TransferData();
        data.SetValue("index", index);
        data.SetValue("pokerList", pokerList);
        data.SetValue("Seat",seat);
        SendNotification(ZhaJHMethodname.OnZJHCloseZhuangTai, data);
        //如果亮牌的人弃牌了。就实例化灰色的牌，否则实例化正常的牌
        if (seat.seatToperateStatus==ENUM_SEATOPERATE_STATUS.Discard)
        {
            if (seat!= PlayerSeat)
            {
                data.SetValue("spriteName", "failpoker");
                SendNotification(ZhaJHMethodname.OnZJHLookPoker, data);//执行看牌的方法，实例化牌  
            }                           
        }
        else
        {
            if (seat != PlayerSeat)
            {
                data.SetValue("spriteName", "normalpoker");
                SendNotification(ZhaJHMethodname.OnZJHLookPoker, data);//执行看牌的方法，实例化牌
            }               
        }      
    }
    #endregion

    #region 底分模式
    /// <summary>
    /// 底分模式
    /// </summary>
    /// <param name="proto"></param>
    internal void LowScoreProxy(ZJH_ROOM_LOW_SCORE proto)
    {        
        SeatEntity seat = GetSeatByPlayerId(proto.playerId);       
        seat.isLowScore = true;
        TransferData data = new TransferData();
        data.SetValue("Seat",seat);
        SendNotification(ZhaJHMethodname.OnZJHSeatLowScore, data);
    }
    #endregion

    #region 收益结算
    /// <summary>
    /// 收益结算
    /// </summary>
    /// <param name="proto"></param>
    public void SettleProxy(ZJH_ROOM_SETTLE proto)
    {        
        ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHCloseCountTime);//关闭时间
        SendNotification(ZhaJHMethodname.OnZJHBtnQP, null);
        LigthPoker(proto);
        CurrentRoom.roomStatus = ENUM_ROOM_STATUS.SETTLEMENT;        
       // List<SeatEntity> seatList = new List<SeatEntity>();
        for (int i = 0; i < proto.zjhSeatCount(); i++)
        {                                      
            SeatEntity seat = GetSeatBySeatId(proto.getZjhSeat(i).pos);                            
            seat.gold= proto.getZjhSeat(i).gold;
            
            seat.profit = proto.getZjhSeat(i).Profit;           
            if (seat.profit>0)
            {
                Debug.Log(seat.pos+"                                             胜利座位号");
                Debug.Log(seat.profit+"                                          胜利分数");
               //seat.homeLorder = true;
            }
            seat.totalProfit+= proto.getZjhSeat(i).Profit;           
            seat.PlayerId = proto.getZjhSeat(i).playerId;
            seat.seatToperateStatus = proto.getZjhSeat(i).opreateStatus;
            if (seat.seatStatus!=ENUM_SEAT_STATUS.IDLE)
            {
                seat.seatStatus = ENUM_SEAT_STATUS.SETTLEMENT;
            }           
            seat.isScore = true;
            seat.totalPour = 0; 
            LowScore(seat);                                              
            SendSeatInfoChangeNotify(seat);            
        }
        CurrentRoom.roomStatus = ENUM_ROOM_STATUS.IDLE;
        UIItemZhaJHRoomInfo.Instance.SetBaseScoreUI(CurrentRoom.baseScore = 0);
        fen = CurrentRoom.scores;
        btnGDD = false;
       // AoutReady();//如果玩家未准备，五秒自动准备        
    }
    #endregion


    #region 通知下一局
    /// <summary>
    /// 下一局
    /// </summary>
    public void NexeGameProxy()
    {
        SendNotification(ZhaJHMethodname.OnZJHInfoSettlement, null);//清空看牌或者失败的标记，清空所有的牌
        SendNotification("HidInfoSettlementView", null);
        DicEvent("isLightPoker", false, ZhaJHMethodname.OnZJHLicensingShow);
        UIItemZhaJHRoomInfo.Instance.SetBaseScoreUI(CurrentRoom.baseScore = 0);       
        if (CurrentRoom.roomSettingId!= RoomMode.Senior)
        {
            for (int i = 0; i < CurrentRoom.seatList.Count; i++)
            {
                if (CurrentRoom.seatList[i].PlayerId == 0) continue;
                //CurrentRoom.seatList[i].seatStatus = ENUM_SEAT_STATUS.IDLE;
                //CurrentRoom.seatList[i].seatToperateStatus = ENUM_SEATOPERATE_STATUS.GAME;
                CurrentRoom.seatList[i].zjhCardType = ZJHCardType.Nothing;
                CurrentRoom.seatList[i].seatToperateStatus = ENUM_SEATOPERATE_STATUS.SEAT_STATUS_IDLE;
                CurrentRoom.seatList[i].pokerStatus = ENUM_POKER_STATUS.OPPOSITE;
                CurrentRoom.seatList[i].betPoints = 0;
                CurrentRoom.seatList[i].profit = 0;
                SendSeatInfoChangeNotify(CurrentRoom.seatList[i]);
            }
            isShoppingIamge = false;

        }
        else
        {
            for (int i = 0; i < CurrentRoom.seatList.Count; i++)
            {
                InspectPlayerGame(CurrentRoom.seatList[i]);
                if (CurrentRoom.seatList[i].PlayerId == 0) continue;
                if (CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.THECARD || CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.BET || CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.WAIT)
                {
                    CurrentRoom.seatList[i].seatStatus = ENUM_SEAT_STATUS.DEAL;
                }
                //CurrentRoom.seatList[i].seatToperateStatus = ENUM_SEATOPERATE_STATUS.GAME;
                CurrentRoom.seatList[i].zjhCardType = ZJHCardType.Nothing;
                CurrentRoom.seatList[i].seatToperateStatus = ENUM_SEATOPERATE_STATUS.SEAT_STATUS_IDLE;
                CurrentRoom.seatList[i].pokerStatus = ENUM_POKER_STATUS.OPPOSITE;
                CurrentRoom.seatList[i].betPoints = 0;
            }
        }
        CurrentRoom.round = 0;//房间轮数        
        UIItemZhaJHRoomInfo.Instance.SetTotalRound(CurrentRoom.round, CurrentRoom.totalRound);
        SendNotification(ZhaJHMethodname.OnZJHHidFenG, null);//高级房开启加注按钮
    }
    #endregion


    private void LowScore(SeatEntity seat)
    {
        if (CurrentRoom.roomSettingId == RoomMode.Senior)
        {
            seat.isLowScore = false;
        }
    }

    #region 普通房游戏结束 总结算界面显示
    /// <summary>
    /// 普通房游戏结束 总结算界面显示
    /// </summary>
    /// <param name="proto"></param>
    internal void GameOverProxy(ZJH_ROOM_GAME_OVER proto)
    {
        if (CurrentRoom.currentLoop == 0) return;         
        List<SeatEntity> seatList = new List<SeatEntity>();        
        for (int i = 0; i < proto.zjhSeatCount(); i++)
        {
            if (proto.getZjhSeat(i).playerId == 0) continue;
            SEAT proto_seat = proto.getZjhSeat(i);
            SeatEntity seat = GetSeatBySeatId(proto.getZjhSeat(i).pos);
            seat.gold = proto_seat.gold;
            seat.Avatar = proto_seat.avatar;
            seat.Nickname = proto_seat.nickname;          
            seatList.Add(seat);
        }
        SortList(seatList);
        TransferData data = new TransferData();
        data.SetValue("seatList", seatList);
        data.SetValue("Room",CurrentRoom);
        data.SetValue("isGameOver", isGameOver);
        SendNotification(ZhaJHMethodname.OnZJHTotalSettlementView, data);
        //DicEvent("seatList", seatList, "TotalSettlementView");
    }
    #endregion


    /// <summary>
    /// 普通房  解散房间成功的结算
    /// </summary>
    /// <param name="proto"></param>
    internal void GameOverProxy1(ZJH_ROOM_DISMISS_SUCCEED proto)
    {
        DismissFailRoomProxy();//清空待解散界面
        List<SeatEntity> seatList = new List<SeatEntity>();
        for (int i = 0; i < proto.zjhSeatCount(); i++)
        {
            if (proto.getZjhSeat(i).playerId == 0) continue;
            SEAT proto_seat = proto.getZjhSeat(i);
            SeatEntity seat = GetSeatBySeatId(proto.getZjhSeat(i).pos);
            seat.gold = proto_seat.gold;
            seat.Avatar = proto_seat.avatar;
            seat.Nickname = proto_seat.nickname;
            seatList.Add(seat);
        }
        SortList(seatList);
        if (seatList.Count==0)
        {
            NetWorkSocket.Instance.SafeClose(GameCtrl.Instance.SocketHandle);
            SceneMgr.Instance.LoadScene(SceneType.Main);
        }
        else
        {
            DicEvent("seatList", seatList, ZhaJHMethodname.OnZJHTotalSettlementView);
        }        
    }
    private List<SeatEntity> SortList(List<SeatEntity> seatList)
    {
        seatList.Sort((SeatEntity seat1, SeatEntity seat2) =>
        {
            if (seat1.gold > seat2.gold)
            {
                return -1;
            }
            else return 1;
        });
        return seatList;
    }


    /// <summary>
    /// 高级房的时候检查玩家分数是不是可以游戏，如果不可以游戏
    /// 则把玩家状态改为空闲
    /// </summary>
    private void InspectPlayerGame(SeatEntity seat)
    {
        if (CurrentRoom.roomSettingId== RoomMode.Senior)
        {
            if ((seat.gold < 50&&CurrentRoom.scores==1)|| (seat.gold < 100 && CurrentRoom.scores == 2))
            {
                seat.seatStatus = ENUM_SEAT_STATUS.IDLE;
            }
        }      
    }

    /// <summary>
    /// 亮牌按钮显示
    /// </summary>
    /// <param name="proto"></param>
    private void LigthPoker(ZJH_ROOM_SETTLE proto)
    {
        for (int i = 0; i < proto.zjhSeatCount(); i++)
        {
            if (proto.getZjhSeat(i).nickname == null) continue;
            if (proto.getZjhSeat(i).pos == PlayerSeat.pos&&proto.getZjhSeat(i).pos!=7)
            {
                //if (PlayerSeat.pokerStatus== ENUM_POKER_STATUS.POSITIVE)
                //{
                if (PlayerSeat.seatStatus != ENUM_SEAT_STATUS.IDLE&&CurrentRoom.currentLoop!=CurrentRoom.maxLoop)
                {
                    DicEvent("isLightPoker", true, ZhaJHMethodname.OnZJHLicensingShow);
                }               
                   
               // }
            }
        }
    }

 
    /// <summary>
    /// 高级房离开玩家账单显示
    /// </summary>
    /// <param name="proto"></param>
    internal void SendBillProxy(ZJH_SEND_BILL proto)
    {
        
        SeatEntity seat = new SeatEntity();
        seat.PlayerId = proto.seat_bill.playerId;
        seat.gold = proto.seat_bill.pour;
        seat.Avatar = proto.seat_bill.avatar;
        seat.Nickname = proto.seat_bill.nickname;            
        TransferData data = new TransferData();
        data.SetValue("Seat",seat);
        SendNotification("BillView", data);

    }
    /// <summary>
    /// 高级房解散房间后房主的账单
    /// </summary>
    internal void OwnerBillProxy()
    {
        List<SeatEntity> seatList = new List<SeatEntity>();
        for (int i = 0; i < CurrentRoom.seatList.Count; i++)
        {
            if (CurrentRoom.seatList[i].PlayerId==0|| CurrentRoom.seatList[i].pos==7) continue;
            seatList.Add(CurrentRoom.seatList[i]);
        }
        if (seatList.Count>0)
        {
            if (seatList.Count==6)
            {
                seatList.Remove(seatList[5]);
            }
            SortList(seatList);          
            TransferData data = new TransferData();
            data.SetValue("seatList", seatList);
            data.SetValue("Room", CurrentRoom);
            SendNotification(ZhaJHMethodname.OnZJHTotalSettlementView, data);
        }
        else
        {
            ZhaJHGameCtrl.Instance.ExitGame();
            //ShowMessage("提示", "房间已解散!", MessageViewType.Ok, ExitGame);
        }

    }
    internal void DismissFailRoomProxy()
    {
        SendNotification(ZhaJHMethodname.OnZJHHideApplyForDissolution, null);//关闭等待解散的界面
    }

    ///// <summary>
    ///// 解散房间失败
    ///// </summary>
    //internal void DismissFailRoomProxy()
    //{        
    //    SendNotification("InfoSettlement", null);//清空看牌或者失败的标记，清空所有的牌
    //    SendNotification("HidInfoSettlementView", null);
    //    DicEvent("isLightPoker", false, ZhaJHMethodname.LicensingShow);
    //    SendNotification("HideApplyForDissolution", null);//关闭等待解散的界面
    //    UIItemZhaJHRoomInfo.Instance.SetBaseScoreUI(CurrentRoom.baseScore = 0);
    //    CurrentRoom.roomStatus = ENUM_ROOM_STATUS.IDLE;
    //    for (int i = 0; i < CurrentRoom.seatList.Count; i++)
    //    {

    //        if (CurrentRoom.seatList[i].PlayerId == 0) continue;
    //        //if (CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.DEAL || CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.THECARD || CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.BET || CurrentRoom.seatList[i].seatStatus == ENUM_SEAT_STATUS.WAIT)
    //        //{
    //        CurrentRoom.seatList[i].seatStatus = ENUM_SEAT_STATUS.IDLE;
    //        CurrentRoom.seatList[i].seatToperateStatus = ENUM_SEATOPERATE_STATUS.SEAT_STATUS_IDLE;
    //        CurrentRoom.seatList[i].pokerStatus = ENUM_POKER_STATUS.OPPOSITE;
    //        SendSeatInfoChangeNotify(CurrentRoom.seatList[i]);
    //        //}
    //    }
    //    CurrentRoom.roomStatus = ENUM_ROOM_STATUS.ROOMDISSOLUTION;

    //}

    /// <summary>
    /// 高级房通知房主有玩家进入的消息
    /// </summary>
    /// <param name="proto"></param>
    internal void ApplyEnterProxy(ZJH_ROOM_APPLY_ENTER proto)
    {       
        PlayerEntityZjh player = new PlayerEntityZjh();
        player.nickname = proto.player.nickname;
        player.playerId = proto.player.playerId;
        player.avatar = proto.player.avatar;
        TransferData data = new TransferData();
        data.SetValue("player", player);        
        SendNotification(ZhaJHMethodname.OnZJHCloneApplyJoin, data);
    }   
    /// <summary>
    /// 高级房的换庄
    /// </summary>
    /// <param name="proto"></param>
    internal void ChangeVillageProxy(ZJH_ROOM_CHANGE_VILLAGE proto)
    {       
        SeatEntity seat = GetSeatBySeatId(proto.zjh_seat.pos);             
        seat.seatToperateStatus = ENUM_SEATOPERATE_STATUS.SEAT_STATUS_IDLE;
        seat.pokerStatus = ENUM_POKER_STATUS.OPPOSITE;
        seat.seatStatus = ENUM_SEAT_STATUS.IDLE;
        seat.homeLorder = true;
        CurrentRoom.roomStatus = ENUM_ROOM_STATUS.IDLE;                   
        SendSeatInfoChangeNotify(seat);
    }
    /// <summary>
    /// 高级房通知房主取消进入房间
    /// </summary>
    /// <param name="proto"></param>
    public void WithdrawEnterRoom(ZJH_ROOM_WITHDRAW_ENTER proto)
    {
        DicEvent("playerId", proto.playerId, ZhaJHMethodname.OnZJHCleantRoom);//此方法在 UIApplyJoinZhaJHView 脚本上        
    }

    /// <summary>
    /// 高级房如果玩家只有一个玩家分数高于50或者100分则终止游戏
    /// </summary>
    internal void RoomStopProxy()
    {
        CurrentRoom.roomStatus = ENUM_ROOM_STATUS.IDLE;     
        for (int i = 0; i < CurrentRoom.seatList.Count; i++)
        {
            if (CurrentRoom.seatList[i].PlayerId!=0&& CurrentRoom.seatList[i].pos!=7)
            {
                CurrentRoom.seatList[i].seatStatus = ENUM_SEAT_STATUS.READY;
            }
            else
            {
                CurrentRoom.seatList[i].seatStatus = ENUM_SEAT_STATUS.IDLE;
            }                            
            CurrentRoom.seatList[i].seatToperateStatus = ENUM_SEATOPERATE_STATUS.SEAT_STATUS_IDLE;
            SendSeatInfoChangeNotify(CurrentRoom.seatList[i]);
        }
    }


    /// <summary>
    /// 离开房间
    /// </summary>
    /// <param name="pbPlayer"></param>

    public void ExitRoom(ZJH_ROOM_LEAVE proto)
    {        
        int counter = 0;
        SeatEntity seat = GetSeatByPlayerId(proto.playerId);
        seat.systemTime = 0;        
        seat.currentLoop = proto.maxLoop;                
        if (seat == null) return;               
        //普通房离开房间 
        if (CurrentRoom.roomSettingId != RoomMode.Senior)
        {
            seat.PlayerId = 0;
            SendSeatInfoChangeNotify(seat);
            AppDebug.Log(seat.Nickname + "离开房间");
            if (seat.pos == PlayerSeat.pos)
            {                
                NetWorkSocket.Instance.SafeClose(GameCtrl.Instance.SocketHandle);
                SceneMgr.Instance.LoadScene(SceneType.Main);
            }
        }
        ////高级房的时候退出
        else if (CurrentRoom.roomSettingId == RoomMode.Senior)
        {
            if (seat.pos == PlayerSeat.pos)
            {
                if (seat.currentLoop==0&&seat.pos==7)
                {
                    return;
                    //NetWorkSocket.Instance.SafeClose();
                    //SceneMgr.Instance.LoadScene(SceneType.Main);
                }
                else if (seat.currentLoop == 0 && seat.pos != 7)
                {
                    NetWorkSocket.Instance.SafeClose(GameCtrl.Instance.SocketHandle);
                    SceneMgr.Instance.LoadScene(SceneType.Main);
                }
                else
                {
                    DicEvent("Seat", seat, ZhaJHMethodname.OnZJHSettlementInterfaceView);//结算信息  
                }            
                
            }
            seat.PlayerId = 0;
            for (int i = 0; i < CurrentRoom.seatList.Count; i++)
            {
                if (CurrentRoom.seatList[i].PlayerId != 0)
                {
                    counter++;
                }
            }
            if (counter == 1 || counter == 2)
            {                         
                CurrentRoom.roomStatus = ENUM_ROOM_STATUS.IDLE;
                for (int i = 0; i < CurrentRoom.seatList.Count; i++)
                {
                    CurrentRoom.seatList[i].isScore = false;
                    CurrentRoom.seatList[i].seatToperateStatus = ENUM_SEATOPERATE_STATUS.SEAT_STATUS_IDLE;                                 
                    SendSeatInfoChangeNotify(CurrentRoom.seatList[i]);
                }
                SendNotification(ZhaJHMethodname.OnZJHInfoSettlement, null);
                DicEvent("isLightPoker", false, ZhaJHMethodname.OnZJHLicensingShow);
            }
            
            seat.isScore = false;
            seat.seatStatus = ENUM_SEAT_STATUS.Dropped;
            seat.homeLorder = false;
            SendSeatInfoChangeNotify(seat);          
            seat.seatStatus = ENUM_SEAT_STATUS.IDLE;
            seat.seatToperateStatus = ENUM_SEATOPERATE_STATUS.SEAT_STATUS_IDLE;
            seat.pokerStatus = ENUM_POKER_STATUS.OPPOSITE;                      
            AppDebug.Log(seat.Nickname + "离开房间");
        }
        fen = 0;
        btnGDD = false;
       
    }
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
    public SeatEntity GetSeatBySeatId(int seatId)
    {        
        if (CurrentRoom == null) return null;
        for (int i = 0; i < CurrentRoom.seatList.Count; ++i)
        {            
            if (CurrentRoom.seatList[i].pos == seatId)
            {                
                return CurrentRoom.seatList[i];
            }
        }
        return null;
    }

   

}
  


