//===================================================
//Author      : WZQ
//CreateTime  ：5/11/2017 2:27:12 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using niuniu.proto;
using NiuNiu;
using System;

public class NiuNiuGameCtrl : SystemCtrlBase<NiuNiuGameCtrl>, IGameCtrl
    {

    private UIRubPokerView_NiuNiu m_UIRubPokerView;//搓牌

    private UIUnitSettlement_NiuNiu m_UISettleView;//小结算

    private UINiuNiuRankListView m_UIResultView;//总结算

    private UIADHWindow_NiuNiu m_UIADHView;//同意解散人数窗口

    public NN_ROOM_CHECK m_Result;
    //public NN_ROOM_CHECK m_Result_Rotal;

    private UISeatInfoView m_UISeatInfoView;//座位信息

    #region DicNotificationInterests 注册UI事件
    /// <summary>
    /// 注册UI事件
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();


        dic.Add("btnResultViewBack_NiuNiu", OnBtnResultViewBack);//结束界面返回按钮btnResultViewBack
        dic.Add("btnResultViewShare_NiuNiu", OnBtnResultViewShareClick);//结束界面分享按钮  //  《------战绩分享
        dic.Add( ConstDefine_NiuNiu.ViewShare_BtnName, OnBtnNiuNiuViewShareClick);//分享按钮点击《------微信邀请

        dic.Add(ConstDefine_NiuNiu.ObKey_OnNiuNiuViewHeadClick, OnHeadClick);//头像按钮点击

        
        //初始加载窗口SetEnterRoomView
        dic.Add("SetEnterRoomView_NiuNiu", SetEnterRoomView);//刚进入游戏时 加载已有窗口

        //语音按钮抬起按下
        dic.Add(ConstDefine_NiuNiu.ObKey_OnBtnMicroUp, OnBtnMicroUp);//发送语音
        dic.Add(ConstDefine_NiuNiu.ObKey_OnBtnMicroCancel, OnBtnMicroCancel);//取消语音

        return dic;
    }


    #endregion

    #region 确认 取消发送语音
    //取消
    private void OnBtnMicroCancel(object[] obj)
    {
        ChatCtrl.Instance.CancelMicro();
    }
    //确认
    private void OnBtnMicroUp(object[] obj)
    {
       
        ChatCtrl.Instance.SendMicro();
    }

  #endregion



    public NiuNiuGameCtrl()
    {

       
        NetDispatcher.Instance.AddEventListener(NN_ROOM_CREATE.CODE, OnServerReturnCreateRoom);//服务器返回创建牛牛房间
        NetDispatcher.Instance.AddEventListener(NN_ROOM_RECREATE.CODE, OnServerReturnReconnectRoom);//服务器返回重连牛牛房间
        NetDispatcher.Instance.AddEventListener(NN_ROOM_ENTER.CODE, OnServerBroadcastEnter);//服务器广播玩家进入消息
        NetDispatcher.Instance.AddEventListener(NN_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播玩家离开消息
       



        NetDispatcher.Instance.AddEventListener(NN_ROOM_ASK_DISMISS.CODE, OnServerBroadcastApplyDisband);//服务器广播 有人发起解散房间
        NetDispatcher.Instance.AddEventListener(NN_ROOM_ANSWER_TO_DISMISS.CODE, OnServerBroadcastApplyDisbandSum);//服务器广播 当前同意人数
        NetDispatcher.Instance.AddEventListener(NN_ROOM_DISMISS_SUCCEED.CODE, OnServerBroadcastDisbandSucceed);//服务器广播 解散房间成功
        NetDispatcher.Instance.AddEventListener(NN_ROOM_DISMISS_FAIL.CODE, OnServerBroadcastDisbandFail);//服务器广播 解散房间失败
        NetDispatcher.Instance.AddEventListener(NN_ROOM_COME_BACK.CODE, OnServerBackHall);//服务器广播返回大厅

        //NetWorkSocket.Instance.OnDisconnect += OnDisConnectCallBack;//断开连接事件

    }

    public override void Dispose()
    {
        base.Dispose();
        NetDispatcher.Instance.RemoveEventListener(NN_ROOM_CREATE.CODE, OnServerReturnCreateRoom);//服务器返回创建牛牛房间
        NetDispatcher.Instance.RemoveEventListener(NN_ROOM_RECREATE.CODE, OnServerReturnReconnectRoom);//服务器返回重连牛牛房间
        NetDispatcher.Instance.RemoveEventListener(NN_ROOM_ENTER.CODE, OnServerBroadcastEnter);//服务器广播玩家进入消息
        NetDispatcher.Instance.RemoveEventListener(NN_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播玩家离开消息

        NetDispatcher.Instance.RemoveEventListener(NN_ROOM_ASK_DISMISS.CODE, OnServerBroadcastApplyDisband);//服务器广播 有人发起解散房间
        NetDispatcher.Instance.RemoveEventListener(NN_ROOM_ANSWER_TO_DISMISS.CODE, OnServerBroadcastApplyDisbandSum);//服务器广播 当前同意人数
        NetDispatcher.Instance.RemoveEventListener(NN_ROOM_DISMISS_SUCCEED.CODE, OnServerBroadcastDisbandSucceed);//服务器广播 解散房间成功
        NetDispatcher.Instance.RemoveEventListener(NN_ROOM_DISMISS_FAIL.CODE, OnServerBroadcastDisbandFail);//服务器广播 解散房间失败
        NetDispatcher.Instance.RemoveEventListener(NN_ROOM_COME_BACK.CODE, OnServerBackHall);//服务器广播返回大厅

        //NetWorkSocket.Instance.OnDisconnect -= OnDisConnectCallBack;//断开连接事件
    }



    #region  IGameCtrl 接口
    //创建房间
    public void CreateRoom(int groupId, List<int> settingIds)
    {
        //GameCtrl
        ClientSendCreateRoom(groupId,  settingIds);
        //m_CurrentType = EnterRoomType.Create;
        //    GameServerProxy.Instance.SetCurrentServer(2);
        //    GameServerEntity entity = GameServerProxy.Instance.CurrentGameServer;
        //    if (NetWorkSocket.Instance.Connected())
        //    {
        //    NetWorkSocket.Instance.Close();
        //    }
        //    ConnectServer(EnterRoomType.Create, entity.ipaddr, entity.port);

    }
    //加入房间
    public void JoinRoom(int roomId)
    {
        ClientSendJoinRoom(roomId);
        //m_nCurrentJoinRoomId = roomId;
        //m_CurrentType = EnterRoomType.Join;
        //if (NetWorkSocket.Instance.Connected())
        //{
        //    NetWorkSocket.Instance.Close();
        //}
        //ConnectServer(EnterRoomType.Join, GameServerProxy.Instance.CurrentGameServer.ipaddr, GameServerProxy.Instance.CurrentGameServer.port);

    }

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
        //ConnectServer(EnterRoomType.Renter, entity.ipaddr, entity.port);
        
    }
    #endregion

    #region QuitRoom 退出房间
    /// <summary>
    /// 退出房间
    /// </summary>
    public void QuitRoom()
    {

        if (RoomNiuNiuProxy.Instance.CurrentRoom.matchId > 0)
        {
            ShowMessage("提示", "是否退赛", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
            return;
        }




        if (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus != NN_ENUM_ROOM_STATUS.IDLE && (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus != NN_ENUM_ROOM_STATUS.GAMEOVER))
        {
            ShowMessage("提示", "游戏过程中无法退出房间", MessageViewType.Ok, null, null, 1, AutoClickType.Ok);
        }
        else if ((RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus == NN_ENUM_ROOM_STATUS.IDLE && (RoomNiuNiuProxy.Instance.CurrentRoom.currentLoop <= 0) )
             || (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus == NN_ENUM_ROOM_STATUS.GAMEOVER))
        {
            //直接退出
            ClientSendLeaveRoom();
            //ShowMessage("提示", "是否退出房间", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
        }    
        else
        {
            ShowMessage("提示", "是否申请解解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom);
        } 

    }
    #endregion

    #region DisbandRoom 解散房间
    /// <summary>
    /// 解散房间
    /// </summary>
    public void DisbandRoom()
    {
        if (RoomNiuNiuProxy.Instance.CurrentRoom.matchId > 0)
        {
            UIViewManager.Instance.ShowMessage("提示", "比赛场不能解散房间");
        }
        else
        {
            if (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus != NN_ENUM_ROOM_STATUS.IDLE && (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus != NN_ENUM_ROOM_STATUS.GAMEOVER))
            {
                ShowMessage("提示", "游戏过程中无法解散房间", MessageViewType.Ok, null, null, 1, AutoClickType.Ok);
                return;
            }
            //未开始前 只有房主能解散

            if (RoomNiuNiuProxy.Instance.CurrentRoom.currentLoop <= 0)
            {
                if (RoomNiuNiuProxy.Instance.PlayerSeat.IsBanker)
                {
                    UIViewManager.Instance.ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom);
                }
                else
                {
                    UIViewManager.Instance.ShowMessage("提示", "暂无权解散房间", MessageViewType.Ok);
                }

            }
            else
            {
                UIViewManager.Instance.ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom);
            }





        }
    }
    #endregion


    /// <summary>
    /// 接收聊天消息
    /// </summary>
    public void OnReceiveMessage(ChatType type, int playerId, string message, string audioName)
    {
        NiuNiu.Seat seat = RoomNiuNiuProxy.Instance.GetSeatByPlayerId(playerId);
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
    public void OnReceiveMessage(ChatType type, int playerId, string message, string audioName, int toPlayerId)
    {

        Seat seat = RoomNiuNiuProxy.Instance.GetSeatByPlayerId(playerId);
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
            Seat toSeat = RoomNiuNiuProxy.Instance.GetSeatByPlayerId(toPlayerId);

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


    #region ClientSendJoinRoom 客户端发送加入房间
    /// <summary>
    /// 客户端发送加入房间
    /// </summary>
    private void ClientSendJoinRoom(int roomId)
        {
  
        NN_ROOM_ENTER_GET proto = new NN_ROOM_ENTER_GET();
        proto.roomId = roomId;

            NetWorkSocket.Instance.Send(proto.encode(), NN_ROOM_ENTER.CODE, GameCtrl.Instance.SocketHandle);
        }
    #endregion









    //#region OnDisConnectCallBack 网络连接中断回调
    ///// <summary>
    ///// 网络连接中断回调
    ///// </summary>
    //private void OnDisConnectCallBack(bool isActiveClose)
    //{
    //    if (SceneMgr.Instance.CurrentSceneType != SceneType.NiuNiu2D) return;
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
    //    Debug.Log("niuniu连接服务器");
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

    #region OnHeadClick 头像点击
    /// <summary>
    /// 头像点击
    /// </summary>
    /// <param name="seatPos"></param>
    private void OnHeadClick(object[] param)
    {
       
        int seatPos = (int)param[0];
        Seat seat = RoomNiuNiuProxy.Instance.GetSeatBySeatPos(seatPos);
        if (seat == RoomNiuNiuProxy.Instance.PlayerSeat && AccountProxy.Instance.CurrentAccountEntity.identity > 0)
        {
            UIViewManager.Instance.OpenWindow(UIWindowType.PlayerInfo);
            return;
        }

        List<SeatEntityBase> lstSeat = new List<SeatEntityBase>();
        for (int i = 0; i < RoomNiuNiuProxy.Instance.CurrentRoom.SeatList.Count; ++i)
        {
            if (RoomNiuNiuProxy.Instance.CurrentRoom.SeatList[i].Pos != seatPos && RoomNiuNiuProxy.Instance.CurrentRoom.SeatList[i].PlayerId > 0)
            {
                lstSeat.Add(RoomNiuNiuProxy.Instance.CurrentRoom.SeatList[i]);
            }
        }

        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.SeatInfo, (GameObject go) =>
        {
            m_UISeatInfoView = go.GetComponent<UISeatInfoView>();
            m_UISeatInfoView.SetUI(seat, lstSeat);
#if IS_LAOGUI
            if (seat != RoomNiuNiuProxy.Instance.PlayerSeat)
#endif
            {
                m_UISeatInfoView.SetEmoji(cfg_interactiveExpressionDBModel.Instance.GetList(), OnBtnInteractiveExpressionClick);
            }
        });
    }
    #endregion

    #region OnBtnInteractiveExpressionClick 互动表情点击
    /// <summary>
    /// 互动表情点击
    /// </summary>
    /// <param name="id"></param>
    private void OnBtnInteractiveExpressionClick(int seatPos,int id)
    {
        m_UISeatInfoView.Close();
        cfg_interactiveExpressionEntity entity = cfg_interactiveExpressionDBModel.Instance.Get(id);
        ChatCtrl.Instance.OnInteractiveClick( proto.common.ENUM_PLAYER_MESSAGE.ANIMATION, entity.code, RoomNiuNiuProxy.Instance.GetSeatBySeatPos(seatPos).PlayerId, entity.sound);
    }
    #endregion

    #region ClientSendCreateRoom 客户端发送创建房间消息
    /// <summary>
    /// 客户端发送创建房间消息
    /// </summary>
    private void ClientSendCreateRoom(int groupId, List<int> settingIds)
    {

        //UIViewManager.Instance.ShowWait();
       

        NN_ROOM_CREATE_GET proto = new NN_ROOM_CREATE_GET();

        for (int i = 0; i < settingIds.Count; ++i)
        {
            proto.addSettingId(settingIds[i]);
        }
   
        //《--------------------配置ID----------
        //List<cfg_settingEntity> lst = cfg_settingDBModel.Instance.GetOptionsByGameId(GameCtrl.Instance.CurrentGameId);

        //for (int i = 0; i < settingIds.Count; ++i)
        //{
 
        //    if (lst[i].status == 1 && lst[i].init == 1)
        //    {
        //        proto.addSettingId(lst[i].id);
        //    }
        //}
        proto.clubId = groupId;

        Debug.Log("开始发送创建房间消息");
        NetWorkSocket.Instance.Send(proto.encode(), NN_ROOM_CREATE.CODE, GameCtrl.Instance.SocketHandle);

    }
        #endregion

        #region ClientSendRebuild 客户端发送重建房间
        /// <summary>
        /// 客户端发送重建房间
        /// </summary>
        private void ClientSendRebuild()
        {
            NetWorkSocket.Instance.Send(null,NN_ROOM_RECREATE.CODE, GameCtrl.Instance.SocketHandle);




        }
    #endregion



    #region OnServerReturnCreateRoom 服务器返回创建房间消息
    /// <summary>
    /// 服务器返回创建房间消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnCreateRoom(byte[] obj)
    {
      
        UIViewManager.Instance.CloseWait();
   
        NN_ROOM_CREATE proto = NN_ROOM_CREATE.decode(obj);
        //收到配置数据存到模型基类

        GameStateManager.Instance.InitCurrentRoom(proto);

        
        Debug.Log("准备创建房间，自己ID：" + AccountProxy.Instance.CurrentAccountEntity.passportId);
        SceneMgr.Instance.LoadScene(SceneType.NiuNiu2D);



    }
    #endregion

    #region OnServerReturnCreateRoom 服务器返回断线重连消息
    /// <summary>
    /// 服务器断线重连房间消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnReconnectRoom(byte[] obj)
    {
        Debug.Log("服务器返回断线重连消息");
       
        
        UIViewManager.Instance.CloseWait();
        NN_ROOM_CREATE proto = NN_ROOM_CREATE.decode(obj);
        GameStateManager.Instance.InitCurrentRoom(proto);
        Debug.Log("准备断线重连创建房间，自己ID：" + AccountProxy.Instance.CurrentAccountEntity.passportId);

        SceneMgr.Instance.LoadScene(SceneType.NiuNiu2D);
        

    }
    #endregion




   


    #region OnServerBroadcastEnter 服务器广播玩家进入消息
    /// <summary>
    /// 服务器广播玩家进入消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnter(byte[] obj)
    {
        Debug.Log("有人加入房间");
        NN_ROOM_ENTER proto = NN_ROOM_ENTER.decode(obj);
        GameStateManager.Instance.EnterRoom(proto);



    }
    #endregion



    #region OnServerBroadcastLeave 服务器广播玩家离开消息
    /// <summary>
    /// 服务器广播玩家离开消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLeave(byte[] obj)
    {


        NN_ROOM_LEAVE proto = NN_ROOM_LEAVE.decode(obj);
        GameStateManager.Instance.LeaveRoom(proto);

    }
    #endregion






    #region OnBtnResultViewBack 牌局结果界面返回按钮点击
    /// <summary>
    /// 牌局结果界面返回按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnResultViewBack(object[] obj)
    {
        //if (RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0)
        //{
        //    m_UIResultView.Close();
        //    MatchCtrl.Instance.StartNext();
        //}
        //else
        //{
        Debug.Log("由点击按钮 退出房间");
        ExitGame();
        //}
    }
    #endregion






    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.RubPoker_NiuNiu:
                OpenRubPokerWindow();
                break;
            case UIWindowType.UnitSettlement_NiuNiu:
                OpenUnitSettlement_NiuNiu();
                break;
            case UIWindowType.RankList_NiuNiu:
                OpenRankList_NiuNiu();
                break;
        }
    }

    #region OpenRubPokerWindow 打开搓牌界面
    /// <summary>
    /// 打开搓牌界面
    /// </summary>
    private void OpenRubPokerWindow()
    {
        if (m_UIRubPokerView != null) return;


        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.RubPoker_NiuNiu, (GameObject go) =>
        {
            m_UIRubPokerView = go.GetComponent<UIRubPokerView_NiuNiu>();
            m_UIRubPokerView.SetUI(RoomNiuNiuProxy.Instance.PlayerSeat,()=> {

                UIRubPokerViewClose();

                if (RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus != NN_ENUM_ROOM_STATUS.LOOKPOCKER && RoomNiuNiuProxy.Instance.PlayerSeat.Pour <= 0)
                {
                    return;
                }

                //向服务器申请 翻开这张
                object[] obj = new object[] { 4 };
                NiuNiuEventDispatcher.Instance.Dispatch("NeedOpenPokerNiuNiu", obj);
                //模拟开牌
               Poker pokerLocal = RoomNiuNiuProxy.Instance.PlayerSeat.PokerList[RoomNiuNiuProxy.Instance.PlayerSeat.PokerList.Count - 1];
                NN_ROOM_DRAW proto = new NN_ROOM_DRAW();
                proto.pos = RoomNiuNiuProxy.Instance.PlayerSeat.Pos;
                NN_POKER poker = new NN_POKER();
                poker.index = pokerLocal.index;
                poker.size = pokerLocal.size;
                poker.color = pokerLocal.color;
                poker.pokerStatus = pokerLocal.status;
                proto.addNnPoker(poker);
                RoomNiuNiuProxy.Instance.OpenAPoker(proto);

                //TransferData data = new TransferData();
                //data.SetValue<NiuNiu.Seat>("Seat", RoomNiuNiuProxy.Instance.PlayerSeat);
                //SendNotification("SetShowPokersUI", data);//设置某玩家手牌 

            });

        }
        );


    }
    #endregion

    public void UIRubPokerViewClose()
    {
        if (m_UIRubPokerView != null)
        {
            m_UIRubPokerView.Close();
            m_UIRubPokerView = null;
        }


    }




    #region OpenSettleView 打开单元小结算界面
    /// <summary>
    /// 打开单元结算界面
    /// </summary>
    private void OpenUnitSettlement_NiuNiu()
    {
        if (m_UIResultView != null) return;
        if (m_Result == null) return;
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.UnitSettlement_NiuNiu, (GameObject go) =>

            {
                m_UISettleView = go.GetComponent<UIUnitSettlement_NiuNiu>();
                m_UISettleView.SetUI(RoomNiuNiuProxy.Instance.CurrentRoom.SeatList, RoomNiuNiuProxy.Instance.PlayerSeat);
                //m_Result = null;

            }

    
        );




    }
    #endregion

    #region OpenResultView 打开结束界面(总结算界面)
    /// <summary>
    /// 打开结束界面
    /// </summary>
    private void OpenRankList_NiuNiu()
    {
        
        if (m_UISettleView != null) m_UISettleView.Close();
        

        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.RankList_NiuNiu, (GameObject go) =>
        {
            m_UIResultView = go.GetComponent<UINiuNiuRankListView>();
            m_UIResultView.ShowConclusionClusionPanel(RoomNiuNiuProxy.Instance.CurrentRoom);
            //m_UIResultView.SetUI(m_Result);
            m_Result = null;
        });
    }
    #endregion

    public void UISettleViewClose()
    {
        if (m_UISettleView != null) m_UISettleView.Close();


    }




    #region OnBtnResultViewShareClick 牌局结果界面分享按钮点击
    /// <summary>
    /// 牌局结果界面分享按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnResultViewShareClick(object[] obj)
    {
        if (m_UIResultView != null)
        {
            m_UIResultView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
        }
        else if (m_UISettleView != null)
        {
            AppDebug.Log("开始分享了啊");
            m_UISettleView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
        }
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


    #region OnBtnMaJiangViewShareClick 分享按钮点击 (微信邀请)
    /// <summary>
    /// 分享按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnNiuNiuViewShareClick(object[] obj)
    {
        Debug.Log("开始调用URL");
        ShareCtrl.Instance.ShareURL(ShareType.InGame);
    }
    #endregion







   


    #region ClientSendLeaveRoom 客户端发送离开房间
    /// <summary>
    /// 客户端发送离开房间
    /// </summary>
    private void ClientSendLeaveRoom()
    {
        NetWorkSocket.Instance.Send(null, NN_ROOM_LEAVE_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion


    #region ClientSendApplyDisbandRoom 客户端发送请求解散房间
    /// <summary>
    /// 客户端发送请求解散房间
    /// </summary>
    private void ClientSendApplyDisbandRoom()
    {
        ////禁止点击开始按钮
        //TransferData data = new TransferData();
        //data.SetValue<bool>("OnOff", true);
        //ModelDispatcher.Instance.Dispatch("EnableAllowStartBtn",data);//设置开始游戏按钮遮罩
       


        //RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus = NN_ENUM_ROOM_STATUS.DISSOLVE;
        NetWorkSocket.Instance.Send(null, NN_ROOM_ASK_DISMISS.CODE, GameCtrl.Instance.SocketHandle);

       
    }
    #endregion



    #region 解散房间相关
    /// <summary>
    /// 服务器广播有人请求解散房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastApplyDisband(byte[] obj)
    {
        Debug.Log("有人发起解散房间");
       
        NN_ROOM_ASK_DISMISS proto = NN_ROOM_ASK_DISMISS.decode(obj);
        if (!proto.hasPos()) return;
        //判断是否是自己发起
        if (proto.pos != RoomNiuNiuProxy.Instance.PlayerSeat.Pos)
        {
            //倒计时??
            UIViewManager.Instance.ShowMessage("提示", "有人发起解散房间，是否同意", MessageViewType.OkAndCancel, ClientSendAgreeDisbandRoom, ClientSendRefuseDisbandRoom, 5f);
            RoomNiuNiuProxy.Instance.SetApplicationDissolution(proto);
          
        }
        else if(proto.pos == RoomNiuNiuProxy.Instance.PlayerSeat.Pos)
        {
            Debug.Log("发起人为自己");
            //说明是自己申请解散
            //加载提示同意人数窗口
            RoomNiuNiuProxy.Instance.SetApplicationDissolution(proto);
            LoadDisbandSumView(RoomNiuNiuProxy.Instance.agreeDissolveCount, RoomNiuNiuProxy.Instance.CurrentRoom.serverTime);


        }

        RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus = NN_ENUM_ROOM_STATUS.DISSOLVE;
        //禁止点击开始按钮 开启遮罩
        TransferData data = new TransferData();
        data.SetValue<bool>("OnOff", true);
        ModelDispatcher.Instance.Dispatch(ConstDefine_NiuNiu.ObKey_EnableAllowStartBtn, data);//设置开始游戏按钮遮罩

    }


    /// <summary>
    /// 服务器广播同意人数
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastApplyDisbandSum(byte[] obj)
    {

        //只存数据
        //数据存到模型层 
        //当前同意人数 由模型层发消息改变显示
        //PBRoom pbRoom = PBRoom.ParseFrom(obj);
        NN_ROOM_ANSWER_TO_DISMISS proto = NN_ROOM_ANSWER_TO_DISMISS.decode(obj);
        RoomNiuNiuProxy.Instance.AgreeDissolveCount(proto);

     
    }

    
    /// <summary>
    ///   加载窗口  （暂用于 刚进入游戏时 加载已有窗口）
    /// </summary>
    /// <param name="data"></param>
    private void SetEnterRoomView(object[] obj)
    {
  
        NiuNiu.Room room = RoomNiuNiuProxy.Instance.CurrentRoom;
        NiuNiu.Seat seat = RoomNiuNiuProxy.Instance.PlayerSeat;


        switch (room.roomStatus)
        {
            case NN_ENUM_ROOM_STATUS.IDLE:
                break;
            case NN_ENUM_ROOM_STATUS.READY:
                break;
            case NN_ENUM_ROOM_STATUS.BEGIN:
                break;
            case NN_ENUM_ROOM_STATUS.RSETTLE:
                break;
            case NN_ENUM_ROOM_STATUS.DEAL:
                break;
            case NN_ENUM_ROOM_STATUS.POUR:
                break;
            case NN_ENUM_ROOM_STATUS.LOOKPOCKER:
                break;
            case NN_ENUM_ROOM_STATUS.DISSOLVE:

                //弹出是否同意解散窗口  或者 已同意人数窗口
                if (seat.Dissolve == NN_ENUM_SEAT_DISSOLVE.NOP)
                {
                    //计算倒计时
                    long time = room.serverTime;
                    long currTime = TimeUtil.GetTimestamp();
                    float countdownTime =( time - currTime) + GlobalInit.Instance.TimeDistance * 0.001f;
                    countdownTime = Mathf.Clamp(countdownTime, 0, 5);
                    //加载是否同意解散窗口
                    UIViewManager.Instance.ShowMessage("提示", "有人发起解散房间，是否同意", MessageViewType.OkAndCancel, ClientSendAgreeDisbandRoom, ClientSendRefuseDisbandRoom, countdownTime, AutoClickType.Cancel);
                
                }
                else if (seat.Dissolve == NN_ENUM_SEAT_DISSOLVE.AGREE && seat.Dissolve == NN_ENUM_SEAT_DISSOLVE.DISAGREE)
                {
                    //加载人数窗口
                    LoadDisbandSumView(RoomNiuNiuProxy.Instance.agreeDissolveCount, RoomNiuNiuProxy.Instance.CurrentRoom.serverTime);



                }
                break;
            case NN_ENUM_ROOM_STATUS.HOG:
                break;
            case NN_ENUM_ROOM_STATUS.GAMEOVER:
                break;
            default:
                break;
        }







    }




    /// <summary>
    /// 客户端发送同意解散房间
    /// </summary>
    private void ClientSendAgreeDisbandRoom()
    {
        NN_ROOM_ANSWER_TO_DISMISS_GET proto = new NN_ROOM_ANSWER_TO_DISMISS_GET();
        proto.dissolve = NN_ENUM_SEAT_DISSOLVE.AGREE;

        NetWorkSocket.Instance.Send(proto.encode(), NN_ROOM_ANSWER_TO_DISMISS.CODE, GameCtrl.Instance.SocketHandle);

        //加载同意数量窗口

        LoadDisbandSumView(RoomNiuNiuProxy.Instance.agreeDissolveCount, RoomNiuNiuProxy.Instance.CurrentRoom.serverTime);


    }

    /// <summary>
    /// 客户端发送拒绝解散房间
    /// </summary>
    private void ClientSendRefuseDisbandRoom()
    {
       
        NN_ROOM_ANSWER_TO_DISMISS_GET proto = new NN_ROOM_ANSWER_TO_DISMISS_GET();
        proto.dissolve = NN_ENUM_SEAT_DISSOLVE.DISAGREE;

        NetWorkSocket.Instance.Send(proto.encode(), NN_ROOM_ANSWER_TO_DISMISS.CODE, GameCtrl.Instance.SocketHandle);

        //加载同意数量窗口  调用数据
        LoadDisbandSumView(RoomNiuNiuProxy.Instance.agreeDissolveCount, RoomNiuNiuProxy.Instance.CurrentRoom.serverTime);




    }

    //加载同意解散人数窗口
    void LoadDisbandSumView(int sum,long svrTime,bool isCountdownNoOff=true)
    {
       
        if (m_UIADHView == null)
        {
            //加载同意数量窗口

            UIViewUtil.Instance.LoadWindowAsync(UIWindowType.ADH_NiuNiu, (GameObject go) =>
            {
                m_UIADHView = go.GetComponent<UIADHWindow_NiuNiu>();

            });

           
        }

        TransferData data = new TransferData();
        data.SetValue<int>("SetADHWindowSum", sum);
        data.SetValue<long>("SetSvrTime", svrTime);
        data.SetValue<bool>("IsCountdownNoOff", isCountdownNoOff);//是否显示倒计时
        m_UIADHView.SetUI(data);




    }




    /// <summary>
    /// 服务器广播解散房间成功
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastDisbandSucceed(byte[] obj)
    {


        //NetWorkSocket.Instance.Close();     //断开连接
        if (m_UIADHView != null)
        {
            m_UIADHView.Close();
        }
        //计数归零
        RoomNiuNiuProxy.Instance.DissolveResults();

        Debug.Log("服务器广播解散房间成功");
        UIViewManager.Instance.ShowMessage("提示", "房间已解散", MessageViewType.Ok, SeeResult);
       

    }

    /// <summary>
    /// 服务器广播返回大厅
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBackHall(byte[] obj)
    {
       
        UIViewManager.Instance.ShowMessage("提示", "游戏已结束,欲知详情请查询战绩。", MessageViewType.Ok, ExitGame,null,3,AutoClickType.Ok);
      
    }


    /// <summary>
    /// 退出本局游戏
    /// </summary>
    private void ExitGame()
    {
        Debug.Log("自动点击");
        GameCtrl.Instance.ExitGame();
    }

    /// <summary>
    /// 服务器广播解散房间失败
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastDisbandFail(byte[] obj)
    {
        if (m_UIADHView!=null)
        {
            m_UIADHView.Close();
        }
        //允许点击开始按钮 关闭遮罩
        TransferData data = new TransferData();
        data.SetValue<bool>("OnOff", false);
        ModelDispatcher.Instance.Dispatch(ConstDefine_NiuNiu.ObKey_EnableAllowStartBtn, data);//设置开始游戏按钮遮罩

        RoomNiuNiuProxy.Instance.DissolveResults();

        UIViewManager.Instance.ShowMessage("提示", "解散房间失败", MessageViewType.Ok);

    }
    #endregion


    //结果
    private void SeeResult()
    {
      
        if (RoomNiuNiuProxy.Instance.CurrentRoom.currentLoop <= 0  || m_Result==null )
        {
            Debug.Log("由解散房间成功 退出房间");
             ExitGame();
            return;
        }
        OpenView(UIWindowType.RankList_NiuNiu);
       
    }

    public RoomEntityBase GetRoomEntity()
    {
       return RoomNiuNiuProxy.Instance.CurrentRoom;
    }


}
