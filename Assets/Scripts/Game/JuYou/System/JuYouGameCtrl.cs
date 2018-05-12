//===================================================
//Author      : WZQ
//CreateTime  ：8/7/2017 3:33:51 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.jy;
using JuYou;
public class JuYouGameCtrl : SystemCtrlBase<JuYouGameCtrl>, IGameCtrl, ISystemCtrl
{

    #region Variable
    private UIWindowSettle_JuYou m_UISettleView;

    private UIWindowResult_JuYou m_UIResultView;
  
  
    ///// <summary>
    ///// 游戏结果(总结束)
    ///// </summary>
    private JY_ROOM_RESULT m_Result;


    #endregion


    #region DicNotificationInterests 注册UI事件
    /// <summary>
    /// 注册UI事件
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();

        dic.Add(ConstDefine_JuYou.ObKey_btnPour, OnBtnJuYouViewPour);//确认下注按钮
        dic.Add(ConstDefine_JuYou.ObKey_btnResultViewBack, OnBtnResultViewBack);//结束界面返回按钮
        dic.Add(ConstDefine_JuYou.ObKey_btnResultViewShare, OnBtnResultViewShareClick);//结束界面分享按钮       
                                                                                  ////dic.Add(ConstDefine.BtnSettleViewReplayOver, OnBtnResultViewReplayOverClick);
        dic.Add(ConstDefine_JuYou.BtnViewShare, OnBtnMaJiangViewShareClick);//分享按钮点击(微信邀请)
        dic.Add(ConstDefine_JuYou.BtnViewReady, OnBtnJuYouViewReadyClick);//准备按钮点击BtnViewStart
        dic.Add(ConstDefine_JuYou.BtnViewStart, OnBtnJuYouViewStartClick);//开始按钮点击
        dic.Add(ConstDefine_JuYou.ObKey_SetPokerStatus, OnPokerClick);//手牌点击
       //dic.Add(ConstDefine_PaiJiu.ObKey_btnChooseBanker, OnBtnChooseBanker);//是否抢庄
        dic.Add(ConstDefine_JuYou.ObKey_OpenViewJuYou, OpenViewJuYou);//加载窗口
        dic.Add(ConstDefine_JuYou.ObKey_SetEnterRoomView, SetEnterRoomView);//刚进入游戏时 加载已有窗口
        dic.Add(ConstDefine_JuYou.ObKey_AloneSettle, ClientSendRefuseAloneSettle);//获取个人结算信息
        dic.Add(ConstDefine_JuYou.ObKey_NextGame, ClientSendRefuseNextGame);//结算界面 下一局点击

        
        return dic;
    }
    #endregion

    #region Constructors
    public JuYouGameCtrl()
    {

        NetDispatcher.Instance.AddEventListener(JY_ROOM_CREATE.CODE, OnServerReturnCreateRoom);//服务器返回创建房间消息
        NetDispatcher.Instance.AddEventListener(JY_ROOM_ENTER.CODE, OnServerBroadcastEnter);//服务器广播玩家进入消息
        NetDispatcher.Instance.AddEventListener(JY_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播玩家离开消息
        NetDispatcher.Instance.AddEventListener(JY_ROOM_READY.CODE, OnServerBroadcastReady);//服务器广播玩家准备消息
        NetDispatcher.Instance.AddEventListener(JY_ROOM_ONLINE.CODE, OnServerBroadcastOnLine);//服务器广播玩家是否在线消息
        NetDispatcher.Instance.AddEventListener(JY_ROOM_APPLYDISMISS.CODE, OnServerBroadcastApplyDisband);//服务器广播解散房间
        NetDispatcher.Instance.AddEventListener(JY_ROOM_REPLYDISMISS.CODE, OnServerBroadcastReplyDismiss);//服务器广播应答解散房间
        NetDispatcher.Instance.AddEventListener(JY_ROOM_RECREATE.CODE, OnServerReturnRebuild); //服务器返回重建房间

        
        NetDispatcher.Instance.AddEventListener(JY_ROOM_GAMESTART.CODE, OnServerBroadcastBegin);//服务器广播开局消息
        NetDispatcher.Instance.AddEventListener(JY_ROOM_BEGIN.CODE, OnServerBroadcastSendPoker);//服务器广播发牌消息
        NetDispatcher.Instance.AddEventListener(JY_ROOM_JETTON.CODE, OnServerBroadcastJetton);//服务器广播下注 
        NetDispatcher.Instance.AddEventListener(JY_ROOM_GIVEUPPOKER.CODE, OnServerBroadcastGiveupPoker);//服务器广播弃牌
        NetDispatcher.Instance.AddEventListener(JY_ROOM_OPENPOKER.CODE, OnServerOpenPoker);//服务器广播翻牌
        NetDispatcher.Instance.AddEventListener(JY_ROOM_SETTLE.CODE, OnServerBroadcastAloneSettle);//服务器广播个人结算信息
        NetDispatcher.Instance.AddEventListener(JY_ROOM_SHUFFLE.CODE, OnServerBroadcastShuffle);//服务器广播重新洗牌
        NetDispatcher.Instance.AddEventListener(JY_ROOM_LOOPENDSETTLE.CODE, OnServerBroadcastSettle);//服务器广播本局结算
        NetDispatcher.Instance.AddEventListener(JY_ROOM_NEXT.CODE, OnServerNextGame);//服务器广播准备下一局
        NetDispatcher.Instance.AddEventListener(JY_ROOM_INFORMBANKER.CODE, OnServerChooseBanker);//换庄
        NetDispatcher.Instance.AddEventListener(JY_ROOM_RESULT.CODE, OnServerResult); //服务器总结算信息
        NetDispatcher.Instance.AddEventListener(JY_ROOM_GAMEOVER.CODE, OnServerGameOver);//游戏结束

        //NetWorkSocket.Instance.OnDisconnect += OnDisConnectCallBack;//断开连接事件

        //DelegateDefine.Instance.OnHeadClick += OnHeadClick;
        //DelegateDefine.Instance.OnPlayPoker += OnPlayPoker;//出牌事件
    }
    #endregion

    #region Dispose
    public override void Dispose()
    {
        base.Dispose();

        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_CREATE.CODE, OnServerReturnCreateRoom);//服务器返回创建麻将房间消息
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_ENTER.CODE, OnServerBroadcastEnter);//服务器广播玩家进入消息
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播玩家离开消息
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_READY.CODE, OnServerBroadcastReady);//服务器广播玩家准备消息
//NetDispatcher.Instance.RemoveEventListener(OP_ROOM_TRUSTEE.CODE, OnServerBroadcastTrustee);//服务器广播玩家托管消息
//NetDispatcher.Instance.RemoveEventListener(OP_MATCH_WAIVER.CODE, OnServerBroadcastWaiver);//服务器广播玩家弃权消息
       NetDispatcher.Instance.RemoveEventListener(JY_ROOM_APPLYDISMISS.CODE, OnServerBroadcastApplyDisband);//服务器广播解散房间
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_REPLYDISMISS.CODE, OnServerBroadcastReplyDismiss);//服务器广播应答解散房间
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_RECREATE.CODE, OnServerReturnRebuild); //服务器返回重建房间


        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_GAMESTART.CODE, OnServerBroadcastBegin);//服务器广播开局消息
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_BEGIN.CODE, OnServerBroadcastSendPoker);//服务器广播发牌消息
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_JETTON.CODE, OnServerBroadcastJetton);//服务器广播下注（某玩家下注某分）
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_GIVEUPPOKER.CODE, OnServerBroadcastGiveupPoker);//服务器广播弃牌
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_OPENPOKER.CODE, OnServerOpenPoker);//服务器广播翻牌
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_SETTLE.CODE, OnServerBroadcastAloneSettle);//服务器广播个人结算信息
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_SHUFFLE.CODE, OnServerBroadcastShuffle);//服务器广播重新洗牌
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_LOOPENDSETTLE.CODE, OnServerBroadcastSettle);//服务器广播本局结算
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_NEXT.CODE, OnServerNextGame);//服务器广播准备下一局
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_INFORMBANKER.CODE, OnServerChooseBanker);//选庄
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_RESULT.CODE, OnServerResult); //服务器总结算信息
        NetDispatcher.Instance.RemoveEventListener(JY_ROOM_GAMEOVER.CODE, OnServerGameOver);//游戏结束

        //NetWorkSocket.Instance.OnDisconnect -= OnDisConnectCallBack;//断开连接事件

        //DelegateDefine.Instance.OnHeadClick -= OnHeadClick;
        //DelegateDefine.Instance.OnPlayPoker -= OnPlayPoker;//出牌事件
    }
    #endregion







    #region Override ISystemCtrl
    #region OpenView 打开窗口
    /// <summary>
    /// 打开窗口
    /// </summary>
    /// <param name="type"></param>
    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Settle:
                OpenSettleView();
                break;
            case UIWindowType.Ranking:
                OpenResultView();
                break;
        }
    }
    #endregion

    #region OpenSettleView 打开结算界面
    /// <summary>
    /// 打开结算界面
    /// </summary>
    private void OpenSettleView()
    {
        if (m_UIResultView != null) return;
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Settle, (GameObject go) =>
        {
            m_UISettleView = go.GetComponent<UIWindowSettle_JuYou>();
            m_UISettleView.Settle(RoomJuYouProxy.Instance.CurrentRoom/*倒计时*/);
        });
    }
    #endregion

    #region OpenResultView 打开结束界面
    /// <summary>
    /// 打开结束界面
    /// </summary>
    private void OpenResultView()
    {
        
        if (m_UISettleView != null) m_UISettleView.Close();
        if (m_Result == null) return;
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Ranking, (GameObject go) =>
        {
            m_UIResultView = go.GetComponent<UIWindowResult_JuYou>();
            m_UIResultView.SetUI(RoomJuYouProxy.Instance.CurrentRoom);
            m_Result = null;
        });
    }
    #endregion
    #endregion


    #region Override IGameCtrl
    #region CreateRoom 创建房间
    /// <summary>
    /// 创建房间
    /// </summary>
    public void CreateRoom(int groupId, List<int> settingIds)
    {
        //转为GameCtrl控制 

        ClientSendCreateRoom(groupId, settingIds);

        //m_CurrentType = EnterRoomType.Create;
        //GameServerEntity entity = GameServerProxy.Instance.CurrentGameServer;
        //if (NetWorkSocket.Instance.Connected())
        //{
        //    NetWorkSocket.Instance.Close();
        //}
        ////ConnectServer(EnterRoomType.Create, entity.ipaddr, entity.port);
        //ConnectServer(EnterRoomType.Create, "192.168.1.123", 8102);
    }
    #endregion

    #region JoinRoom 加入房间
    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="roomId"></param>
    public void JoinRoom(int roomId)
    {
        ClientSendJoinRoom(roomId);
        //m_nCurrentJoinRoomId = roomId;
        //m_CurrentType = EnterRoomType.Join;

        //GameServerEntity entity = GameServerProxy.Instance.CurrentGameServer;
        ////entity.ipaddr = "192.168.1.105";
        ////entity.port = 8122;
        //if (NetWorkSocket.Instance.Connected())
        //{
        //    NetWorkSocket.Instance.Close();
        //}
        ////ConnectServer(EnterRoomType.Join, entity.ipaddr, entity.port);
        //ConnectServer(EnterRoomType.Join, "192.168.1.123", 8102);
    }
    #endregion

    #region RebuildRoom 重建房间
    /// <summary>
    /// 重建房间
    /// </summary>
    public void RebuildRoom()
    {
        ClientSendRebuild();
        //m_CurrentType = EnterRoomType.Renter;

        //GameServerEntity entity = GameServerProxy.Instance.CurrentGameServer;

        //if (NetWorkSocket.Instance.Connected())
        //{
        //    NetWorkSocket.Instance.Close();
        //}
        ////ConnectServer(EnterRoomType.Renter, entity.ipaddr, entity.port);
        //ConnectServer(EnterRoomType.Renter, "192.168.1.123", 8102);
    }
    #endregion

    #region QuitRoom 退出房间
    /// <summary>
    /// 退出房间
    /// </summary>
    public void QuitRoom()
    {
        
 
        if (RoomJuYouProxy.Instance.CurrentRoom.matchId > 0)
        {
            ShowMessage("提示", "是否退赛", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
            return;
        }

        if (RoomJuYouProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.GAME )
        {
            ShowMessage("提示", "游戏过程中无法退出房间", MessageViewType.Ok, null, null, 1, AutoClickType.Ok);
            return;
        }



        if ((RoomJuYouProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.IDLE || (RoomPaiJiuProxy.Instance.CurrentRoom.currentLoop == 0)))
        {
            
            ShowMessage("提示", "是否退出房间", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
        }
        else
        {

            ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom);
        }


    }
    #endregion

    #region DisbandRoom 解散房间
    /// <summary>
    /// 解散房间
    /// </summary>
    public void DisbandRoom()
    {
        if (RoomJuYouProxy.Instance.CurrentRoom.matchId > 0)
        {
            UIViewManager.Instance.ShowMessage("提示", "比赛场不能解散房间");
        }
        else
        {
            UIViewManager.Instance.ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom);
        }
    }
    #endregion

    #region OnReceiveMessage接收聊天消息
    /// <summary>
    /// 接收聊天消息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="playerId"></param>
    /// <param name="message"></param>
    public void OnReceiveMessage(ChatType type, int playerId, string message, string audioName)
    {
        SeatEntity seat = RoomJuYouProxy.Instance.GetSeatByPlayerId(playerId);
        if (type == ChatType.Expression)
        {
            UIItemChat.Instance.ShowExpression(seat.Index, message);
        }
        else
        {
            if (!string.IsNullOrEmpty(audioName))
            {
                AudioEffectManager.Instance.Play(string.Format("{0}_{1}", seat.Gender, audioName), Vector3.zero, false);
            }
            UIItemChat.Instance.ShowMessage(seat.Index, message);
        }
    }
    #endregion

    #region GetRoomEntity 获取房间数据实体
    /// <summary>
    /// 获取房间数据实体
    /// </summary>
    /// <returns></returns>
    public RoomEntityBase GetRoomEntity()
    {
        return RoomJuYouProxy.Instance.CurrentRoom;
    }
    #endregion

    public void OnReceiveMessage(ChatType type, int playerId, string message, string audioName, int toPlayerId)
    {
        SeatEntity seat = RoomJuYouProxy.Instance.GetSeatByPlayerId(playerId);
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
            SeatEntity toSeat = RoomJuYouProxy.Instance.GetSeatByPlayerId(toPlayerId);

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



    //#region OnDisConnectCallBack 网络连接中断回调
    ///// <summary>
    ///// 网络连接中断回调
    ///// </summary>
    //private void OnDisConnectCallBack(bool isActiveClose)
    //{
    //    if (SceneMgr.Instance.CurrentSceneType != SceneType.JuYou3D) return;
    //    if (isActiveClose)
    //    {
    //        if (this != null)
    //        {
    //            RebuildRoom();
    //        }
    //    }
    //    else
    //    {
    //        RebuildRoom();
    //    }
    //}
    //#endregion

    //#region ConnectServer 连接服务器
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
    //#endregion

    //#region OnConnectedCallBack 连接服务器回调
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
    //            ClientSendCreateRoom();
    //        }
    //        else if (m_CurrentType == EnterRoomType.Join)
    //        {
    //            ClientSendJoinRoom(m_nCurrentJoinRoomId);
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
    //#endregion


    #region SeeResult 查看牌局总结算信息
    /// <summary>
    /// 查看牌局总结算信息
    /// </summary>
    private void SeeResult()
    {
        //if (m_Result == null)
        //{
        //    ExitGame();
        //    return;
        //}
        if (RoomJuYouProxy.Instance.PlayerSeat.isJoinGame ==false)
        {
            ExitGame();
            return;
        }
        OpenResultView();
    }
    #endregion

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
    #endregion

    #region ExitGame 退出本局游戏
    /// <summary>
    /// 退出本局游戏
    /// </summary>
    private void ExitGame()
    {
        GameCtrl.Instance.ExitGame();
    }
    #endregion





    #region 客户端发送消息

    #region ClientSendCreateRoom 客户端发送创建房间消息
    /// <summary>
    /// 客户端发送创建房间消息
    /// </summary>
    private void ClientSendCreateRoom(int groupId, List<int> settingIds)
    {
        JY_ROOM_CREATE_GET proto = new JY_ROOM_CREATE_GET();

        for (int i = 0; i < settingIds.Count; ++i)
        {
            proto.addSettingId(settingIds[i]);
        }
        //List<cfg_settingEntity> lst = cfg_settingDBModel.Instance.GetOptionsByGameId(GameCtrl.Instance.CurrentGameId);
        //for (int i = 0; i < lst.Count; ++i)
        //{
        //    if (lst[i].status == 1 && lst[i].init == 1)
        //    {
        //        proto.addSettingId(lst[i].id);
        //    }
        //}
        //proto.clubId = groupId;
        NetWorkSocket.Instance.Send(proto.encode(), JY_ROOM_CREATE_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendJoinRoom 客户端发送加入房间
    /// <summary>
    /// 客户端发送加入房间
    /// </summary>
    private void ClientSendJoinRoom(int roomId)
    {
        JY_ROOM_ENTER_GET proto = new JY_ROOM_ENTER_GET();
        proto.roomId = roomId;
        NetWorkSocket.Instance.Send(proto.encode(), JY_ROOM_ENTER_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRebuild 客户端发送重建房间
    /// <summary>
    /// 客户端发送重建房间
    /// </summary>
    private void ClientSendRebuild()
    {
        NetWorkSocket.Instance.Send(null, JY_ROOM_RECREATE_GET.CODE, GameCtrl.Instance.SocketHandle);
        //ClientSendFocus(true);
    }
    #endregion

    #region ClientSendLeaveRoom 客户端发送离开房间
    /// <summary>
    /// 客户端发送离开房间
    /// </summary>
    private void ClientSendLeaveRoom()
    {
        NetWorkSocket.Instance.Send(null, JY_ROOM_LEAVE_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion



    #region ClientSendApplyDisbandRoom 客户端发送请求解散房间
    /// <summary>
    /// 客户端发送请求解散房间
    /// </summary>
    private void ClientSendApplyDisbandRoom()
    {
        NetWorkSocket.Instance.Send(null, JY_ROOM_APPLYDISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendAgreeDisbandRoom 客户端发送同意解散房间
    /// <summary>
    /// 客户端发送同意解散房间
    /// </summary>
    private void ClientSendAgreeDisbandRoom()
    {
        JY_ROOM_REPLYDISMISS_GET proto = new JY_ROOM_REPLYDISMISS_GET();
        proto.isDismiss = true;
        NetWorkSocket.Instance.Send(proto.encode(), JY_ROOM_REPLYDISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRefuseDisbandRoom 客户端发送拒绝解散房间
    /// <summary>
    /// 客户端发送拒绝解散房间
    /// </summary>
    private void ClientSendRefuseDisbandRoom()
    {
        JY_ROOM_REPLYDISMISS_GET proto = new JY_ROOM_REPLYDISMISS_GET();
        proto.isDismiss = false;
        NetWorkSocket.Instance.Send(proto.encode(), JY_ROOM_REPLYDISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRefuseDisbandRoom 客户端发送获取个人结算信息
    /// <summary>
    /// 客户端发送拒绝解散房间
    /// </summary>
    private void ClientSendRefuseAloneSettle(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, JY_ROOM_SETTLE_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRefuseDisbandRoom 客户端发送开始下一把
    /// <summary>
    /// 客户端发送拒绝解散房间
    /// </summary>
    private void ClientSendRefuseNextGame(object[] obj)
    {
        if (m_UISettleView != null)  m_UISettleView.Close();
        NetWorkSocket.Instance.Send(null, JY_ROOM_NEXT_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
 


    #endregion




    #endregion

    #region 按钮点击


    #region 客户端发送确认下注
    /// <summary>
    ///  确认下注
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnJuYouViewPour(object[] obj)
    {

        if (obj == null)
        {
            NetWorkSocket.Instance.Send(null, JY_ROOM_JETTON_GET.CODE, GameCtrl.Instance.SocketHandle);
        }
        else
        {
            JY_ROOM_JETTON_GET proto = new JY_ROOM_JETTON_GET();
            proto.pour = (int)obj[0];
            proto.isAllBag = (bool)obj[1];
            NetWorkSocket.Instance.Send(proto.encode(), JY_ROOM_JETTON_GET.CODE, GameCtrl.Instance.SocketHandle);
        }

    }
    #endregion

   


    #region OnBtnMaJiangViewShareClick 分享按钮点击
    /// <summary>
    /// 分享按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMaJiangViewShareClick(object[] obj)
    {
        AppDebug.Log("微信邀请");
        ShareCtrl.Instance.ShareURL(ShareType.InGame);
    }
    #endregion

    #region OnBtnResultViewBack 牌局结果界面返回按钮点击
    /// <summary>
    /// 牌局结果界面返回按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnResultViewBack(object[] obj)
    {
        ExitGame();
        //if (RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0)
        //{
        //    m_UIResultView.Close();
        //    MatchCtrl.Instance.StartNext();
        //}
        //else
        //{
        //    ExitGame();
        //}
    }
    #endregion

    #region OnBtnResultViewShareClick 牌局结果界面分享按钮点击
    /// <summary>
    /// 牌局结果界面分享按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnResultViewShareClick(object[] obj)
    {
        if (m_UIResultView != null)
        {
            AppDebug.Log("开始总结算分享了啊");
            m_UIResultView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
        }
        else if (m_UISettleView != null)
        {
            AppDebug.Log("开始分享了啊");
            m_UISettleView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
        }
    }
    #endregion



    #region 客户端发送准备
    /// <summary>
    /// 客户端发送准备
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnJuYouViewReadyClick(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, JY_ROOM_READY_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region 客户端发送开始
    /// <summary>
    /// 客户端发送开始
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnJuYouViewStartClick(object[] obj)
    {
        NetWorkSocket.Instance.Send(null, JY_ROOM_GAMESTART_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    
    #region 客户端发送翻牌
    /// <summary>
    /// 客户端发送翻牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnPokerClick(object[] obj)
    {
        JY_ROOM_OPENPOKER_GET proto = new JY_ROOM_OPENPOKER_GET();

        //98*--+
       

        List<int> pokerIndexs = (List<int>)obj[0];
        for (int i = 0; i < pokerIndexs.Count; i++)
        {
            proto.addPokerIndex(pokerIndexs[i]);
        }
        NetWorkSocket.Instance.Send(proto.encode(), JY_ROOM_OPENPOKER.CODE, GameCtrl.Instance.SocketHandle);
    }
    public void OnPokerClick(MaJiangCtrl_JuYou ctrl)
    {
        OnPokerClick(new object[] { new List<int> { ctrl.Poker.index }  });

    }

    #endregion


    #endregion


    #region 服务器返回消息
    #region OnServerReturnCreateRoom 服务器返回创建房间消息
    /// <summary>
    /// 服务器返回创建房间消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnCreateRoom(byte[] obj)
    {
        JY_ROOM_CREATE proto = JY_ROOM_CREATE.decode(obj);
        UIViewManager.Instance.CloseWait();
        RoomJuYouProxy.Instance.InitRoom(proto.room);

        SceneMgr.Instance.LoadScene(SceneType.JuYou3D);
    }
    #endregion

    #region OnServerBroadcastReady 服务器广播玩家准备消息
    /// <summary>
    /// 服务器广播玩家准备消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastReady(byte[] obj)
    {
        JY_ROOM_READY proto = JY_ROOM_READY.decode(obj);
        RoomJuYouProxy.Instance.Ready(proto);
    }
    #endregion

    #region OnServerBroadcastEnter 服务器广播玩家进入消息
    /// <summary>
    /// 服务器广播玩家进入消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnter(byte[] obj)
    {
        JY_ROOM_ENTER proto = JY_ROOM_ENTER.decode(obj);
        RoomJuYouProxy.Instance.EnterRoom(proto);

    }
    #endregion

    #region OnServerBroadcastLeave 服务器广播玩家离开消息
    /// <summary>
    /// 服务器广播玩家离开消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLeave(byte[] obj)
    {
        JY_ROOM_LEAVE proto = JY_ROOM_LEAVE.decode(obj);
        RoomJuYouProxy.Instance.ExitRoom(proto);
    }
    #endregion

    #region OnServerBroadcastLeave 服务器广播玩家是否在线
    /// <summary>
    /// 服务器广播玩家是否在线消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastOnLine(byte[] obj)
    {

        JY_ROOM_ONLINE proto = JY_ROOM_ONLINE.decode(obj);
        RoomJuYouProxy.Instance.OnLine(proto);
    }
    #endregion




    #region OnServerBroadcastBegin 服务器广播开局
    /// <summary>
    /// 服务器广播开局
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastBegin(byte[] obj)
    {
        JY_ROOM_GAMESTART proto = JY_ROOM_GAMESTART.decode(obj);
        RoomJuYouProxy.Instance.Begin(proto);

        if (JuYouSceneCtrl.Instance != null)
        {
            JuYouSceneCtrl.Instance.Begin(RoomJuYouProxy.Instance.CurrentRoom, true);
        }
    }
    #endregion

    #region OnServerBroadcastBegin 服务器广播发牌
    /// <summary>
    /// 服务器广播开局
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastSendPoker(byte[] obj)
    {
        JY_ROOM_BEGIN proto = JY_ROOM_BEGIN.decode(obj);
        RoomJuYouProxy.Instance.SendPoker(proto);

        if (JuYouSceneCtrl.Instance != null)
        {

            JuYouSceneCtrl.Instance.DealPoker(proto.seat.pos, true);
        }
    }
    #endregion

    #region OnServerBroadcastLeave 服务器广播玩家下注
    /// <summary>
    /// 服务器广播下注
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastJetton(byte[] obj)
    {
        JY_ROOM_JETTON proto = JY_ROOM_JETTON.decode(obj);
        RoomJuYouProxy.Instance.Jetton(proto);
        if (JuYouSceneCtrl.Instance != null && proto.hasPour())
        {
            //JuYouSceneCtrl.Instance.DealPoker(proto.pos, true);
            JuYouSceneCtrl.Instance.BroadcastJetton(proto.pos);
            //BroadcastJetton
            //JuYouSceneCtrl.Instance.Begin(RoomJuYouProxy.Instance.CurrentRoom, true);
        }
    }
    #endregion


    #region OnServerBroadcastLeave 服务器广播玩家个人结算
    /// <summary>
    /// 服务器广播个人结算
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastAloneSettle(byte[] obj)
    {

        JY_ROOM_SETTLE proto = JY_ROOM_SETTLE.decode(obj);
        RoomJuYouProxy.Instance.AloneSettle(proto);

        if (JuYouSceneCtrl.Instance != null)
        {
            //玩家个人弃牌
            JuYouSceneCtrl.Instance.AloneSettle(proto.pos,false);
        }



        //JY_ROOM_JETTON proto = JY_ROOM_JETTON.decode(obj);
        //RoomJuYouProxy.Instance.Jetton(proto);
        //if (JuYouSceneCtrl.Instance != null && proto.hasPour())
        //{
        //    JuYouSceneCtrl.Instance.DealPoker(proto.pos, true);
        //    //JuYouSceneCtrl.Instance.Begin(RoomJuYouProxy.Instance.CurrentRoom, true);
        //}
    }
    #endregion

    #region OnServerBroadcastshuffle 服务器广播重新洗牌
    /// <summary>
    /// 服务器广播重新洗牌
    /// </summary>
    private void OnServerBroadcastShuffle(byte[] obj)
    {

        if (JuYouSceneCtrl.Instance != null)
        {
            JuYouSceneCtrl.Instance.Shuffle();  
        }


    }
    #endregion


    #region OnServerBroadcastGiveupPoker 服务器广播玩家弃牌
    /// <summary>
    ///  服务器广播玩家弃牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastGiveupPoker(byte[] obj)
    {
        JY_ROOM_GIVEUPPOKER proto = JY_ROOM_GIVEUPPOKER.decode(obj);
        RoomJuYouProxy.Instance.GiveupPoker(proto);
        
        if (JuYouSceneCtrl.Instance != null)
        {
            //玩家弃牌
            JuYouSceneCtrl.Instance.AloneSettle(proto.pos,true);

        }
    }
    #endregion
    #region OnServerOpenPoker 服务器广播翻牌
    /// <summary>
    /// 服务器广播翻牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerOpenPoker(byte[] obj)
    {
        JY_ROOM_OPENPOKER proto = JY_ROOM_OPENPOKER.decode(obj);

        //RoomPaiJiuProxy.Instance.OnServerOpenPoker(proto);

        if (JuYouSceneCtrl.Instance != null)
        {

            JuYouSceneCtrl.Instance.SetHandPokerStatus(proto.pos, proto.getPokerIndexList());
        }



    }
    #endregion


    #region OnServerBroadcastSettle 服务器广播结算
    /// <summary>
    /// 服务器广播结算
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastSettle(byte[] obj)
    {
        AppDebug.Log("服务器广播每局结算");

        JY_ROOM_LOOPENDSETTLE proto = JY_ROOM_LOOPENDSETTLE.decode(obj);
        

        RoomJuYouProxy.Instance.OnServerResult(proto);

        if (JuYouSceneCtrl.Instance != null)
        {
            JuYouSceneCtrl.Instance.Settle();
        }



    }
    #endregion


    #region OnServerNextGame 服务器广播准备下一局
    /// <summary>
    /// 
    /// </summary>
    private void OnServerNextGame(byte[] obj)
    {
        AppDebug.Log("服务器广播准备下一局");
        if (m_UISettleView != null) m_UISettleView.Close();
        JY_ROOM_NEXT proto = JY_ROOM_NEXT.decode(obj);
        RoomJuYouProxy.Instance.OnServerNextGame(proto);
        if (JuYouSceneCtrl.Instance != null)
        {
            JuYouSceneCtrl.Instance.NextGame();
        }


    }
    #endregion

    #region OnServerBroadcastSettle 服务器广播换庄
    /// <summary>
    /// 服务器广播换庄
    /// </summary>
    private void OnServerChooseBanker(byte[] obj)
    {
        AppDebug.Log("服务器广播换庄");
        JY_ROOM_INFORMBANKER proto = JY_ROOM_INFORMBANKER.decode(obj);
        RoomJuYouProxy.Instance.OnServerChooseBanker(proto);
        //if (PaiJiuSceneCtrl.Instance != null)
        //{
        //    PaiJiuSceneCtrl.Instance.ChooseBanker();
        //}



    }
    #endregion

    #region OnServerResult 服务器广播总结算信息
    /// <summary>
    /// 服务器广播总结算信息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerResult(byte[] obj)
    {
        JY_ROOM_RESULT proto = JY_ROOM_RESULT.decode(obj);
        m_Result = proto;
        RoomJuYouProxy.Instance.OnServerResult(proto);
    }
    #endregion



    #region OnServerGameOver 服务器广播游戏结束
    /// <summary>
    /// 服务器广播游戏结束
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerGameOver(byte[] obj)
    {
        AppDebug.Log("服务器广播游戏结束");


        if (JuYouSceneCtrl.Instance != null)
        {
            JuYouSceneCtrl.Instance.GameOver();
        }

        //SeeResult();
    }
    #endregion



    #region OnServerReturnRebuild 服务器返回重建房间
    /// <summary>
    /// 服务器返回重建房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnRebuild(byte[] obj)
    {
        JY_ROOM_RECREATE proto = JY_ROOM_RECREATE.decode(obj);

        RoomJuYouProxy.Instance.InitRoom(proto.room);


        SceneMgr.Instance.LoadScene(SceneType.JuYou3D);
    }
    #endregion


    #region OnServerBroadcastApplyDisband 服务器广播解散房间
    /// <summary>
    /// 服务器广播解散房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastApplyDisband(byte[] obj)
    {

        JY_ROOM_APPLYDISMISS proto = JY_ROOM_APPLYDISMISS.decode(obj);
        if (proto.hasIsSucceed())
        {
            if (proto.isSucceed)
            {
                //RoomPaiJiuProxy.Instance.RoomDisband();
                UIViewManager.Instance.ShowMessage("提示", "房间已解散", MessageViewType.Ok, SeeResult);
            }
            else
            {
                RoomJuYouProxy.Instance.CurrentRoom.roomStatus = ROOM_STATUS.IDLE;
                UIViewManager.Instance.ShowMessage("提示", "房间解散失败", MessageViewType.Ok);
            }
        }
        else
        {
            if (proto.hasPos())
            {


                //是否是自己申请
                if (proto.pos == RoomJuYouProxy.Instance.PlayerSeat.Pos)
                {
                    RoomJuYouProxy.Instance.CurrentRoom.roomStatus = ROOM_STATUS.IDLE;
                    UIViewManager.Instance.ShowMessage("提示", "等待申请解散结果", MessageViewType.None);

                }
                else
                {
                    UIViewManager.Instance.ShowMessage("提示", "有人发起解散房间，是否同意", MessageViewType.OkAndCancel, ClientSendAgreeDisbandRoom, ClientSendRefuseDisbandRoom, 10f, AutoClickType.Cancel);

                }

            }


        }
       if(proto.hasStatus())   RoomJuYouProxy.Instance.CurrentRoom.roomStatus = proto.status;

        ////是否解散成功 //是否已经是申请解散状态 //某玩家是否同意解散 //该玩家是否是自己
        //PAIGOW_ROOM_DISMISS proto = PAIGOW_ROOM_DISMISS.decode(obj);
        //if (proto.hasIsSucceed())
        //{
        //    if (proto.isSucceed)
        //    {
        //        RoomPaiJiuProxy.Instance.RoomDisband();
        //        UIViewManager.Instance.ShowMessage("提示", "房间已解散", MessageViewType.Ok, SeeResult);
        //    }
        //    else
        //    {
        //        UIViewManager.Instance.ShowMessage("提示", "房间解散失败", MessageViewType.Ok);

        //    }
        //}
        //else
        //{
        //    //是否是申请解散
        //    if (RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus != ROOM_STATUS.DISMISS)
        //    {
        //        //是否是自己申请
        //        if (proto.pos == RoomPaiJiuProxy.Instance.PlayerSeat.Pos)
        //        {
        //            UIViewManager.Instance.ShowMessage("提示", "等待申请解散结果", MessageViewType.None);

        //        }
        //        else
        //        {
        //            UIViewManager.Instance.ShowMessage("提示", "有人发起解散房间，是否同意", MessageViewType.OkAndCancel, ClientSendAgreeDisbandRoom, ClientSendRefuseDisbandRoom, 10f, AutoClickType.Cancel);

        //        }
        //    }
        //    else
        //    {
        //        //某人同意或不同意
        //        if (proto.pos == RoomPaiJiuProxy.Instance.PlayerSeat.Pos)
        //        {
        //            UIViewManager.Instance.ShowMessage("提示", "等待申请解散结果", MessageViewType.None);

        //        }

        //    }

        //}

        //RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus = proto.room_status;
    }
    #endregion

    /// <summary>
    /// 服务器广播应答解散房间
    /// </summary>
    private void OnServerBroadcastReplyDismiss (byte[] obj)
    {
        //RoomJuYouProxy.Instance.PlayerSeat.
         UIViewManager.Instance.ShowMessage("提示", "等待申请解散结果", MessageViewType.None);
    }


    #endregion



    //OpenViewPaiJiu  以观察者模式提供加载窗口
    private void OpenViewJuYou(object[] obj)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            UIWindowType type = (UIWindowType)obj[i];
            OpenView(type);

        }

    }

    #region 加载房间后 加载已有窗口
    /// <summary>
    /// 加载房间后 加载已有窗口
    /// </summary>
    /// <param name="obj"></param>
    private void SetEnterRoomView(object[] obj)
    {
        //加载等待解散 或 是否同意解散
        RoomEntity room = RoomJuYouProxy.Instance.CurrentRoom;
        SeatEntity playerSeat = RoomJuYouProxy.Instance.PlayerSeat;


        switch (room.roomStatus)
        {

            //结算状态 
            case ROOM_STATUS.SETTLE:
               ClientSendRefuseNextGame(null);

                break;
            case ROOM_STATUS.DISMISS:

                bool isOperate = false;
                for (int i = 0; i < room.OperatePosList.Count; i++)
                {
                    if (room.OperatePosList[i] == playerSeat.Pos)
                    {
                        isOperate = true;
                    }
                }
                if (isOperate)
                {
                    UIViewManager.Instance.ShowMessage("提示", "等待解散结果", MessageViewType.None);
                }
                else
                {
                    UIViewManager.Instance.ShowMessage("提示", "有人发起解散房间，是否同意", MessageViewType.OkAndCancel, ClientSendAgreeDisbandRoom, ClientSendRefuseDisbandRoom, 10f, AutoClickType.Cancel);
                }

                break;

        }


    }

  
    #endregion


}
