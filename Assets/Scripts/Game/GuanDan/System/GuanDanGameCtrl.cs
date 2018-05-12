//===================================================
//Author      : WZQ
//CreateTime  ：11/6/2017 11:10:53 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuanDan;
using guandan.proto;
public class GuanDanGameCtrl : SystemCtrlBase<GuanDanGameCtrl>, IGameCtrl, ISystemCtrl
{

    #region Variable
    private DRB.MahJong.UISettleView m_UISettleView;

    private UIResultView m_UIResultView;

    private UISeatInfoView m_UISeatInfoView;

    private UIDisbandView m_UIDisbandView;
    /// <summary>
    /// 当前进入房间类型
    /// </summary>
    private EnterRoomType m_CurrentType = EnterRoomType.None;
    /// <summary>
    /// 当前加入的房间Id
    /// </summary>
    private int m_nCurrentJoinRoomId;
    ///// <summary>
    ///// 游戏结果
    ///// </summary>
    //private OP_ROOM_RESULT m_Result;

    public bool IsTingByPlayPoker { get; set; }

    public bool IsLiangXi { get; set; }

   
    //private MaJiangCtrl m_CurrentSelect;

    private bool m_isChangingSeat;

    //public Queue<IGameCommand> CommandQueue = new Queue<IGameCommand>();//命令队列
    #endregion


    #region DicNotificationInterests 注册UI事件
    /// <summary>
    /// 注册UI事件
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        //dic.Add("btnReady", OnBtnSettleViewReadyClick);//结算界面准备按钮
        //dic.Add("btnSettleViewResult", OnBtnSettleViewResultClick);//结算界面查看结果按钮
        //dic.Add("btnResultViewBack", OnBtnResultViewBack);//结束界面返回按钮
        //dic.Add("btnResultViewShare", OnBtnResultViewShareClick);//结束界面分享按钮
        //dic.Add("btnSettleViewShare", OnBtnResultViewShareClick);//结算界面分享按钮
        //dic.Add(ConstDefine.BtnSettleViewReplayOver, OnBtnResultViewReplayOverClick);
        //dic.Add(ConstDefine.BtnMaJiangViewAuto, OnBtnMaJiangViewAutoClick);//托管按钮点击
        //dic.Add(ConstDefine.BtnMaJiangViewCancelAuto, OnBtnMaJiangViewCancelAutoClick);//取消托管按钮点击
        //dic.Add(ConstDefine.BtnMaJiangViewShare, OnBtnMaJiangViewShareClick);//分享按钮点击
        //dic.Add(ConstDefine.BtnMaJiangViewReady, OnBtnMaJiangViewReadyClick);//准备按钮点击
        //dic.Add(ConstDefine.BtnMaJiangViewCancelReady, OnBtnMaJiangViewCancelReadyClick);//取消准备按钮点击
        //dic.Add("OnOperatorClick", OnOperatorClick);//吃碰杠胡按钮点击
        //dic.Add("OnMahjongViewHeadClick", OnHeadClick);//头像按钮点击
        //dic.Add("OnBtnChangeSeatClick", OnBtnChangeSeatClick);//换座位按钮点击
        //dic.Add("btnDisbandViewAgree", OnBtnDisbandViewAgree);//解散房间界面同意按钮
        //dic.Add("btnDisbandViewRefuse", OnBtnDisbandViewRefuse);//解散房间界面拒绝按钮
        return dic;
    }
    #endregion

    #region Constructors
    public GuanDanGameCtrl()
    {
        NetDispatcher.Instance.AddEventListener(GD_CREATE_ROOM.CODE, OnServerReturnCreateRoom);//服务器返回创建牛牛房间
        NetDispatcher.Instance.AddEventListener(GD_REENTER.CODE, OnServerReturnReconnectRoom);//服务器返回重连牛牛房间
        NetDispatcher.Instance.AddEventListener(GD_ENTER_ROOM.CODE, OnServerBroadcastEnter);//服务器广播玩家进入消息
        NetDispatcher.Instance.AddEventListener(GD_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播玩家离开消息

        //NetDispatcher.Instance.AddEventListener(OP_ROOM_RESULT.CODE, OnServerReturnResult);//服务器返回结果消息
    }
    #endregion

    #region Dispose
    public override void Dispose()
    {
        base.Dispose();

        NetDispatcher.Instance.RemoveEventListener(GD_CREATE_ROOM.CODE, OnServerReturnCreateRoom);//服务器返回创建牛牛房间
        NetDispatcher.Instance.RemoveEventListener(GD_REENTER.CODE, OnServerReturnReconnectRoom);//服务器返回重连牛牛房间
        NetDispatcher.Instance.RemoveEventListener(GD_ENTER_ROOM.CODE, OnServerBroadcastEnter);//服务器广播玩家进入消息
        NetDispatcher.Instance.RemoveEventListener(GD_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播玩家离开消息

        //NetDispatcher.Instance.RemoveEventListener(OP_ROOM_RESULT.CODE, OnServerReturnResult);//服务器返回结果消息
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
            case UIWindowType.Result:
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
        //UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Settle, (GameObject go) =>
        //{
        //    m_UISettleView = go.GetComponent<UISettleView>();
        //    string huTaiCount = string.Empty;
        //    cfg_settingEntity settingEntity = RoomGuanDanProxy.Instance.GetConfigByLabel("胡台数");
        //    if (settingEntity != null)
        //    {
        //        huTaiCount = settingEntity.name;
        //    }
        //    cfg_settingEntity fengEntity = RoomGuanDanProxy.Instance.GetConfigByName("带风");
        //    bool isFeng = fengEntity != null;
        //    m_UISettleView.Settle(RoomGuanDanProxy.Instance.CurrentRoom.SeatList, RoomGuanDanProxy.Instance.PlayerSeat.Pos, RoomGuanDanProxy.Instance.CurrentRoom.currentLoop, RoomGuanDanProxy.Instance.CurrentRoom.maxLoop, RoomGuanDanProxy.Instance.CurrentRoom.Prob, RoomGuanDanProxy.Instance.CurrentRoom.matchId, RoomGuanDanProxy.Instance.CurrentRoom.LuckPoker, RoomGuanDanProxy.Instance.CurrentRoom.isQuan, RoomGuanDanProxy.Instance.CurrentRoom.IsOver, RoomGuanDanProxy.Instance.CurrentRoom.roomId, huTaiCount, isFeng);
        //});
    }
    #endregion

    #region OpenResultView 打开结束界面
    /// <summary>
    /// 打开结束界面
    /// </summary>
    private void OpenResultView()
    {
        if (m_UISettleView != null) m_UISettleView.Close();
        //if (m_Result == null) return;
        //UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Result, (GameObject go) =>
        //{
        //    m_UIResultView = go.GetComponent<UIResultView>();
        //    m_UIResultView.SetUI(m_Result);
        //    m_Result = null;
        //});
    }
    #endregion

    #region OpenDisbandView 打开解散界面
    /// <summary>
    /// 打开解散界面
    /// </summary>
    public void OpenDisbandView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Disband, (GameObject go) =>
        {
            m_UIDisbandView = go.GetComponent<UIDisbandView>();
            Debug.Log(RoomGuanDanProxy.Instance.CurrentRoom.DisbandTime);
            Debug.Log(TimeUtil.GetTimestampMS());
            Debug.Log(GlobalInit.Instance.TimeDistance);
            m_UIDisbandView.SetUI(RoomGuanDanProxy.Instance.CurrentRoom.SeatList, RoomGuanDanProxy.Instance.PlayerSeat, (RoomGuanDanProxy.Instance.CurrentRoom.DisbandTime - TimeUtil.GetTimestampMS() + GlobalInit.Instance.TimeDistance) / 1000, RoomGuanDanProxy.Instance.CurrentRoom.DisbandTimeMax / 1000);
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
        ClientSendCreateRoom(groupId, settingIds);
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
    }
    #endregion

    #region RebuildRoom 重建房间
    /// <summary>
    /// 重建房间
    /// </summary>
    public void RebuildRoom()
    {
        ClientSendRebuild();
    }
    #endregion

    #region QuitRoom 退出房间
    /// <summary>
    /// 退出房间
    /// </summary>
    public void QuitRoom()
    {
        //if (RoomGuanDanProxy.Instance.CurrentRoom.matchId > 0)
        //{
        //    ShowMessage("提示", "是否退赛", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
        //    return;
        //}
        //if ((RoomGuanDanProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Ready && (RoomGuanDanProxy.Instance.CurrentRoom.currentLoop == 1 || RoomGuanDanProxy.Instance.CurrentRoom.currentLoop == 0)) || RoomGuanDanProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Replay)
        //{
        //    ClientSendLeaveRoom();
        //}
        //else
        //{
        //    DisbandRoom();
        //}
    }
    #endregion

    #region DisbandRoom 解散房间
    /// <summary>
    /// 解散房间
    /// </summary>
    public void DisbandRoom()
    {
        if (RoomGuanDanProxy.Instance.CurrentRoom.matchId > 0)
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
    public void OnReceiveMessage(ChatType type, int playerId, string message, string audioName, int toPlayerId)
    {
        SeatEntity seat = RoomGuanDanProxy.Instance.GetSeatByPlayerId(playerId);
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
            SeatEntity toSeat = RoomGuanDanProxy.Instance.GetSeatByPlayerId(toPlayerId);

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

    #region GetRoomEntity 获取房间数据实体
    /// <summary>
    /// 获取房间数据实体
    /// </summary>
    /// <returns></returns>
    public RoomEntityBase GetRoomEntity()
    {
        return RoomGuanDanProxy.Instance.CurrentRoom;
    }
    #endregion
    #endregion

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
        //OpenResultView();
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
        //OP_ROOM_CREATE_GET proto = new OP_ROOM_CREATE_GET();

        //for (int i = 0; i < settingIds.Count; ++i)
        //{
        //    proto.addSettingId(settingIds[i]);
        //}

        //List<cfg_settingEntity> lst = cfg_settingDBModel.Instance.GetOptionsByGameId(GameProxy.Instance.CurrentGameServer.gameId);
        //for (int i = 0; i < lst.Count; ++i)
        //{
        //    if (lst[i].status == 1 && lst[i].init == 1)
        //    {
        //        proto.addSettingId(lst[i].id);
        //    }
        //}
        //proto.clubId = groupId;
        //NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_CREATE_GET.CODE);
    }
    #endregion

    #region ClientSendJoinRoom 客户端发送加入房间
    /// <summary>
    /// 客户端发送加入房间
    /// </summary>
    private void ClientSendJoinRoom(int roomId)
    {
        //OP_ROOM_ENTER_GET proto = new OP_ROOM_ENTER_GET();
        //proto.roomId = roomId;
        //NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_ENTER.CODE);
    }
    #endregion

    #region ClientSendRebuild 客户端发送重建房间
    /// <summary>
    /// 客户端发送重建房间
    /// </summary>
    private void ClientSendRebuild()
    {
        //NetWorkSocket.Instance.Send(null, OP_ROOM_RECREATE.CODE);
        //ClientSendFocus(true);
    }
    #endregion

    #region ClientSendLeaveRoom 客户端发送离开房间
    /// <summary>
    /// 客户端发送离开房间
    /// </summary>
    private void ClientSendLeaveRoom()
    {
        //NetWorkSocket.Instance.Send(null, OP_ROOM_LEAVE.CODE);
    }
    #endregion

    #region ClientSendTrustee 客户端发送托管消息
    /// <summary>
    /// 客户端发送托管消息
    /// </summary>
    private void ClientSendTrustee(bool isTrustee)
    {
        //OP_ROOM_TRUSTEE_GET proto = new OP_ROOM_TRUSTEE_GET();
        //proto.isTrustee = isTrustee;
        //NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_TRUSTEE_GET.CODE);
    }
    #endregion

    #region ClientSendReady 客户端发送准备消息
    /// <summary>
    /// 客户端发送准备消息
    /// </summary>
    public void ClientSendReady()
    {
        //NetWorkSocket.Instance.Send(null, OP_ROOM_READY_GET.CODE);
    }
    #endregion

    #region ClientSendCancelReady 客户端发送取消准备消息
    /// <summary>
    /// 客户端发送取消准备消息
    /// </summary>
    private void ClientSendCancelReady()
    {
        //NetWorkSocket.Instance.Send(null, OP_ROOM_UNREADY_GET.CODE);
    }
    #endregion

    #region ClientSendPlayPoker 客户端发送出牌消息
    /// <summary>
    /// 客户端发送出牌消息
    /// </summary>
    /// <param name="poker">出的牌</param>
    public void ClientSendPlayPoker(Poker poker)
    {
//        if (poker == null) return;
//        if (RoomGuanDanProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Ready) return;
//        if (RoomGuanDanProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Settle) return;
//        if (RoomGuanDanProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Replay) return;
//#if !IS_LEPING && !IS_TAILAI && !IS_LUALU
//        if (RoomGuanDanProxy.Instance.CurrentOperator != RoomGuanDanProxy.Instance.PlayerSeat) return;
//#endif
//#if IS_LONGGANG
//        if (MahJongHelper.CheckUniversal(poker, RoomGuanDanProxy.Instance.PlayerSeat.UniversalList)) return;
//#endif

//#if IS_LEPING
//        ClientSendPass();
//#endif
//        if (IsTingByPlayPoker)
//        {
//            ClientSendPass();
//        }


//        OP_ROOM_OPERATE_GET proto = new OP_ROOM_OPERATE_GET();
//        proto.index = poker.index;
//        proto.isListen = IsTingByPlayPoker;
//        AppDebug.Log("客户端出牌 索引:" + poker.index + "   花色:" + poker.color + "  大小:" + poker.size);
//        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_OPERATE.CODE);
    }
    #endregion

    #region ClientSendPass 客户端发送过消息
    /// <summary>
    /// 客户端发送过消息
    /// </summary>
    public void ClientSendPass()
    {
        //NetWorkSocket.Instance.Send(null, OP_ROOM_FIGHT_PASS.CODE);
    }
    #endregion



 



   

    #region ClientSendRefuseDisbandRoom 客户端发送拒绝解散房间
    /// <summary>
    /// 客户端发送拒绝解散房间
    /// </summary>
    private void ClientSendRefuseDisbandRoom()
    {
        //OP_ROOM_DISMISS_GET proto = new OP_ROOM_DISMISS_GET();
        //proto.isDismiss = false;
        //NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_DISMISS_GET.CODE);
    }
    #endregion

    #region ClientSendApplyDisbandRoom 客户端发送申请解散房间
    /// <summary>
    /// 客户端发送申请解散房间
    /// </summary>
    private void ClientSendApplyDisbandRoom()
    {
        //OP_ROOM_NEW_DISMISS_GET proto = new OP_ROOM_NEW_DISMISS_GET();
        //proto.isDismiss = true;
        //NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_NEW_DISMISS_GET.CODE);
    }
    #endregion

    #region ClientSendDisbandRoom 客户端发送解散房间
    /// <summary>
    /// 客户端发送解散房间
    /// </summary>
    /// <param name="isAgree"></param>
    private void ClientSendDisbandRoom(bool isAgree)
    {
        //OP_ROOM_NEW_DISMISS_GET proto = new OP_ROOM_NEW_DISMISS_GET();
        //proto.isDismiss = isAgree;
        //NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_NEW_DISMISS_GET.CODE);
    }
    #endregion

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

    #region ClientSendChangeSeat 客户端发送换座位消息
    /// <summary>
    /// 客户端发送换座位消息
    /// </summary>
    /// <param name="seatPos"></param>
    private void ClientSendChangeSeat(int seatPos)
    {
        //OP_ROOM_SITDOWN_GET proto = new OP_ROOM_SITDOWN_GET();
        //proto.pos = seatPos;
        //NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_SITDOWN_GET.CODE);
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


        //CommandQueue.Clear();
        GD_CREATE_ROOM proto = GD_CREATE_ROOM.decode(obj);
        UIViewManager.Instance.CloseWait();
        RoomGuanDanProxy.Instance.InitRoom(proto.roominfo);
        SceneMgr.Instance.LoadScene(SceneType.GuanDan2D);
        //MaJiangGameCtrl
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


        //UIViewManager.Instance.CloseWait();
        //GD_REENTER proto = NN_ROOM_CREATE.decode(obj);
        //GameStateManager.Instance.InitCurrentRoom(proto);
        //Debug.Log("准备断线重连创建房间，自己ID：" + AccountProxy.Instance.CurrentAccountEntity.passportId);

        //SceneMgr.Instance.LoadScene(SceneType.NiuNiu2D);


    }
    #endregion


    #region OnServerBroadcastReady 服务器广播玩家准备消息
    /// <summary>
    /// 服务器广播玩家准备消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastReady(byte[] obj)
    {
        //OP_ROOM_READY proto = OP_ROOM_READY.decode(obj);
        //RoomGuanDanProxy.Instance.Ready(proto.playerId);
    }
    #endregion

    #region OnServerBroadcastCancelReady 服务器广播取消准备
    /// <summary>
    /// 服务器广播取消准备
    /// </summary>
    /// <param name="data"></param>
    private void OnServerBroadcastCancelReady(byte[] data)
    {
        //OP_ROOM_UNREADY proto = OP_ROOM_UNREADY.decode(data);
        //RoomGuanDanProxy.Instance.CancelReady(proto.playerId);
    }
    #endregion

    #region OnServerBroadcastEnter 服务器广播玩家进入消息
    /// <summary>
    /// 服务器广播玩家进入消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnter(byte[] obj)
    {

        //OP_ROOM_ENTER proto = OP_ROOM_ENTER.decode(obj);
        //RoomGuanDanProxy.Instance.EnterRoom(proto);
    }
    #endregion

    #region OnServerBroadcastLeave 服务器广播玩家离开消息
    /// <summary>
    /// 服务器广播玩家离开消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLeave(byte[] obj)
    {
        //OP_ROOM_LEAVE proto = OP_ROOM_LEAVE.decode(obj);
        //RoomGuanDanProxy.Instance.ExitRoom(proto);
    }
    #endregion










    #endregion
}
