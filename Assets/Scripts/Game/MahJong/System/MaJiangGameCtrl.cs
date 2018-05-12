//===================================================
//Author      : DRB
//CreateTime  ：5/11/2017 1:45:51 PM
//Description ：麻将游戏控制器
//===================================================
using System;
using System.Collections.Generic;
using DRB.MahJong;
using proto.mahjong;
using UnityEngine;
using proto.common;

public class MaJiangGameCtrl : SystemCtrlBase<MaJiangGameCtrl>, IGameCtrl, ISystemCtrl
{
    #region Variable
    private UISettleView m_UISettleView;

    private UIResultView m_UIResultView;

    private UISeatInfoView m_UISeatInfoView;

    private UIDisbandView m_UIDisbandView;
    /// <summary>
    /// 游戏结果
    /// </summary>
    private OP_ROOM_RESULT m_Result;

    public bool IsTingByPlayPoker { get; set; }

    public bool IsLiangXi { get; set; }

    private List<MaJiangCtrl> m_LiangXiSelect = new List<MaJiangCtrl>();
    private List<Poker> m_LiangXiPoker = new List<Poker>();
    private MaJiangCtrl m_CurrentSelect;

    private bool m_isChangingSeat;

    private List<MaJiangCtrl> m_SwapSelect = new List<MaJiangCtrl>();

    public Queue<IGameCommand> CommandQueue = new Queue<IGameCommand>();//命令队列
    #endregion

    #region DicNotificationInterests 注册UI事件
    /// <summary>
    /// 注册UI事件
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, UIDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add("btnReady", OnBtnSettleViewReadyClick);//结算界面准备按钮
        dic.Add("btnSettleViewResult", OnBtnSettleViewResultClick);//结算界面查看结果按钮
        dic.Add("btnResultViewBack", OnBtnResultViewBack);//结束界面返回按钮
        dic.Add("btnResultViewShare", OnBtnResultViewShareClick);//结束界面分享按钮
        dic.Add("btnSettleViewShare", OnBtnResultViewShareClick);//结算界面分享按钮
        dic.Add(ConstDefine.BtnSettleViewReplayOver, OnBtnResultViewReplayOverClick);
        dic.Add(ConstDefine.BtnMaJiangViewAuto, OnBtnMaJiangViewAutoClick);//托管按钮点击
        dic.Add(ConstDefine.BtnMaJiangViewCancelAuto, OnBtnMaJiangViewCancelAutoClick);//取消托管按钮点击
        dic.Add(ConstDefine.BtnGameViewShare, OnBtnMaJiangViewShareClick);//分享按钮点击
        dic.Add(ConstDefine.BtnMaJiangViewReady, OnBtnMaJiangViewReadyClick);//准备按钮点击
        dic.Add(ConstDefine.BtnMaJiangViewCancelReady, OnBtnMaJiangViewCancelReadyClick);//取消准备按钮点击
        dic.Add("OnOperatorClick", OnOperatorClick);//吃碰杠胡按钮点击
        dic.Add("OnMahjongViewHeadClick", OnHeadClick);//头像按钮点击
        dic.Add("OnBtnChangeSeatClick", OnBtnChangeSeatClick);//换座位按钮点击
        dic.Add("btnDisbandViewAgree", OnBtnDisbandViewAgree);//解散房间界面同意按钮
        dic.Add("btnDisbandViewRefuse", OnBtnDisbandViewRefuse);//解散房间界面拒绝按钮
        return dic;
    }
    #endregion

    #region Constructors
    public MaJiangGameCtrl()
    {
        NetDispatcher.Instance.AddEventListener(OP_ROOM_RESULT.CODE, OnServerReturnResult);//服务器返回结果消息

        NetDispatcher.Instance.AddEventListener(OP_ROOM_INFO.CODE, OnServerReturnCreateRoom);//服务器返回创建麻将房间消息
        NetDispatcher.Instance.AddEventListener(OP_ROOM_ENTER.CODE, OnServerBroadcastEnter);//服务器广播玩家进入消息
        NetDispatcher.Instance.AddEventListener(OP_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播玩家离开消息
        NetDispatcher.Instance.AddEventListener(OP_ROOM_READY.CODE, OnServerBroadcastReady);//服务器广播玩家准备消息
        NetDispatcher.Instance.AddEventListener(OP_ROOM_UNREADY.CODE, OnServerBroadcastCancelReady);//服务器广播玩家取消准备消息
        NetDispatcher.Instance.AddEventListener(OP_ROOM_TRUSTEE.CODE, OnServerBroadcastTrustee);//服务器广播玩家托管消息
        NetDispatcher.Instance.AddEventListener(OP_MATCH_WAIVER.CODE, OnServerBroadcastWaiver);//服务器广播玩家弃权消息
        NetDispatcher.Instance.AddEventListener(OP_ROOM_DISMISS.CODE, OnServerBroadcastApplyDisband);//服务器广播解散房间
        NetDispatcher.Instance.AddEventListener(OP_ROOM_NEW_DISMISS.CODE, OnServerBroadcastApplyDisband_New);//服务器广播解散房间(新版)
        NetDispatcher.Instance.AddEventListener(OP_ROOM_RECREATE.CODE, OnServerReturnRebuild); //服务器返回重建房间
        NetDispatcher.Instance.AddEventListener(OP_ROOM_BEGIN.CODE, OnServerBroadcastBegin);//服务器广播开局消息
        NetDispatcher.Instance.AddEventListener(OP_ROOM_GET_POKER.CODE, OnServerBroadcastDrawPoker);//服务器广播摸牌消息
        NetDispatcher.Instance.AddEventListener(OP_ROOM_OPERATE.CODE, OnServerBroadcastPlayPoker);//服务器广播出牌消息
        NetDispatcher.Instance.AddEventListener(OP_ROOM_ASK_FIGHT.CODE, OnServerPushAskFight);//服务器询问是否吃碰杠胡
        NetDispatcher.Instance.AddEventListener(OP_ROOM_FIGHT.CODE, OnServerBroadcastFight);//服务器广播吃碰杠胡消息
        NetDispatcher.Instance.AddEventListener(OP_ROOM_FIGHT_PASS.CODE, OnServerReturnPass);//服务器返回过
        NetDispatcher.Instance.AddEventListener(OP_ROOM_FIGHT_WAIT.CODE, OnServerReturnOperateWait);//服务器返回吃碰杠胡等待
        NetDispatcher.Instance.AddEventListener(OP_ROOM_SETTLE.CODE, OnServerBroadcastSettle);//服务器广播结算
        NetDispatcher.Instance.AddEventListener(OP_PLAYER_STATUS.CODE, OnServerBroadcastOffLine);//服务器广播掉线
        NetDispatcher.Instance.AddEventListener(OP_ROOM_AFK.CODE, OnServerBroadcastFocus);//服务器广播焦点
        NetDispatcher.Instance.AddEventListener(OP_ROOM_LUCK_POKER.CODE, OnServerBroadcastLuckPoker);//服务器广播翻牌
        NetDispatcher.Instance.AddEventListener(OP_ROOM_CHANGE_LUCK.CODE, OnServerBroadcastChangeLuckPoker);//服务器广播换宝
        NetDispatcher.Instance.AddEventListener(OP_ROOM_RANDOM_DICE.CODE, OnServerBroadcastRollDice);//服务器广播换宝摇骰子
        NetDispatcher.Instance.AddEventListener(OP_ROOM_STATUS.CODE, OnServerBroadcastStatus);//服务器广播房间状态
        NetDispatcher.Instance.AddEventListener(OP_ROOM_FIGHT_GOLD.CODE, OnServerBroadcastGoldChanged);//服务器广播金币变更
        NetDispatcher.Instance.AddEventListener(OP_ROOM_SITDOWN.CODE, OnServerBroadcastChangeSeat);//服务器广播换座位
        NetDispatcher.Instance.AddEventListener(OP_ROOM_RECHECK.CODE, OnServerBroadcastCheck);//服务器广播数据检测
        NetDispatcher.Instance.AddEventListener(OP_ROOM_SWAP_SETTING.CODE, OnServerBroadcastSwapSetting);
        NetDispatcher.Instance.AddEventListener(OP_ROOM_SWAP_BEGIN.CODE, OnServerBroadcastSwap);
        NetDispatcher.Instance.AddEventListener(OP_ROOM_SET_LACK.CODE, OnServerBroadcastLackColor);
    }
    #endregion

    #region Dispose
    public override void Dispose()
    {
        base.Dispose();
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_RESULT.CODE, OnServerReturnResult);//服务器返回结果消息
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_INFO.CODE, OnServerReturnCreateRoom);//服务器返回创建麻将房间消息
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_ENTER.CODE, OnServerBroadcastEnter);//服务器广播玩家进入消息
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_LEAVE.CODE, OnServerBroadcastLeave);//服务器广播玩家离开消息
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_READY.CODE, OnServerBroadcastReady);//服务器广播玩家准备消息
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_UNREADY.CODE, OnServerBroadcastCancelReady);//服务器广播玩家取消准备消息
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_TRUSTEE.CODE, OnServerBroadcastTrustee);//服务器广播玩家托管消息
        NetDispatcher.Instance.RemoveEventListener(OP_MATCH_WAIVER.CODE, OnServerBroadcastWaiver);//服务器广播玩家弃权消息
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_DISMISS.CODE, OnServerBroadcastApplyDisband);//服务器广播解散房间
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_NEW_DISMISS.CODE, OnServerBroadcastApplyDisband_New);//服务器广播解散房间(新版)
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_RECREATE.CODE, OnServerReturnRebuild); //服务器返回重建房间
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_BEGIN.CODE, OnServerBroadcastBegin);//服务器广播开局消息
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_GET_POKER.CODE, OnServerBroadcastDrawPoker);//服务器广播摸牌消息
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_OPERATE.CODE, OnServerBroadcastPlayPoker);//服务器广播出牌消息
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_ASK_FIGHT.CODE, OnServerPushAskFight);//服务器询问是否吃碰杠胡
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_FIGHT.CODE, OnServerBroadcastFight);//服务器广播吃碰杠胡消息
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_FIGHT_PASS.CODE, OnServerReturnPass);//服务器返回过
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_FIGHT_WAIT.CODE, OnServerReturnOperateWait);//服务器返回吃碰杠胡等待
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_SETTLE.CODE, OnServerBroadcastSettle);//服务器广播结算
        NetDispatcher.Instance.RemoveEventListener(OP_PLAYER_STATUS.CODE, OnServerBroadcastOffLine);//服务器广播掉线
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_AFK.CODE, OnServerBroadcastFocus);//服务器广播焦点
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_LUCK_POKER.CODE, OnServerBroadcastLuckPoker);//服务器广播翻牌
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_CHANGE_LUCK.CODE, OnServerBroadcastChangeLuckPoker);//服务器广播换宝
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_RANDOM_DICE.CODE, OnServerBroadcastRollDice);//服务器广播换宝摇骰子
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_STATUS.CODE, OnServerBroadcastStatus);//服务器广播房间状态
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_FIGHT_GOLD.CODE, OnServerBroadcastGoldChanged);//服务器广播金币变更
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_SITDOWN.CODE, OnServerBroadcastChangeSeat);//服务器广播换座位
        NetDispatcher.Instance.RemoveEventListener(OP_ROOM_RECHECK.CODE, OnServerBroadcastCheck);//服务器广播数据检测
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
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Settle, (GameObject go) =>
        {
            m_UISettleView = go.GetComponent<UISettleView>();
            string huTaiCount = string.Empty;
            cfg_settingEntity settingEntity = RoomMaJiangProxy.Instance.GetConfigByLabel("胡台数");
            if (settingEntity != null)
            {
                huTaiCount = settingEntity.name;
            }
            cfg_settingEntity fengEntity = RoomMaJiangProxy.Instance.GetConfigByName("带风");
            bool isFeng = fengEntity != null;
            m_UISettleView.Settle(RoomMaJiangProxy.Instance.CurrentRoom.SeatList, RoomMaJiangProxy.Instance.PlayerSeat.Pos, RoomMaJiangProxy.Instance.CurrentRoom.currentLoop, RoomMaJiangProxy.Instance.CurrentRoom.maxLoop, RoomMaJiangProxy.Instance.CurrentRoom.Prob, RoomMaJiangProxy.Instance.CurrentRoom.matchId, RoomMaJiangProxy.Instance.CurrentRoom.LuckPoker, RoomMaJiangProxy.Instance.CurrentRoom.isQuan, RoomMaJiangProxy.Instance.CurrentRoom.IsOver, RoomMaJiangProxy.Instance.CurrentRoom.roomId, huTaiCount, isFeng);
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
        m_UIResultView = UIViewUtil.Instance.LoadWindow(UIWindowType.Result).GetComponent<UIResultView>();
        m_UIResultView.SetUI(m_Result, RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0);
        m_Result = null;
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
            Debug.Log(RoomMaJiangProxy.Instance.CurrentRoom.DisbandTime);
            Debug.Log(TimeUtil.GetTimestampMS());
            Debug.Log(GlobalInit.Instance.TimeDistance);
            m_UIDisbandView.SetUI(RoomMaJiangProxy.Instance.CurrentRoom.SeatList, RoomMaJiangProxy.Instance.PlayerSeat, (RoomMaJiangProxy.Instance.CurrentRoom.DisbandTime - TimeUtil.GetTimestampMS() + GlobalInit.Instance.TimeDistance) / 1000, RoomMaJiangProxy.Instance.CurrentRoom.DisbandTimeMax / 1000);
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
        Reset();
        ClientSendRebuild();
    }
    #endregion

    #region QuitRoom 退出房间
    /// <summary>
    /// 退出房间
    /// </summary>
    public void QuitRoom()
    {
        if (RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0)
        {
            ShowMessage("提示", "是否退赛", MessageViewType.OkAndCancel, ClientSendLeaveRoom);
            return;
        }
        if ((RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Ready
            && (RoomMaJiangProxy.Instance.CurrentRoom.currentLoop == 1
            || RoomMaJiangProxy.Instance.CurrentRoom.currentLoop == 0))
            || RoomMaJiangProxy.Instance.CurrentRoom.isReplay)
        {
            ClientSendLeaveRoom();
        }
        else
        {
            DisbandRoom();
        }
    }
    #endregion

    #region DisbandRoom 解散房间
    /// <summary>
    /// 解散房间
    /// </summary>
    public void DisbandRoom()
    {
        if (RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0)
        {
            UIViewManager.Instance.ShowMessage("提示", "比赛场不能解散房间");
        }
        else
        {
            UIViewManager.Instance.ShowMessage("提示", "是否解散房间", MessageViewType.OkAndCancel, ClientSendApplyDisbandRoom_New);
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
        SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatByPlayerId(playerId);
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
            SeatEntity toSeat = RoomMaJiangProxy.Instance.GetSeatByPlayerId(toPlayerId);

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
        return RoomMaJiangProxy.Instance.CurrentRoom;
    }
    #endregion
    #endregion

    #region SeeResult 查看牌局总结算信息
    /// <summary>
    /// 查看牌局总结算信息
    /// </summary>
    private void SeeResult()
    {
        if (m_Result == null)
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

    #region Reset 重置数据
    /// <summary>
    /// 重置数据
    /// </summary>
    private void Reset()
    {
        CommandQueue.Clear();
        m_LiangXiPoker.Clear();
        m_LiangXiSelect.Clear();
        IsLiangXi = false;
        IsTingByPlayPoker = false;
        m_Result = null;
        m_isChangingSeat = false;
        m_CurrentSelect = null;
        m_SwapSelect.Clear();
        RoomMaJiangProxy.Instance.ClearRoom();
    }
    #endregion

    #region Replay 回放
    /// <summary>
    /// 回放
    /// </summary>
    /// <param name="replayEntity"></param>
    public void Replay(RecordReplayEntity replayEntity)
    {
        RoomMaJiangProxy.Instance.InitRoom(replayEntity);
        SceneMgr.Instance.LoadScene(SceneType.MaJiang3D);
    }
    #endregion


    #region 按钮点击
    #region OnPokerClick 手牌点击
    /// <summary>
    /// 手牌点击
    /// </summary>
    /// <param name="param"></param>
    public void OnPokerClick(MaJiangCtrl handPoker)
    {
        if (IsLiangXi)
        {
            if (m_CurrentSelect != null)
            {
                handPoker.isSelect = false;
                m_CurrentSelect = null;
            }

            if (m_LiangXiSelect.Contains(handPoker))
            {
                m_LiangXiPoker.Remove(handPoker.Poker);
                m_LiangXiSelect.Remove(handPoker);
                handPoker.isSelect = false;
            }
            else
            {
                for (int i = 0; i < RoomMaJiangProxy.Instance.AllCanLiangXi.Count; ++i)
                {
                    if (MahJongHelper.ContainPoker(handPoker.Poker, RoomMaJiangProxy.Instance.AllCanLiangXi[i]))
                    {
                        m_LiangXiPoker.Add(handPoker.Poker);
                        m_LiangXiSelect.Add(handPoker);
                        handPoker.isSelect = true;
                        break;
                    }
                }

            }
            UIItemOperator.Instance.ShowOK(MahJongHelper.CheckLiangXi(m_LiangXiPoker));
            return;
        }

        if (MahJongHelper.ContainPoker(handPoker.Poker, RoomMaJiangProxy.Instance.PlayerSeat.DingJiangPoker))
        {
            return;
        }

        if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Swap)
        {
            if (m_CurrentSelect != null && m_CurrentSelect == handPoker)
            {
                m_CurrentSelect.isSelect = false;
                m_CurrentSelect = null;
                return;
            }
            else if (m_CurrentSelect != null)
            {
                m_SwapSelect.Add(m_CurrentSelect);
                m_CurrentSelect = null;
            }
            if (RoomMaJiangProxy.Instance.PlayerSeat.SwapPoker.Count > 0) return;
            if (m_SwapSelect.Contains(handPoker))
            {
                m_SwapSelect.Remove(handPoker);
                handPoker.isSelect = false;
            }
            else
            {
                m_SwapSelect.Add(handPoker);
                handPoker.isSelect = true;
            }

            bool canSwap = true;
            if (m_SwapSelect.Count == 3)
            {
                for (int i = 1; i < m_SwapSelect.Count; ++i)
                {
                    if (m_SwapSelect[i].Poker.color != m_SwapSelect[i - 1].Poker.color)
                    {
                        canSwap = false;
                        break;
                    }
                }
            }
            else
            {
                canSwap = false;
            }
            UIItemOperator.Instance.ShowOK(canSwap);
            return;
        }



        if (m_CurrentSelect == null)
        {
            m_CurrentSelect = handPoker;
            handPoker.isSelect = true;
            if (!RoomMaJiangProxy.Instance.PlayerSeat.IsTing)
            {
                UIItemTingTip.Instance.Show(m_CurrentSelect, RoomMaJiangProxy.Instance.GetHu(m_CurrentSelect.Poker));
            }
        }
        else
        {
            if (m_CurrentSelect == handPoker)
            {
                if (m_CurrentSelect != null)
                {
                    m_CurrentSelect.isSelect = false;
                    m_CurrentSelect = null;
                }
#if !IS_LUALU
                ClientSendPlayPoker(handPoker.Poker);
#endif
            }
            else
            {
                m_CurrentSelect.isSelect = false;
                m_CurrentSelect = handPoker;
                handPoker.isSelect = true;
                if (!RoomMaJiangProxy.Instance.PlayerSeat.IsTing)
                {
                    UIItemTingTip.Instance.Show(m_CurrentSelect, RoomMaJiangProxy.Instance.GetHu(m_CurrentSelect.Poker));
                }
            }
        }
    }
    #endregion

    #region OnOperatorClick 操作按钮点击
    /// <summary>
    /// 操作按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnOperatorClick(object[] obj)
    {
        OperatorType type = (OperatorType)obj[0];

        Debug.Log("点击了按钮" + type.ToString());

        if (type == OperatorType.Ting)
        {
            IsTingByPlayPoker = true;
            return;
        }
        else if (type == OperatorType.Cancel)
        {
            IsTingByPlayPoker = false;
            IsLiangXi = false;
            for (int i = 0; i < m_LiangXiSelect.Count; ++i)
            {
                m_LiangXiSelect[i].isSelect = false;
            }
            m_LiangXiSelect.Clear();
            m_LiangXiPoker.Clear();

            List<MaJiangCtrl> lstHand = MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.PlayerSeat.Pos);
            for (int i = 0; i < lstHand.Count; ++i)
            {
                lstHand[i].isGray = false;
            }
            return;
        }
        else if (type == OperatorType.Pass)
        {
            IsTingByPlayPoker = false;
            IsLiangXi = false;
            for (int i = 0; i < m_LiangXiSelect.Count; ++i)
            {
                m_LiangXiSelect[i].isSelect = false;
            }
            m_LiangXiSelect.Clear();
            m_LiangXiPoker.Clear();

            List<MaJiangCtrl> lstHand = MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.PlayerSeat.Pos);
            for (int i = 0; i < lstHand.Count; ++i)
            {
                lstHand[i].isGray = false;
            }
        }
        else if (type == OperatorType.LiangXi)
        {
#if !IS_LUALU
            if (m_CurrentSelect != null)
            {
                m_CurrentSelect.isSelect = false;
                m_CurrentSelect = null;
            }
            if (RoomMaJiangProxy.Instance.AllCanLiangXi.Count == 3)
            {
                List<Poker> pokers = new List<Poker>();
                for (int i = 0; i < RoomMaJiangProxy.Instance.AllCanLiangXi.Count; ++i)
                {
                    pokers.Add(RoomMaJiangProxy.Instance.AllCanLiangXi[i][0]);
                }
                ClientSendLiangXi(pokers);
            }
            else
            {
                IsLiangXi = true;

                List<MaJiangCtrl> lstHand = MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.PlayerSeat.Pos);

                List<MaJiangCtrl> lstLiangXi = new List<MaJiangCtrl>();

                for (int i = 0; i < lstHand.Count; ++i)
                {
                    for (int j = 0; j < RoomMaJiangProxy.Instance.AllCanLiangXi.Count; ++j)
                    {
                        if (MahJongHelper.ContainPoker(lstHand[i].Poker, RoomMaJiangProxy.Instance.AllCanLiangXi[j]))
                        {
                            bool isExists = false;
                            for (int k = 0; k < lstLiangXi.Count; ++k)
                            {
                                if (lstLiangXi[k].Poker.index == lstHand[i].Poker.index)
                                {
                                    isExists = true;
                                    break;
                                }
                            }
                            if (!isExists)
                            {
                                lstLiangXi.Add(lstHand[i]);
                            }
                        }
                    }
                }

                for (int i = 0; i < lstHand.Count; ++i)
                {
                    bool isExists = false;
                    for (int j = 0; j < lstLiangXi.Count; ++j)
                    {
                        if (lstLiangXi[j].Poker.index == lstHand[i].Poker.index)
                        {
                            isExists = true;
                            break;
                        }
                    }
                    if (!isExists)
                    {
                        lstHand[i].isGray = true;
                    }
                }
            }
            return;
#endif
        }
        else if (type == OperatorType.Ok)
        {
            if (IsLiangXi)
            {
                IsLiangXi = false;
                for (int i = 0; i < m_LiangXiSelect.Count; ++i)
                {
                    m_LiangXiSelect[i].isSelect = false;
                }
                ClientSendLiangXi(m_LiangXiPoker);
                m_LiangXiSelect.Clear();
                m_LiangXiPoker.Clear();

                List<MaJiangCtrl> lstHand = MahJongManager.Instance.GetHand(RoomMaJiangProxy.Instance.PlayerSeat.Pos);
                for (int i = 0; i < lstHand.Count; ++i)
                {
                    lstHand[i].isGray = false;
                }
            }

            if (m_SwapSelect != null && m_SwapSelect.Count > 0)
            {
                List<Poker> lst = new List<Poker>();
                for (int i = 0; i < m_SwapSelect.Count; ++i)
                {
                    m_SwapSelect[i].isSelect = false;
                    lst.Add(m_SwapSelect[i].Poker);
                }
                ClientSendSelectSwap(lst);
                m_SwapSelect.Clear();
            }
            return;
        }
        else if (type == OperatorType.Wan)
        {
            ClientSendLackColor(1);
            return;
        }
        else if (type == OperatorType.Tong)
        {
            ClientSendLackColor(2);
            return;
        }
        else if (type == OperatorType.Tiao)
        {
            ClientSendLackColor(3);
            return;
        }

        List<Poker> lstPoker = obj[1] == null ? null : (obj[1] as List<Poker>);

        ClientSendOperate(type, lstPoker);
    }
    #endregion

    #region OnHeadClick 头像点击
    /// <summary>
    /// 头像点击
    /// </summary>
    /// <param name="seatPos"></param>
    private void OnHeadClick(object[] param)
    {
        int seatPos = (int)param[0];
        SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatBySeatId(seatPos);
#if !IS_LAOGUI
        if (seat == RoomMaJiangProxy.Instance.PlayerSeat && AccountProxy.Instance.CurrentAccountEntity.identity > 0)
        {
            UIViewManager.Instance.OpenWindow(UIWindowType.PlayerInfo);
            return;
        }
#endif

        List<SeatEntityBase> lstSeat = new List<SeatEntityBase>();
        for (int i = 0; i < RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count; ++i)
        {
            if (RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].Pos != seatPos && RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].PlayerId > 0)
            {
                lstSeat.Add(RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i]);
            }
        }

        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.SeatInfo, (GameObject go) =>
        {
            m_UISeatInfoView = go.GetComponent<UISeatInfoView>();
            m_UISeatInfoView.SetUI(seat, lstSeat);
#if IS_LAOGUI
            if (seat != RoomMaJiangProxy.Instance.PlayerSeat)
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
    private void OnBtnInteractiveExpressionClick(int seatPos, int id)
    {
        m_UISeatInfoView.Close();
        cfg_interactiveExpressionEntity entity = cfg_interactiveExpressionDBModel.Instance.Get(id);
        ChatCtrl.Instance.OnInteractiveClick(ENUM_PLAYER_MESSAGE.ANIMATION, entity.code, RoomMaJiangProxy.Instance.GetSeatBySeatId(seatPos).PlayerId, entity.sound);
    }
    #endregion

    #region OnBtnChangeSeatClick 换座位按钮点击
    /// <summary>
    /// 换座位按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnChangeSeatClick(object[] obj)
    {
        if (m_isChangingSeat) return;
        int seatIndex = (int)obj[0];

        SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatBySeatIndex(seatIndex);
        if (seat == null) return;

        int pos = seat.Pos;
        if (seat.Pos == 3 && RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count == 2)
        {
            pos = 2;
        }
        ClientSendChangeSeat(pos);
    }
    #endregion

    #region OnBtnSettleViewReadyClick 结算界面准备按钮点击
    /// <summary>
    /// 结算界面准备按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSettleViewReadyClick(object[] obj)
    {
        ClientSendReady();
        m_UISettleView.Close();
    }
    #endregion

    #region OnBtnMaJiangViewReadyClick 麻将场景UI准备按钮点击
    /// <summary>
    /// 麻将场景UI准备按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMaJiangViewReadyClick(object[] obj)
    {
        ClientSendReady();
    }
    #endregion

    #region OnBtnMaJiangViewCancelReadyClick 麻将场景取消准备按钮点击
    /// <summary>
    /// 麻将场景取消准备按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMaJiangViewCancelReadyClick(object[] obj)
    {
        ClientSendCancelReady();
    }
    #endregion

    #region OnBtnMaJiangViewShareClick 分享按钮点击
    /// <summary>
    /// 分享按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMaJiangViewShareClick(object[] obj)
    {
        ShareCtrl.Instance.ShareURL(ShareType.InGame);
    }
    #endregion

    #region OnBtnMaJiangViewAutoClick 托管按钮点击
    /// <summary>
    /// 托管按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMaJiangViewAutoClick(object[] obj)
    {
        if (RoomMaJiangProxy.Instance.CurrentRoom == null) return;
        if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Ready) return;

        if (RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0)
        {
            if (RoomMaJiangProxy.Instance.PlayerSeat.IsTrustee) return;
            ClientSendTrustee(true);
        }
        else
        {
            RoomMaJiangProxy.Instance.Trustee(RoomMaJiangProxy.Instance.PlayerSeat.PlayerId, true);
        }

    }
    #endregion

    #region OnBtnMaJiangViewCancelAutoClick 取消托管按钮点击
    /// <summary>
    /// 取消托管按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnMaJiangViewCancelAutoClick(object[] obj)
    {
        if (RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0)
        {
            if (!RoomMaJiangProxy.Instance.PlayerSeat.IsTrustee) return;
            ClientSendTrustee(false);
        }
        else
        {
            RoomMaJiangProxy.Instance.Trustee(RoomMaJiangProxy.Instance.PlayerSeat.PlayerId, false);
        }
    }
    #endregion

    #region OnBtnSettleViewResultClick 牌局结算界面查看结果按钮点击
    /// <summary>
    /// 牌局结算界面查看结果按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnSettleViewResultClick(object[] obj)
    {
        m_UISettleView.Close();
        SeeResult();
    }
    #endregion

    #region OnBtnResultViewReplayOverClick 牌局结果界面回放结束按钮点击
    /// <summary>
    /// 牌局结果界面回放结束按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnResultViewReplayOverClick(object[] obj)
    {
        ExitGame();
    }
    #endregion

    #region OnBtnResultViewBack 牌局结果界面返回按钮点击
    /// <summary>
    /// 牌局结果界面返回按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnResultViewBack(object[] obj)
    {
        if (RoomMaJiangProxy.Instance.CurrentRoom.matchId > 0)
        {
            m_UIResultView.Close();
            MatchCtrl.Instance.StartNext();
        }
        else
        {
            ExitGame();
        }
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
            m_UIResultView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
        }
        else if (m_UISettleView != null)
        {
            AppDebug.Log("开始分享了啊");
            m_UISettleView.StartCoroutine(ShareCtrl.Instance.ScreenCapture(OnScreenCaptureComplete));
        }
    }
    #endregion

    #region OnBtnDisbandViewRefuse 解散房间界面拒绝按钮点击
    /// <summary>
    /// 解散房间界面拒绝按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnDisbandViewRefuse(object[] obj)
    {
        ClientSendDisbandRoom_New(false);
    }
    #endregion

    #region OnBtnDisbandViewAgree 解散房间界面同意按钮点击
    /// <summary>
    /// 解散房间界面同意按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnDisbandViewAgree(object[] obj)
    {
        ClientSendDisbandRoom_New(true);
    }
    #endregion
    #endregion

    #region 客户端发送消息
    #region ClientSendCreateRoom 客户端发送创建房间消息
    /// <summary>
    /// 客户端发送创建房间消息
    /// </summary>
    private void ClientSendCreateRoom(int groupId, List<int> settingIds)
    {
        OP_ROOM_CREATE_GET proto = new OP_ROOM_CREATE_GET();
        for (int i = 0; i < settingIds.Count; ++i)
        {
            proto.addSettingId(settingIds[i]);
        }
        proto.clubId = groupId;
        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_CREATE_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendJoinRoom 客户端发送加入房间
    /// <summary>
    /// 客户端发送加入房间
    /// </summary>
    private void ClientSendJoinRoom(int roomId)
    {
        OP_ROOM_ENTER_GET proto = new OP_ROOM_ENTER_GET();
        proto.roomId = roomId;
        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_ENTER.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRebuild 客户端发送重建房间
    /// <summary>
    /// 客户端发送重建房间
    /// </summary>
    private void ClientSendRebuild()
    {
        NetWorkSocket.Instance.Send(null, OP_ROOM_RECREATE.CODE, GameCtrl.Instance.SocketHandle);
        ClientSendFocus(true);
    }
    #endregion

    #region ClientSendLeaveRoom 客户端发送离开房间
    /// <summary>
    /// 客户端发送离开房间
    /// </summary>
    private void ClientSendLeaveRoom()
    {
        NetWorkSocket.Instance.Send(null, OP_ROOM_LEAVE.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendTrustee 客户端发送托管消息
    /// <summary>
    /// 客户端发送托管消息
    /// </summary>
    private void ClientSendTrustee(bool isTrustee)
    {
        OP_ROOM_TRUSTEE_GET proto = new OP_ROOM_TRUSTEE_GET();
        proto.isTrustee = isTrustee;
        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_TRUSTEE_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendReady 客户端发送准备消息
    /// <summary>
    /// 客户端发送准备消息
    /// </summary>
    public void ClientSendReady()
    {
        NetWorkSocket.Instance.Send(null, OP_ROOM_READY_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendCancelReady 客户端发送取消准备消息
    /// <summary>
    /// 客户端发送取消准备消息
    /// </summary>
    private void ClientSendCancelReady()
    {
        NetWorkSocket.Instance.Send(null, OP_ROOM_UNREADY_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendPlayPoker 客户端发送出牌消息
    /// <summary>
    /// 客户端发送出牌消息
    /// </summary>
    /// <param name="poker">出的牌</param>
    public void ClientSendPlayPoker(Poker poker)
    {
        if (poker == null) return;
        if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Ready) return;
        if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Settle) return;
        if (RoomMaJiangProxy.Instance.CurrentRoom.isReplay) return;
#if !IS_LEPING && !IS_TAILAI && !IS_LUALU
        if (RoomMaJiangProxy.Instance.CurrentRoom.CurrentOperator != RoomMaJiangProxy.Instance.PlayerSeat) return;
#endif
#if IS_LONGGANG
        if (MahJongHelper.CheckUniversal(poker, RoomMaJiangProxy.Instance.PlayerSeat.UniversalList)) return;
#endif

#if IS_LEPING
        ClientSendPass();
#endif
        if (IsTingByPlayPoker)
        {
            ClientSendPass();
        }


        OP_ROOM_OPERATE_GET proto = new OP_ROOM_OPERATE_GET();
        proto.index = poker.index;
        proto.isListen = IsTingByPlayPoker;
        AppDebug.Log("客户端出牌 索引:" + poker.index + "   花色:" + poker.color + "  大小:" + poker.size);
        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_OPERATE.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendPass 客户端发送过消息
    /// <summary>
    /// 客户端发送过消息
    /// </summary>
    public void ClientSendPass()
    {
        NetWorkSocket.Instance.Send(null, OP_ROOM_FIGHT_PASS.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendOperate 客户端发送吃碰杠胡消息
    /// <summary>
    /// 客户端发送吃碰杠胡消息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="poker"></param>
    public void ClientSendOperate(OperatorType type, List<Poker> pokers)
    {
        AppDebug.Log("客户端" + type.ToString());

        switch (type)
        {
            case OperatorType.Pass:
                ClientSendPass();
                return;
            case OperatorType.LiangXi:
                ClientSendLiangXi(pokers);
                return;
        }
        OP_ROOM_FIGHT_GET proto = new OP_ROOM_FIGHT_GET();
        proto.typeId = (ENUM_POKER_TYPE)type;
        if (pokers != null)
        {
            for (int i = 0; i < pokers.Count; ++i)
            {
                proto.addIndex(pokers[i].index);
                Debug.Log(pokers[i].ToString("{0}_{1}_{2}"));
            }
        }
        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_FIGHT_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendLiangXi 客户端发送亮喜消息
    /// <summary>
    /// 客户端发送亮喜消息
    /// </summary>
    /// <param name="pokers"></param>
    private void ClientSendLiangXi(List<Poker> pokers)
    {
        OP_ROOM_SHOW_LUCK_GET proto = new OP_ROOM_SHOW_LUCK_GET();
        for (int i = 0; i < pokers.Count; ++i)
        {
            Poker poker = pokers[i];
            OP_POKER op_poker = new OP_POKER()
            {
                color = poker.color,
                index = poker.index,
                size = poker.size,
                pos = poker.pos,
            };
            proto.addPoker(op_poker);
        }
        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_SHOW_LUCK_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendApplyDisbandRoom 客户端发送请求解散房间
    /// <summary>
    /// 客户端发送请求解散房间
    /// </summary>
    private void ClientSendApplyDisbandRoom()
    {
        OP_ROOM_DISMISS_GET proto = new OP_ROOM_DISMISS_GET();
        proto.isDismiss = true;

        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_DISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendAgreeDisbandRoom 客户端发送同意解散房间
    /// <summary>
    /// 客户端发送同意解散房间
    /// </summary>
    private void ClientSendAgreeDisbandRoom()
    {
        OP_ROOM_DISMISS_GET proto = new OP_ROOM_DISMISS_GET();
        proto.isDismiss = true;
        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_DISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendRefuseDisbandRoom 客户端发送拒绝解散房间
    /// <summary>
    /// 客户端发送拒绝解散房间
    /// </summary>
    private void ClientSendRefuseDisbandRoom()
    {
        OP_ROOM_DISMISS_GET proto = new OP_ROOM_DISMISS_GET();
        proto.isDismiss = false;
        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_DISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendApplyDisbandRoom_New 客户端发送申请解散房间（新版）
    /// <summary>
    /// 客户端发送申请解散房间（新版）
    /// </summary>
    private void ClientSendApplyDisbandRoom_New()
    {
        OP_ROOM_NEW_DISMISS_GET proto = new OP_ROOM_NEW_DISMISS_GET();
        proto.isDismiss = true;
        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_NEW_DISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendDisbandRoom_New 客户端发送解散房间（新版）
    /// <summary>
    /// 客户端发送解散房间（新版）
    /// </summary>
    /// <param name="isAgree"></param>
    private void ClientSendDisbandRoom_New(bool isAgree)
    {
        OP_ROOM_NEW_DISMISS_GET proto = new OP_ROOM_NEW_DISMISS_GET();
        proto.isDismiss = isAgree;
        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_NEW_DISMISS_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendFocus 客户端发送焦点切换消息
    /// <summary>
    /// 客户端发送焦点切换消息
    /// </summary>
    /// <param name="isFocus"></param>
    public void ClientSendFocus(bool isFocus)
    {
        OP_ROOM_AFK_GET proto = new OP_ROOM_AFK_GET();
        proto.isAfk = !isFocus;

        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_AFK_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendChangeSeat 客户端发送换座位消息
    /// <summary>
    /// 客户端发送换座位消息
    /// </summary>
    /// <param name="seatPos"></param>
    private void ClientSendChangeSeat(int seatPos)
    {
        OP_ROOM_SITDOWN_GET proto = new OP_ROOM_SITDOWN_GET();
        proto.pos = seatPos;
        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_SITDOWN_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendSelectSwap 客户端发送选择交换牌
    /// <summary>
    /// 客户端发送选择交换牌
    /// </summary>
    /// <param name="pokers"></param>
    private void ClientSendSelectSwap(List<Poker> pokers)
    {
        OP_ROOM_SWAP_SETTING_GET proto = new OP_ROOM_SWAP_SETTING_GET();
        for (int i = 0; i < pokers.Count; ++i)
        {
            OP_POKER op_poker = new OP_POKER();
            op_poker.index = pokers[i].index;
            op_poker.color = pokers[i].color;
            op_poker.size = pokers[i].size;
            op_poker.pos = pokers[i].pos;
            proto.addPoker(op_poker);
        }

        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_SWAP_SETTING_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendLackColor 客户端发送定缺
    /// <summary>
    /// 客户端发送定缺
    /// </summary>
    /// <param name="color"></param>
    private void ClientSendLackColor(int color)
    {
        OP_ROOM_SET_LACK_GET proto = new OP_ROOM_SET_LACK_GET();
        proto.color = color;
        NetWorkSocket.Instance.Send(proto.encode(), OP_ROOM_SET_LACK_GET.CODE, GameCtrl.Instance.SocketHandle);
    }
    #endregion

    #region ClientSendMingTi 客户端发送明提
    /// <summary>
    /// 客户端发送明提
    /// </summary>
    private void ClientSendMingTi()
    {

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
        CommandQueue.Clear();
        OP_ROOM_INFO proto = OP_ROOM_INFO.decode(obj);

        RoomMaJiangProxy.Instance.InitRoom(proto);
        SceneMgr.Instance.LoadScene(SceneType.MaJiang3D);
    }
    #endregion

    #region OnServerBroadcastReady 服务器广播玩家准备消息
    /// <summary>
    /// 服务器广播玩家准备消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastReady(byte[] obj)
    {
        OP_ROOM_READY proto = OP_ROOM_READY.decode(obj);
        RoomMaJiangProxy.Instance.Ready(proto.playerId);
    }
    #endregion

    #region OnServerBroadcastCancelReady 服务器广播取消准备
    /// <summary>
    /// 服务器广播取消准备
    /// </summary>
    /// <param name="data"></param>
    private void OnServerBroadcastCancelReady(byte[] data)
    {
        OP_ROOM_UNREADY proto = OP_ROOM_UNREADY.decode(data);
        RoomMaJiangProxy.Instance.CancelReady(proto.playerId);
    }
    #endregion

    #region OnServerBroadcastEnter 服务器广播玩家进入消息
    /// <summary>
    /// 服务器广播玩家进入消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastEnter(byte[] obj)
    {

        OP_ROOM_ENTER proto = OP_ROOM_ENTER.decode(obj);
        RoomMaJiangProxy.Instance.EnterRoom(proto);
    }
    #endregion

    #region OnServerBroadcastLeave 服务器广播玩家离开消息
    /// <summary>
    /// 服务器广播玩家离开消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLeave(byte[] obj)
    {
        OP_ROOM_LEAVE proto = OP_ROOM_LEAVE.decode(obj);
        RoomMaJiangProxy.Instance.ExitRoom(proto);

        if (RoomMaJiangProxy.Instance.PlayerSeat != null && proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId)
        {
            GameCtrl.Instance.ExitGame();
        }
    }
    #endregion

    #region OnServerBroadcastWaiver 服务器广播玩家弃权消息
    /// <summary>
    /// 服务器广播玩家弃权消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastWaiver(byte[] obj)
    {
        OP_MATCH_WAIVER proto = new OP_MATCH_WAIVER();
        RoomMaJiangProxy.Instance.Waiver(proto.playerId);
    }
    #endregion

    #region OnServerBroadcastBegin 服务器广播开局
    /// <summary>
    /// 服务器广播开局
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastBegin(byte[] obj)
    {
        OP_ROOM_BEGIN proto = OP_ROOM_BEGIN.decode(obj);

        IGameCommand command = new BeginCommand(proto);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastDrawPoker 服务器广播摸牌消息
    /// <summary>
    /// 服务器广播摸牌消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastDrawPoker(byte[] obj)
    {
        OP_ROOM_GET_POKER proto = OP_ROOM_GET_POKER.decode(obj);

        try
        {
            if (proto.playerId == AccountProxy.Instance.CurrentAccountEntity.passportId && proto.color == 0)
            {
                AppDebug.ThrowError("服务器广播玩家摸了一张空牌");
            }
        }
        catch (Exception e)
        {
            LogSystem.LogError(e.ToString());
            RebuildRoom();
            return;
        }
        IGameCommand command = new DrawPokerCommand(proto.playerId, new Poker(proto.index, proto.color, proto.size, 0), proto.isLast, proto.isBuhua, proto.countdown);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastOperate 服务器广播出牌消息
    /// <summary>
    /// 服务器广播出牌消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastPlayPoker(byte[] obj)
    {
        OP_ROOM_OPERATE proto = OP_ROOM_OPERATE.decode(obj);
        IsTingByPlayPoker = false;

        try
        {
            if (proto.color == 0)
            {
                AppDebug.ThrowError("服务器发送的出牌花色是0");
            }
        }
        catch (Exception e)
        {
            LogSystem.LogError(e.ToString());
            RebuildRoom();
            return;
        }

        IGameCommand command = new PlayPokerCommand(proto.playerId, new Poker(proto.index, proto.color, proto.size), proto.isListen);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerPushAskFight 服务器询问是否吃碰杠胡
    /// <summary>
    /// 服务器询问是否吃碰杠胡
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerPushAskFight(byte[] obj)
    {
        OP_ROOM_ASK_FIGHT proto = OP_ROOM_ASK_FIGHT.decode(obj);
        List<PokerCombinationEntity> AskPokerGroup = null;
        int askLeng = proto.askPokerGroupCount();
        if (askLeng > 0)
        {
            AskPokerGroup = new List<PokerCombinationEntity>();
            for (int i = 0; i < askLeng; ++i)
            {
                List<Poker> lst = new List<Poker>();
                OP_POKER_GROUP op_group = proto.getAskPokerGroup(i);
                for (int j = 0; j < op_group.pokerCount(); ++j)
                {
                    OP_POKER op_poker = op_group.getPoker(j);
                    lst.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
                }
                PokerCombinationEntity combination = new PokerCombinationEntity((OperatorType)op_group.typeId, (int)op_group.subTypeId, lst);
                AskPokerGroup.Add(combination);
            }
        }

        IGameCommand command = new AskOperateCommand(AskPokerGroup, proto.countdown);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastFight 服务器广播吃碰杠胡
    /// <summary>
    /// 服务器广播吃碰杠胡
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastFight(byte[] obj)
    {
        OP_ROOM_FIGHT proto = OP_ROOM_FIGHT.decode(obj);
        AppDebug.Log(string.Format("服务器广播{0} {1}了", proto.playerId, (OperatorType)proto.typeId));

        List<Poker> lst = new List<Poker>(proto.pokerCount());
        for (int i = 0; i < proto.pokerCount(); ++i)
        {
            OP_POKER op_poker = proto.getPoker(i);
            lst.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
        }

        IGameCommand command = new OperateCommand(proto.playerId, (OperatorType)proto.typeId, (int)proto.subTypeId, lst, proto.countdown);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerReturnPass 服务器返回过
    /// <summary>
    /// 服务器返回过
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnPass(byte[] obj)
    {
        AppDebug.Log("服务器返回Pass");
        IGameCommand command = new OperateCommand(RoomMaJiangProxy.Instance.PlayerSeat.PlayerId, OperatorType.Pass, 0, null, 0);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerReturnOperateWait 服务器返回吃碰杠胡等待
    /// <summary>
    /// 服务器返回吃碰杠胡等待
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnOperateWait(byte[] obj)
    {
        AppDebug.Log("服务器返回吃碰杠胡等待");
        UIItemOperator.Instance.Close();
    }
    #endregion

    #region OnServerBroadcastTrustee 服务器广播托管消息
    /// <summary>
    /// 服务器广播托管消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastTrustee(byte[] obj)
    {
        OP_ROOM_TRUSTEE proto = OP_ROOM_TRUSTEE.decode(obj);
        RoomMaJiangProxy.Instance.Trustee(proto.playerId, proto.isTrustee);
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
        OP_ROOM_SETTLE proto = OP_ROOM_SETTLE.decode(obj);

        IGameCommand command = new SettleCommand(proto);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerReturnResult 服务器返回结果消息
    /// <summary>
    /// 服务器返回结果消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnResult(byte[] obj)
    {
        OP_ROOM_RESULT proto = OP_ROOM_RESULT.decode(obj);
        m_Result = proto;
    }
    #endregion

    #region OnServerReturnRebuild 服务器返回重建房间
    /// <summary>
    /// 服务器返回重建房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerReturnRebuild(byte[] obj)
    {
        OP_ROOM_INFO proto = OP_ROOM_INFO.decode(obj);

        CommandQueue.Clear();
        RoomMaJiangProxy.Instance.InitRoom(proto);
        SceneMgr.Instance.LoadScene(SceneType.MaJiang3D);
    }
    #endregion

    #region OnServerBroadcastLuckPoker 服务器广播翻牌
    /// <summary>
    /// 服务器广播翻牌
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastLuckPoker(byte[] obj)
    {
        OP_ROOM_LUCK_POKER proto = OP_ROOM_LUCK_POKER.decode(obj);

        IGameCommand command = new LuckPokerCommand(new Poker(proto.index, proto.color, proto.size));
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastChangeLuckPoker 服务器广播换宝
    /// <summary>
    /// 服务器广播换宝
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastChangeLuckPoker(byte[] obj)
    {
        OP_ROOM_CHANGE_LUCK proto = OP_ROOM_CHANGE_LUCK.decode(obj);
        Poker prevLuckPoker = new Poker(0, proto.color, proto.size);
        Poker currLuckPoker = new Poker(proto.index, 0, 0);

        IGameCommand command = new ChangeLuckPokerCommand(prevLuckPoker, currLuckPoker);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastRollDice 服务器广播摇骰子
    /// <summary>
    /// 服务器广播摇骰子
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastRollDice(byte[] obj)
    {
        OP_ROOM_RANDOM_DICE proto = OP_ROOM_RANDOM_DICE.decode(obj);

        IGameCommand command = new RollDiceCommand(proto.playerId, proto.diceA);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastApplyDisband 服务器广播解散房间
    /// <summary>
    /// 服务器广播解散房间
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastApplyDisband(byte[] obj)
    {
        OP_ROOM_DISMISS proto = OP_ROOM_DISMISS.decode(obj);

        if (proto.hasIsSucceed())
        {
            if (proto.isSucceed)
            {
                Debug.Log("哈哈哈哈哈哈哈哈哈");
                UIViewManager.Instance.ShowMessage("提示", "房间已解散", MessageViewType.Ok, SeeResult);
            }
            else
            {
                SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatByPlayerId(proto.playerId);
                string nickName = "有人";
                if (seat != null)
                {
                    nickName = seat.Nickname;
                }
                UIViewManager.Instance.ShowMessage("提示", string.Format("{0}不同意解散房间", nickName), MessageViewType.Ok);
            }
        }
        else
        {
            SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatByPlayerId(proto.playerId);
            string nickName = "有人";
            if (seat != null)
            {
                nickName = seat.Nickname;
            }
            UIViewManager.Instance.ShowMessage("提示", string.Format("{0}发起解散房间，是否同意", nickName), MessageViewType.OkAndCancel, ClientSendAgreeDisbandRoom, ClientSendRefuseDisbandRoom, 10f, AutoClickType.Cancel);
        }
    }
    #endregion

    #region OnServerBroadcastApplyDisband_New 服务器广播解散房间(新版)
    /// <summary>
    /// 服务器广播解散房间(新版)
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastApplyDisband_New(byte[] obj)
    {
        OP_ROOM_NEW_DISMISS proto = OP_ROOM_NEW_DISMISS.decode(obj);
        if (proto.hasIsSucceed())
        {
            if (proto.isSucceed)
            {
                UIViewManager.Instance.ShowMessage("提示", "房间已解散", MessageViewType.Ok, SeeResult);
            }
            else
            {
                string nickName = "有人";
                for (int i = 0; i < RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count; ++i)
                {
                    if (RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].DisbandState == DisbandState.Refuse)
                    {
                        nickName = RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].Nickname;
                    }
                }
                UIViewManager.Instance.ShowMessage("提示", string.Format("{0}不同意解散房间", nickName), MessageViewType.Ok);
                for (int i = 0; i < RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count; ++i)
                {
                    RoomMaJiangProxy.Instance.CurrentRoom.SeatList[i].DisbandState = DisbandState.Wait;
                }
            }
            if (m_UIDisbandView != null)
            {
                m_UIDisbandView.Close();
            }
        }
        else
        {
            RoomMaJiangProxy.Instance.SetSeatDisbandState(proto.playerId, (DisbandState)proto.dismiss, proto.dismissTime);
            if (m_UIDisbandView == null)
            {
                OpenDisbandView();
            }
            else
            {
                m_UIDisbandView.SetUI(RoomMaJiangProxy.Instance.CurrentRoom.SeatList, RoomMaJiangProxy.Instance.PlayerSeat, 0f, RoomMaJiangProxy.Instance.CurrentRoom.DisbandTimeMax / 1000);
            }
        }
    }
    #endregion

    #region OnServerBroadcastOffLine 服务器广播离线
    /// <summary>
    /// 服务器广播离线
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastOffLine(byte[] obj)
    {
        if (RoomMaJiangProxy.Instance == null) return;

        OP_PLAYER_STATUS proto = OP_PLAYER_STATUS.decode(obj);
        RoomMaJiangProxy.Instance.SetOnLine(proto.playerId, proto.online > 0);
    }
    #endregion

    #region OnServerBroadcastFocus 服务器广播焦点
    /// <summary>
    /// 服务器广播焦点
    /// </summary>
    /// <param name="buffer"></param>
    private void OnServerBroadcastFocus(byte[] buffer)
    {
        OP_ROOM_AFK proto = OP_ROOM_AFK.decode(buffer);
        RoomMaJiangProxy.Instance.SetFocus(proto.playerId, !proto.isAfk);
    }
    #endregion

    #region OnServerBroadcastStatus 服务器广播状态
    /// <summary>
    /// 服务器广播状态
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastStatus(byte[] obj)
    {
        OP_ROOM_STATUS proto = OP_ROOM_STATUS.decode(obj);

        IGameCommand command = new ChangeStatusCommand((RoomEntity.RoomStatus)proto.status);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastGoldChanged 服务器广播金币变化
    /// <summary>
    /// 服务器广播金币变化
    /// </summary>
    /// <param name="obj"></param>
    private void OnServerBroadcastGoldChanged(byte[] obj)
    {
        OP_ROOM_FIGHT_GOLD proto = OP_ROOM_FIGHT_GOLD.decode(obj);

        for (int i = 0; i < proto.seatCount(); ++i)
        {
            RoomMaJiangProxy.Instance.SetGold(proto.getSeat(i).playerId, proto.getSeat(i).gold);
        }
    }
    #endregion

    #region OnServerBroadcastDouble 服务器广播翻倍
    /// <summary>
    /// 服务器广播翻倍
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastDouble(byte[] bytes)
    {
        OP_ROOM_DOUBLE proto = OP_ROOM_DOUBLE.decode(bytes);

        RoomMaJiangProxy.Instance.SetDouble(proto.playerId, proto.isDouble);
    }
    #endregion

    #region OnServerBroadcastChangeSeat 服务器广播换座位消息
    /// <summary>
    /// 服务器广播换座位消息
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastChangeSeat(byte[] bytes)
    {
        OP_ROOM_SITDOWN proto = OP_ROOM_SITDOWN.decode(bytes);

        if (RoomMaJiangProxy.Instance.SetPos(proto.playerId, proto.pos))
        {
            if (proto.playerId == RoomMaJiangProxy.Instance.PlayerSeat.PlayerId)
            {
                m_isChangingSeat = true;
                CameraCtrl.Instance.SetPos(RoomMaJiangProxy.Instance.PlayerSeat.Pos, () =>
                {
                    m_isChangingSeat = false;
                });
            }
        }
    }
    #endregion

    #region OnServerBroadcastCheck 服务器广播检测消息
    /// <summary>
    /// 服务器广播检测消息
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastCheck(byte[] bytes)
    {
        OP_ROOM_RECHECK proto = OP_ROOM_RECHECK.decode(bytes);

        List<Poker> lst = new List<Poker>();
        for (int i = 0; i < proto.pokerCount(); ++i)
        {
            OP_POKER op_poker = proto.getPoker(i);
            lst.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
        }

        IGameCommand command = new CheckCommand(lst);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastSwap 服务器广播选择交换牌
    /// <summary>
    /// 服务器广播选择交换牌
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastSwapSetting(byte[] bytes)
    {
        OP_ROOM_SWAP_SETTING proto = OP_ROOM_SWAP_SETTING.decode(bytes);
        List<Poker> lst = new List<Poker>();
        for (int i = 0; i < proto.pokerCount(); ++i)
        {
            OP_POKER op_poker = proto.getPoker(i);
            lst.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
        }

        IGameCommand command = new SetSwapPokerCommand(proto.playerId, lst);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastSwap 服务器广播交换
    /// <summary>
    /// 服务器广播交换
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastSwap(byte[] bytes)
    {
        OP_ROOM_SWAP_BEGIN proto = OP_ROOM_SWAP_BEGIN.decode(bytes);

        List<Poker> lst = new List<Poker>();
        for (int i = 0; i < proto.pokerCount(); ++i)
        {
            OP_POKER op_poker = proto.getPoker(i);
            lst.Add(new Poker(op_poker.index, op_poker.color, op_poker.size, op_poker.pos));
        }

        IGameCommand command = new SwapPokerCommand((int)proto.mode, lst);
        CommandQueue.Enqueue(command);
    }
    #endregion

    #region OnServerBroadcastLackColor 服务器广播缺门
    /// <summary>
    /// 服务器广播缺门
    /// </summary>
    /// <param name="bytes"></param>
    private void OnServerBroadcastLackColor(byte[] bytes)
    {
        OP_ROOM_SET_LACK proto = OP_ROOM_SET_LACK.decode(bytes);

        IGameCommand command = new LackColorCommand(proto.playerId, proto.color);
        CommandQueue.Enqueue(command);
    }
    #endregion
    #endregion
}