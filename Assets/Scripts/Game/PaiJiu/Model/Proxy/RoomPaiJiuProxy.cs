//===================================================
//Author      : WZQ
//CreateTime  ：7/4/2017 5:58:33 PM
//Description ：牌九数据
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.paigow;
using PaiJiu;


#region 
#endregion

public class RoomPaiJiuProxy : ProxyBase<RoomPaiJiuProxy>
{

    public const string ON_BEGIN = "OnPaijiuBegin";

    #region Variable
    /// <summary>
    /// 当前房间数据实体
    /// </summary>
    public PaiJiu.Room CurrentRoom;
    /// <summary>
    /// 玩家座位数据实体
    /// </summary>
    public Seat PlayerSeat;
    /// <summary>
    /// 房间座位数量
    /// </summary>
    public const int ROOM_SEAT_COUNT = 4;

    /// <summary>
    /// 庄家位置
    /// </summary>
    public Seat BankerSeat=null;

    #endregion

    

    #region 初始化房间信息  构建房间
    /// <summary>
    /// 初始化房间信息  构建房间
    /// </summary>
    /// <param name="proto"></param>
    public void InitRoom(PAIGOW_ROOM protoRoom)   //------------------------使用协议类 接收全部信息  例骰子--------------------------------------------
    {
        BankerSeat = null;

        CurrentRoom = new PaiJiu.Room()
        {
            currentLoop = protoRoom.loop,
            roomId = protoRoom.roomId,
            //matchId = protoRoom.matchId,//比赛场
            roomStatus = protoRoom.room_status,
            maxLoop = protoRoom.maxLoop,
            mahJongSum = protoRoom.mahJongSum,
        };


        if (protoRoom.hasLoopEnd()) CurrentRoom.loopEnd = protoRoom.loopEnd;
        if (protoRoom.hasRemainMahjong()) CurrentRoom.remainMahjong = protoRoom.remainMahjong;
        Debug.Log("初始化房间信息  构建房间  CurrentRoom 房间ID" + CurrentRoom.roomId + "状态：" + CurrentRoom.roomStatus);
        
        
        //收到数据存到模型基类<------------------------------<保存配置>----------------------------------------
        CurrentRoom.Config.Clear();
        for (int i = 0; i < protoRoom.getSettingIdList().Count; i++)
        {
            cfg_settingEntity settingEntity = cfg_settingDBModel.Instance.Get(protoRoom.getSettingId(i));
            if (settingEntity != null)
            {
                CurrentRoom.Config.Add(settingEntity);
            }
        }

        //NiuNiu.RoomNiuNiuProxy
        //获得当前游戏模式
        for (int i = 0; i < CurrentRoom.Config.Count; i++)
        {
            if (CurrentRoom.Config[i].tags.Equals("banker"))
            {
                CurrentRoom.roomModel = (ROOM_MODEL)(CurrentRoom.Config[i].value-1);
                Debug.Log("服务器发送的房间 庄模式为为：" + CurrentRoom.Config[i].value);
            }
        }
        //------------------------------------------------骰子数据
        Debug.Log("-----剩余牌墙具体信息---protoRoom.hasMahjongs＿remain()--------" + protoRoom.hasMahjongs＿remain());
        //剩余牌墙具体信息
        if (protoRoom.hasMahjongs＿remain())
        {
            Debug.Log("-----剩余牌墙具体信息-----------" + protoRoom.getMahjongs＿remainList().Count);
            List<PAIGOW_MAHJONG> Mahjongs＿remain = protoRoom.getMahjongs＿remainList();
            CurrentRoom.pokerWall.Clear();
            for (int i = 0; i < Mahjongs＿remain.Count; i++)
            {
                Poker poker = new Poker();
                poker.SetPoker(Mahjongs＿remain[i]);
                CurrentRoom.pokerWall.Add(poker);
                Debug.Log("-----剩余牌墙具体信息-----------" + poker.ToChinese());
            }
        }

        //座位
        CurrentRoom.SeatList = new List<PaiJiu.Seat>();
        for (int i = 0; i < protoRoom.paigowSeatCount(); ++i)
        {
            PAIGOW_SEAT paijiu_Seat = protoRoom.getPaigowSeat(i);
           
            PaiJiu.Seat seat = new PaiJiu.Seat();

            AppDebug.Log(string.Format("手牌长度"+paijiu_Seat.paigowMahjongCount()));
            //创建牌 接收
            for (int j = 0; j < paijiu_Seat.paigowMahjongCount(); j++)
            {
                //添加空牌
                seat.PokerList.Add(new Poker((j + 1), 0, 0, PAIGOW_STATUS.HIDE));
            }

            for (int j = 0; j < paijiu_Seat.getPaigowMahjongList().Count; j++)
            {
                AppDebug.Log(string.Format("---------------------------手牌" + paijiu_Seat.getPaigowMahjongList()[j].type+"_"+ paijiu_Seat.getPaigowMahjongList()[j].size));

            }

            seat.SetSeat(paijiu_Seat);
            CurrentRoom.SeatList.Add(seat);
            if (paijiu_Seat.isBanker) BankerSeat = seat;//庄家Pos
            if (paijiu_Seat.isBanker) Debug.Log(string.Format("庄家pos:{0}  下注{1}", paijiu_Seat.pos, paijiu_Seat.pour));

        }

        //已操作解散房间座位
        if (protoRoom.hasOperatePosList())
        {
            for (int i = 0; i < protoRoom.getOperatePosListList().Count; i++)
            {
                CurrentRoom.OperatePosList.Add(protoRoom.getOperatePosListList()[i]);
            }
        } 
        //正在选庄座位
        if (protoRoom.hasChooseBankerPos()) CurrentRoom.ChooseBankerSeat = GetSeatBySeatId(protoRoom.chooseBankerPos);

        //（时间戳）
        if (protoRoom.hasUnixtime())
        {
            CurrentRoom.Unixtime = protoRoom.unixtime;
        }

        if (protoRoom.hasDealTime())
        {
            CurrentRoom.dealTime = protoRoom.dealTime;
        }

        CalculateSeatIndex(CurrentRoom);
        
        PeopleCounting();

        //同意房间解散人数
        //if (CurrentRoom.roomStatus == PAIGOW_ENUM_ROOM_STATUS.DISSOLVE)
        //{
        //    //计算当前同意解散人数
        //    ADHEnterRoom(protoRoom.getPaigowSeatList());

        //}

        //座位抢庄状态
        if (BankerSeat != null && CurrentRoom.roomStatus == ROOM_STATUS.GRABBANKER)
        {
            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {
                CurrentRoom.SeatList[i].isGrabBanker = 0;
            }

        }


        AppDebug.Log(string.Format("第一个玩家信息 位置{0} 庄{1} 转化索引{2} 玩家ID{3}", CurrentRoom.SeatList[0].Pos, CurrentRoom.SeatList[0].IsBanker, CurrentRoom.SeatList[0].Index, CurrentRoom.SeatList[0].PlayerId));
        AppDebug.Log(string.Format("自己玩家位置{0}", PlayerSeat.Pos));


    }
    #endregion

    #region CalculateSeatIndex 计算座位的客户端序号
    /// <summary>
    /// 计算座位的客户端序号
    /// </summary>
    /// <param name="room">房间信息</param>
    private void CalculateSeatIndex(PaiJiu.Room room)
    {
        PlayerSeat = null;
        if (room == null) return;
        for (int i = 0; i < room.SeatList.Count; ++i)
        {
            if (room.SeatList[i].PlayerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
            {
                PlayerSeat = room.SeatList[i];
                for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
                {
                    PaiJiu.Seat seat = room.SeatList[j];
                    int seatIndex = seat.Pos - PlayerSeat.Pos;
                    seatIndex = seatIndex < 0 ? seatIndex + ROOM_SEAT_COUNT : seatIndex;
                    seat.Index = seatIndex;
                    if (CurrentRoom.SeatList.Count == 2)
                    {
                        if (seat.Index != 0)
                        {
                            seat.Index = 2;
                        }
                    }
                }
                break;
            }
        }
        if (PlayerSeat == null)
        {
            PlayerSeat = room.SeatList[0];
            for (int j = 0; j < CurrentRoom.SeatList.Count; ++j)
            {
                PaiJiu.Seat seat = room.SeatList[j];
                int seatIndex = seat.Pos - PlayerSeat.Pos;
                seatIndex = seatIndex < 0 ? seatIndex + ROOM_SEAT_COUNT : seatIndex;
                seat.Index = seatIndex;
                if (CurrentRoom.SeatList.Count == 2)
                {
                    if (seat.Index != 0)
                    {
                        seat.Index = 2;
                    }
                }
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

    #region ADHEnterRoom
    //初始计算当前同意解散房间人数
    /// <summary>
    /// 
    /// </summary>
    /// <param name="nn_seatList"></param>
    private void ADHEnterRoom(List<PAIGOW_SEAT> paijiu_Seat)
    {

        CurrentRoom.agreeDissolveCount = 0;


        int sum = 0;
        for (int i = 0; i < paijiu_Seat.Count; i++)
        {
            if (paijiu_Seat[i].hasIsDismiss ())
            {
                if (paijiu_Seat[i].isDismiss)
                {
                    sum++;
                }

            }



        }
        CurrentRoom.agreeDissolveCount = sum;




    }
    #endregion

    #region EnterRoom 进入房间
    /// <summary>
    /// 进入房间
    /// </summary>
    /// <param name="pbSeat"></param>
    public void EnterRoom(PAIGOW_ROOM_ENTER proto)
    {
        Debug.Log(proto.pos + "进入房间");
        PaiJiu.Seat seat = GetSeatBySeatId(proto.pos);
        if (seat == null) return;
        seat.PlayerId = proto.playerId;
        seat.Nickname = proto.nickname;
        seat.Avatar = proto.avatar;
        seat.Gender = proto.gender;
        seat.Pos = proto.pos;

        //seat.Gold = proto.gold;            //<-----------------------------------《玩家基本金币》
        PeopleCounting();
        SendSeatInfoChangeNotify(seat);
#if IS_ZHANGJIAKOU
       if(BankerSeat!=null) SendSeatInfoChangeNotify(BankerSeat);
#endif
        AppDebug.Log(seat.Nickname + "进入房间,SeatIndex:"+seat.Index);
    }
    #endregion

    #region ExitRoom 离开房间
    /// <summary>
    /// 离开房间
    /// </summary>
    /// <param name="proto"></param>
    public void ExitRoom(PAIGOW_ROOM_LEAVE proto)
    {
        PaiJiu.Seat seat = GetSeatByPlayerId(proto.playerId);
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

    #region Ready 准备
    /// <summary>
    /// 准备
    /// </summary>
    /// <param name="proto"></param>
    public void Ready(PAIGOW_ROOM_READY proto)
    {
        if (CurrentRoom.roomStatus == ROOM_STATUS.IDLE) CurrentRoom.roomStatus = ROOM_STATUS.READY;
        PaiJiu.Seat seat = GetSeatBySeatId(proto.pos);
        if (seat == null) return;
        seat.seatStatus = SEAT_STATUS.SEAT_STATUS_READY;
        //seat.IsTrustee = false;


        //需要庄家点开始
//#if IS_ZHANGJIAKOU
        SendRoomInfoChangeNotify();
//#else
//        SendSeatInfoChangeNotify(seat);
//#endif
        AppDebug.Log(seat.Nickname + "准备");
    }
    #endregion

    #region GameStart 通知开始游戏
    public void GameStart(PAIGOW_ROOM_GAMESTART proto)
    {
        if (proto.hasLoop())
        {
            CurrentRoom.currentLoop = proto.loop;

            CurrentRoom.loopEnd = false;
            CurrentRoom.isCutPan = false;
            CurrentRoom.isBombPan = false;
            //刷新房间局数
            TransferData roomData = new TransferData();
            roomData.SetValue("Room", CurrentRoom);
            SendNotification(ConstDefine_PaiJiu.ObKey_RoomInfoChanged, roomData);//房间信息改变
        }
    }
    #endregion

    #region NoticeJetton 通知下注
    /// <summary>
    /// 下注
    /// </summary>
    /// <param name="proto"></param>
    public void NoticeJetton(PAIGOW_ROOM_INFORM_JETTON proto)
    {
        CurrentRoom.roomStatus = ROOM_STATUS.POUR;
        CurrentRoom.loopEnd = false;
        List<int> pourPosList = proto.getPosList();
        for (int i = 0; i < pourPosList.Count; i++)
        {
            PaiJiu.Seat seat = GetSeatBySeatId(pourPosList[i]);
            if (seat == null) continue;
           
            seat.Pour = 0;
     
                AppDebug.Log(seat.Nickname + "开始下注");

            //提示该谁下注
            //设置倒计时
            SetCountDown(proto.unixtime);
            //SendSeatInfoChangeNotify(seat);
            //SendRoomInfoChangeNotify();

        }

        // 1 判断自己是否下注
        // 2 控制提示
        SendRoomInfoChangeNotify();



    }
    #endregion

    #region JETTON 下注某分
    /// <summary>
    /// 下注某分
    /// </summary>
    /// <param name="proto"></param>
    public void Jetton(PAIGOW_ROOM_JETTON proto)
    {
        PaiJiu.Seat seat = GetSeatBySeatId(proto.pos);
        if (seat == null) return;
 
       if(proto.hasPour()) seat.Pour = proto.pour;
     
        //显示其下注内容
        SendRoomInfoChangeNotify();
     
        AppDebug.Log(seat.Nickname + "下注了,下注分："+seat.Pour);
    }
    #endregion

    #region Begin 开局发牌
    /// <summary>
    /// 开局发牌
    /// </summary>
    /// <param name="proto"></param>
    public void Begin(PAIGOW_ROOM_BEGIN proto)
    {
        AppDebug.Log("发牌");
        CurrentRoom.roomStatus =ROOM_STATUS.LOOP;
        CurrentRoom.remainMahjong = proto.remainMahjong;//剩余牌数量

        //设置骰子数据
        CurrentRoom.SetDiceEntity(proto.firstGivePos, proto.diceFirst, proto.diceSecond);
        Debug.Log("-----剩余牌墙具体信息-----------" + proto.hasMahjongs＿remain());
        //剩余牌墙具体信息
        if (proto.hasMahjongs＿remain())
        {
            Debug.Log("-----剩余牌墙具体信息-----------" + proto.getMahjongs＿remainList().Count);
            List<PAIGOW_MAHJONG> Mahjongs＿remain = proto.getMahjongs＿remainList();
            CurrentRoom.pokerWall.Clear();
            for (int i = 0; i < Mahjongs＿remain.Count; i++)
            {
                Poker poker = new Poker();
                poker.SetPoker(Mahjongs＿remain[i]);
                CurrentRoom.pokerWall.Add(poker);
                Debug.Log("-----剩余牌墙具体信息-----------" + poker.ToChinese());
            }
        }

        //设置数据

        //其他人牌为空
        //int HandPokerCount = proto.paigow_seat.getPaigowMahjongList().Count;

        for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
        {
            if (CurrentRoom.SeatList[i].PlayerId > 0)
            {
                //CurrentRoom.SeatList[i].PokerList.Clear();
                for (int j = 0; j < 2; j++)
                {
                    //添加空牌
                if(CurrentRoom.SeatList[i].PokerList.Count < 2)    CurrentRoom.SeatList[i].PokerList.Add(new Poker((j + 1), 0, 0, PAIGOW_STATUS.HIDE));

                }

            }

        }


        //接收牌
        for (int i = 0; i < proto.getPaigowSeatList().Count ; i++)
        {

            Seat seat = GetSeatBySeatId(proto.getPaigowSeatList()[i].pos);
            if (seat == null) continue;

            seat.SetSeat(proto.getPaigowSeatList()[i]);
        }

     

        //设置倒计时
        SetCountDown(proto.unixtime);
        //发送消息
        SendRoomInfoChangeNotify();

        //sceneCtrl 开局
        TransferData data = new TransferData();
        data.SetValue("SeatPos", PlayerSeat.Pos);
        data.SetValue("SeatCount", CurrentRoom.SeatList.Count);
        //data.SetValue("SeatDirection", PlayerSeat.Direction);
        SendNotification(ON_BEGIN, data);



    }

    public void Begin()
    {
        CurrentRoom.roomStatus = ROOM_STATUS.BEGIN;
        SendRoomInfoChangeNotify();
    }
    #endregion

    #region OnServerOpenPoker 翻牌
    /// <summary>
    /// 某人翻开某牌
    /// </summary>
    /// <param name="proto"></param>
    public void OnServerOpenPoker(PAIGOW_ROOM_DRAW proto)
    {
        AppDebug.Log(string.Format("开牌内容长度:{0} 座位号:{1}", proto.paigowMahjongCount(), proto.pos));
        PaiJiu.Seat seat = GetSeatBySeatId(proto.pos);
       List< PAIGOW_MAHJONG> protoMajiangList = proto.getPaigowMahjongList();
        for (int i = 0; i < protoMajiangList.Count; i++)
        {
              AppDebug.Log(string.Format("开牌信息:index:{0} size{1} type{2}", protoMajiangList[i].index, protoMajiangList[i].size, protoMajiangList[i].type));
            for (int j = 0; j <  seat.PokerList.Count; j++)
            {
                if (protoMajiangList[i]!=null && protoMajiangList[i].index== seat.PokerList[j].index)
                {
                    seat.PokerList[j].SetPoker(protoMajiangList[i]);
                    AppDebug.Log(string.Format("设置手牌数据:index:{0} size{1} type{2}", protoMajiangList[i].index, protoMajiangList[i].size, protoMajiangList[i].type));
                    continue;
                }

            }
        }     

    }


    #endregion

    #region OnServerResult 结算
    /// <summary>
    ///  结算
    /// </summary>
    /// <param name="proto"></param>
    public void OnServerResult(PAIGOW_ROOM_OPENPOKERRESULT proto)
    {
        PAIGOW_ROOM room = proto.paigow_room;
         
        CurrentRoom.isCutPan = false;
        CurrentRoom.SetRoom(room);
        CurrentRoom.roomStatus = ROOM_STATUS.SETTLE;
    
        for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
        {
            Debug.Log(string.Format("服务器发送{0}总收益{1}", CurrentRoom.SeatList[i].Pos, CurrentRoom.SeatList[i].TotalEarnings));
        }

        //设置倒计时
       if(room.hasUnixtime()) SetCountDown(room.unixtime);
        //刷新房间 座位信息
        SendRoomInfoChangeNotify();
    }


    #endregion

    #region OnServerNextGame 清空数据 准备下一次
    public void OnServerNextGame()
    {
        CurrentRoom.pokerWall.Clear();
        //清空手牌
        for (int i = 0; i <CurrentRoom.SeatList.Count; i++)
        {

            for (int j = 0; j < CurrentRoom.SeatList[i].PokerList.Count; j++)
            {
                CurrentRoom.SeatList[i].TablePokerList.Add(CurrentRoom.SeatList[i].PokerList[j]);
            }
            CurrentRoom.SeatList[i].PokerList.Clear();
            if (!CurrentRoom.SeatList[i].IsBanker) CurrentRoom.SeatList[i].Pour = 0;
            if (CurrentRoom.loopEnd && CurrentRoom.SeatList[i].IsBanker)
            {
                CurrentRoom.SeatList[i].Pour = 0;
            }
     
        }


        //刷新房间 座位信息
        SendRoomInfoChangeNotify();



    }
    #endregion

    #region OnServerChooseBanker   选庄
    public void OnServerChooseBanker(PAIGOW_ROOM_CHOOSEBANKER proto)
    {
        AppDebug.Log(string.Format("通知选庄 座位号：{0}", proto.pos));
        for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
        {
            if (CurrentRoom.SeatList[i] != null && CurrentRoom.SeatList[i].IsBanker)
            {
                CurrentRoom.SeatList[i].IsBanker = false;
            }
        }


            Seat seat = GetSeatBySeatId(proto.pos);

        if (proto.hasIsBanker())
        {
            //说明选庄成功
            CurrentRoom.roomStatus = ROOM_STATUS.IDLE;
            BankerSeat = seat;
            seat.IsBanker = true;
            CurrentRoom.ChooseBankerSeat = null;
           
        }
        else
        {
            //否则就是通知谁在选庄
            BankerSeat = null;
            CurrentRoom.roomStatus = ROOM_STATUS.CHOOSEBANKER;
            CurrentRoom.ChooseBankerSeat = seat;
            //设置由谁选庄 UIScenePaiJiuView

        }

        for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
        {
            if (CurrentRoom.SeatList[i].PlayerId > 0)
            {
                //显示庄
                TransferData data = new TransferData();
                data.SetValue("seat", CurrentRoom.SeatList[i]);
                ModelDispatcher.Instance.Dispatch(ConstDefine_PaiJiu.ObKey_SetBankerAni, data);

            }
        }


        SendRoomInfoChangeNotify();




    }

    #endregion

    #region OnServerChangeBanker 通知换庄
    public void OnServerChangeBanker(PAIGOW_ROOM_CHANGEBANKER proto)
    {
        CurrentRoom.roomStatus = ROOM_STATUS.READY;
        if (proto.hasPos())
        {
        Seat seat = GetSeatBySeatId(proto.pos);
            if (seat != null)
            {
                for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
                {
                    CurrentRoom.SeatList[i].IsBanker = false;
                }
                seat.IsBanker = true;
                BankerSeat = seat;

                for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
                {
                //显示庄
                TransferData data = new TransferData();
                data.SetValue("seat", CurrentRoom.SeatList[i]);
                ModelDispatcher.Instance.Dispatch(ConstDefine_PaiJiu.ObKey_SetBankerAni, data);

                }


            }
        }

        SendRoomInfoChangeNotify();
    }
    #endregion

    #region OnServerRobBanker 抢庄
    public void OnServerRobBanker(PAIGOW_ROOM_GRABBANKER proto)
    {

        if (proto.hasPos())
        {
            Seat seat = GetSeatBySeatId(proto.pos);

            if (proto.hasIsGrabBanker())
            {
                //某玩家抢庄
                if (seat != null)
                {
                    //seat //是否抢庄
                    seat.isGrabBanker = proto.isGrabBanker;                   
                }
            }
            else
            {
                //抢庄结束
                //说明选庄成功
                //CurrentRoom.roomStatus = ROOM_STATUS.IDLE;

                Debug.Log("抢到庄的是pos:" + seat.Pos);

                seat.IsBanker = true;
                BankerSeat = seat;
                for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
                {
                  
                    CurrentRoom.SeatList[i].isGrabBanker = 0;
                }

                //设置骰子数据
                CurrentRoom.SetDiceEntity(proto.pos, proto.diceFirst, proto.secondDice);

            }
        }
        else
        {
            if (!proto.hasIsGrabBanker())
            {
                //通知抢庄
                BankerSeat = null;

                for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
                {
                    CurrentRoom.SeatList[i].IsBanker = false;
                    CurrentRoom.SeatList[i].isGrabBanker = 3;

                    //显示庄
                    TransferData data = new TransferData();
                    data.SetValue("seat", CurrentRoom.SeatList[i]);
                    ModelDispatcher.Instance.Dispatch(ConstDefine_PaiJiu.ObKey_SetBankerAni, data);
                }

                CurrentRoom.roomStatus = ROOM_STATUS.GRABBANKER;

            }
        }
        

        SendRoomInfoChangeNotify();

    }

    #endregion

    #region OnServerCutPoker 切牌数据处理
    public void OnServerCutPoker(PAIGOW_ROOM_CUTPOKER proto)
    {

        if (proto.hasPos())
        {
            Seat seat = GetSeatBySeatId(proto.pos);

            if (proto.hasIsCutPoker())
            {
                Debug.Log("某玩家确认是否切牌 pos:"+seat.Pos);
                //某玩家确认是否切牌
                seat.isCutPoker = (Seat.CutPoker) proto.isCutPoker;

            }
            else
            {
                Debug.Log("服务器询问是否需要切牌 pos:"+seat.Pos);

                for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
                {
                    CurrentRoom.SeatList[i].isCutPoker = Seat.CutPoker.None;
                }

                CurrentRoom.roomStatus = ROOM_STATUS.CUTPOKER;
                //询问是否需要切牌     
                seat.isCutPoker = Seat.CutPoker.IsNotOperating;
            }

            if (proto.hasCutPokerIndex())
            {
                Debug.Log("某玩家操作切牌信息 pos:" + seat.Pos);
                //某玩家操作切牌信息
                seat.isCutPoker = Seat.CutPoker.IsCut;

            }

            SendSeatInfoChangeNotify(seat);

        }




    }

    #endregion

    #region OnServerCutGuo 切锅数据处理
    public void OnServerCutGuo(PAIGOW_ROOM_CUTPAN proto)
    {
        if (proto.hasPos())
        {
            Seat seat = GetSeatBySeatId(proto.pos);
            if (seat == null) return;
            if (proto.hasIsCutGuo())
            {
                seat.isCutGuo = proto.isCutGuo ? 1 : 2;
            }
            else
            {
                //通知切锅
                seat.isCutGuo = 3;
            }
            SendSeatInfoChangeNotify(seat);

        }
    }
    #endregion

    #region RoomDisband 房间解散成功
    public void RoomDisband()
    {
        if (BankerSeat != null)
        {
            BankerSeat.TotalEarnings = 0;
            BankerSeat.Pour = 0;
            for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
            {
                if (CurrentRoom.SeatList[i].PlayerId > 0 && !CurrentRoom.SeatList[i].IsBanker)
                {
                    BankerSeat.TotalEarnings -= CurrentRoom.SeatList[i].TotalEarnings;
                }

            }

           


        }
    }
    #endregion

    #region RoomResult 总结算房间信息
    public void RoomResult(PAIGOW_ROOM room)
    {
        
        CurrentRoom.SetRoom(room);
        int maxScore = 0;
        for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
        {
            if (CurrentRoom.SeatList[i].PlayerId > 0 && CurrentRoom.SeatList[i].TotalEarnings > maxScore) maxScore = CurrentRoom.SeatList[i].TotalEarnings;
        }
        for (int i = 0; i < CurrentRoom.SeatList.Count; i++)
        {
            if (CurrentRoom.SeatList[i].PlayerId > 0)
                CurrentRoom.SeatList[i].isBigWinner = (CurrentRoom.SeatList[i].TotalEarnings > 0 && CurrentRoom.SeatList[i].TotalEarnings >= maxScore);

        }


    }

    #endregion

    #region GetSeat 获取座位实体
    /// <summary>
    /// 根据玩家Id获取座位
    /// </summary>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public PaiJiu.Seat GetSeatByPlayerId(int playerId)
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
    public PaiJiu.Seat GetSeatBySeatId(int seatId)
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
    public PaiJiu.Seat GetSeatBySeatIndex(int seatIndex)
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
        SendNotification(ConstDefine_PaiJiu.ObKey_RoomInfoChanged, roomData);//房间信息改变

        for (int i = 0; i < CurrentRoom.SeatList.Count; ++i)
        {
            SendSeatInfoChangeNotify(CurrentRoom.SeatList[i]);   //------------------------座位信息改变--------------------------------
        }
    }
    #endregion

    #region SendSeatInfoChangeNotify 发送座位信息变更消息
    /// <summary>
    /// 发送座位信息变更消息
    /// </summary>
    /// <param name="seat"></param>
    private void SendSeatInfoChangeNotify(PaiJiu.Seat seat)
    {
        if (seat == null) return;

        TransferData data = new TransferData();
        data.SetValue("Seat", seat);
        data.SetValue("IsPlayer", seat == PlayerSeat);
        data.SetValue("RoomStatus", CurrentRoom.roomStatus);
        data.SetValue("CurrentRoom", CurrentRoom);
        data.SetValue("BankerSeat", BankerSeat);
        data.SetValue("ChooseBankerSeat", CurrentRoom.ChooseBankerSeat);
        SendNotification(ConstDefine_PaiJiu.ObKey_SeatInfoChanged, data); 
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