//===================================================
//Author      : CZH
//CreateTime  ：6/14/2017 11:41:05 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.oegame.zjh.protobuf;
using zjh.proto;
using UnityEngine.SceneManagement;
using ZhaJh;

public class ZhaJHGameCtrl : SystemCtrlBase<ZhaJHGameCtrl>, IGameCtrl, ISystemCtrl
{
    private ZJH_ROOM_GAME_OVER m_Result;
    public  UIZJHTotalSettlement m_UIResultView;

    private ZJH_ROOM_GAME_OVER protoGmeOver;

    private UISeatInfoView m_UISeatInfoView;
    /// <summary>
    /// 当前进入房间类型
    /// </summary>
    private EnterRoomType m_CurrentType = EnterRoomType.Renter;
    /// <summary>
    /// 当前加入的房间Id
    /// </summary>
    private int m_nCurrentJoinRoomId;
    /// <summary>
    /// 是否使用内网
    /// </summary>
    private bool isIntranet = false;

    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        
        dic.Add("OnBtnResultViewZJHShareClick", OnBtnResultViewZJHShareClick);//结束界面分享按钮      
        dic.Add(ZhaJHButtonConstant.btnZhaJHViewShare, OnBtnZJHViewShareClick);//分享按钮点击---微信邀请            
        dic.Add(ZhaJHButtonConstant.btnZhaJHViewReady, ClientSendReady);
        dic.Add(ZhaJHButtonConstant.btnZhaJHViewLicensing, HairPoker);
        dic.Add(ZhaJHButtonConstant.btnZJHWithNotes, WithNotes);
        dic.Add(ZhaJHButtonConstant.btnZJHLookPoker, LookPoker);
        dic.Add(ZhaJHButtonConstant.btnZJHLosePoker, LosePoker);
        dic.Add(ZhaJHMethodname.OnZJHliangfen, TwoBranch);      
        dic.Add(ZhaJHMethodname.OnZJHThanPoker, ThanPoker);//比牌
        dic.Add(ZhaJHButtonConstant.btnZhaJHViewLightPoker, LightLook);//亮牌
        dic.Add(ZhaJHMethodname.OnZJHDealLookEnd, DealLookEnd);
        dic.Add(ZhaJHMethodname.OnZJHAgreeRefuse, AgreeRefuseGameCtrl);
        dic.Add(ZhaJHMethodname.btnWithdraw, BtnWithdraw);//高级房玩家取消进入房间
        dic.Add("OnHeadZJHClick", OnHeadZJHClick);//头像点击事件注册  
        dic.Add(ZhaJHMethodname.OnZJHReturnHall, ReturnHall);  //方法在普通房游戏结算的界面的预制体上的脚本使用
        return dic;
    }

   

    public override void Dispose()
    {
        base.Dispose();
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_CREATE.CODE, OnServerReturnCreateRoom);//创建房间
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_ENTER.CODE, OnServerBroadcastEnter);//进入房间
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//离开房间
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_READY.CODE, OnServerBroadcastReady);//准备        
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_DEAL.CODE, OnServerBroadcastPlayDeliverPoker);//发牌 
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_NEXT_POUR.CODE, OnServerBroadcastBet); //通知下注
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_ADD_POUR.CODE, OnServerBroadAddPour);//加注
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_FOLLOW_POUR.CODE, OnServerBroadFollowPour);//跟注
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_OPEN_LOOK_POCKER.CODE, OnServerBroadLookPoker);//看牌
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_LOSE_POKER.CODE, OnServerBroadLosePoker);//弃牌
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_COMPARE_POKER.CODE, OnServerBroadComparePoker);//比牌
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_SETTLE.CODE, OnServerBroadSettle);//收益结算
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_LIGHTLOOK.CODE, OnServerBroadLightLook);//亮牌
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_NEXT_GAME.CODE, OnServerBroadNexeGame);//服务器广播下一局
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_RECREATE.CODE, OnServerReturnRebuild);//服务器广播重连房间
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_APPLY_DISMISS.CODE, OnServerBroadcastApplyDismissRoom);//申请解散房间
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_REPLY_DISMISS.CODE, OnServerBroadcastReplyDismissRoom);//答复解散房间的消息
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_DISMISS_SUCCEED.CODE, OnServerBroadcastDismissSucceedRoom);//解散房间成功
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_DISMISS_FAIL.CODE, OnServerBroadcastDismissFailRoom);//解散房间失败
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_GAME_OVER.CODE, OnServerBroadcastGameOver);//服务器广播游戏结束
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_APPLY_ENTER.CODE, OnServerBroadcastApplyEnter);//高级房服务器通知房主申请加入房间
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_CHANGE_VILLAGE.CODE, OnServerBroadcastChangeVillage);//高级房服务器广播换庄
        NetDispatcher.Instance.RemoveEventListener(ZJH_WAIT_AGREE.CODE, OnServerBroadcastWaitAgree);//高级房服务器广播等待房主同意
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_ENTER_FAILED.CODE, OnServerBroadcastEnterFailed);//高级房服务器广播拒绝玩家进入
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_WITHDRAW_ENTER.CODE, OnServerBroadcastEnterWithdrwaEnterRoom);//高级房服务器广播玩家取消进入房间
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_STOP.CODE, OnServerBroadcastEnterRoomStop);//高级房如果只有一个人分数超过50分，则终止游戏
        NetDispatcher.Instance.RemoveEventListener(ZJH_SEND_BILL.CODE, OnServerBroadcastEnterSendBill);//高级房服务器广播离开玩家的账单
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_LOW_SCORE.CODE, OnServerBroadcastLowScore);//底分模式
        NetDispatcher.Instance.RemoveEventListener(ZJH_ROOM_SEND_MONEY.CODE, OnServerBroadcastSendMoney);//派彩
    }

   
    public ZhaJHGameCtrl()
    {
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_CREATE.CODE, OnServerReturnCreateRoom);//创建房间
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_ENTER.CODE, OnServerBroadcastEnter);//进入房间
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//离开房间
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_READY.CODE, OnServerBroadcastReady);//准备        
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_DEAL.CODE, OnServerBroadcastPlayDeliverPoker);//发牌 
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_NEXT_POUR.CODE, OnServerBroadcastBet); //通知下注
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_ADD_POUR.CODE, OnServerBroadAddPour);//加注
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_FOLLOW_POUR.CODE, OnServerBroadFollowPour);//跟注
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_OPEN_LOOK_POCKER.CODE, OnServerBroadLookPoker);//看牌
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_LOSE_POKER.CODE, OnServerBroadLosePoker);//弃牌
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_COMPARE_POKER.CODE, OnServerBroadComparePoker);//比牌
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_SETTLE.CODE, OnServerBroadSettle);//收益结算
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_LIGHTLOOK.CODE, OnServerBroadLightLook);//亮牌
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_NEXT_GAME.CODE, OnServerBroadNexeGame);//服务器广播下一局
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_RECREATE.CODE, OnServerReturnRebuild);//服务器广播重连房间
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_APPLY_DISMISS.CODE, OnServerBroadcastApplyDismissRoom);//申请解散房间
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_REPLY_DISMISS.CODE, OnServerBroadcastReplyDismissRoom);//答复解散房间的消息
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_DISMISS_SUCCEED.CODE, OnServerBroadcastDismissSucceedRoom);//解散房间成功
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_DISMISS_FAIL.CODE, OnServerBroadcastDismissFailRoom);//解散房间失败
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_GAME_OVER.CODE, OnServerBroadcastGameOver);//服务器广播游戏结束
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_APPLY_ENTER.CODE, OnServerBroadcastApplyEnter);//高级房服务器通知房主申请加入房间
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_CHANGE_VILLAGE.CODE, OnServerBroadcastChangeVillage);//高级房服务器广播换庄
        NetDispatcher.Instance.AddEventListener(ZJH_WAIT_AGREE.CODE, OnServerBroadcastWaitAgree);//高级房服务器广播等待房主同意
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_ENTER_FAILED.CODE, OnServerBroadcastEnterFailed);//高级房服务器广播拒绝玩家进入
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_WITHDRAW_ENTER.CODE, OnServerBroadcastEnterWithdrwaEnterRoom);//高级房服务器广播玩家取消进入房间
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_STOP.CODE, OnServerBroadcastEnterRoomStop);//高级房如果只有一个人分数超过50分，则终止游戏
        NetDispatcher.Instance.AddEventListener(ZJH_SEND_BILL.CODE, OnServerBroadcastEnterSendBill);//高级房服务器广播离开玩家的账单
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_LOW_SCORE.CODE, OnServerBroadcastLowScore);//底分模式
        NetDispatcher.Instance.AddEventListener(ZJH_ROOM_SEND_MONEY.CODE, OnServerBroadcastSendMoney);//派彩
      //  NetWorkSocket.Instance.OnDisconnect += OnDisConnectCallBack;//断开连接事件
    }
   

    /// <summary>
    /// 解散房间接口方法的实现
    /// </summary>
    public void DisbandRoom()
    {        
        if (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId != RoomMode.Senior)
        {
            //if (RoomZhaJHProxy.Instance.CurrentRoom.roomStatus == ENUM_ROOM_STATUS.IDLE || RoomZhaJHProxy.Instance.CurrentRoom.roomStatus == ENUM_ROOM_STATUS.SETTLEMENT || RoomZhaJHProxy.Instance.CurrentRoom.roomStatus == ENUM_ROOM_STATUS.MATCH_STATUS_RESULT)
            //{
                ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom);
            //}           
            //else
            //{
            //    ShowMessage("提示", "游戏中不能解散,请打完这一局", MessageViewType.Ok);
            //}
        }
        else if (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId == RoomMode.Senior)
        {
            if (RoomZhaJHProxy.Instance.PlayerSeat.pos == 7&& RoomZhaJHProxy.Instance.CurrentRoom.roomStatus == ENUM_ROOM_STATUS.IDLE|| RoomZhaJHProxy.Instance.CurrentRoom.roomStatus == ENUM_ROOM_STATUS.SETTLEMENT)
            {
                ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
                // ClientSendLeaveRoom();
            }
            else if (RoomZhaJHProxy.Instance.PlayerSeat.pos == 7 && (RoomZhaJHProxy.Instance.CurrentRoom.roomStatus != ENUM_ROOM_STATUS.IDLE|| RoomZhaJHProxy.Instance.CurrentRoom.roomStatus == ENUM_ROOM_STATUS.SETTLEMENT))
            {
                ShowMessage("提示", "游戏中不能解散房间！", MessageViewType.Ok);
            }
            else if ( RoomZhaJHProxy.Instance.PlayerSeat.pos != 7)
            {
                ShowMessage("提示", "只有房主可以解散房间！", MessageViewType.Ok);
            }
        }
    }
    /// <summary>
    /// 离开房间接口方法的实现
    /// </summary>
    public void QuitRoom()
    {
        //ClientSendLeaveRoom();
        if (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId!= RoomMode.Senior)
        {           
            if (RoomZhaJHProxy.Instance.CurrentRoom.currentLoop == 0&&RoomZhaJHProxy.Instance.PlayerSeat.pos != 1)
            {
                ShowMessage("提示", "是否退出房间", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
            }
            else if (RoomZhaJHProxy.Instance.CurrentRoom.currentLoop == 0&&RoomZhaJHProxy.Instance.PlayerSeat.pos==1)
            {
                ClientSendLeaveRoom();
            }
            else
            {
                ShowMessage("提示", "游戏已经开始，中途不能退出", MessageViewType.Ok, null);
            }
        }
        else if (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId == RoomMode.Senior)
        {
            if (RoomZhaJHProxy.Instance.PlayerSeat.pos == 7 && RoomZhaJHProxy.Instance.CurrentRoom.roomStatus == ENUM_ROOM_STATUS.IDLE)
            {
                ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
                // ClientSendLeaveRoom();
            }
            else if (RoomZhaJHProxy.Instance.PlayerSeat.pos == 7 && RoomZhaJHProxy.Instance.CurrentRoom.roomStatus != ENUM_ROOM_STATUS.IDLE)
            {
                ShowMessage("提示", "游戏中不能解散房间！", MessageViewType.Ok, null);
            }
            else if ((RoomZhaJHProxy.Instance.CurrentRoom.roomStatus == ENUM_ROOM_STATUS.IDLE && RoomZhaJHProxy.Instance.PlayerSeat.pos != 7)|| (RoomZhaJHProxy.Instance.PlayerSeat.pos != 7 && RoomZhaJHProxy.Instance.PlayerSeat.seatToperateStatus==ENUM_SEATOPERATE_STATUS.Discard))
            {
                ShowMessage("提示", "是否退出房间", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
            }
            else
            {
                ShowMessage("提示", "游戏中不能离开房间", MessageViewType.Ok, null);
            }
        }
      
    }
    /// <summary>
    /// 重建房间接口方法的实现
    /// </summary>
    public void RebuildRoom()
    {
        ClientSendRebuild();
    }

    /// <summary>
    /// 加入房间接口的实现
    /// </summary>
    public void JoinRoom(int roomId)
    {
        ClientSendJoinRoom(roomId);
    }

    /// <summary>
    /// 游戏结束客户端点击返回大厅按钮
    /// </summary>
    private void ReturnHall(object[] obj)
    {
        ExitGame();
    }
    /// <summary>
    /// 退出本局游戏
    /// </summary>
    public void ExitGame()
    {
        GameCtrl.Instance.ExitGame();
    }
    /// <summary>
    /// 创建房间接口的实现
    /// </summary>
    public void CreateRoom(int groupId, List<int> settingIds)
    {
        ClientSendCreateRoom(groupId,  settingIds);
    }
  

    /// <summary>
    /// 客户端发送请求解散房间
    /// </summary>
    private void ClientSendApplyDisbandRoom()
    {
        NetWorkSocket.Instance.Send(null, ZJH_ROOM_APPLY_DISMISS_GET.CODE,GameCtrl.Instance.SocketHandle);
       // RoomZhaJHProxy.Instance.AgreeDissolveCount(1);
    }

    #region ClientSendFocus 客户端发送焦点切换消息
    /// <summary>
    /// 客户端发送焦点切换消息
    /// </summary>
    /// <param name="isFocus"></param>
    public void ClientSendFocus(bool isFocus)
    {
        //OP_ROOM_AFK_GET proto = new OP_ROOM_AFK_GET();
        //proto.isAfk = !isFocus;

        //NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_AFK_GET.CODE);
    }
    #endregion


    /// <summary>
    /// 服务器广播解散房间失败
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastDismissFailRoom(byte[] obj)
    {
        UIViewManager.Instance.ShowMessage("提示", "有人不同意解散房间", MessageViewType.Ok, Fail);
    }

    private void Fail()
    {
        RoomZhaJHProxy.Instance.DismissFailRoomProxy();
    }
    /// <summary>
    /// 服务器广播解散房间成功
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastDismissSucceedRoom(byte[] obj)
    {
        if (protoGmeOver == null) ExitGame();
        if (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId!= RoomMode.Senior)
        {
            if (RoomZhaJHProxy.Instance.CurrentRoom.currentLoop == 0)
            {
                ShowMessage("提示", "房间已解散，请按‘确认’键退出。", MessageViewType.Ok, ExitGame);
            }
            else
            {
                RoomZhaJHProxy.Instance.DismissFailRoomProxy();
                RoomZhaJHProxy.Instance.isGameOver = false;
            }
            return;
        }
        else if (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId == RoomMode.Senior)
        {
            if (RoomZhaJHProxy.Instance.PlayerSeat.pos == 7)
            {
                if (RoomZhaJHProxy.Instance.CurrentRoom.currentLoop!=0)
                {
                    RoomZhaJHProxy.Instance.OwnerBillProxy();
                }
                else
                {
                    ShowMessage("提示", "房间已解散!", MessageViewType.Ok, ExitGame);
                }
            }
            else
            {
                if (RoomZhaJHProxy.Instance.CurrentRoom.currentLoop==0)
                {
                    ShowMessage("提示", "房间已解散，请按‘确认’键退出。", MessageViewType.Ok, ExitGame);
                }                              
            }            
        }
    }

    /// <summary>
    /// 答复解散房间的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastReplyDismissRoom(byte[] obj)
    {
        ZJH_ROOM_REPLY_DISMISS proto = ZJH_ROOM_REPLY_DISMISS.decode(obj);
        RoomZhaJHProxy.Instance.AgreeDissolveCount(proto.getZjhCommonList().Count);
    }
    /// <summary>
    ///服务器广播申请解散房间的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastApplyDismissRoom(byte[] obj)
    {
        UIViewManager.Instance.ShowMessage("提示", "有人发起解散房间，是否同意", MessageViewType.OkAndCancel, ClientSendAgreeDisbandRoom, ClientSendRefuseDisbandRoom, 5f, AutoClickType.Cancel);
    }
    /// <summary>
    /// 服务器广播准备
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastReady(byte[] obj)
    {
        ZJH_ROOM_READY proto = ZJH_ROOM_READY.decode(obj);
        RoomZhaJHProxy.Instance.Ready(proto);
    }
    /// <summary>
    /// 服务器广播发牌的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastPlayDeliverPoker(byte[] obj)
    {
        ZJH_ROOM_DEAL proto = ZJH_ROOM_DEAL.decode(obj);
        RoomZhaJHProxy.Instance.HariPokerProxy(proto);
        if (ZhaJHSceneCtrl.Instance != null)
        {
            ZhaJHSceneCtrl.Instance.Begin();//同IP提醒
        }
    }

    /// <summary>
    /// 服务器广播通知玩家下注
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastBet(byte[] obj)
    {        
        ZJH_ROOM_NEXT_POUR proto = ZJH_ROOM_NEXT_POUR.decode(obj);
        RoomZhaJHProxy.Instance.NoticeBet(proto);
    }
    /// <summary>
    /// 服务器广播玩家加注
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadAddPour(byte[] obj)
    {
        ZJH_ROOM_ADD_POUR proto = ZJH_ROOM_ADD_POUR.decode(obj);
        RoomZhaJHProxy.Instance.AddPour(proto);
    }
    /// <summary>
    /// 服务器广播跟注的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadFollowPour(byte[] obj)
    {
        ZJH_ROOM_FOLLOW_POUR proto = ZJH_ROOM_FOLLOW_POUR.decode(obj);
       // RoomZhaJHProxy.Instance.WithPour(proto.pos);
        RoomZhaJHProxy.Instance.WithPour(proto);
    }
    /// <summary>
    /// 服务器广播看牌的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadLookPoker(byte[] obj)
    {
        ZJH_ROOM_OPEN_LOOK_POCKER proto = ZJH_ROOM_OPEN_LOOK_POCKER.decode(obj);       
        RoomZhaJHProxy.Instance.LookPokerProxy(proto);
    }
    /// <summary>
    /// 服务器广播弃牌的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadLosePoker(byte[] obj)
    {        
        ZJH_ROOM_LOSE_POKER proto = ZJH_ROOM_LOSE_POKER.decode(obj);        
        RoomZhaJHProxy.Instance.LosePokerProxy(proto.pos);
    }
    /// <summary>
    /// 服务器广播比牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadComparePoker(byte[] obj)
    {
        ZJH_ROOM_COMPARE_POKER proto = ZJH_ROOM_COMPARE_POKER.decode(obj);
        RoomZhaJHProxy.Instance.ThenPokerProxy(proto);
    }
    /// <summary>
    /// 服务器广播亮牌的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadLightLook(byte[] obj)
    {
        ZJH_ROOM_LIGHTLOOK proto = ZJH_ROOM_LIGHTLOOK.decode(obj);        
        RoomZhaJHProxy.Instance.LightLookProxy(proto);
    }

    /// <summary>
    /// 高级房服务器广播离开玩家的账单
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnterSendBill(byte[] obj)
    {
        return;
        ZJH_SEND_BILL proto = ZJH_SEND_BILL.decode(obj);
        RoomZhaJHProxy.Instance.SendBillProxy(proto);
    }


    /// <summary>
    /// 服务器广播收益结算
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadSettle(byte[] obj)
    {
        ZJH_ROOM_SETTLE proto = ZJH_ROOM_SETTLE.decode(obj);
        RoomZhaJHProxy.Instance.SettleProxy(proto);
    }
    /// <summary>
    /// 服务器通知下一局
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadNexeGame(byte[] obj)
    {        
        RoomZhaJHProxy.Instance.NexeGameProxy();
    }
    /// <summary>
    /// 高级房服务器广播申请加入房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastApplyEnter(byte[] obj)
    {        
        ZJH_ROOM_APPLY_ENTER proto = ZJH_ROOM_APPLY_ENTER.decode(obj);
        RoomZhaJHProxy.Instance.ApplyEnterProxy(proto);
    }
    /// <summary>
    /// 服务器广播等待玩家进入
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastWaitAgree(byte[] obj)
    {      
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.UIZJHWait, (GameObject go) =>
        {
            
        });
    }
    /// <summary>
    /// 服务器广播拒绝玩家进入
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnterFailed(byte[] obj)
    {
        UIViewManager.Instance.ShowMessage("提示", "房主拒绝您进入房间 . . .", MessageViewType.Ok,null);        
        ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHWithdrawEnterRoom, null);//关闭等待进入的界面
    }
    /// <summary>
    /// 服务器广播取消进入房间的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnterWithdrwaEnterRoom(byte[] obj)
    {
        ZJH_ROOM_WITHDRAW_ENTER proto = ZJH_ROOM_WITHDRAW_ENTER.decode(obj);
        RoomZhaJHProxy.Instance.WithdrawEnterRoom(proto);      
    }


    /// <summary>
    /// 服务器广播游戏结束
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastGameOver(byte[] obj)
    {       
        if (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId != RoomMode.Senior)
        {
            ZJH_ROOM_GAME_OVER proto = ZJH_ROOM_GAME_OVER.decode(obj);
            protoGmeOver = proto;
            RoomZhaJHProxy.Instance.GameOverProxy(proto);
        }
        else if (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId == RoomMode.Senior)
        {
            ShowMessage("提示", "房间已解散，请按‘确认’键退出。", MessageViewType.Ok, ExitGame);
        }
        //ExitGame();
    }
    /// <summary>
    /// 服务器广播重连房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnRebuild(byte[] obj)
    {
        ZJH_ROOM_CREATE proto = ZJH_ROOM_CREATE.decode(obj);
        UIViewManager.Instance.CloseWait();
        RoomZhaJHProxy.Instance.InitRoom(proto);
       // GlobalInit.Instance.CurrentRoom = RoomZhaJHProxy.Instance.CurrentRoom;
        SceneMgr.Instance.LoadScene(SceneType.ZhaJH);
    }

    /// <summary>
    /// 服务器广播离开房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLeave(byte[] obj)
    {
        ZJH_ROOM_LEAVE proto = ZJH_ROOM_LEAVE.decode(obj);
        RoomZhaJHProxy.Instance.ExitRoom(proto);      
    }
    
    /// <summary>
    /// 服务器广播有玩家进入
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnter(byte[] obj)
    {
        ZJH_ROOM_ENTER proto = ZJH_ROOM_ENTER.decode(obj);        
        RoomZhaJHProxy.Instance.EnterRoom(proto);       
    }
    /// <summary>
    /// 服务器广播创建房间的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnCreateRoom(byte[] obj)
    {
        ZJH_ROOM_CREATE proto = ZJH_ROOM_CREATE.decode(obj);        
        UIViewManager.Instance.CloseWait();
        RoomZhaJHProxy.Instance.InitRoom(proto);
        //GlobalInit.Instance.CurrentRoom = RoomZhaJHProxy.Instance.CurrentRoom;
        SceneMgr.Instance.LoadScene(SceneType.ZhaJH);             
    }

    /// <summary>
    /// 服务器广播派彩
    /// </summary>
    private void OnServerBroadcastSendMoney(byte[] obj)
    {
        ZJH_ROOM_SEND_MONEY proto = ZJH_ROOM_SEND_MONEY.decode(obj);
        RoomZhaJHProxy.Instance.SendMoneyProxy(proto);
    }
    /// <summary>
    /// 高级房服务器广播换庄
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastChangeVillage(byte[] obj)
    {
        ZJH_ROOM_CHANGE_VILLAGE proto = ZJH_ROOM_CHANGE_VILLAGE.decode(obj);
        RoomZhaJHProxy.Instance.ChangeVillageProxy(proto);
    }
    /// <summary>
    /// 高级房游戏暂停
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnterRoomStop(byte[] obj)
    {
        RoomZhaJHProxy.Instance.RoomStopProxy();
    }
    /// <summary>
    /// 服务器广播底分模式
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLowScore(byte[] obj)
    {
        ZJH_ROOM_LOW_SCORE proto = ZJH_ROOM_LOW_SCORE.decode(obj);
        RoomZhaJHProxy.Instance.LowScoreProxy(proto);
    }



    #region OnDisConnectCallBack 网络连接中断回调
    /// <summary>
    /// 网络连接中断回调
    /// </summary>
    private void OnDisConnectCallBack(bool isActiveClose)
    {
        if (SceneMgr.Instance.CurrentSceneType != SceneType.ZhaJH) return;
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
    /// 连接服务器
    /// </summary>
    /// <param name="type"></param>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public void ConnectServer(EnterRoomType type, string ip, int port)
    {
        UIViewManager.Instance.ShowWait();
        m_CurrentType = type;
        NetWorkSocket.Instance.BeginConnect(ip, port, OnConnectedCallBack);
    }
    /// <summary>
    /// 连接服务器回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnConnectedCallBack(bool isSuccess)
    {
        UIViewManager.Instance.CloseWait();
        if (isSuccess)
        {
            if (m_CurrentType == EnterRoomType.Create)
            {
                
            }
            else if (m_CurrentType == EnterRoomType.Join)
            {
                ClientSendJoinRoom(m_nCurrentJoinRoomId);//加入房间
            }
            else
            {
                ClientSendRebuild();
            }
        }
        else
        {
            UIViewManager.Instance.ShowMessage("错误提示", "网络连接失败", MessageViewType.Ok, ExitGame);
        }

    }

   
    /// <summary>
    /// 客户端发送创建房间消息
    /// </summary>
    private void ClientSendCreateRoom(int groupId, List<int> settingIds)
    {
        ZJH_ROOM_CREATE_GET proto = new ZJH_ROOM_CREATE_GET();
        for (int i = 0; i < settingIds.Count; ++i)
        {
            proto.addSettingId(settingIds[i]);
        }
        //List<cfg_settingEntity> lst = cfg_settingDBModel.Instance.GetOptionsByGameId(GameCtrl.Instance.CurrentGameId);
        //for (int i = 0; i < lst.Count; ++i)
        //{
        //    if (lst[i].status == 1 && lst[i].init == 1)
        //    {
        //        Debug.Log(lst[i].id+"                  配置ID");
        //        proto.addSettingId(lst[i].id);               
        //    }
        //}
        proto.clubId = groupId;
        NetWorkSocket.Instance.Send(proto.encode(), ZJH_ROOM_CREATE.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送重建房间
    /// </summary>
    private void ClientSendRebuild()
    {
        NetWorkSocket.Instance.Send(null, ZJH_ROOM_RECREATE.CODE, GameCtrl.Instance.SocketHandle);
    }
   
  
    /// <summary>
    /// 客户端发送加入房间
    /// </summary>
    private void ClientSendJoinRoom(int roomId)
    {
        ZJH_ROOM_ENTER_GET proto = new ZJH_ROOM_ENTER_GET();
        proto.roomId = roomId;
        NetWorkSocket.Instance.Send(proto.encode(), ZJH_ROOM_ENTER.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送准备
    /// </summary>
    private void ClientSendReady(object[] obj)
    {               
        NetWorkSocket.Instance.Send(null, ZJH_ROOM_READY.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送发牌的消息
    /// </summary>
    /// <param name="type"></param>
    private void HairPoker(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, ZJH_ROOM_BEGIN.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送发牌结束，通知下注
    /// </summary>
    private void DealLookEnd(object[] obj)
    {
        NetWorkSocket.Instance.Send(null,ZJH_ROOM_DEAL_END.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送加注分数
    /// </summary>
    private void TwoBranch(object[] obj)
    {
        ZJH_ROOM_ADD_POUR_GET proto = new ZJH_ROOM_ADD_POUR_GET();
        proto.pour = (int)obj[0];
        //proto.pour = 2;
        NetWorkSocket.Instance.Send(proto.encode(), ZJH_ROOM_ADD_POUR.CODE, GameCtrl.Instance.SocketHandle);       
    }
    //private void TwoBranch1(object[] obj)
    //{
    //    ZJH_ROOM_ADD_POUR_GET proto = new ZJH_ROOM_ADD_POUR_GET();
    //    proto.pour = 5;
    //    NetWorkSocket.Instance.Send(proto.encode(), ZJH_ROOM_ADD_POUR.CODE);      
    //}
    //private void TwoBranch2(object[] obj)
    //{
    //    ZJH_ROOM_ADD_POUR_GET proto = new ZJH_ROOM_ADD_POUR_GET();
    //    proto.pour = 10;
    //    NetWorkSocket.Instance.Send(proto.encode(), ZJH_ROOM_ADD_POUR.CODE);       
    //}
    /// <summary>
    /// 客户端发送跟注
    /// </summary>
    public void WithNotes(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, ZJH_ROOM_FOLLOW_POUR.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送看牌
    /// </summary>
    public void LookPoker(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, ZJH_ROOM_OPEN_LOOK_POCKER.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送比牌的消息
    /// </summary>
    private void ThanPoker(object[] obj)
    {
        ZJH_ROOM_COMPARE_POKER proto = new ZJH_ROOM_COMPARE_POKER();        
        proto.pos= RoomZhaJHProxy.Instance.theCardPos;
        NetWorkSocket.Instance.Send(proto.encode(), ZJH_ROOM_COMPARE_POKER.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送弃牌的消息
    /// </summary>
    private void LosePoker(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, ZJH_ROOM_LOSE_POKER.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送亮牌消息
    /// </summary>
    /// <param name="obj"></param>
    private void LightLook(object[] obj)
    {
        NetWorkSocket.Instance.Send(null,ZJH_ROOM_LIGHTLOOK.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送同意解散房间
    /// </summary>
    private void ClientSendAgreeDisbandRoom()
    {
        ZJH_ROOM_REPLY_DISMISS_GET proto = new ZJH_ROOM_REPLY_DISMISS_GET();
        proto.zjh_enum_roomresult = ENUM_ROOMRESULT.AGREE;          
        NetWorkSocket.Instance.Send(proto.encode(), ZJH_ROOM_REPLY_DISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 客户端发送拒绝解散房间
    /// </summary>
    private void ClientSendRefuseDisbandRoom()
    {
        ZJH_ROOM_REPLY_DISMISS_GET proto = new ZJH_ROOM_REPLY_DISMISS_GET();
        proto.zjh_enum_roomresult = ENUM_ROOMRESULT.DISAGREE;
        NetWorkSocket.Instance.Send(proto.encode(), ZJH_ROOM_REPLY_DISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 高级房客户端发送同意或者拒绝的消息
    /// </summary>
    /// <param name="obj"></param>
    private void AgreeRefuseGameCtrl(object[] obj)
    {
        bool isbool =(bool)obj[0];
        PlayerEntityZjh playerZjh=(PlayerEntityZjh)obj[1];        
        PLAYER player = new PLAYER();        
        player.playerId = playerZjh.playerId;
        ZJH_ROOM_APPLY_ENTER_GET proto = new ZJH_ROOM_APPLY_ENTER_GET();
        proto.agree_or_not = isbool;
        proto.player = player;
        NetWorkSocket.Instance.Send(proto.encode(), ZJH_ROOM_APPLY_ENTER_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    /// <summary>
    /// 高级房客户端发送取消进入房间
    /// </summary>
    /// <param name="obj"></param>
    private void BtnWithdraw(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, ZJH_ROOM_WITHDRAW_ENTER.CODE, GameCtrl.Instance.SocketHandle);
    }

    /// <summary>
    /// 打开视图
    /// </summary>
    /// <param name="type"></param>
    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.UIZJHTotalSettlement:
                //OpenUnitSettlement_ZJH();
                break;           
        }
    }

    //private void OpenUnitSettlement_ZJH()
    //{        
    //    if (m_Result == null) return;
    //    UIViewUtil.Instance.LoadWindow(UIWindowType.UIZJHTotalSettlement, (GameObject go) =>
    //    {
    //        m_UIResultView = go.GetComponent<UIZJHTotalSettlement>();
    //        //m_UIResultView.SetUI(m_Result);
    //        m_Result = null;
    //    });
    //}

    /// <summary>
    /// 离开房间
    /// </summary>
    public void ClientSendLeaveRoom()
    {              
      NetWorkSocket.Instance.Send(null, ZJH_ROOM_LEAVE.CODE, GameCtrl.Instance.SocketHandle);       
    }

    private int m_SelectSeatPos;

    #region OnHeadClick 头像点击
    /// <summary>
    /// 头像点击
    /// </summary>
    /// <param name="seatPos"></param>
    private void OnHeadZJHClick(object[] obj)
    {
        int seatPos = (int)obj[0];
        m_SelectSeatPos = seatPos;
        SeatEntity seat = RoomZhaJHProxy.Instance.GetSeatBySeatId(seatPos);        
        if (seat == RoomZhaJHProxy.Instance.PlayerSeat && AccountProxy.Instance.CurrentAccountEntity.identity > 0)
        {
            UIViewManager.Instance.OpenWindow(UIWindowType.PlayerInfo);
            return;
        }

        List<SeatEntityBase> lstSeat = new List<SeatEntityBase>();
        for (int i = 0; i < RoomZhaJHProxy.Instance.CurrentRoom.seatList.Count; ++i)
        {
            if (RoomZhaJHProxy.Instance.CurrentRoom.seatList[i].pos != seatPos)
            {
                lstSeat.Add(RoomZhaJHProxy.Instance.CurrentRoom.seatList[i]);
            }
        }

        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.SeatInfo, (GameObject go) =>
        {
            m_UISeatInfoView = go.GetComponent<UISeatInfoView>();
            m_UISeatInfoView.SetUI(seat, lstSeat);
            m_UISeatInfoView.SetEmoji(cfg_interactiveExpressionDBModel.Instance.GetList(), OnBtnInteractiveExpressionClick);
        });
    }
    #endregion

    #region OnBtnInteractiveExpressionClick 互动表情点击
    /// <summary>
    /// 互动表情点击
    /// </summary>
    /// <param name="id"></param>
    private void OnBtnInteractiveExpressionClick(int seatPos, int id)
    {
        m_UISeatInfoView.Close();
        cfg_interactiveExpressionEntity entity = cfg_interactiveExpressionDBModel.Instance.Get(id);
        Debug.Log(RoomZhaJHProxy.Instance.GetSeatBySeatId(m_SelectSeatPos).PlayerId + "                         传过去互动表情座位号");
        ChatCtrl.Instance.OnInteractiveClick(proto.common.ENUM_PLAYER_MESSAGE.ANIMATION, entity.code, RoomZhaJHProxy.Instance.GetSeatBySeatId(m_SelectSeatPos).PlayerId, entity.sound);
    }
    #endregion


    #region OnBtnMaJiangViewShareClick 分享按钮点击
    /// <summary>
    /// 分享按钮点击及微信邀请
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnZJHViewShareClick(object[] obj)
    {
        Debug.Log("           打开微信邀请                ");
        ShareCtrl.Instance.ShareURL(ShareType.InGame);
       
    }
    #endregion


    /// <summary>
    /// 牌局结果界面分享按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnResultViewZJHShareClick(object[] obj)
    {
        if (m_UIResultView != null)
        {
            Debug.Log("·······开始分享了·······");
            m_UIResultView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
        }
        //else if (m_UISettleView != null)
        //{
        //    AppDebug.Log("开始分享了啊");
        //    m_UISettleView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
        //}
    }

    #region OnScreenCaptureComplete 截屏完成回调
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
    /// 获取房间数据实体
    /// </summary>
    /// <returns></returns>
    public RoomEntityBase GetRoomEntity()
    {
        return RoomZhaJHProxy.Instance.CurrentRoom;
    }

    public void OnReceiveMessage(ChatType type, int playerId, string message, string audioName, int toPlayerId)
    {
        Debug.Log(toPlayerId+"                            TOID");
        SeatEntity seat = RoomZhaJHProxy.Instance.GetSeatByPlayerId(playerId);
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
            SeatEntity toSeat = RoomZhaJHProxy.Instance.GetSeatByPlayerId(toPlayerId);

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

   


    #endregion
}
