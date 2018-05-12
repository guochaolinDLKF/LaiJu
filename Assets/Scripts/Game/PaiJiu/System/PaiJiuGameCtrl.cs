//===================================================
//Author      : WZQ
//CreateTime  ：7/4/2017 11:25:51 AM
//Description ：牌九游戏控制器
//===================================================

using System;
using System.Collections.Generic;
using PaiJiu;
using proto.paigow;
using UnityEngine;


public class PaiJiuGameCtrl : SystemCtrlBase<PaiJiuGameCtrl>, IGameCtrl, ISystemCtrl
{
    #region Variable
    //private UISettleView m_UISettleView;//设置窗口
    private UIUnitSettlement_PaiJiu m_UISettleView;//小结算窗口
    private UIRankerListView_PaiJiu m_UIResultView;//游戏结果窗口（总）


    /// <summary>
    /// 游戏结果
    /// </summary>
    //private OP_ROOM_RESULT m_Result;
    private PAIGOW_ROOM m_Result;// 结算信息

    #endregion

    #region DicNotificationInterests 注册UI事件
    /// <summary>
    /// 注册UI事件
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();

        dic.Add(ConstDefine_PaiJiu.ObKey_btnPour, OnBtnPaiJiuViewPour);//确认下注按钮
        dic.Add(ConstDefine_PaiJiu.ObKey_btnResultViewBack, OnBtnResultViewBack);//结束界面返回按钮
        dic.Add(ConstDefine_PaiJiu.ObKey_btnResultViewShare, OnBtnResultViewShareClick);//结束界面分享按钮  ObKey_btnResultViewShare     
        //dic.Add(ConstDefine.BtnSettleViewReplayOver, OnBtnResultViewReplayOverClick);
        dic.Add(ConstDefine_PaiJiu.BtnPaiJiuViewShare, OnBtnMaJiangViewShareClick);//分享按钮点击  BtnPaiJiuViewShare
        dic.Add(ConstDefine_PaiJiu.BtnPaiJiuViewReady, OnBtnPaiJiuViewReadyClick);//准备按钮点击btnMaJiangViewReady
        dic.Add(ConstDefine_PaiJiu.ObKey_SetPokerStatus, OnPokerClick);//手牌点击
        dic.Add(ConstDefine_PaiJiu.ObKey_btnChooseBanker, OnBtnChooseBanker);//是否坐庄
        dic.Add(ConstDefine_PaiJiu.ObKey_btnRobBanker, OnBtnRobBanker);//是否抢庄
        dic.Add(ConstDefine_PaiJiu.ObKey_OpenViewPaiJiu, OpenViewPaiJiu);//加载窗口
        dic.Add(ConstDefine_PaiJiu.ObKey_SetEnterRoomView, SetEnterRoomView);//刚进入游戏时 加载已有窗口

        dic.Add(ConstDefine_PaiJiu.ObKey_btnCutPoker, OnBtnClientSendCutPokerClick);//确认切牌按钮点击
        dic.Add(ConstDefine_PaiJiu.ObKey_btnStartGame, OnBtnPaiJiuViewStartGameClick);//开始按钮点击
        dic.Add(ConstDefine_PaiJiu.ObKey_btnQieGuo, OnBtnPaiJiuViewQieGuoClick);//切锅按钮点击
        dic.Add(ConstDefine_PaiJiu.ObKey_SettleOnComplete, SettleOnComplete);//完成结算回调
        return dic;
    }
    #endregion

    #region Constructors
    public PaiJiuGameCtrl()
    {

        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_CREATE.CODE, OnServerReturnCreateRoom);//服务器返回创建房间消息
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_ENTER.CODE, OnServerBroadcastEnter);//服务器广播玩家进入消息
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播玩家离开消息
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_READY.CODE, OnServerBroadcastReady);//服务器广播玩家准备消息
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_DISMISS.CODE, OnServerBroadcastApplyDisband);//服务器广播解散房间
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_RECREATE.CODE, OnServerReturnRebuild); //服务器返回重建房间

        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_GAMESTART.CODE, OnServerBroadcastGameStart);//服务器通知开始游戏
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_INFORM_JETTON.CODE, OnServerBroadcastInfoRMJetton);//服务器通知下注（某玩家开始下注）
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_JETTON.CODE, OnServerBroadcastJetton);//服务器广播下注（某玩家下注某分）     
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_BEGIN.CODE, OnServerBroadcastBegin);//服务器广播开局消息
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_DRAW.CODE, OnServerOpenPoker);//服务器广播翻牌
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_OPENPOKERRESULT.CODE, OnServerBroadcastSettle);//服务器广播开牌结算
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_NEXTGAME.CODE, OnServerNextGame);//服务器广播准备下一局
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_CHOOSEBANKER.CODE, OnServerChooseBanker);//选庄
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_CHANGEBANKER.CODE, OnServerChangeBanker);//换庄
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_GAMEOVER.CODE, OnServerGameOver);//游戏结束
        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_TOTALSETTLE.CODE, OnServerResultInfo);//服务器广播总结算信息 

        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_CUTPOKER.CODE, OnServerCutPoker);//服务器广播切牌

        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_GRABBANKER.CODE, OnServerRobBanker);//服务器广播抢庄

        NetDispatcher.Instance.AddEventListener(PAIGOW_ROOM_CUTPAN.CODE, OnServerCutGuo);//服务器广播切锅 

        //NetWorkSocket.Instance.OnDisconnect += OnDisConnectCallBack;//断开连接事件
        //DelegateDefine.Instance.OnHeadClick += OnHeadClick;
        //DelegateDefine.Instance.OnPlayPoker += OnPlayPoker;//出牌事件
    }
    #endregion

    #region Dispose
    public override void Dispose()
    {
        base.Dispose();

        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_CREATE.CODE, OnServerReturnCreateRoom);//服务器返回创建麻将房间消息
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_ENTER.CODE, OnServerBroadcastEnter);//服务器广播玩家进入消息
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播玩家离开消息
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_READY.CODE, OnServerBroadcastReady);//服务器广播玩家准备消息
        //NetDispatcher.Instance.RemoveEventListener(OP_ROOM_TRUSTEE.CODE, OnServerBroadcastTrustee);//服务器广播玩家托管消息
        //NetDispatcher.Instance.RemoveEventListener(OP_MATCH_WAIVER.CODE, OnServerBroadcastWaiver);//服务器广播玩家弃权消息
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_DISMISS.CODE, OnServerBroadcastApplyDisband);//服务器广播解散房间
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_RECREATE.CODE, OnServerReturnRebuild); //服务器返回重建房间

        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_GAMESTART.CODE, OnServerBroadcastGameStart);//服务器通知开始游戏
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_INFORM_JETTON.CODE, OnServerBroadcastInfoRMJetton);//服务器通知下注（某玩家开始下注）
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_JETTON.CODE, OnServerBroadcastJetton);//服务器广播下注（某玩家下注某分）
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_BEGIN.CODE, OnServerBroadcastBegin);//服务器广播开局消息
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_DRAW.CODE, OnServerOpenPoker);//服务器广播翻牌
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_OPENPOKERRESULT.CODE, OnServerBroadcastSettle);//服务器广播开牌结算
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_NEXTGAME.CODE, OnServerNextGame);//服务器广播准备下一局
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_CHOOSEBANKER.CODE, OnServerChooseBanker);//选庄
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_CHANGEBANKER.CODE, OnServerChangeBanker);//换庄
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_GAMEOVER.CODE, OnServerGameOver);//游戏结束

        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_TOTALSETTLE.CODE, OnServerResultInfo);//服务器广播总结算信息 
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_CUTPOKER.CODE, OnServerCutPoker);//切牌
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_GRABBANKER.CODE, OnServerRobBanker);//服务器广播抢庄 
        NetDispatcher.Instance.RemoveEventListener(PAIGOW_ROOM_CUTPAN.CODE, OnServerCutGuo);//服务器广播切锅
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
            case UIWindowType.UnitSettlement_PaiJiu:
                OpenUnitSettlement_PaiJiu();
                break;
            case UIWindowType.RankList_PaiJiu://RankList_PaiJiu
                OpenResultView();
                break;
        }
    }
    #endregion

    #region OpenSettleView 打开结算界面
    /// <summary>
    /// 打开结算界面(小结)
    /// </summary>
    private void OpenUnitSettlement_PaiJiu()
    {
        if (m_UIResultView != null) return;
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.UnitSettlement_PaiJiu, (GameObject go) =>
        {
            m_UISettleView = go.GetComponent<UIUnitSettlement_PaiJiu>();
            m_UISettleView.SetUI(RoomPaiJiuProxy.Instance.CurrentRoom.SeatList,
                () =>
                {
                    m_UISettleView = null;
                    //是否由小结算带出总结算
                    //if(m_Result!=null) OpenResultView();
                }
                );
            //m_UISettleView.Settle(RoomMaJiangProxy.Instance.CurrentRoom.SeatList, RoomMaJiangProxy.Instance.CurrentRoom.CurrentLoop, RoomMaJiangProxy.Instance.CurrentRoom.MaxLoop, RoomMaJiangProxy.Instance.CurrentRoom.Prob, RoomMaJiangProxy.Instance.CurrentRoom.matchId, RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker);
        });
    }
    #endregion

    #region OpenResultView 打开结束界面
    /// <summary>
    /// 打开结束界面（总结）
    /// </summary>
    private void OpenResultView()
    {
        if (m_UISettleView != null) m_UISettleView.Close();
        if (m_Result == null) return;
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.RankList_PaiJiu, (GameObject go) =>
        {
            m_UIResultView = go.GetComponent<UIRankerListView_PaiJiu>();
            m_UIResultView.SetUI(RoomPaiJiuProxy.Instance.CurrentRoom);
            m_Result = null;
        });
    }
    #endregion
    #endregion

    #region Override IGameCtrl



    /// <summary>
    ///  解散房间
    /// </summary>
    public void DisbandRoom()
    {

        if (RoomPaiJiuProxy.Instance.CurrentRoom.matchId > 0)
        {
            UIViewManager.Instance.ShowMessage("提示", "比赛场不能解散房间");
        }
        else
        {
            if (RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.LOOP || RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.SETTLE)
            {
                ShowMessage("提示", "游戏过程中无法解散房间", MessageViewType.Ok, null, null, 1, AutoClickType.Ok);
                return;
            }
            else
            {
                UIViewManager.Instance.ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom);
            }

        }

    }



    #region CreateRoom 创建房间
    /// <summary>
    /// 创建房间
    /// </summary>
    public void CreateRoom(int groupId, List<int> settingIds)
    {

        ClientSendCreateRoom(groupId,settingIds);


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
        Debug.Log("------------------------------------------QuitRoom------------------------------------------");
        if (RoomPaiJiuProxy.Instance.CurrentRoom.matchId > 0)
        {
            ShowMessage("提示", "是否退赛", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
            return;
        }

        if (RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.LOOP || RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.SETTLE)
        {
            ShowMessage("提示", "游戏过程中无法退出房间", MessageViewType.Ok, null, null, 1, AutoClickType.Ok);
            return;
        }



        if ((RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.IDLE || (/*RoomPaiJiuProxy.Instance.CurrentRoom.currentLoop == 1 ||*/ RoomPaiJiuProxy.Instance.CurrentRoom.currentLoop == 0)))//|| RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Replay)
        {
            //RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Replay //重播
            ShowMessage("提示", "是否退出房间", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
        }
        else
        {

            ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom);
        }
    }
    #endregion



    /// <summary>
    /// 接收聊天消息
    /// </summary>
    public void OnReceiveMessage(ChatType type, int playerId, string message, string audioName, int toPlayerId)
    {
        Seat seat = RoomPaiJiuProxy.Instance.GetSeatByPlayerId(playerId);
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
            Seat toSeat = RoomPaiJiuProxy.Instance.GetSeatByPlayerId(toPlayerId);

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

    /// <summary>
    /// 获取房间数据实体
    /// </summary>
    /// <returns></returns>
    public RoomEntityBase GetRoomEntity()
    {
        return RoomPaiJiuProxy.Instance.CurrentRoom;
    }

    #endregion



    #region SeeResult 查看牌局总结算信息
    /// <summary>
    /// 查看牌局总结算信息
    /// </summary>
    private void SeeResult()
    {
        if (m_Result == null || RoomPaiJiuProxy.Instance.CurrentRoom.currentLoop <= 0)
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


    #region 按钮点击
    #region OnPokerClick 手牌点击
    /// <summary>
    /// 手牌点击
    /// </summary>
    /// <param name="param"></param>
    private void OnPokerClick(object[] param)
    {
        List<UIItemHandPoker_PaiJiu> handPokerList = (List<UIItemHandPoker_PaiJiu>)param[0];
        PAIGOW_ROOM_DRAW_GET proto = new PAIGOW_ROOM_DRAW_GET();
        for (int i = 0; i < handPokerList.Count; i++)
        {
            proto.addIndex(handPokerList[i].Majiang.Poker.index);
        }

        if (proto.getIndexList().Count <= 0)
        {
            AppDebug.Log("没有要开的牌");
            return;
        }
        NetWorkSocket.Instance.Send(proto.encode(), PAIGOW_ROOM_DRAW_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion


    #region 是否坐庄按钮点击
    private void OnBtnChooseBanker(object[] obj)
    {
        bool isChooseBanker = false;
        if (obj.Length > 0 && obj[0] != null)
        {
            isChooseBanker = (bool)obj[0];
        }
        PAIGOW_ROOM_CHOOSEBANKER_GET proto = new PAIGOW_ROOM_CHOOSEBANKER_GET();
        proto.isChooseBanker = isChooseBanker;
        NetWorkSocket.Instance.Send(proto.encode(), PAIGOW_ROOM_CHOOSEBANKER_GET.CODE, GameCtrl.Instance.SocketHandle);

    }
    #endregion

    #region 是否抢庄按钮点击
    private void OnBtnRobBanker(object[] obj)
    {
        int isRobBanker = 2;
        if (obj.Length > 0 && obj[0] != null)
        {
            isRobBanker = (int)obj[0];
        }
        PAIGOW_ROOM_GRABBANKER_GET proto = new PAIGOW_ROOM_GRABBANKER_GET();
        proto.isGrabBanker = isRobBanker;
        NetWorkSocket.Instance.Send(proto.encode(), PAIGOW_ROOM_GRABBANKER_GET.CODE, GameCtrl.Instance.SocketHandle);

    }
    #endregion




    //    #region OnHeadClick 头像点击
    //    /// <summary>
    //    /// 头像点击
    //    /// </summary>
    //    /// <param name="seatPos"></param>
    //    private void OnHeadClick(int seatPos)
    //    {
    //        SeatEntity entity = RoomMaJiangProxy.Instance.GetSeatBySeatId(seatPos);
    //        if (entity == RoomMaJiangProxy.Instance.PlayerSeat && AccountProxy.Instance.CurrentAccountEntity.identity > 0)
    //        {
    //            UIViewManager.Instance.OpenWindow(UIWindowType.PlayerInfo);
    //            return;
    //        }
    //        UIViewUtil.Instance.LoadWindow(UIWindowType.SeatInfo, (GameObject go) =>
    //        {
    //            go.GetComponent<UISeatInfoView>().SetUI(entity.PlayerId, entity.Nickname, entity.Gender, entity.IP);
    //        });
    //    }
    //    #endregion



    #region OnBtnMaJiangViewReadyClick 场景UI准备按钮点击
    /// <summary>
    /// 场景UI准备按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPaiJiuViewReadyClick(object[] obj)
    {
        ClientSendReady();
    }
    #endregion

    #region OnBtnPaiJiuViewStartGameClick 场景UI开始按钮点击
    /// <summary>
    /// 场景UI开始按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPaiJiuViewStartGameClick(object[] obj)
    {
        ClientSendStartGame();
    }
    #endregion

    #region OnBtnPaiJiuViewQieGuoClick 场景UI切锅按钮点击
    /// <summary>
    /// 场景UI切锅按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPaiJiuViewQieGuoClick(object[] obj)
    {
        bool isCutGuo = (bool)obj[0];
        ClientSendQieGuo(isCutGuo);
    }
    #endregion


    #region OnBtnPaiJiuViewPour 下注按钮点击
    /// <summary>
    /// 下注按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPaiJiuViewPour(object[] obj)
    {
        int pour = (int)obj[0];
#if IS_ZHANGJIAKOU
        bool quanXia = (bool)obj[1];
#endif
        if (!RoomPaiJiuProxy.Instance.PlayerSeat.IsBanker && pour > RoomPaiJiuProxy.Instance.BankerSeat.Pour)
        {
            UIViewManager.Instance.ShowMessage("提示", "下注分数不能大于庄家分数", MessageViewType.Ok);
            return;
        }

#if IS_PAIJIU
                if (RoomPaiJiuProxy.Instance.PlayerSeat.IsBanker && pour < 10)
                {
                    UIViewManager.Instance.ShowMessage("提示", "下注分数不能小于10分", MessageViewType.Ok);
                    return;
                }
#endif

        ClientSendPour(pour);
    }
    #endregion


    #region OnBtnPaiJiuViewOpenPoker 场景按钮点击开牌
    /// <summary>
    /// 按钮点击开牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnPaiJiuViewOpenPoker(object[] obj)
    {
        List<int> indexList = (List<int>)obj[0];
        ClientSendOpenPoker(indexList);
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

    #region 切牌相关
    /// <summary>
    ///  是否切牌 
    /// </summary>
    /// <param name="obj"></param>
    public void OnBtnClientSendCutPokerClick(object[] obj)
    {

        ClientSendCutPoker(((int)obj[0]));
    }
    /// <summary>
    ///  切牌信息 切的第几墩
    /// </summary>
    /// <param name="dun"></param>
    public void OnBtnClientSendCutPokerInfo(int dun)
    {

        ClientSendCutPoker(0, dun);
    }
    #endregion


    /// <summary>
    /// 完成结算回调
    /// </summary>
    /// <param name="obj"></param>
    public void SettleOnComplete(object[] obj)
    {
        //清空桌面
        RoomPaiJiuProxy.Instance.OnServerNextGame();
        if (PaiJiuSceneCtrl.Instance != null)
        {
            PaiJiuSceneCtrl.Instance.NextGame();
        }

        ClientSendNextGame();
    }


    #endregion

    #region 客户端发送消息
    #region ClientSendCreateRoom 客户端发送创建房间消息
    /// <summary>
    /// 客户端发送创建房间消息
    /// </summary>
    private void ClientSendCreateRoom(int groupId,List<int> settingIds)
    {
        PAIGOW_ROOM_CREATE_GET proto = new PAIGOW_ROOM_CREATE_GET();
        

        //《--------------------配置ID----------
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
        proto.clubId = groupId;
        NetWorkSocket.Instance.Send(proto.encode(), PAIGOW_ROOM_CREATE.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendJoinRoom 客户端发送加入房间
    /// <summary>
    /// 客户端发送加入房间
    /// </summary>
    private void ClientSendJoinRoom(int roomId)
    {
        PAIGOW_ROOM_ENTER_GET proto = new PAIGOW_ROOM_ENTER_GET();
        //OP_ROOM_ENTER_GET proto = new OP_ROOM_ENTER_GET();
        proto.roomId = roomId;
        NetWorkSocket.Instance.Send(proto.encode(), PAIGOW_ROOM_ENTER_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRebuild 客户端发送重建房间
    /// <summary>
    /// 客户端发送重建房间
    /// </summary>
    private void ClientSendRebuild()
    {

        NetWorkSocket.Instance.Send(null, PAIGOW_ROOM_RECREATE_GET.CODE, GameCtrl.Instance.SocketHandle);                              //------------<<重建房间消息未定义----------------
    }
    #endregion

    #region ClientSendLeaveRoom 客户端发送离开房间
    /// <summary>
    /// 客户端发送离开房间
    /// </summary>
    private void ClientSendLeaveRoom()
    {

        NetWorkSocket.Instance.Send(null, PAIGOW_ROOM_LEAVE_GET.CODE, GameCtrl.Instance.SocketHandle);
        //NetWorkSocket.Instance.Send(null, OP_ROOM_LEAVE.CODE);
    }
    #endregion

    #region ClientSendReady 客户端发送准备消息
    /// <summary>
    /// 客户端发送准备消息
    /// </summary>
    public void ClientSendReady()
    {
        AppDebug.Log("牌九发送准备 玩家位置：" + RoomPaiJiuProxy.Instance.PlayerSeat.Pos);
        NetWorkSocket.Instance.Send(null, PAIGOW_ROOM_READY_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendStartGame 客户端发送开始游戏
    /// <summary>
    /// 客户端发送开始游戏
    /// </summary>
    public void ClientSendStartGame()
    {
        //开始游戏 暂 只有张家口使用
        NetWorkSocket.Instance.Send(null, PAIGOW_ROOM_GAMESTART.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion



    #region ClientSendPour 客户端发送下注分数消息
    /// <summary>
    /// 客户端发送下注分数消息
    /// </summary>
    public void ClientSendPour(int pour)
    {
        PAIGOW_ROOM_JETTON_GET proto = new PAIGOW_ROOM_JETTON_GET();
        proto.pour = pour;
        NetWorkSocket.Instance.Send(proto.encode(), PAIGOW_ROOM_JETTON_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion


    #region ClientSendCutPokerComplete 客户端发送完成切牌
    /// <summary>
    /// 客户端发送完成切牌
    /// </summary>
    public void ClientSendCutPokerComplete()
    {
        PAIGOW_ROOM_CUTPOKER_GET proto = new PAIGOW_ROOM_CUTPOKER_GET();
        NetWorkSocket.Instance.Send(proto.encode(), PAIGOW_ROOM_CUTPOKER_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion




    #region ClientSendInitWait 客户端发送完成开局
    /// <summary>
    /// 客户端发送完成开局
    /// </summary>
    public void ClientSendBeginComplete()
    {

        NetWorkSocket.Instance.Send(null, PAIGOW_ROOM_BEGIN.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRobComplete 客户端发送完成抢庄
    /// <summary>
    /// 客户端发送完成抢庄
    /// </summary>
    public void ClientSendRobComplete()
    {
        NetWorkSocket.Instance.Send(null, PAIGOW_ROOM_GRABBANKER.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion


    #region ClientSendGetSettle 客户端发送get结算
    /// <summary>
    /// 客户端发送get结算
    /// </summary>
    public void ClientSendGetSettle()
    {
        if (RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus == ROOM_STATUS.LOOP)
        {
            NetWorkSocket.Instance.Send(null, PAIGOW_ROOM_OPENPOKERRESULT.CODE, GameCtrl.Instance.SocketHandle);
        }
    }
    #endregion

    #region ClientSendQieGuo 客户端发送切锅
    /// <summary>
    /// 客户端发送切锅
    /// </summary>
    public void ClientSendQieGuo(bool isCutGuo)
    {
        PAIGOW_ROOM_CUTPAN proto = new PAIGOW_ROOM_CUTPAN();
        proto.isCutGuo = isCutGuo;
        NetWorkSocket.Instance.Send(proto.encode(), PAIGOW_ROOM_CUTPAN.CODE, GameCtrl.Instance.SocketHandle);

    }
    #endregion







    #region ClientSendReady 客户端发送开牌消息
    /// <summary>
    /// 客户端发送开牌消息
    /// </summary>
    public void ClientSendOpenPoker(List<int> indexList)                                      //《----------------发送开牌消息------------------
    {

        //列表信息

        NetWorkSocket.Instance.Send(null, PAIGOW_ROOM_READY.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion


    #region ClientSend 客户端发送完成每次结算
    /// <summary>
    /// 客户端发送完成每次结算
    /// </summary>
    private void ClientSendNextGame()
    {
        //PAIGOW_ROOM_NEXTGAME_GET proto = new PAIGOW_ROOM_NEXTGAME_GET();
        NetWorkSocket.Instance.Send(null, PAIGOW_ROOM_NEXTGAME_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion




    #region ClientSendApplyDisbandRoom 客户端发送请求解散房间
    /// <summary>
    /// 客户端发送请求解散房间
    /// </summary>
    private void ClientSendApplyDisbandRoom()
    {
        //OP_ROOM_DISMISS_GET proto = new OP_ROOM_DISMISS_GET();
        //proto.isDismiss = true;
        //NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_DISMISS.CODE);

        PAIGOW_ROOM_DISMISS_GET proto = new PAIGOW_ROOM_DISMISS_GET();
        proto.isDismiss = true;
        NetWorkSocket.Instance.Send(proto.encode(), PAIGOW_ROOM_DISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendAgreeDisbandRoom 客户端发送同意解散房间
    /// <summary>
    /// 客户端发送同意解散房间
    /// </summary>
    private void ClientSendAgreeDisbandRoom()
    {
        PAIGOW_ROOM_DISMISS_GET proto = new PAIGOW_ROOM_DISMISS_GET();
        proto.isDismiss = true;
        NetWorkSocket.Instance.Send(proto.encode(), PAIGOW_ROOM_DISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRefuseDisbandRoom 客户端发送拒绝解散房间
    /// <summary>
    /// 客户端发送拒绝解散房间
    /// </summary>
    private void ClientSendRefuseDisbandRoom()
    {
        PAIGOW_ROOM_DISMISS_GET proto = new PAIGOW_ROOM_DISMISS_GET();
        proto.isDismiss = false;
        NetWorkSocket.Instance.Send(proto.encode(), PAIGOW_ROOM_DISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendCutPoker 客户端发送切牌
    /// <summary>
    /// 客户端发送切牌
    /// </summary>
    private void ClientSendCutPoker(int isCut = 0, int dun = -1)
    {

        PAIGOW_ROOM_CUTPOKER_GET proto = new PAIGOW_ROOM_CUTPOKER_GET();

        if (isCut > 0) proto.isCutPoker = isCut;
        if (dun >= 0) proto.cutPokerIndex = dun;

        NetWorkSocket.Instance.Send(proto.encode(), PAIGOW_ROOM_CUTPOKER_GET.CODE, GameCtrl.Instance.SocketHandle);
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
        PAIGOW_ROOM_CREATE proto = PAIGOW_ROOM_CREATE.decode(obj);
        //OP_ROOM_INFO proto = OP_ROOM_INFO.decode(obj);
        UIViewManager.Instance.CloseWait();
        RoomPaiJiuProxy.Instance.InitRoom(proto.paigow_room);

        SceneMgr.Instance.LoadScene(SceneType.PaiJiu3D);
    }
    #endregion

    #region OnServerBroadcastReady 服务器广播玩家准备消息
    /// <summary>
    /// 服务器广播玩家准备消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastReady(byte[] obj)
    {
        PAIGOW_ROOM_READY proto = PAIGOW_ROOM_READY.decode(obj);
        RoomPaiJiuProxy.Instance.Ready(proto);
    }
    #endregion

    #region OnServerBroadcastEnter 服务器广播玩家进入消息
    /// <summary>
    /// 服务器广播玩家进入消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnter(byte[] obj)
    {
        PAIGOW_ROOM_ENTER proto = PAIGOW_ROOM_ENTER.decode(obj);
        RoomPaiJiuProxy.Instance.EnterRoom(proto);

    }
    #endregion

    #region OnServerBroadcastLeave 服务器广播玩家离开消息
    /// <summary>
    /// 服务器广播玩家离开消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLeave(byte[] obj)
    {
        PAIGOW_ROOM_LEAVE proto = PAIGOW_ROOM_LEAVE.decode(obj);
        RoomPaiJiuProxy.Instance.ExitRoom(proto);
    }
    #endregion

    #region OnServer 服务器广播开始游戏
    /// <summary>
    /// 服务器广播玩家离开消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastGameStart(byte[] obj)
    {

        PAIGOW_ROOM_GAMESTART proto = PAIGOW_ROOM_GAMESTART.decode(obj);
        RoomPaiJiuProxy.Instance.GameStart(proto);
    }
    #endregion


    #region OnServerBroadcastLeave 服务器广播 通知玩家开始下注
    /// <summary>
    /// 服务器广播玩家离开消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastInfoRMJetton(byte[] obj)
    {

        PAIGOW_ROOM_INFORM_JETTON proto = PAIGOW_ROOM_INFORM_JETTON.decode(obj);
        RoomPaiJiuProxy.Instance.NoticeJetton(proto);
    }
    #endregion

    #region OnServerBroadcastLeave 服务器广播玩家下注某分
    /// <summary>
    /// 服务器广播玩家离开消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastJetton(byte[] obj)
    {
        PAIGOW_ROOM_JETTON proto = PAIGOW_ROOM_JETTON.decode(obj);
        RoomPaiJiuProxy.Instance.Jetton(proto);
    }
    #endregion


    #region OnServerBroadcastBegin 服务器广播开局发牌
    /// <summary>
    /// 服务器广播开局发牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastBegin(byte[] obj)
    {
        PAIGOW_ROOM_BEGIN proto = PAIGOW_ROOM_BEGIN.decode(obj);


        if (proto.hasPaigowSeat())
        {
            //真发牌
            RoomPaiJiuProxy.Instance.Begin(proto);

            if (PaiJiuSceneCtrl.Instance != null)
            {
                PaiJiuSceneCtrl.Instance.Begin(RoomPaiJiuProxy.Instance.CurrentRoom, true);
            }
        }
        else
        {
            RoomPaiJiuProxy.Instance.Begin();
            //只开局
            if (PaiJiuSceneCtrl.Instance != null)
            {
                PaiJiuSceneCtrl.Instance.Begin(true);
            }
        }



    }
    #endregion










    //切牌
    #region OnServerCutPoker 服务器广播切牌
    /// <summary>
    /// 服务器广播切牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerCutPoker(byte[] obj)
    {
        PAIGOW_ROOM_CUTPOKER proto = PAIGOW_ROOM_CUTPOKER.decode(obj);
        RoomPaiJiuProxy.Instance.OnServerCutPoker(proto);

        if (proto.hasPos() && proto.hasCutPokerIndex())
        {

            Debug.Log("============================================开始切牌=======================================");
            if (PaiJiuSceneCtrl.Instance != null)
            {
                PaiJiuSceneCtrl.Instance.CutPoker(proto.cutPokerIndex);

            }
        }

    }
    #endregion

    #region OnServerCutPoker 服务器广播抢庄
    /// <summary>
    /// 服务器广播切牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerRobBanker(byte[] obj)
    {
        Debug.Log("服务器广播抢庄:" + obj.Length);
        PAIGOW_ROOM_GRABBANKER proto = PAIGOW_ROOM_GRABBANKER.decode(obj);
        RoomPaiJiuProxy.Instance.OnServerRobBanker(proto);

        //抢庄结束
        if (proto.hasPos() && !proto.hasIsGrabBanker()) PaiJiuSceneCtrl.Instance.RobEnd(RoomPaiJiuProxy.Instance.GetSeatBySeatId(proto.pos));

    }
    #endregion

    #region OnServerCutGuo 服务器广播切锅
    /// <summary>
    /// 服务器广播切锅
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerCutGuo(byte[] obj)
    {
        Debug.Log("服务器广播切锅:");
        PAIGOW_ROOM_CUTPAN proto = PAIGOW_ROOM_CUTPAN.decode(obj);
        RoomPaiJiuProxy.Instance.OnServerCutGuo(proto);


    }
    #endregion








    #region OnServerOpenPoker 服务器广播翻牌
    /// <summary>
    /// 服务器广播翻牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerOpenPoker(byte[] obj)
    {
        PAIGOW_ROOM_DRAW proto = PAIGOW_ROOM_DRAW.decode(obj);

        RoomPaiJiuProxy.Instance.OnServerOpenPoker(proto);

        if (PaiJiuSceneCtrl.Instance != null)
        {

            PaiJiuSceneCtrl.Instance.SetHandPokerStatus(proto.pos, RoomPaiJiuProxy.Instance.GetSeatBySeatId(proto.pos));
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
        AppDebug.Log("服务器广播结算");
        PAIGOW_ROOM_OPENPOKERRESULT proto = PAIGOW_ROOM_OPENPOKERRESULT.decode(obj);

        RoomPaiJiuProxy.Instance.OnServerResult(proto);

        //是否是最后一局
        //if (RoomPaiJiuProxy.Instance.CurrentRoom.currentLoop >= RoomPaiJiuProxy.Instance.CurrentRoom.maxLoop)
        //{
        //    m_Result = proto.paigow_room;
        //}
        if (PaiJiuSceneCtrl.Instance != null)
        {
            PaiJiuSceneCtrl.Instance.Settle(proto.paigow_room.loopEnd);
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
        RoomPaiJiuProxy.Instance.OnServerNextGame();
        if (PaiJiuSceneCtrl.Instance != null)
        {
            PaiJiuSceneCtrl.Instance.NextGame();
        }


    }
    #endregion

    #region OnServerBroadcastSettle 服务器广播选庄
    /// <summary>
    /// 服务器广播选庄
    /// </summary>
    private void OnServerChooseBanker(byte[] obj)
    {
        AppDebug.Log("服务器广播选庄");
        PAIGOW_ROOM_CHOOSEBANKER proto = PAIGOW_ROOM_CHOOSEBANKER.decode(obj);
        RoomPaiJiuProxy.Instance.OnServerChooseBanker(proto);
        if (PaiJiuSceneCtrl.Instance != null)
        {
            PaiJiuSceneCtrl.Instance.ChooseBanker();
        }



    }
    #endregion

    #region OnServer 服务器广播换庄
    /// <summary>
    /// 服务器广播换庄
    /// </summary>
    private void OnServerChangeBanker(byte[] obj)
    {
        AppDebug.Log("服务器广播换庄");
        PAIGOW_ROOM_CHANGEBANKER proto = PAIGOW_ROOM_CHANGEBANKER.decode(obj);
        RoomPaiJiuProxy.Instance.OnServerChangeBanker(proto);

    }
    #endregion



    //PAIGOW_ROOM_CHANGEBANKER




    #region OnServerGameOver 服务器广播游戏结束
    /// <summary>
    /// 服务器广播游戏结束
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerGameOver(byte[] obj)
    {
        AppDebug.Log("服务器广播游戏结束");

        PAIGOW_ROOM_GAMEOVER proto = PAIGOW_ROOM_GAMEOVER.decode(obj);

        if (proto.hasPaigowRoom() && proto.paigow_room.roomId > 0)
        {
            m_Result = proto.paigow_room;
            RoomPaiJiuProxy.Instance.RoomResult(m_Result);
        }
        //else
        //{
        //  //现在根据loop加载总结算 该消息有信息时不再结算
        //  SeeResult();
        //}
        SeeResult();


    }
    #endregion


    #region OnServerResultInfo 服务器广播总结算信息
    /// <summary>
    /// 服务器广播总结算信息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerResultInfo(byte[] obj)
    {
        AppDebug.Log("服务器广播总结算信息");

        PAIGOW_ROOM_TOTALSETTLE proto = PAIGOW_ROOM_TOTALSETTLE.decode(obj);

        if (proto.hasPaigowRoom())
        {
            m_Result = proto.paigow_room;
            RoomPaiJiuProxy.Instance.RoomResult(m_Result);
        }


    }
    #endregion






    #region OnServerReturnRebuild 服务器返回重建房间
    /// <summary>
    /// 服务器返回重建房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnRebuild(byte[] obj)
    {
        PAIGOW_ROOM_RECREATE proto = PAIGOW_ROOM_RECREATE.decode(obj);

        RoomPaiJiuProxy.Instance.InitRoom(proto.paigow_room);
        //RoomMaJiangProxy.Instance.InitRoom(proto);

        SceneMgr.Instance.LoadScene(SceneType.PaiJiu3D);
    }
    #endregion


    #region OnServerBroadcastApplyDisband 服务器广播解散房间
    /// <summary>
    /// 服务器广播解散房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastApplyDisband(byte[] obj)
    {

        //是否解散成功 //是否已经是申请解散状态 //某玩家是否同意解散 //该玩家是否是自己
        PAIGOW_ROOM_DISMISS proto = PAIGOW_ROOM_DISMISS.decode(obj);
        if (proto.hasIsSucceed())
        {
            if (proto.isSucceed)
            {
                RoomPaiJiuProxy.Instance.RoomDisband();
                UIViewManager.Instance.ShowMessage("提示", "房间已解散", MessageViewType.Ok, SeeResult);
            }
            else
            {
                UIViewManager.Instance.ShowMessage("提示", "房间解散失败", MessageViewType.Ok);

            }
        }
        else
        {
            //是否是申请解散
            if (RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus != ROOM_STATUS.DISMISS)
            {
                //是否是自己申请
                if (proto.pos == RoomPaiJiuProxy.Instance.PlayerSeat.Pos)
                {
                    UIViewManager.Instance.ShowMessage("提示", "等待申请解散结果", MessageViewType.None);

                }
                else
                {
                    UIViewManager.Instance.ShowMessage("提示", "有人发起解散房间，是否同意", MessageViewType.OkAndCancel, ClientSendAgreeDisbandRoom, ClientSendRefuseDisbandRoom, 10f, AutoClickType.Cancel);

                }
            }
            else
            {
                //某人同意或不同意
                if (proto.pos == RoomPaiJiuProxy.Instance.PlayerSeat.Pos)
                {
                    UIViewManager.Instance.ShowMessage("提示", "等待申请解散结果", MessageViewType.None);

                }

            }

        }

        RoomPaiJiuProxy.Instance.CurrentRoom.roomStatus = proto.room_status;
    }
    #endregion

    #endregion



    //OpenViewPaiJiu  以观察者模式提供加载窗口
    private void OpenViewPaiJiu(object[] obj)
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
        PaiJiu.Room room = RoomPaiJiuProxy.Instance.CurrentRoom;
        PaiJiu.Seat playerSeat = RoomPaiJiuProxy.Instance.PlayerSeat;


        switch (room.roomStatus)
        {


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
