//===================================================
//Author      : CZH
//CreateTime  ：9/1/2017 2:28:53 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.gp;
using GuPaiJiu;

public class GuPaiJiuGameCtrl : SystemCtrlBase<GuPaiJiuGameCtrl>, IGameCtrl, ISystemCtrl
{
    /// <summary>
    /// 结算窗口UI
    /// </summary>
    private UIGuPaiJiuResultView m_UIGuPaiJiuResultView; 
    /// <summary>
    /// 解散房间成功发的结算
    /// </summary>
    private GP_ROOM_TOTALSETTLE m_GuPaiJiuResult;
    /// <summary>
    /// 当前进入房间类型
    /// </summary>
    private EnterRoomType m_CurrentType = EnterRoomType.Renter;
    /// <summary>
    /// 当前加入的房间Id
    /// </summary>
    private int m_nCurrentJoinRoomId;
    /// <summary>
    /// 是否是内网
    /// </summary>
    private bool isIntranet=true;

    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add(ConstantGuPaiJiu.GuPaiJiuClientSendReady, GuPaiJiuClientSendReady);//客户端发送准备
        dic.Add(ConstantGuPaiJiu.GuPaiJiuClientSendGameStart, GuPaiJiuClientSendGameStart);//客户端发送开始游戏
        dic.Add(ConstantGuPaiJiu.GuPaiJiuClientSendBottomPour, GuPaiJiuClientSendBottomPour);//客户端发送下注分数
        dic.Add(ConstantGuPaiJiu.GuPaiJiuClientSendGroupPoker, GuPaiJiuClientSendGroupPoker);//客户端发送组合牌
        dic.Add(ConstantGuPaiJiu.GuPaiJiuClientEmptyReceive, GuPaiJiuClientEmptyReceive);//客户端发送发牌结束
        dic.Add(ConstantGuPaiJiu.GuPaiJiuClientSendInformNext, GuPaiJiuClientSendInformNext);//客户端通知开始下一次
        dic.Add(ConstantGuPaiJiu.OnBtnResultViewGuPaiJiuShareClick, OnBtnResultViewGuPaiJiuShareClick);//微信分享
        dic.Add(ConstantGuPaiJiu.GuPaiJiuClisentSendAutoGroupPoker, GuPaiJiuClisentSendAutoGroupPoker);//自动组合牌
        dic.Add(ConstantGuPaiJiu.GuPaiJiuClisentSendPokerWallEnd, GuPaiJiuClisentSendPokerWallEnd);//牌墙生成完毕
        dic.Add(ConstantGuPaiJiu.GuPaiJiuClisentSendCutPoker, GuPaiJiuClisentSendCutPoker);//客户端发送切牌
        dic.Add(ConstantGuPaiJiu.btnGuPaiJiuViewShare, OnBtnGuPaiJiuViewShareClick);//微信邀请
        dic.Add("OnHeadGuPaiJiuClick", OnHeadGuPaiJiuClick);//头像点击
        dic.Add("ReturnHall", ReturnHall);//游戏结束，返回大厅

        return dic;
    }

    public override void Dispose()
    {
        base.Dispose();
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_CREATE.CODE, OnServerReturnCreateRoom);//创建房间
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_ENTER.CODE, OnServerBroadcastEnter);//加入房间
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_READY.CODE,OnServerBroadcastReady);//准备
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_GAMESTART.CODE,OnServerBroadcastGameStart);//开始游戏        
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_INFORMJETTON.CODE,OnServerBroadcastBet);//通知下注
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_BOTTOMPOUR.CODE, OnServerBroadcastBottomPour);//下注
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_BEGIN.CODE, OnServerBroadcastBegin);//开始发空牌
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_VALIDDEAL.CODE, OnServerBroadcastValidDeal);//发实牌   
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_GROUPPOKER.CODE, OnServerBroadcastGroup);//组合牌  
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_SETTLE.CODE, OnServerBroadcastSettle);//每次结算  
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_LOOPSETTLE.CODE, OnServerBroadcastLoopSettle);//每局结算
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_RECREATE.CODE, OnServerBroadcastRecreate);//断线重连
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_INFORMBANKER.CODE, OnServerBroadcastInfoRebanker);//服务器通知坐庄
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_APPLYDISMISS.CODE, OnServerBroadcastRoomApplydisMiss);//服务器广播申请解散房间的消息
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_TOTALSETTLE.CODE, OnServerReturnResult);//解散房间成功的结算
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_GAMEOVER.CODE, OnServerBroadcastGameOver);//游戏结束
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播离开的消息
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_AFK.CODE, OnServerBroadcastRoomAfk);//服务器广播玩家掉线
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_CUTPOKER.CODE, OnServerBroadcastTellCutPoker);//服务器广播切牌
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_CUTPOKERDONE.CODE, OnServerBroadcastCutPokerEnd);//服务器广播切牌完毕
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_GRABBANKER.CODE, OnServerBroadcastGrabBanker);//服务器广播开始抢庄
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_BUILDPOKERWALL.CODE, OnServerBroadcastBuildPokerWall);//生成牌墙
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_CLEARDESKTOP.CODE, OnServerBroadcastCleardesktop);//清空桌面
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_DRAWPOKER.CODE, OnServerBroadcastDrawPoker);//服务器广播翻牌
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_INFORMBANKERDRAW.CODE, OnServerBroadcastIsBankeDraw);//服务器广播通知庄家翻牌
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_CUTPAN.CODE, OnServerBroadcastIsCutpan);//服务器通知切锅
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_CUOPAI.CODE,OnServerBroadCuoPai);//小牌九搓牌
        NetDispatcher.Instance.RemoveEventListener(GP_ROOM_GROUPMAXPOKERPHINT.CODE, OnServerBroadPromptPoker);//提示

        //NetWorkSocket.Instance.OnDisconnect += OnDisConnectCallBack;//断开连接事件

    }



    public GuPaiJiuGameCtrl()
    {
        NetDispatcher.Instance.AddEventListener(GP_ROOM_CREATE.CODE, OnServerReturnCreateRoom);//创建房间
        NetDispatcher.Instance.AddEventListener(GP_ROOM_ENTER.CODE, OnServerBroadcastEnter);//加入房间
        NetDispatcher.Instance.AddEventListener(GP_ROOM_READY.CODE, OnServerBroadcastReady);//准备
        NetDispatcher.Instance.AddEventListener(GP_ROOM_GAMESTART.CODE, OnServerBroadcastGameStart);//开始游戏        
        NetDispatcher.Instance.AddEventListener(GP_ROOM_INFORMJETTON.CODE, OnServerBroadcastBet);//通知下注
        NetDispatcher.Instance.AddEventListener(GP_ROOM_BOTTOMPOUR.CODE, OnServerBroadcastBottomPour);//下注
        NetDispatcher.Instance.AddEventListener(GP_ROOM_BEGIN.CODE, OnServerBroadcastBegin);//开始发空牌
        NetDispatcher.Instance.AddEventListener(GP_ROOM_VALIDDEAL.CODE, OnServerBroadcastValidDeal);//发实牌   
        NetDispatcher.Instance.AddEventListener(GP_ROOM_GROUPPOKER.CODE, OnServerBroadcastGroup);//组合牌  
        NetDispatcher.Instance.AddEventListener(GP_ROOM_SETTLE.CODE, OnServerBroadcastSettle);//每次结算  
        NetDispatcher.Instance.AddEventListener(GP_ROOM_LOOPSETTLE.CODE, OnServerBroadcastLoopSettle);//每局结算
        NetDispatcher.Instance.AddEventListener(GP_ROOM_RECREATE.CODE, OnServerBroadcastRecreate);//断线重连
        NetDispatcher.Instance.AddEventListener(GP_ROOM_INFORMBANKER.CODE, OnServerBroadcastInfoRebanker);//服务器通知坐庄
        NetDispatcher.Instance.AddEventListener(GP_ROOM_APPLYDISMISS.CODE, OnServerBroadcastRoomApplydisMiss);//服务器广播申请解散房间的消息
        NetDispatcher.Instance.AddEventListener(GP_ROOM_TOTALSETTLE.CODE, OnServerReturnResult);//解散房间成功的结算
        NetDispatcher.Instance.AddEventListener(GP_ROOM_GAMEOVER.CODE, OnServerBroadcastGameOver);//游戏结束
        NetDispatcher.Instance.AddEventListener(GP_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播离开的消息
        NetDispatcher.Instance.AddEventListener(GP_ROOM_AFK.CODE, OnServerBroadcastRoomAfk);// 服务器广播玩家掉线
        NetDispatcher.Instance.AddEventListener(GP_ROOM_CUTPOKER.CODE, OnServerBroadcastTellCutPoker);//服务器广播切牌
        NetDispatcher.Instance.AddEventListener(GP_ROOM_CUTPOKERDONE.CODE, OnServerBroadcastCutPokerEnd);//服务器广播切牌完毕
        NetDispatcher.Instance.AddEventListener(GP_ROOM_GRABBANKER.CODE, OnServerBroadcastGrabBanker);//服务器广播开始抢庄
        NetDispatcher.Instance.AddEventListener(GP_ROOM_BUILDPOKERWALL.CODE, OnServerBroadcastBuildPokerWall);//生成牌墙
        NetDispatcher.Instance.AddEventListener(GP_ROOM_CLEARDESKTOP.CODE, OnServerBroadcastCleardesktop);//清空桌面
        NetDispatcher.Instance.AddEventListener(GP_ROOM_DRAWPOKER.CODE, OnServerBroadcastDrawPoker);//服务器广播翻牌
        NetDispatcher.Instance.AddEventListener(GP_ROOM_INFORMBANKERDRAW.CODE, OnServerBroadcastIsBankeDraw);//服务器广播通知庄家翻牌
        NetDispatcher.Instance.AddEventListener(GP_ROOM_CUTPAN.CODE,OnServerBroadcastIsCutpan);//服务器通知切锅
        NetDispatcher.Instance.AddEventListener(GP_ROOM_CUOPAI.CODE, OnServerBroadCuoPai);//小牌九搓牌
        NetDispatcher.Instance.AddEventListener(GP_ROOM_GROUPMAXPOKERPHINT.CODE, OnServerBroadPromptPoker);//提示

        // NetWorkSocket.Instance.OnDisconnect += OnDisConnectCallBack;//断开连接事件
    }



    #region 实现 IGameCtrl和IsystemCtrl 的接口
    /// <summary>
    /// 创建房间
    /// </summary>
    public void CreateRoom(int groupId, List<int> settingIds)
    {
        //m_CurrentType = EnterRoomType.Create;//当前状态创建
        //GameServerEntity entity = GameServerProxy.Instance.CurrentGameServer;//给服务器赋值
        ////如果有服务器就创建房间
        //if (NetWorkSocket.Instance.Connected)
        //{
            ClientSendCreateRoom( settingIds);
        //}
        //else
        //{            
        //    if (isIntranet)            
        //        ConnectServer(EnterRoomType.Create, "192.168.1.123", 8103);
        //    else
        //        ConnectServer(EnterRoomType.Create, entity.ipaddr, entity.port);           
        //}
    }
    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="roomId"></param>
    public void JoinRoom(int roomId)
    {
        m_nCurrentJoinRoomId = roomId;
        //m_CurrentType = EnterRoomType.Join;
        //if (NetWorkSocket.Instance.Connected)
        //{
            ClientSendJoinRoom(m_nCurrentJoinRoomId);
        //}
        //else
        //{
        //    if(isIntranet)
        //        ConnectServer(EnterRoomType.Join, "192.168.1.123", 8103);
        //    else
        //        ConnectServer(EnterRoomType.Join, GameServerProxy.Instance.CurrentGameServer.ipaddr, GameServerProxy.Instance.CurrentGameServer.port);
        //}
    }
    /// <summary>
    /// 重建房间
    /// </summary>
    public void RebuildRoom()
    {
        //m_CurrentType = EnterRoomType.Renter;
        //if (NetWorkSocket.Instance.Connected)
        //{
        ClientSendRebuild();        
        //}
        //else
        //{
        //    if(isIntranet)
        //        ConnectServer(EnterRoomType.Renter, "192.168.1.123", 8103);
        //    else 
        //        ConnectServer(EnterRoomType.Renter, GameServerProxy.Instance.CurrentGameServer.ipaddr, GameServerProxy.Instance.CurrentGameServer.port);
        //}
    }
    /// <summary>
    /// 离开房间
    /// </summary>
    public void QuitRoom()
    {
        if (RoomGuPaiJiuProxy.Instance.CurrentRoom.currentLoop==0)
        {
            UIViewManager.Instance.ShowMessage("提示", "是否离开房间", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
        }      
    }
    /// <summary>
    /// 解散房间
    /// </summary>
    public void DisbandRoom()
    {
        //if (RoomGuPaiJiuProxy.Instance.CurrentRoom.roomStatus==ROOM_STATUS.IDLE)
        //{
            ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom);
        //}
        //else
        //{
        //    ShowMessage("提示", "游戏中不能解散,请打完这一局", MessageViewType.Ok);
        //}
    }
   
    /// <summary>
    /// 返回房间实体
    /// </summary>
    /// <returns></returns>
    public RoomEntityBase GetRoomEntity()
    {
        return RoomGuPaiJiuProxy.Instance.CurrentRoom;
    }

    public void OpenView(UIWindowType type)
    {
        throw new NotImplementedException();
    }

    ///// <summary>
    ///// 连接服务器
    ///// </summary>
    ///// <param name="type"></param>
    ///// <param name="ip"></param>
    ///// <param name="port"></param>
    //public void ConnectServer(EnterRoomType type, string ip, int port)
    //{
    //    UIViewManager.Instance.ShowWait();
    //    m_CurrentType = type;
    //    NetWorkSocket.Instance.BeginConnect(ip, port, OnConnectedCallBack);
    //}
    ///// <summary>
    ///// 连接服务器回调
    ///// </summary>
    ///// <param name="obj"></param>
    //private void OnConnectedCallBack(bool isSuccess)
    //{
    //    UIViewManager.Instance.CloseWait();
    //    if (isSuccess)
    //    {
    //        if (m_CurrentType == EnterRoomType.Create)
    //        {
    //            ClientSendCreateRoom();//创建房间
    //        }
    //        else if (m_CurrentType == EnterRoomType.Join)
    //        {
    //            ClientSendJoinRoom(m_nCurrentJoinRoomId);//加入房间
    //        }
    //        else
    //        {
    //            ClientSendRebuild();
    //        }
    //    }
    //    else
    //    {
    //        UIViewManager.Instance.ShowMessage("错误提示", "网络连接失败", MessageViewType.Ok, ExitGame);
    //    }

    //}
    #endregion


    #region 客户端发送消息
    //=======================================《客户端发送消息》==================================================================
    /// <summary>
    /// 客户端发送创建房间消息
    /// </summary>
    private void ClientSendCreateRoom(List<int> settingIds)
    {
        GP_ROOM_CREATE_GET proto = new GP_ROOM_CREATE_GET();

        for (int i = 0; i < settingIds.Count; ++i)
        {
            proto.addSettingId(settingIds[i]);
        }
        //List<cfg_settingEntity> lst = cfg_settingDBModel.Instance.GetOptionsByGameId(GameCtrl.Instance.CurrentGameId);       
        //for (int i = 0; i < lst.Count; ++i)
        //{
        //    if (lst[i].status == 1 && lst[i].init == 1)
        //    {               
        //       proto.addSettingId(lst[i].id);
        //    }
        //}
        NetWorkSocket.Instance.Send(proto.encode(), GP_ROOM_CREATE.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 客户端发送加入房间
    /// </summary>
    private void ClientSendJoinRoom(int roomId)
    {
        GP_ROOM_ENTER_GET proto = new GP_ROOM_ENTER_GET();
        proto.roomId = roomId;
        NetWorkSocket.Instance.Send(proto.encode(), GP_ROOM_ENTER.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 客户端发送重建房间
    /// </summary>
    private void ClientSendRebuild()
    {
        NetWorkSocket.Instance.Send(null, GP_ROOM_RECREATE.CODE, GameCtrl.Instance.SocketHandle);
        ClientSendFocus(true);
    }
    /// <summary>
    /// 客户端发送准备的消息
    /// </summary>
    private void GuPaiJiuClientSendReady(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, GP_ROOM_READY.CODE, GameCtrl.Instance.SocketHandle);       
    }

    /// <summary>
    /// 客户端发送焦点切换消息
    /// </summary>
    /// <param name="isFocus"></param>
    public void ClientSendFocus(bool isFocus)
    {
        GP_ROOM_AFK_GET proto = new GP_ROOM_AFK_GET();
        proto.isAfk = !isFocus;
        NetWorkSocket.Instance.Send(proto.encode(), GP_ROOM_AFK.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 切牌按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void GuPaiJiuClisentSendCutPoker(object[] obj)
    {       
        int cut = (int)obj[0];
        ClientSendCutPoker(cut);
    }

    /// <summary>
    /// 切牌
    /// </summary>
    /// <param name="dun"></param>
    public void OnGuPaiJiuClientSendCutPokerInfo(int dun)
    {
        ClientSendCutPoker(0, dun);
    }

    /// <summary>
    /// 客户端发送切牌
    /// </summary>
    private void ClientSendCutPoker(int isCut = 0, int dun = -1)
    {
        GP_ROOM_CUTPOKER_GET proto = new GP_ROOM_CUTPOKER_GET();
        if (isCut > 0) proto.isCutPoker = isCut;
        if (dun >= 0) proto.cutPokerIndex = dun;
        NetWorkSocket.Instance.Send(proto.encode(), GP_ROOM_CUTPOKER_GET.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 客户端发送切锅或者不切
    /// </summary>
    /// <param name="cutPan"></param>
    public void ClientSendCutPan(int cutPan)
    {
        GP_ROOM_CUTPAN_GET proto = new GP_ROOM_CUTPAN_GET();
        proto.isCutPan = cutPan;
        NetWorkSocket.Instance.Send(proto.encode(),GP_ROOM_CUTPAN_GET.CODE, GameCtrl.Instance.SocketHandle);
    }


    /// <summary>
    /// 客户端发送抢庄
    /// </summary>
    /// <param name="banker"></param>
    public void ClientSendGrabBanker(int banker)
    {
        GP_ROOM_GRABBANKER_GET proto = new GP_ROOM_GRABBANKER_GET();
        proto.isGrabBanker = banker;
        NetWorkSocket.Instance.Send(proto.encode(), GP_ROOM_GRABBANKER_GET.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 抢庄完毕
    /// </summary>
    public void ClientSendGrabBakerEnd()
    {
        NetWorkSocket.Instance.Send(null,GP_ROOM_GRABBANKERDONE_GET.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 客户端发送搓牌
    /// </summary>
    public void ClientSendCuoPai()
    {
        NetWorkSocket.Instance.Send(null, GP_ROOM_CUOPAI_GET.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 提示
    /// </summary>
    public void ClientSendPromptPoker()
    {
        NetWorkSocket.Instance.Send(null, GP_ROOM_GROUPMAXPOKERPHINT.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 客户端发送游戏开始
    /// </summary>
    /// <param name="obj"></param>
    private void GuPaiJiuClientSendGameStart(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, GP_ROOM_GAMESTART.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 客户端发送牌墙生成完毕
    /// </summary>
    /// <param name="obj"></param>
    private void GuPaiJiuClisentSendPokerWallEnd(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, GP_ROOM_POKERWALLDONE.CODE, GameCtrl.Instance.SocketHandle);
    }
        
    /// <summary>
    /// 客户端发送下注的分数
    /// </summary>
    private void GuPaiJiuClientSendBottomPour(object[] obj)
    {
        GP_ROOM_BOTTOMPOUR_GET proto = new GP_ROOM_BOTTOMPOUR_GET();
        proto.firstPour =(int)obj[0];
        proto.secondPour= (int)obj[1];
        proto.thirdPour = (int)obj[2];
        if (proto.firstPour == 0)
        {
            ShowMessage("提示", "下注一道不能为空，请重新下注", MessageViewType.Ok);
            return;
        }
        else
        {
            NetWorkSocket.Instance.Send(proto.encode(), GP_ROOM_BOTTOMPOUR.CODE, GameCtrl.Instance.SocketHandle);
        }
        //if (JudgeBetPour((proto.firstPour+proto.secondPour)))
        //{
       // NetWorkSocket.Instance.Send(proto.encode(), GP_ROOM_BOTTOMPOUR.CODE);
        //}          
    }
    /// <summary>
    /// 判断是否爆锅
    /// </summary>
    private bool JudgeBetPour(int sum)
    {       
        if (RoomGuPaiJiuProxy.Instance.CurrentRoom.IsAddPanBase)
        {          
            if (sum <= RoomGuPaiJiuProxy.Instance.CurrentRoom.PanBase)
            {
                return IsExceedCap(sum);               
            }
            else
            {
                string textBet = string.Format("下注不能超过锅底上限{0}，请重新下注", RoomGuPaiJiuProxy.Instance.CurrentRoom.PanBase);
                ShowMessage("提示", textBet, MessageViewType.Ok);
                return false;
            }
        }
        else
        {
            return IsExceedCap(sum);
        }
    }

    /// <summary>
    /// 是否超过封顶分
    /// </summary>
    /// <returns></returns>
    private bool IsExceedCap(int sum)
    {
        if (!RoomGuPaiJiuProxy.Instance.isBet)
        {
            if (sum <= RoomGuPaiJiuProxy.Instance.CurrentRoom.scoreLimit)
            {
                return true;
            }
            else
            {
                string textBet = string.Format("下注不能超过封顶上限{0}，请重新下注", RoomGuPaiJiuProxy.Instance.CurrentRoom.scoreLimit);
                ShowMessage("提示", textBet, MessageViewType.Ok);
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 客户端发送切牌完毕
    /// </summary>
    public void GuPaiJiuClientCutPokerDone()
    {
        NetWorkSocket.Instance.Send(null, GP_ROOM_CUTPOKERDONE.CODE, GameCtrl.Instance.SocketHandle);
    }


    /// <summary>
    /// 客户端发送发牌结束
    /// </summary>
    /// <param name="obj"></param>
    private void GuPaiJiuClientEmptyReceive(object[] obj)
    {
        NetWorkSocket.Instance.Send(null,GP_ROOM_EMPTYRECEIVE.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送组合牌的消息
    /// </summary>
    /// <param name="obj"></param>
    private void GuPaiJiuClientSendGroupPoker(object[] obj)
    {
        GP_ROOM_GROUPPOKER_GET proto = new GP_ROOM_GROUPPOKER_GET();
        List<int> intList = (List<int>)obj[0];
        for (int i = 0; i < intList.Count; i++)
        {
            proto.addPokerIndexList(intList[i]);
        }
        NetWorkSocket.Instance.Send(proto.encode(), GP_ROOM_GROUPPOKER.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 客户端发送自动组合牌
    /// </summary>
    /// <param name="obj"></param>
    private void GuPaiJiuClisentSendAutoGroupPoker(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, GP_ROOM_AUTOGROUPPOKER.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 客户端发送翻牌
    /// </summary>
    /// <param name="seat"></param>
    /// <param name="dun"></param>
    public void GuPaiJiuClisentSendDrawPoker(SeatEntity seat,int dun)
    {
        for (int i = 0; i < RoomGuPaiJiuProxy.Instance.CurrentRoom.seatList.Count; i++)
        {
            if (RoomGuPaiJiuProxy.Instance.CurrentRoom.seatList[i].IsBanker)
            {
                if (RoomGuPaiJiuProxy.Instance.PlayerSeat == RoomGuPaiJiuProxy.Instance.CurrentRoom.seatList[i]&&RoomGuPaiJiuProxy.Instance.CurrentRoom.roomStatus==ROOM_STATUS.CHECK)
                {
                    GP_ROOM_DRAWPOKER_GET proto = new GP_ROOM_DRAWPOKER_GET();
                    proto.index = dun;
                    proto.pos = seat.Pos;
                    Debug.Log(dun+"                                       向服务器发送的顿数");
#if IS_CHUANTONGPAIJIU
                    NetWorkSocket.Instance.Send(proto.encode(), GP_ROOM_DRAWPOKER_GET.CODE, GameCtrl.Instance.SocketHandle);
#endif
                }
            }           
        }
    }

    /// <summary>
    /// 客户端发送全开
    /// </summary>
    public void GuPaiJiuClisentSendDrawOpen()
    {
        NetWorkSocket.Instance.Send(null, GP_ROOM_SETTLE.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 通知开始下一次
    /// </summary>
    private void GuPaiJiuClientSendInformNext(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, GP_ROOM_INFORMNEXT.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 退出本局游戏
    /// </summary>
    private void ExitGame()
    {
        if (NetWorkSocket.Instance.Connected(GameCtrl.Instance.SocketHandle))
        {
            NetWorkSocket.Instance.SafeClose(GameCtrl.Instance.SocketHandle);
        }
        SceneMgr.Instance.LoadScene(SceneType.Main);
    }

    /// <summary>
    /// 客户端发送离开的消息
    /// </summary>
    public void ClientSendLeaveRoom()
    {
        NetWorkSocket.Instance.Send(null, GP_ROOM_LEAVE.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送请求解散房间
    /// </summary>
    private void ClientSendApplyDisbandRoom()
    {
        NetWorkSocket.Instance.Send(null, GP_ROOM_APPLYDISMISS.CODE, GameCtrl.Instance.SocketHandle);       
    }
#endregion

#region 服务器广播消息

    /// <summary>
    /// 服务器广播创建房间的消息
    /// </summary>
    /// <param name="obj"></param>
    private void  OnServerReturnCreateRoom(byte[] obj)
    {       
        GP_ROOM_CREATE proto = GP_ROOM_CREATE.decode(obj);
        UIViewManager.Instance.CloseWait();
        RoomGuPaiJiuProxy.Instance.InitRoom(proto);
        SceneMgr.Instance.LoadScene(SceneType.GuPaiJiu);
    }
    /// <summary>
    /// 服务器广播进入房间的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnter(byte[] obj)
    {
        GP_ROOM_ENTER proto = GP_ROOM_ENTER.decode(obj);
        RoomGuPaiJiuProxy.Instance.EnterRoom(proto);
    }
    /// <summary>
    /// 服务器广播离开的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLeave(byte[] obj)
    {
        GP_ROOM_LEAVE proto = GP_ROOM_LEAVE.decode(obj);
        RoomGuPaiJiuProxy.Instance.LeaveProxy(proto);
    }
    /// <summary>
    /// 服务器广播准备的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastReady(byte[] obj)
    {
        GP_ROOM_READY proto = GP_ROOM_READY.decode(obj);
        RoomGuPaiJiuProxy.Instance.ReadyProxy(proto);
    }
    /// <summary>
    /// 服务器广播游戏开始
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastGameStart(byte[] obj)
    {
        GP_ROOM_GAMESTART proto = GP_ROOM_GAMESTART.decode(obj);
        RoomGuPaiJiuProxy.Instance.GameStartProxy(proto);
        if (GuPaiJiuSceneCtrl.Instance != null)
        {
            GuPaiJiuSceneCtrl.Instance.Begin();//同IP提醒
        }
    }

    /// <summary>
    /// 服务器广播通知庄家翻牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastIsBankeDraw(byte[] obj)
    {
        GP_ROOM_INFORMBANKERDRAW proto = GP_ROOM_INFORMBANKERDRAW.decode(obj);
        RoomGuPaiJiuProxy.Instance.IsBankeDrawProxy(proto);
    }


    /// <summary>
    /// 服务器广播抢庄
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastGrabBanker(byte[] obj)
    {
        GP_ROOM_GRABBANKER proto = GP_ROOM_GRABBANKER.decode(obj);
        if (!proto.hasPos()&&proto.hasUnixtime()) RoomGuPaiJiuProxy.Instance.TallGrabBankerProxy(proto);
        if (proto.hasIsGrabBanker()&& proto.hasPos()) RoomGuPaiJiuProxy.Instance.GrabBankerAndNoBaker(proto);
        if (!proto.hasIsGrabBanker()&& proto.hasPos())
        {
            RoomGuPaiJiuProxy.Instance.GrabBankerProxy(proto);
        }      
    }

    /// <summary>
    /// 服务器通知庄家切锅
    /// </summary>
    private void OnServerBroadcastIsCutpan(byte[] obj)
    {
        GP_ROOM_CUTPAN proto = GP_ROOM_CUTPAN.decode(obj);
        RoomGuPaiJiuProxy.Instance.RoomCutpanProxy(proto);        
    }

    /// <summary>
    /// 服务器广播玩家掉线
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastRoomAfk(byte[] obj)
    {
        GP_ROOM_AFK proto = GP_ROOM_AFK.decode(obj);       
        RoomGuPaiJiuProxy.Instance.SetFocus(proto.playerId, !proto.isAfk);
    }

    /// <summary>
    /// 服务器广播开始下注
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastBet(byte[] obj)
    {
        GP_ROOM_INFORMJETTON proto = GP_ROOM_INFORMJETTON.decode(obj);
        RoomGuPaiJiuProxy.Instance.BetProxy(proto);
    }

    /// <summary>
    /// 服务器广播下注分数
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastBottomPour(byte[] obj)
    {
        GP_ROOM_BOTTOMPOUR proto = GP_ROOM_BOTTOMPOUR.decode(obj);
        RoomGuPaiJiuProxy.Instance.BottomPourProxy(proto);
    }

    /// <summary>
    /// 服务器广播生成牌墙
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastBuildPokerWall(byte[] obj)
    {       
        RoomGuPaiJiuProxy.Instance.BeginProxy();
    }

    private void OnServerBroadcastCleardesktop(byte[] obj)
    {
        RoomGuPaiJiuProxy.Instance.CleardesktopProxy();
    }


    /// <summary>
    /// 服务器广播开始发牌的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastBegin(byte[] obj)
    {
        GP_ROOM_BEGIN proto = GP_ROOM_BEGIN.decode(obj);
        RoomGuPaiJiuProxy.Instance.BeginProxy(proto);     
    }

    /// <summary>
    /// 服务器广播组合牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastValidDeal(byte[] obj)
    {
        GP_ROOM_VALIDDEAL proto = GP_ROOM_VALIDDEAL.decode(obj);
        RoomGuPaiJiuProxy.Instance.ValidDealProxy(proto);
    }
    /// <summary>
    /// 服务器广播组合牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastGroup(byte[] obj)
    {
        GP_ROOM_GROUPPOKER proto = GP_ROOM_GROUPPOKER.decode(obj);
        RoomGuPaiJiuProxy.Instance.GroupPokerProxy(proto);
    }
    /// <summary>
    /// 服务器广播每次结算
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastSettle(byte[] obj)
    {
        GP_ROOM_SETTLE proto = GP_ROOM_SETTLE.decode(obj);
        RoomGuPaiJiuProxy.Instance.SettleProxy(proto);
    }

    /// <summary>
    /// 服务器广播断线重连
    /// </summary>
    private void OnServerBroadcastRecreate(byte[] obj)
    {
        GP_ROOM_RECREATE proto = GP_ROOM_RECREATE.decode(obj);
        UIViewManager.Instance.CloseWait();
        RoomGuPaiJiuProxy.Instance.InitRoom(proto);
        SceneMgr.Instance.LoadScene(SceneType.GuPaiJiu);
    }

    /// <summary>
    /// 服务器广播每局结算
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLoopSettle(byte[] obj)
    {
        GP_ROOM_LOOPSETTLE proto = GP_ROOM_LOOPSETTLE.decode(obj);
        RoomGuPaiJiuProxy.Instance.LoopSettleProxy(proto);
    }

    /// <summary>
    /// 服务器通知坐庄
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastInfoRebanker(byte[] obj)
    {
        GP_ROOM_INFORMBANKER proto = GP_ROOM_INFORMBANKER.decode(obj);
        RoomGuPaiJiuProxy.Instance.InfoRebankerProxy(proto);
    }


    private void OnServerBroadcastDrawPoker(byte[] obj)
    {
        GP_ROOM_DRAWPOKER proto = GP_ROOM_DRAWPOKER.decode(obj);
        RoomGuPaiJiuProxy.Instance.DrawPokerProxy(proto);
    }

    /// <summary>
    /// 服务器通知切牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastTellCutPoker(byte[] obj)
    {
        GP_ROOM_CUTPOKER proto = GP_ROOM_CUTPOKER.decode(obj);
        if (proto.hasPos() && !proto.hasIsCutPoker()&&!proto.hasCutPokerIndex()) RoomGuPaiJiuProxy.Instance.TellCutPokerProxy(proto);
        if (proto.hasPos() && proto.hasIsCutPoker())
        {
            RoomGuPaiJiuProxy.Instance.CutPokerProxy(proto);
        }
        if (proto.hasPos() && proto.hasCutPokerIndex())
        {
            RoomGuPaiJiuProxy.Instance.CutPokerAniProxy(proto);
        }
    }
    /// <summary>
    /// 切牌完毕
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastCutPokerEnd(byte[] obj)
    {
        RoomGuPaiJiuProxy.Instance.CutPokerEndProxy();
    }

    /// <summary>
    /// 提示
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadPromptPoker(byte[] obj)
    {
        GP_ROOM_GROUPMAXPOKERPHINT proto = GP_ROOM_GROUPMAXPOKERPHINT.decode(obj);
        RoomGuPaiJiuProxy.Instance.PromptPokerProxy(proto);
    }

    /// <summary>
    ///小牌九，服务器广播玩家搓牌结束
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadCuoPai(byte[] obj)
    {
        GP_ROOM_CUOPAI proto = GP_ROOM_CUOPAI.decode(obj);
        RoomGuPaiJiuProxy.Instance.CuoPaiProxy(proto);

    }


    /// <summary>
    /// 服务器广播申请解散房间 
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastRoomApplydisMiss(byte[] obj)
    {
        GP_ROOM_APPLYDISMISS proto = GP_ROOM_APPLYDISMISS.decode(obj);
        if (proto.hasIsSucceed())
        {
            if (proto.isSucceed)
            {
                UIViewManager.Instance.ShowMessage("提示", "房间已解散", MessageViewType.Ok, SeeResult);
            }
            else
            {
                UIViewManager.Instance.ShowMessage("提示", "有人不同意解散房间", MessageViewType.Ok);
                RoomGuPaiJiuProxy.Instance.isSendProxy(true);//打开自动配牌
            }
        }
        else
        {
            UIViewManager.Instance.ShowMessage("提示", "有人发起解散房间，是否同意", MessageViewType.OkAndCancel, ClientSendAgreeDisbandRoom, ClientSendRefuseDisbandRoom, 120f, AutoClickType.Ok);
            RoomGuPaiJiuProxy.Instance.isSendProxy(false);  //关闭自动配牌         
        }
    }

    /// <summary>
    /// 服务器广播解散成功结算
    /// </summary>
    /// <param name = "obj" ></ param >
    private void OnServerReturnResult(byte[] obj)
    {
        GP_ROOM_TOTALSETTLE proto = GP_ROOM_TOTALSETTLE.decode(obj);
        m_GuPaiJiuResult = proto;
    }


    //解散房间成功打开结算窗口
    private void SeeResult()
    {
        if (m_GuPaiJiuResult == null)
        {
            ExitGame();
            return;
        }
        OpenResultView();
    }

#region OpenResultView 打开结束界面
    /// <summary>
    /// 打开结束界面
    /// </summary>
    private void OpenResultView()
    {
        if (m_GuPaiJiuResult == null) return;
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.GuPaiJiuResult, (GameObject go) =>
        {
            m_UIGuPaiJiuResultView = go.GetComponent<UIGuPaiJiuResultView>();
            m_UIGuPaiJiuResultView.SetUI(m_GuPaiJiuResult);
            m_GuPaiJiuResult = null;
        });
    }
#endregion

    /// <summary>
    /// 服务器广播游戏结束
    /// </summary>
    private void OnServerBroadcastGameOver(byte[] obj)
    {       
        GP_ROOM_GAMEOVER proto = GP_ROOM_GAMEOVER.decode(obj);
        if (!proto.hasRoom())
        {
            ExitGame();
            return;
        }
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.GuPaiJiuResult, (GameObject go) =>
        {
            m_UIGuPaiJiuResultView = go.GetComponent<UIGuPaiJiuResultView>();
            m_UIGuPaiJiuResultView.SetUI(proto);
            m_GuPaiJiuResult = null;
        });
    }

#endregion



#region OnDisConnectCallBack 网络连接中断回调
    /// <summary>
    /// 网络连接中断回调
    /// </summary>
    private void OnDisConnectCallBack(bool isActiveClose)
    {
        if (SceneMgr.Instance.CurrentSceneType != SceneType.GuPaiJiu) return;
        if (isActiveClose)
        {
            if (this != null)
            {
                RebuildRoom();
            }
        }
        else
        {
            RebuildRoom();
        }
    }
#endregion

    /// <summary>
    /// 客户端发送同意解散房间
    /// </summary>
    private void ClientSendAgreeDisbandRoom()
    {
        GP_ROOM_REPLYDISMISS_GET proto = new GP_ROOM_REPLYDISMISS_GET();
        proto.isDismiss = true;
        NetWorkSocket.Instance.Send(proto.encode(), GP_ROOM_REPLYDISMISS.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 客户端发送拒绝解散房间
    /// </summary>
    private void ClientSendRefuseDisbandRoom()
    {
        GP_ROOM_REPLYDISMISS_GET proto = new GP_ROOM_REPLYDISMISS_GET();
        proto.isDismiss = false;
        NetWorkSocket.Instance.Send(proto.encode(), GP_ROOM_REPLYDISMISS.CODE, GameCtrl.Instance.SocketHandle);
    }


#region OnHeadClick 头像点击
    /// <summary>
    /// 头像点击
    /// </summary>
    /// <param name="seatPos"></param>
    private void OnHeadGuPaiJiuClick(object[] obj)
    {
        int seatPos = (int)obj[0];
        SeatEntity seat = RoomGuPaiJiuProxy.Instance.GetSeatBySeatId(seatPos);
        if (seat == RoomGuPaiJiuProxy.Instance.PlayerSeat && AccountProxy.Instance.CurrentAccountEntity.identity > 0)
        {
            UIViewManager.Instance.OpenWindow(UIWindowType.PlayerInfo);
            return;
        }

        List<SeatEntityBase> lstSeat = new List<SeatEntityBase>();
        for (int i = 0; i < RoomGuPaiJiuProxy.Instance.CurrentRoom.seatList.Count; ++i)
        {
            if (RoomGuPaiJiuProxy.Instance.CurrentRoom.seatList[i].Pos != seatPos)
            {
                lstSeat.Add(RoomGuPaiJiuProxy.Instance.CurrentRoom.seatList[i]);
            }
        }
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.SeatInfo, (GameObject go) =>
        {
            go.GetComponent<UISeatInfoView>().SetUI(seat, lstSeat);
        });
    }
#endregion



    /// <summary>
    /// 游戏结束客户端点击返回大厅按钮
    /// </summary>
    private void ReturnHall(object[] obj)
    {
        ExitGame();
    }


    /// <summary>
    /// 牌局结果界面分享按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnResultViewGuPaiJiuShareClick(object[] obj)
    {
        if (m_UIGuPaiJiuResultView != null)
        {
            Debug.Log("·······开始分享了·······");
            m_UIGuPaiJiuResultView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
        }
        //else if (m_UISettleView != null)
        //{
        //    AppDebug.Log("开始分享了啊");
        //    m_UISettleView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
        //}
    }
    /// <summary>
    /// 截屏完成回调
    /// </summary>
    /// <param name="tex"></param>
    private void OnScreenCaptureComplete(Texture2D tex)
    {
        string path = LocalFileManager.Instance.LocalFilePath + "share/";
        if (!IOUtil.DirectoryExists(path))
        {
            IOUtil.CreateDirectory(path);
        }
        IOUtil.Write(path + "record.jpg", tex.EncodeToJPG());
        AppDebug.Log("分享完了啊");
        ShareCtrl.Instance.ShareTexture(WXShareType.WXSceneSession, path + "record.jpg");
    }
    /// <summary>
    /// 聊天
    /// </summary>
    /// <param name="type"></param>
    /// <param name="playerId"></param>
    /// <param name="message"></param>
    /// <param name="audioName"></param>
    /// <param name="toPlayerId"></param>
    public void OnReceiveMessage(ChatType type, int playerId, string message, string audioName, int toPlayerId)
    {
        SeatEntity seat = RoomGuPaiJiuProxy.Instance.GetSeatByPlayerId(playerId);
        if (type == ChatType.Expression)//表情
        {
            if (!string.IsNullOrEmpty(audioName))
            {
                AudioEffectManager.Instance.Play(string.Format("{0}", audioName), Vector3.zero, false);
            }
            UIItemChat.Instance.ShowExpression(seat.Index, message);
        }
        else if (type == ChatType.InteractiveExpression)//互动表情
        {
            SeatEntity toSeat = RoomGuPaiJiuProxy.Instance.GetSeatByPlayerId(toPlayerId);

            UIItemChat.Instance.ShowInteractiveExpression(seat.Index, message, toSeat.Index, audioName);
        }
        else if (type == ChatType.MicroPhone)//语音
        {
            UIItemChat.Instance.ShowMicroPhone(seat.Index);
        }
        else if (type == ChatType.Message)//文字
        {
            if (!string.IsNullOrEmpty(audioName))
            {
                AudioEffectManager.Instance.Play(string.Format("{0}_{1}", seat.Gender, audioName), Vector3.zero, false);
            }
            UIItemChat.Instance.ShowMessage(seat.Index, message);
        }
    }

#region OnBtnMaJiangViewShareClick 分享按钮点击
    /// <summary>
    /// 分享按钮点击及微信邀请
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnGuPaiJiuViewShareClick(object[] obj)
    {        
        ShareCtrl.Instance.ShareURL(ShareType.InGame);
        Debug.Log("           打开微信邀请                ");
    }

#endregion
}
